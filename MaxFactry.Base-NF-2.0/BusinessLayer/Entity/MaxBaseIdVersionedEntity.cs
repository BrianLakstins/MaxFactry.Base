// <copyright file="MaxBaseIdVersionedEntity.cs" company="Lakstins Family, LLC">
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
// <change date="11/10/2014" author="Brian A. Lakstins" description="Initial Release">
// <change date="12/2/2014" author="Brian A. Lakstins" description="Add caching of current item.">
// <change date="3/26/2015" author="Brian A. Lakstins" description="Moved to MaxFactry.Base">
// <change date="6/8/2015" author="Brian A. Lakstins" description="Updated cache usage.">
// <change date="4/20/2016" author="Brian A. Lakstins" description="Updated to use centralized caching.">
// <change date="4/20/2016" author="Brian A. Lakstins" description="Fix returning null from GetCurrent.">
// <change date="7/12/2016" author="Brian A. Lakstins" description="Separate out code to get next version.">
// <change date="1/16/2021" author="Brian A. Lakstins" description="Update definition of cache keys.">
// <change date="3/20/2024" author="Brian A. Lakstins" description="Happy birthday to my mom.  Sara Jean Lakstins (Cartwright) - 3/20/1944 to 3/14/2019.">
// <change date="3/23/2024" author="Brian A. Lakstins" description="Updated for change to dependency class.">
// <change date="3/24/2024" author="Brian A. Lakstins" description="Updated for changes namespaces">
// <change date="3/22/2025" author="Brian A. Lakstins" description="Don't make active when inserting.">
// <change date="4/9/2025" author="Brian A. Lakstins" description="Override SetInitial method instead of altering insert.">
// </changelog>
#endregion

namespace MaxFactry.Base.BusinessLayer
{
    using System;
    using MaxFactry.Core;
    using MaxFactry.Base.DataLayer;
    using MaxFactry.Base.DataLayer.Library;

    /// <summary>
    /// Base entity for interacting with files.
    /// </summary>
    public abstract class MaxBaseIdVersionedEntity : MaxBaseGuidKeyEntity
    {
        private static object _oLock = new object();

		/// <summary>
        /// Initializes a new instance of the MaxBaseIdVersionedEntity class
		/// </summary>
		/// <param name="loData">object to hold data</param>
        public MaxBaseIdVersionedEntity(MaxFactry.Base.DataLayer.MaxData loData)
            : base(loData)
		{
		}

        /// <summary>
        /// Initializes a new instance of the MaxBaseIdVersionedEntity class.
        /// </summary>
        /// <param name="loDataModelType">Type of data model.</param>
        public MaxBaseIdVersionedEntity(Type loDataModelType)
            : base(loDataModelType)
        {
        }

        /// <summary>
        /// Gets or sets the name of the versioned information.
        /// </summary>
        public string Name
        {
            get
            {
                return this.GetString(this.MaxBaseIdVersionedDataModel.Name);
            }

            set
            {
                this.Set(this.MaxBaseIdVersionedDataModel.Name, value);
            }
        }

        /// <summary>
        /// Gets the version
        /// </summary>
        public int Version
        {
            get
            {
                return this.GetInt(this.MaxBaseIdVersionedDataModel.Version);
            }
        }

        /// <summary>
        /// Gets the Data Model for this entity
        /// </summary>
        protected MaxBaseIdVersionedDataModel MaxBaseIdVersionedDataModel
        {
            get
            {
                return (MaxBaseIdVersionedDataModel)MaxDataLibrary.GetDataModel(this.DataModelType);
            }
        }

