// <copyright file="MaxBaseIdEntity.cs" company="Lakstins Family, LLC">
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
// <change date="6/26/2014" author="Brian A. Lakstins" description="Copied from MaxCRUDEntity.">
// <change date="6/27/2014" author="Brian A. Lakstins" description="Fix SelectAll to use empty MaxData.">
// <change date="7/3/2014" author="Brian A. Lakstins" description="Add support for managing stream data.">
// <change date="7/18/2014" author="Brian A. Lakstins" description="Added methods to serialize and deserialized properties as they are saved.">
// <change date="11/04/2014" author="Brian A. Lakstins" description="Added support for base option flag property.">
// <change date="11/10/2014" author="Brian A. Lakstins" description="Update to moving Get and Set methods from MaxData.">
// <change date="11/27/2014" author="Brian A. Lakstins" description="Fix to delete method.">
// <change date="12/2/2014" author="Brian A. Lakstins" description="Update delete to just mark deleted.  Add RemoveFromStorage method to permanently delete a record.">
// <change date="12/18/2014" author="Brian A. Lakstins" description="Update for change to repository to only select non-deleted items.">
// <change date="1/14/2015" author="Brian A. Lakstins" description="Add ability to set the storage key.  Move LoadAll to base class.">
// <change date="3/26/2015" author="Brian A. Lakstins" description="Moved to MaxFactry.Base">
// <change date="5/5/2015" author="Brian A. Lakstins" description="Fix to require Id to be set to update or remove from storage.">
// <change date="1/10/2016" author="Brian A. Lakstins" description="Better handling of updates and objects when data is changed.">
// <change date="7/18/2016" author="Brian A. Lakstins" description="Fix sorting by created date to include milliseconds.">
// <change date="12/21/2016" author="Brian A. Lakstins" description="Updated to set OptionFlagList to zero if not set.">
// <change date="4/20/2017" author="Brian A. Lakstins" description="Add central code for setting properties.">
// <change date="6/21/2017" author="Brian A. Lakstins" description="Handle using Extension when the key does not match the datamodel.">
// <change date="7/6/2017" author="Brian A. Lakstins" description="Use MaxData internal extension ability to handle storage of Extension property and still allow for 'temp' properties that don't get saved.  Fix for load.">
// <change date="9/1/2017" author="Brian A. Lakstins" description="Fix error updating when only extended properties have changed.">
// <change date="2/16/2018" author="Brian A. Lakstins" description="Update extension update so it won't update if not changed and won't store extention fields that start with an underscore">
// <change date="11/29/2018" author="Brian A. Lakstins" description="Added AttributeIndex property">
// <change date="2/8/2019" author="Brian A. Lakstins" description="Add some events">
// <change date="2/10/2019" author="Brian A. Lakstins" description="Add method to load by last update date">
// <change date="12/3/2019" author="Brian A. Lakstins" description="Add method for archive process">
// <change date="12/11/2019" author="Brian A. Lakstins" description="Prevent loading of temporary extended properties.">
// <change date="5/19/2020" author="Brian A. Lakstins" description="Update log message.">
// <change date="5/20/2020" author="Brian A. Lakstins" description="Update logging.">
// <change date="5/21/2020" author="Brian A. Lakstins" description="Update archive logging to only debug level when it's not applicable to the entity.">
// <change date="5/31/2020" author="Brian A. Lakstins" description="Move archive methods to base class.">
// <change date="8/3/2020" author="Brian A. Lakstins" description="Remove StopWatch because that level of precision is not needed.">
// <change date="8/13/2020" author="Brian A. Lakstins" description="Move SetObject and GetObject functionality to MaxEntity">
// <change date="9/4/2020" author="Brian A. Lakstins" description="Add default checking.  Fix issue with case on matching Guid to string">
// <change date="1/16/2021" author="Brian A. Lakstins" description="Update definition of cache keys.">
// <change date="1/18/2021" author="Brian A. Lakstins" description="Remove deleted items from cache.">
// <change date="3/1/2021" author="Brian A. Lakstins" description="Turn off archive because it requires loading all.">
// <change date="4/12/2021" author="Brian A. Lakstins" description="Fix loading from cache when cache is based on property.">
// </changelog>
#endregion

namespace MaxFactry.Base.BusinessLayer
{
    using System;
    using System.IO;
    using MaxFactry.Core;
    using MaxFactry.Base.BusinessLayer;
    using MaxFactry.Base.DataLayer;
    using System.Diagnostics;

