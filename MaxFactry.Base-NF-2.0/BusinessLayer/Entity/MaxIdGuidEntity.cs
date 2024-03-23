// <copyright file="MaxIdGuidEntity.cs" company="Lakstins Family, LLC">
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
// <change date="6/26/2014" author="Brian A. Lakstins" description="Copied from MaxIdEntity, rebased on MaxBaseEntity, removed AppId functionality.">
// <change date="11/10/2014" author="Brian A. Lakstins" description="Update to moving Get and Set methods from MaxData.">
// <change date="3/26/2015" author="Brian A. Lakstins" description="Moved to MaxFactry.Base">
// <change date="6/8/2015" author="Brian A. Lakstins" description="Added setting the Id to make updates not require loads.">
// <change date="1/10/2016" author="Brian A. Lakstins" description="Updated to meet .NET Micro Framework restrictions.">
// <change date="3/8/2016" author="Brian A. Lakstins" description="Updated properties to virtual so they can be overridden.">
// <change date="4/20/2016" author="Brian A. Lakstins" description="Centralize caching so LoadById always caches.">
// <change date="8/5/2016" author="Brian A. Lakstins" description="Updated to only automatically set ID when it is empty.">
// <change date="12/21/2016" author="Brian A. Lakstins" description="Updated blank cache to use a the base MaxDataModel instead of null.">
// <change date="5/23/2017" author="Brian A. Lakstins" description="Updated to cache MaxData instead of MaxEntity">
// <change date="10/24/2017" author="Brian A. Lakstins" description="Updated to clear loadbyid cache and renamed loadbyid to loadbyidcache">
// <change date="2/10/2019" author="Brian A. Lakstins" description="Add method to load by created date">
// <change date="12/29/2019" author="Brian A. Lakstins" description="Add loading from archive.">
// <change date="9/3/2020" author="Brian A. Lakstins" description="Add Default check process.">
// <change date="9/4/2020" author="Brian A. Lakstins" description="Remove Default check process - moved to MaxBaseIdEntity">
// <change date="1/16/2021" author="Brian A. Lakstins" description="Update definition of cache keys.">
// <change date="3/20/2024" author="Brian A. Lakstins" description="Happy birthday to my mom.  Sara Jean Lakstins (Cartwright) - 3/20/1944 to 3/14/2019.">
// <change date="3/22/2024" author="Brian A. Lakstins" description="Remove EntityPropertyKeyIndex because it will no longer be used.">
// <change date="3/23/2024" author="Brian A. Lakstins" description="Updated for change to dependency class.">
// </changelog>// </changelog>
#endregion

namespace MaxFactry.Base.BusinessLayer
{
    using System;
    using MaxFactry.Core;
    using MaxFactry.Base.DataLayer;

    /// <summary>
    /// Base Business Layer Entity
    /// </summary>
    public abstract class MaxIdGuidEntity : MaxEntity
    {
        private object _oLock = new object();

        /// <summary>
        /// Initializes a new instance of the MaxIdGuidEntity class
        /// </summary>
        /// <param name="loData">object to hold data</param>
        public MaxIdGuidEntity(MaxData loData)
            : base(loData)
        {
        }

        /// <summary>
        /// Initializes a new instance of the MaxIdGuidEntity class.
        /// </summary>
        /// <param name="loDataModelType">Type of data model.</param>
        public MaxIdGuidEntity(Type loDataModelType)
            : base(loDataModelType)
        {
        }

        /// <summary>
        /// Gets the unique Identifier for this entity
        /// </summary>
        public virtual Guid Id
        {
            get
            {
                return this.GetGuid(this.MaxIdDataModel.Id);
            }
        }

        /// <summary>
        /// Gets or sets the unique Identifier for this entity
        /// </summary>
        public virtual string AlternateId
        {
            get
            {
                return this.GetString(this.MaxIdDataModel.AlternateId);
            }

            set
            {
                this.Set(this.MaxIdDataModel.AlternateId, value);
            }
        }

        /// <summary>
        /// Gets the date the entity was created
        /// </summary>
        public virtual DateTime CreatedDate
        {
            get
            {
                return this.GetDateTime(this.MaxIdDataModel.CreatedDate);
            }
        }

        /// <summary>
        /// Gets the Data Model for this entity
        /// </summary>
        protected MaxIdGuidDataModel MaxIdDataModel
        {
            get
            {
                return (MaxIdGuidDataModel)MaxDataLibrary.GetDataModel(this.DataModelType);
            }
        }

