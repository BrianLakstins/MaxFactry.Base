﻿// <copyright file="MaxBaseIdIndexEntity.cs" company="Lakstins Family, LLC">
// Copyright (c) Brian A. Lakstins (http://www.lakstins.com/brian/)
// </copyright>

#region License
// <license>
// This software is provided 'as-is', without any express or implied warranty. In no 
// event will the author be held liable for any damages arising from the use of this 
// software.
//  
// Permission is granted to anyone to use this software for any purpose, including 
// commercial applications, and to alter it and redistribute it freely, subject to the 
// following restrictions:
// 
// 1. The origin of this software must not be misrepresented; you must not claim that 
// you wrote the original software. If you use this software in a product, an 
// acknowledgment (see the following) in the product documentation is required.
// 
// Portions Copyright (c) Brian A. Lakstins (http://www.lakstins.com/brian/)
// 
// 2. Altered source versions must be plainly marked as such, and must not be 
// misrepresented as being the original software.
// 
// 3. This notice may not be removed or altered from any source distribution.
// </license>
#endregion

#region Change Log
// <changelog>
// <change date="11/10/2014" author="Brian A. Lakstins" description="Initial release.">
// <change date="3/26/2015" author="Brian A. Lakstins" description="Moved to MaxFactry.Base">
// <change date="6/8/2015" author="Brian A. Lakstins" description="Updated to use cache.">
// <change date="12/9/2015" author="Brian A. Lakstins" description="Updated to include indexId as part of storage id.">
// <change date="1/14/2016" author="Brian A. Lakstins" description="Updated to return blank if something exists in the index and null if nothing exists.">
// <change date="4/20/2016" author="Brian A. Lakstins" description="Updated to use centralized caching.">
// <change date="8/30/2016" author="Brian A. Lakstins" description="Fix insert when already exists.">
// <change date="12/12/2019" author="Brian A. Lakstins" description="Reset data for new record instead of reusing loaded data with modifications.">
// <change date="1/16/2021" author="Brian A. Lakstins" description="Update definition of cache keys.">
// <change date="3/20/2024" author="Brian A. Lakstins" description="Happy birthday to my mom.  Sara Jean Lakstins (Cartwright) - 3/20/1944 to 3/14/2019.">
// <change date="3/22/2024" author="Brian A. Lakstins" description="Updated for change to MaxData">
// <change date="3/24/2024" author="Brian A. Lakstins" description="Updated for changes namespaces">
// <change date="6/3/2025" author="Brian A. Lakstins" description="Update to use base methods for managing data">
// <change date="6/4/2025" author="Brian A. Lakstins" description="Fix issue inserting after updating">
// </changelog>
#endregion

namespace MaxFactry.Base.BusinessLayer
{
	using System;
    using MaxFactry.Base.DataLayer;
    using MaxFactry.Base.DataLayer.Library;

    /// <summary>
    /// Base Business Layer Entity
    /// </summary>
    public abstract class MaxBaseIdIndexEntity : MaxBaseEntity
	{
		/// <summary>
        /// Initializes a new instance of the MaxBaseIdIndexEntity class
		/// </summary>
		/// <param name="loData">object to hold data</param>
		public MaxBaseIdIndexEntity(MaxData loData)
			: base(loData)
		{
		}

        /// <summary>
        /// Initializes a new instance of the MaxBaseIdIndexEntity class.
        /// </summary>
        /// <param name="loDataModelType">Type of data model.</param>
        public MaxBaseIdIndexEntity(Type loDataModelType) 
            : base(loDataModelType)
        {
        }

		/// <summary>
		/// Gets or sets the Id of this index.
		/// </summary>
        public Guid IndexId
		{
			get
			{
                return this.GetGuid(this.MaxBaseIdIndexDataModel.IndexId);
			}

            set
            {
                this.Set(this.MaxBaseIdIndexDataModel.IndexId, value);
            }
		}

        /// <summary>
        /// Gets or sets the name associated with the value
        /// </summary>
        public string Name
        {
            get
            {
                return this.GetString(this.MaxBaseIdIndexDataModel.Name);
            }

            set
            {
                this.Set(this.MaxBaseIdIndexDataModel.Name, value);
            }
        }

