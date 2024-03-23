// <copyright file="MaxBaseIdIndexEntity.cs" company="Lakstins Family, LLC">
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
// </changelog>
#endregion

namespace MaxFactry.Base.BusinessLayer
{
	using System;
    using MaxFactry.Core;
    using MaxFactry.Base.BusinessLayer;
    using MaxFactry.Base.DataLayer;

	/// <summary>
	/// Base Business Layer Entity
	/// </summary>
	public abstract class MaxBaseIdIndexEntity : MaxBaseIdEntity
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
            if (!Guid.Empty.Equals(loIndexId))
            {
                MaxDataList loDataList = this.SelectAllByIndexIdCache(loIndexId);
                bool lbFound = false;
                if (loDataList.Count > 0)
                {
                    for (int lnD = 0; lnD < loDataList.Count; lnD++)
                    {
                        if (this.Load(loDataList[lnD]))
                        {
                            if (!this.IsDeleted && this.Name.Equals(lsName))
                            {
                                if (!this.IsActive || this.Value != lsValue)
                                {
                                    this.IsActive = true;
                                    this.Value = lsValue;
                                    this.Update();
                                }

                                lbFound = true;
                            }
                        }
                    }
                }

                if (!lbFound)
                {
                    this.Data.Clear();
                    this.IndexId = loIndexId;
                    this.Name = lsName;
                    this.Value = lsValue;
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
            if (!Guid.Empty.Equals(loIndexId))
            {
                MaxDataList loDataList = this.SelectAllByIndexIdCache(loIndexId);
                if (loDataList.Count > 0)
                {
                    lsR = string.Empty;
                    for (int lnD = 0; lnD < loDataList.Count; lnD++)
                    {
                        if (this.Load(loDataList[lnD]))
                        {
                            if (this.Name.ToLower() == lsName.ToLower())
                            {
                                return this.Value;
                            }
                        }
                    }
                }
            }

            return lsR;
        }

        /// <summary>
        /// Same as SelectAllByIndexId, but caches results.
        /// </summary>
        /// <param name="loIndexId">Id of Index to user for lookup</param>
        /// <returns>List of matching records</returns>
        protected MaxDataList SelectAllByIndexIdCache(Guid loIndexId)
        {
            this.IndexId = loIndexId;
            string lsCacheDataKey = this.GetCacheKey() + "LoadAllByIndexId/" + loIndexId;
            MaxDataList loDataList = MaxCacheRepository.Get(this.GetType(), lsCacheDataKey, typeof(MaxDataList)) as MaxDataList;
            if (null == loDataList)
            {
                loDataList = MaxBaseIdIndexRepository.SelectAllByIndexId(this.Data, loIndexId);
                MaxCacheRepository.Set(this.GetType(), lsCacheDataKey, loDataList);
            }

            return loDataList;
        }
	}
}