        /// <summary>
        /// Loads an entity based on the Id
        /// </summary>
        /// <param name="loId">The Id of the entity to load</param>
        /// <returns>True if data was found, loaded, and not marked as deleted.  False could be not found, or deleted.</returns>
        public virtual bool LoadByIdCache(Guid loId)
        {
            string lsCacheIdDataKey = this.GetCacheKey() + "LoadById/" + loId.ToString();
            MaxData loData = MaxCacheRepository.Get(this.GetType(), lsCacheIdDataKey, typeof(MaxData)) as MaxData;
            if (null == loData)
            {
                loData = MaxIdGuidRepository.SelectById(this.Data, loId);
                if (null == loData)
                {
                    //// Try loading from archive
                    if (!this.Data.DataModel.DataStorageName.EndsWith("MaxArchive"))
                    {
                        MaxDataModel loDataModel = MaxFactry.Core.MaxFactryLibrary.Create(this.Data.DataModel.GetType(), this.Data.DataModel.DataStorageName + "MaxArchive") as MaxDataModel;
                        if (null != loDataModel)
                        {
                            MaxData loDataArchive = this.Data.Clone();
                            loData = MaxIdGuidRepository.SelectById(loDataArchive, loId);
                            if (null != loData)
                            {
                                MaxCacheRepository.Set(this.GetType(), lsCacheIdDataKey, loData);
                            }
                        }
                    }
                }
                else
                {
                    MaxCacheRepository.Set(this.GetType(), lsCacheIdDataKey, loData);
                }
            }

            if (this.Load(loData))
            {
                if (null == loData.Get(((MaxIdGuidDataModel)loData.DataModel).Id))
                {
                    return false;
                }
                
                return true;
            }

            this.Clear();
            MaxCacheRepository.Set(this.GetType(), lsCacheIdDataKey, this.Data.Clone());
            return false;
        }

        /// <summary>
        /// Loads all entities of this type that have not been marked as deleted and created since the date passed
        /// </summary>
        /// <param name="ldCreatedDate">Date to use to look up</param>
        /// <returns>List of entities</returns>
        public virtual MaxEntityList LoadAllSinceCreatedDate(DateTime ldCreatedDate)
        {
            //// Add a Query 
            MaxDataQuery loDataQuery = new MaxDataQuery();
            loDataQuery.StartGroup();
            loDataQuery.AddFilter(this.MaxIdDataModel.CreatedDate, ">", ldCreatedDate);
            loDataQuery.EndGroup();
            MaxDataList loDataList = MaxBaseIdRepository.Select(this.GetData(), loDataQuery, 0, 0, string.Empty);
            MaxEntityList loEntityList = MaxEntityList.Create(this.GetType(), loDataList);
            return loEntityList;
        }

        /// <summary>
        /// Sets the Id.
        /// </summary>
        /// <param name="loId">Id to use for this entity.</param>
        public virtual void SetId(Guid loId)
        {
            this.Set(this.MaxIdDataModel.Id, loId);
        }

        /// <summary>
        /// Inserts a new record
        /// </summary>
        /// <returns>true if inserted.  False if cannot be inserted.</returns>
        public override bool Insert()
        {
            int lnLimit = 10;
            int lnTry = 0;
            bool lbR = false;
            while (!lbR && lnTry < lnLimit)
            {
                lbR = this.Insert(Guid.NewGuid());
                lnTry++;
            }

            return lbR;
        }

        /// <summary>
        /// Inserts a new record
        /// </summary>
        /// <param name="loId">Id for the new record</param>
        /// <returns>true if inserted.  False if cannot be inserted.</returns>
        public virtual bool Insert(Guid loId)
        {
            if (Guid.Empty.Equals(this.Id))
            {
                this.Set(this.MaxIdDataModel.Id, loId);
            }

            this.Set(this.MaxIdDataModel.CreatedDate, DateTime.UtcNow);
            if (base.Insert())
            {
                string lsCacheKey = this.GetCacheKey() + "LoadById/" + MaxConvertLibrary.ConvertToString(typeof(object), this.Id);
                MaxCacheRepository.Set(this.GetType(), lsCacheKey, this.Data);
                lsCacheKey = this.GetCacheKey() + "LoadAll*";
                MaxCacheRepository.Remove(this.GetType(), lsCacheKey);
                return true;
            }

            return false;
        }

        /// <summary>
        /// Updates an existing record and removes record from Cache.
        /// </summary>
        /// <returns>true if updated.  False if cannot be updated.</returns>
        public override bool Update()
        {
            if (base.Update())
            {
                string lsCacheKey = this.GetCacheKey() + "LoadById/" + MaxConvertLibrary.ConvertToString(typeof(object), this.Id);
                MaxCacheRepository.Set(this.GetType(), lsCacheKey, this.Data);
                return true;
            }

            return false;
        }

        /// <summary>
        /// Marks a record as deleted and removes record from Cache.
        /// </summary>
        /// <returns>true if marked as deleted.  False if it cannot be deleted.</returns>
        public override bool Delete()
        {
            if (base.Delete())
            {
                string lsCacheKey = this.GetCacheKey() + "LoadById/" + MaxConvertLibrary.ConvertToString(typeof(object), this.Id);
                MaxCacheRepository.Remove(this.GetType(), lsCacheKey);
                return true;
            }

            return false;
        }
    }
}