        /// <summary>
        /// Gets the current entity for the virtual path.
        /// </summary>
        /// <param name="lsName">Key used for the versioned information.</param>
        /// <returns>Current entity.</returns>
        public virtual MaxBaseIdVersionedEntity GetCurrent(string lsName)
        {
            string lsCacheDataKey = this.GetCacheKey() + "LoadAllByNameCurrent/" + lsName;
            MaxData loData = MaxCacheRepository.Get(this.GetType(), lsCacheDataKey, typeof(MaxData)) as MaxData;
            if (this.Load(loData))
            {
                return this;
            }

            MaxDataQuery loDataQuery = this.GetDataQuery();
            loDataQuery.StartGroup();
            loDataQuery.AddFilter(new MaxDataFilter(this.MaxBaseIdVersionedDataModel.Name, "=", lsName));
            loDataQuery.AddAnd();
            loDataQuery.AddFilter(new MaxDataFilter(this.MaxBaseIdVersionedDataModel.IsActive, "=", true));
            loDataQuery.EndGroup();

            MaxDataList loDataList = MaxBaseReadRepository.Select(this.Data, loDataQuery, 0, 0, string.Empty);
            if (loDataList.Count.Equals(0))
            {
                loDataQuery = this.GetDataQuery();
                loDataQuery.StartGroup();
                loDataQuery.AddFilter(new MaxDataFilter(this.MaxBaseIdVersionedDataModel.Name, "=", lsName));
                loDataQuery.EndGroup();
                loDataList = MaxBaseReadRepository.Select(this.Data, loDataQuery, 0, 0, string.Empty);
            }

            int lnVersionCurrent = -1;
            for (int lnD = 0; lnD < loDataList.Count; lnD++)
            {
                if (this.Load(loDataList[lnD]) && this.Version > lnVersionCurrent)
                {
                    lnVersionCurrent = this.Version;
                }
            }

            if (lnVersionCurrent >= 0)
            {
                // Mark any that are not the latest version as inactive to speed up future requests.
                for (int lnD = 0; lnD < loDataList.Count; lnD++)
                {
                    if (this.Load(loDataList[lnD]) && this.IsActive && this.Version != lnVersionCurrent)
                    {
                        this.IsActive = false;
                        this.Update();
                    }
                }

                for (int lnD = 0; lnD < loDataList.Count; lnD++)
                {
                    if (this.Load(loDataList[lnD]) && this.Version == lnVersionCurrent)
                    {
                        if (!this.IsActive)
                        {
                            this.IsActive = true;
                            this.Update();
                        }

                        MaxCacheRepository.Set(this.GetType(), lsCacheDataKey, this.Data);
                        return this;
                    }
                }
            }

            this.Clear();
            MaxCacheRepository.Set(this.GetType(), lsCacheDataKey, this.Data.Clone());
            return this;
        }

        /// <summary>
        /// Gets the next version based on however the data is stored.
        /// </summary>
        /// <returns>integer to use for next version</returns>
        public virtual int GetNextVersion()
        {
            int lnR = 0;
            lock (_oLock)
            {
                MaxDataQuery loDataQuery = this.GetDataQuery();
                loDataQuery.StartGroup();
                loDataQuery.AddFilter(new MaxDataFilter(this.MaxBaseIdVersionedDataModel.Name, "=", this.Name));
                loDataQuery.EndGroup();
                MaxDataList loDataList = MaxBaseReadRepository.Select(this.Data, loDataQuery, 0, 0, string.Empty);
                //// Get the largest version number of any entity regardless of active or deleted.
                int lnVersionCurrent = 0;
                for (int lnD = 0; lnD < loDataList.Count; lnD++)
                {
                    int lnVersion = MaxConvertLibrary.ConvertToInt(typeof(object), loDataList[lnD].Get(this.MaxBaseIdVersionedDataModel.Version));
                    if (lnVersion > lnVersionCurrent)
                    {
                        lnVersionCurrent = lnVersion;
                    }
                }

                lnR = lnVersionCurrent + 1;
            }

            return lnR;
        }

        protected override void SetInitial()
        {
            base.SetInitial();
            this.Set(this.MaxBaseIdVersionedDataModel.Version, this.GetNextVersion());
        }
    }
}