    /// <summary>
    /// Base Business Layer Entity
    /// </summary>
    public abstract class MaxBaseIdEntity : MaxIdGuidEntity
    {
        /// <summary>
        /// Lock for thread safety
        /// </summary>
        private static object _oLock = new object();

        /// <summary>
        /// Index of datamodel keys 
        /// </summary>
        private MaxIndex _oKeyList = null;

        /// <summary>
        /// Initializes a new instance of the MaxBaseIdEntity class
        /// </summary>
        /// <param name="loData">object to hold data</param>
        public MaxBaseIdEntity(MaxData loData) : base(loData)
        {
        }

        /// <summary>
        /// Initializes a new instance of the MaxBaseIdEntity class.
        /// </summary>
        /// <param name="loDataModelType">Type of data model.</param>
        public MaxBaseIdEntity(Type loDataModelType)
            : base(loDataModelType)
        {
        }

        /// <summary>
        /// Gets the date the entity was last updated
        /// </summary>
        public virtual DateTime LastUpdateDate
        {
            get
            {
                return this.GetDateTime(this.MaxBaseIdDataModel.LastUpdateDate);
            }
        }

        /// <summary>
        /// Gets a value indicating whether this is deleted
        /// </summary>
        public virtual bool IsDeleted
        {
            get
            {
                return this.GetBoolean(this.MaxBaseIdDataModel.IsDeleted);
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether this is active
        /// </summary>
        public virtual bool IsActive
        {
            get
            {
                return this.GetBoolean(this.MaxBaseIdDataModel.IsActive);
            }

            set
            {
                this.Set(this.MaxBaseIdDataModel.IsActive, value);
            }
        }

        public virtual bool IsDefault
        {
            get
            {
                bool lbR = false;
                if (Guid.Empty != this.Id)
                {
                    string lsId = this.Id.ToString("B").ToUpper();
                    MaxIndex loDefaultIndex = this.GetDefaultIdIndex();
                    if (loDefaultIndex.Contains(lsId))
                    {
                        lbR = true;
                    }
                }

                return lbR;
            }
        }

        /// <summary>
        /// Gets or sets an index of names and values that can be used in any way
        /// </summary>
        public virtual MaxIndex AttributeIndex
        {
            get
            {
                MaxIndex loR = this.GetObject(this.MaxBaseIdDataModel.AttributeIndex, typeof(MaxIndex)) as MaxIndex;
                if (null != loR)
                {
                    return loR;
                }

                loR = new MaxIndex();
                this.SetObject(this.MaxBaseIdDataModel.AttributeIndex, loR);
                return loR;
            }

            set
            {
                this.SetObject(this.MaxBaseIdDataModel.AttributeIndex, value);
            }
        }

        /// <summary>
        /// Gets the Data Model for this entity
        /// </summary>
        protected MaxBaseIdDataModel MaxBaseIdDataModel
        {
            get
            {
                return (MaxBaseIdDataModel)MaxDataLibrary.GetDataModel(this.DataModelType);
            }
        }

        /// <summary>
        /// Default is to sort by created date.
        /// </summary>
        /// <returns>Sortable created date.</returns>
        public override string GetDefaultSortString()
        {
            return MaxConvertLibrary.ConvertToSortString(typeof(object), this.CreatedDate) + base.GetDefaultSortString();
        }

        public override MaxEntityList LoadAllCache(params string[] laPropertyNameList)
        {
            MaxEntityList loR = base.LoadAllCache(laPropertyNameList);
            if (this.CheckDefaults(loR))
            {
                loR = base.LoadAllCache(laPropertyNameList);
            }

            return loR;
        }

        public virtual MaxEntityList LoadAllActiveCache(params string[] laPropertyNameList)
        {
            MaxEntityList loR = base.LoadAllByPropertyCache(this.MaxBaseIdDataModel.IsActive, true, laPropertyNameList);
            return loR;
        }

        /// <summary>
        /// Loads the Data for this entity
        /// </summary>
        /// <param name="loData">Data for the entity</param>
        /// <returns>True if loaded, false if not loaded</returns>
        public override bool Load(MaxData loData)
        {
            bool lbR = base.Load(loData);
            if (lbR)
            {
                if (MaxDataLibrary.GetDataModel(this.DataModelType) is MaxBaseIdDataModel)
                {
                    object loExtension = this.Get(this.MaxBaseIdDataModel.Extension);
                    if (null != loExtension && loExtension is byte[])
                    {
                        MaxIndex loExtensionIndex = MaxConvertLibrary.DeserializeObject(this.Data.DataModel.GetType(), (byte[])loExtension, typeof(MaxIndex)) as MaxIndex;
                        if (null != loExtensionIndex)
                        {
                            foreach (string lsExtendedKey in loExtensionIndex.GetSortedKeyList())
                            {
                                bool lbIsStored = true;
                                if (lsExtendedKey.Length > 1)
                                {
                                    if (lsExtendedKey.Substring(0, 1) == "_")
                                    {
                                        lbIsStored = false;
                                    }
                                    else if (lsExtendedKey.Length > 4)
                                    {
                                        if (lsExtendedKey.Substring(0, 4) == "temp")
                                        {
                                            lbIsStored = false;
                                        }
                                    }
                                }

                                if (lbIsStored)
                                {
                                    this.Data.Set(lsExtendedKey, loExtensionIndex[lsExtendedKey]);
                                }
                            }
                        }
                    }
                }
            }

            return lbR;
        }

        /// <summary>
        /// Gets a value if the option flag has been set.
        /// </summary>
        /// <param name="lnOption">Option flag from 0 to 63</param>
        /// <returns>value indicating whether the flag is set or not</returns>
        public bool IsOptionFlagSet(short lnOption)
        {
            return this.GetBit(this.MaxBaseIdDataModel.OptionFlagList, lnOption);
        }

        /// <summary>
        /// Sets the option to the Value parameter
        /// </summary>
        /// <param name="lnOption">Option flag from 0 to 63</param>
        /// <param name="lbValue">Value to set the option.</param>
        public void SetOptionFlag(short lnOption, bool lbValue)
        {
            this.SetBit(this.MaxBaseIdDataModel.OptionFlagList, lnOption, lbValue);
        }

        /// <summary>
        /// Inserts a new record
        /// </summary>
        /// <param name="loId">Id for the new record</param>
        /// <returns>true if inserted.  False if cannot be inserted.</returns>
        public override bool Insert(Guid loId)
        {
            this.Set(this.MaxBaseIdDataModel.LastUpdateDate, DateTime.UtcNow);
            return base.Insert(loId);
        }

        /// <summary>
        /// Updates an existing record
        /// </summary>
        /// <returns>true if updated.  False if cannot be updated.</returns>
        public override bool Update()
        {
            OnUpdateBefore();
            if (Guid.Empty.Equals(this.Id))
            {
                throw new MaxException("The Id is empty.  A MaxBaseIdEntity cannot be updated with an empty Id.");
            }

            this.SetProperties();
            if (this.IsDataChanged)
            {
                this.Set(this.MaxBaseIdDataModel.LastUpdateDate, DateTime.UtcNow);
                MaxIndex loPropertyChangedIndex = new MaxIndex();
                string[] laKey = this.Data.DataModel.GetKeyList();
                foreach (string lsKey in laKey)
                {
                    if (this.Data.GetIsChanged(lsKey))
                    {
                        loPropertyChangedIndex.Add(lsKey);
                    }
                }

                if (MaxStorageWriteRepository.Update(this.Data))
                {
                    string lsCacheKey = this.GetCacheKey() + "LoadAll*";
                    MaxCacheRepository.Remove(this.GetType(), lsCacheKey);
                    lsCacheKey = this.GetCacheKey() + "LoadById/" + this.Id;
                    MaxCacheRepository.Set(this.GetType(), lsCacheKey, this.Data);
                    OnUpdateAfter();
                    return true;
                }
            }

            OnUpdateFail();
            return false;
        }

        /// <summary>
        /// Marks a record as deleted.
        /// </summary>
        /// <returns>true if marked as deleted.  False if it cannot be deleted.</returns>
        public override bool Delete()
        {
            OnDeleteBefore();
            this.Set(this.MaxBaseIdDataModel.IsDeleted, true);
            bool lbR = this.Update();
            if (lbR)
            {
                string lsCacheKey = this.GetCacheKey() + "LoadById/" + this.Id;
                MaxCacheRepository.Remove(this.GetType(), lsCacheKey);
                this.OnDeleteAfter();
            }
            else
            {
                this.OnDeleteFail();
            }

            return lbR;
        }

        /// <summary>
        /// Deletes an existing record and deleted any related streams.
        /// </summary>
        /// <returns>true if deleted.  False if it cannot be deleted.</returns>
        public virtual bool RemoveFromStorage()
        {
            if (!Guid.Empty.Equals(this.Id))
            {
                return base.Delete();
            }

            return false;
        }

        /// <summary>
        /// Creates a record in another data storage location and deletes this one from storage.
        /// </summary>
        /// <returns></returns>
        public override bool Archive()
        {
            bool lbR = false;
            try
            {
                lbR = base.Archive();
                if (!lbR)
                {
                    MaxLogLibrary.Log(new MaxLogEntryStructure("EntityArchive", MaxEnumGroup.LogDebug, "{this.GetType()} for {this.Id} is not coded to be archived. {this.Data.DataModel.DataStorageName}", this.GetType(), this.Id, this.Data.DataModel.DataStorageName));
                }
            }
            catch (Exception loE)
            {
                MaxLogLibrary.Log(new MaxLogEntryStructure("EntityArchive", MaxEnumGroup.LogError, "Error archiving {Type} {Id}", loE, this.GetType(), this.Id));
            }

            return lbR;
        }

        public virtual int Archive(DateTime ldCreatedDate, DateTime ldLastUpdatedDate, bool lbInactiveOnly)
        {
            int lnR = 0;
            DateTime ldStart = DateTime.UtcNow;
            /*
            MaxEntityList loEntityList = this.LoadAll();
            MaxLogLibrary.Log(new MaxLogEntryStructure("EntityArchive-" + this.GetType() + "-Start", MaxEnumGroup.LogInfo, "Archiving from {ldCreatedDate} to {ldLastUpdatedDate}.  Checking {loEntityList.Count} items.", ldCreatedDate, ldLastUpdatedDate, loEntityList.Count));
            for (int lnE = 0; lnE < loEntityList.Count; lnE++)
            {
                MaxBaseIdEntity loEntity = loEntityList[lnE] as MaxBaseIdEntity;
                if (loEntity.CreatedDate < ldCreatedDate || loEntity.LastUpdateDate < ldLastUpdatedDate)
                {
                    if (!lbInactiveOnly || (lbInactiveOnly && !loEntity.IsActive))
                    {
                        if (loEntity.Archive())
                        {
                            lnR++;
                        }
                    }
                }
            }
            */

            MaxLogLibrary.Log(new MaxLogEntryStructure("EntityArchive-" + this.GetType() + "-End", MaxEnumGroup.LogInfo, "Archived {Count} in {Seconds}", lnR, (DateTime.UtcNow - ldStart).TotalSeconds));
            return lnR;
        }

        /// <summary>
        /// Loads all entities of this type that have not been marked as deleted and updated since the date passed
        /// <param name="ldLastUpdate">Date to use to look up</param>
        /// <returns>List of entities</returns>
        public virtual MaxEntityList LoadAllSinceLastUpdateDate(DateTime ldLastUpdate, params string[] laPropertyNameList)
        {
            //// Add a Query 
            MaxDataQuery loDataQuery = new MaxDataQuery();
            loDataQuery.StartGroup();
            loDataQuery.AddFilter(this.MaxBaseIdDataModel.LastUpdateDate, ">", ldLastUpdate);
            loDataQuery.EndGroup();
            int lnTotal = 0;

            string[] laDataNameList = this.GetDataNameList(this.Data.DataModel, laPropertyNameList);
            MaxDataList loDataList = MaxBaseIdRepository.Select(this.GetData(), loDataQuery, 0, 0, string.Empty, out lnTotal, laDataNameList);
            MaxEntityList loEntityList = MaxEntityList.Create(this.GetType(), loDataList);
            loEntityList.Total = lnTotal;
            return loEntityList;
        }

        protected MaxIndex KeyList
        {
            get
            {
                if (null == this._oKeyList)
                {
                    this._oKeyList = new MaxIndex();
                    string[] laKey = this.Data.DataModel.GetKeyList();
                    foreach (string lsKey in laKey)
                    {
                        this._oKeyList.Add(lsKey, true);
                    }

                    this._oKeyList.Add(this.Data.DataModel.StorageKey, true);
                }

                return this._oKeyList;
            }
        }

        /// <summary>
        /// Sets the datamodel properties for any extended data
        /// </summary>
        protected override void SetProperties()
        {
            if (null == this.Get(this.MaxBaseIdDataModel.IsDeleted))
            {
                this.Set(this.MaxBaseIdDataModel.IsDeleted, false);
            }

            if (null == this.Get(this.MaxBaseIdDataModel.IsActive))
            {
                this.Set(this.MaxBaseIdDataModel.IsActive, false);
            }

            if (this.GetLong(this.MaxBaseIdDataModel.OptionFlagList) < 0)
            {
                this.Set(this.MaxBaseIdDataModel.OptionFlagList, 0);
            }

            string[] laExtendedKey = this.Data.GetExtendedKeyList();
            if (laExtendedKey.Length > 0 && MaxDataLibrary.GetDataModel(this.DataModelType) is MaxBaseIdDataModel)
            {
                bool lbIsChanged = false;
                MaxIndex loExtensionIndex = MaxConvertLibrary.DeserializeObject(this.GetByte(this.MaxBaseIdDataModel.Extension), typeof(MaxIndex)) as MaxIndex;
                if (null == loExtensionIndex)
                {
                    loExtensionIndex = new MaxIndex();
                }

                foreach (string lsExtendedKey in laExtendedKey)
                {
                    bool lbIsStored = true;
                    if (lsExtendedKey.Length > 1)
                    {
                        if (lsExtendedKey.Substring(0, 1) == "_")
                        {
                            lbIsStored = false;
                        }
                        else if (lsExtendedKey.Length > 4)
                        {
                            if (lsExtendedKey.Substring(0, 4) == "temp")
                            {
                                lbIsStored = false;
                            }
                        }
                    }

                    if (lbIsStored)
                    {
                        object loValue = this.Data.Get(lsExtendedKey);
                        if (!(loValue is Stream) && !(loValue is byte[]))
                        {
                            if (null == loExtensionIndex[lsExtendedKey] || null == loValue)
                            {
                                loExtensionIndex.Add(lsExtendedKey, loValue);
                                lbIsChanged = true;
                            }
                            else if (!loExtensionIndex[lsExtendedKey].Equals(loValue))
                            {
                                loExtensionIndex.Add(lsExtendedKey, loValue);
                                lbIsChanged = true;
                            }
                        }
                    }
                }

                if (lbIsChanged)
                {
                    this.Set(this.MaxBaseIdDataModel.Extension, MaxConvertLibrary.SerializeObjectToBinary(this.Data.DataModel.GetType(), loExtensionIndex));
                }
            }

            base.SetProperties();
        }


        protected virtual bool CheckDefaults(MaxEntityList loEntityList)
        {
            bool lbR = false;
            //// Check defaults once per application run
            string lsCacheKey = this.GetCacheKey() + "CheckDefaults";
            object loIsChecked = MaxCacheRepository.Get(this.GetType(), lsCacheKey, typeof(bool));
            if (null == loIsChecked || (loIsChecked is bool && !(bool)loIsChecked))
            {
                lock (_oLock)
                {
                    loIsChecked = MaxCacheRepository.Get(this.GetType(), lsCacheKey, typeof(bool));
                    if (null == loIsChecked || (loIsChecked is bool && !(bool)loIsChecked))
                    {
                        MaxCacheRepository.Set(this.GetType(), lsCacheKey, true);
                        MaxIndex loDefaultIndex = this.GetDefaultIdIndex();
                        for (int lnE = 0; lnE < loEntityList.Count; lnE++)
                        {
                            MaxBaseIdEntity loEntity = loEntityList[lnE] as MaxBaseIdEntity;
                            string lsId = loEntity.Id.ToString("B").ToUpper();
                            if (loDefaultIndex.Contains(lsId))
                            {
                                DateTime ldUpdate = MaxConvertLibrary.ConvertToDateTime(typeof(object), loDefaultIndex[lsId]);
                                if (ldUpdate < loEntity.LastUpdateDate)
                                {
                                    //// This entity is current with the default
                                    loDefaultIndex[lsId] = DateTime.MaxValue;
                                }
                                else
                                {
                                    //// This entity exists, but needs to be updated with the latest default
                                    loDefaultIndex[lsId] = DateTime.MinValue;
                                }
                            }
                        }

                        string[] laKey = loDefaultIndex.GetSortedKeyList();
                        foreach (string lsId in laKey)
                        {
                            DateTime ldUpdate = MaxConvertLibrary.ConvertToDateTime(typeof(object), loDefaultIndex[lsId]);
                            if (ldUpdate == DateTime.MinValue)
                            {
                                lbR = true;
                                this.UpdateDefault(lsId);
                            }
                            else if (ldUpdate < DateTime.UtcNow)
                            {
                                lbR = true;
                                this.AddDefault(lsId);
                            }
                        }
                    }
                }
            }

            return lbR;
        }

        /// <summary>
        /// Keys are strings in Guid Registry format {00000000-0000-0000-0000-000000000000}
        /// Values are dates the the default was last changed code.
        /// </summary>
        /// <returns></returns>
        protected virtual MaxIndex GetDefaultIdIndex()
        {
            MaxIndex loR = new MaxIndex();
            return loR;
        }

        protected virtual void AddDefault(string lsId)
        {
        }

        protected virtual void UpdateDefault(string lsId)
        {
        }
    }
}