        /// <summary>
        /// Gets or sets the name associated with the value
        /// </summary>
        public string Value
        {
            get
            {
                return this.GetString(this.MaxBaseIdIndexDataModel.Value);
            }

            set
            {
                this.Set(this.MaxBaseIdIndexDataModel.Value, value);
            }
        }

        /// <summary>
        /// Gets the Data Model for this entity
        /// </summary>
        protected MaxBaseIdIndexDataModel MaxBaseIdIndexDataModel
        {
            get
            {
                return (MaxBaseIdIndexDataModel)MaxDataLibrary.GetDataModel(this.DataModelType);
            }
        }

        /// <summary>
        /// Saves the value related to the index and the name
        /// Sets the Active flag when a value is updated that already exists.
        /// </summary>
        /// <param name="loIndexId">Index to save to</param>
        /// <param name="lsName">Name of the parameter</param>
        /// <param name="lsValue">Value for the parameter</param>
        public virtual void SaveValue(Guid loIndexId, string lsName, string lsValue)
        {
            if (!Guid.Empty.Equals(loIndexId) && !string.IsNullOrEmpty(lsName))
            {
                MaxEntityList loList = this.LoadAllActiveByIndexIdNameCache(loIndexId, lsName);
                if (loList.Count > 0)
                {
                    MaxBaseIdIndexEntity loEntity = loList[loList.Count - 1] as MaxBaseIdIndexEntity;
                    if (loEntity.Value != lsValue)
                    {
                        loEntity.Value = lsValue;
                        loEntity.Update();
                    }

                    if (loList.Count > 1)
                    {
                        for (int lnE = 0; lnE < loList.Count - 1; lnE++)
                        {
                            loEntity = loList[lnE] as MaxBaseIdIndexEntity;
                            loEntity.IsActive = false;
                            loEntity.Update();
                        }
                    }
                }
                else
                {
                    this.Data.Clear();
                    this.IndexId = loIndexId;
                    this.Name = lsName;
                    this.Value = lsValue;
                    this.IsActive = true;
                    this.Insert();
                }
            }
        }

        /// <summary>
        /// Gets the value based on the index and name
        /// </summary>
        /// <param name="loIndexId">Index to look up</param>
        /// <param name="lsName">Name related to the value</param>
        /// <returns>The value related to the name</returns>
        public virtual string GetValue(Guid loIndexId, string lsName)
        {
            string lsR = null;
            if (!Guid.Empty.Equals(loIndexId) && !string.IsNullOrEmpty(lsName))
            {
                MaxEntityList loList = this.LoadAllActiveByIndexIdNameCache(loIndexId, lsName);
                if (loList.Count > 0)
                {
                    MaxBaseIdIndexEntity loEntity = loList[loList.Count - 1] as MaxBaseIdIndexEntity;
                    lsR = loEntity.Value;
                    if (loList.Count > 1)
                    {
                        for (int lnE = 0; lnE < loList.Count - 1; lnE++)
                        {
                            loEntity = loList[lnE] as MaxBaseIdIndexEntity;
                            loEntity.IsActive = false;
                            loEntity.Update();
                        }
                    }
                }
            }

            return lsR;
        }

        protected MaxEntityList LoadAllActiveByIndexIdNameCache(Guid loIndexId, string lsName)
        {
            MaxData loData = new MaxData(this.Data.DataModel);
            loData.Set(this.MaxBaseIdIndexDataModel.Name, lsName);
            loData.Set(this.MaxBaseIdIndexDataModel.IndexId, loIndexId);
            MaxDataQuery loDataQuery = this.GetDataQuery();
            loDataQuery.StartGroup();
            loDataQuery.AddFilter(new MaxDataFilter(this.MaxBaseDataModel.IsActive, "=", true));
            loDataQuery.EndGroup();
            return this.LoadAllByPageCache(loData, 0, 0, this.MaxBaseDataModel.CreatedDate, loDataQuery);
        }
	}
}
