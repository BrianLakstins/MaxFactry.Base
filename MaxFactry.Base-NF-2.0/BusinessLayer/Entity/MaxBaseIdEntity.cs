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
// <change date="3/20/2024" author="Brian A. Lakstins" description="Happy birthday to my mom.  Sara Jean Lakstins (Cartwright) - 3/20/1944 to 3/14/2019.">
// <change date="3/22/2024" author="Brian A. Lakstins" description="Change parent class.  Remove unused field.  Remove properties that are in parent class.  Add archive properties that were in previous parent class.  Remove methods that are in parent class.">
// <change date="3/24/2024" author="Brian A. Lakstins" description="Updated for changes namespaces">
// <change date="1/21/2025" author="Brian A. Lakstins" description="Change base class">
// </changelog>
#endregion

namespace MaxFactry.Base.BusinessLayer
{
    using System;
    using System.IO;
    using MaxFactry.Core;
    using MaxFactry.Base.DataLayer;
    using MaxFactry.Base.DataLayer.Library;

    /// <summary>
    /// Base Business Layer Entity.  Being deprecated in favor of MaxBaseGuidKeyEntity class which has only basic functionality.
    /// </summary>
    public abstract class MaxBaseIdEntity : MaxBaseGuidKeyEntity
    {
        /// <summary>
        /// Lock for thread safety
        /// </summary>
        private static object _oLock = new object();

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
        /// Gets or sets the unique Identifier for this entity
        /// </summary>
        public virtual string AlternateId
        {
            get
            {
                return this.GetString(this.MaxBaseIdDataModel.AlternateId);
            }

            set
            {
                this.Set(this.MaxBaseIdDataModel.AlternateId, value);
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
        /// Gets or sets the last date this entity was archived
        /// </summary>
        protected DateTime ArchiveDate
        {
            get
            {
                DateTime ldR = DateTime.MinValue;
                string lsCacheKey = this.GetCacheKey() + "Archive";
                object ldArchive = MaxCacheRepository.Get(typeof(object), lsCacheKey, typeof(DateTime));
                if (null != ldArchive && ldArchive is DateTime)
                {
                    ldR = (DateTime)ldArchive;
                }
                else
                {
                    ldArchive = MaxConfigurationLibrary.GetValue(MaxEnumGroup.ScopePersistent, lsCacheKey);
                    if (null != ldArchive)
                    {
                        ldR = MaxConvertLibrary.ConvertToDateTimeUtc(typeof(object), ldArchive);
                    }
                }

                return ldR;
            }

            set
            {
                string lsCacheKey = this.GetCacheKey() + "Archive";
                MaxCacheRepository.Set(typeof(object), lsCacheKey, value);
                MaxConfigurationLibrary.SetValue(MaxEnumGroup.ScopePersistent, lsCacheKey, value);
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

        public override MaxEntityList LoadAllCache(params string[] laPropertyNameList)
        {
            MaxEntityList loR = base.LoadAllCache(laPropertyNameList);
            if (this.CheckDefaults(loR))
            {
                loR = base.LoadAllCache(laPropertyNameList);
            }

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
        /// Checks to see if the entity was archived recently
        /// </summary>
        /// <param name="loArchiveFrequency"></param>
        /// <returns></returns>
        protected virtual bool CanProcessArchive(TimeSpan loArchiveFrequency)
        {
            bool lbR = false;
            DateTime ldNow = DateTime.UtcNow;
            if (this.ArchiveDate < ldNow - loArchiveFrequency)
            {
                lock (_oLock)
                {
                    if (this.ArchiveDate < ldNow - loArchiveFrequency)
                    {
                        this.ArchiveDate = ldNow;
                        lbR = true;
                    }
                }
            }

            return lbR;
        }

        /// <summary>
        /// Creates a record in another data storage location and deletes this one from storage.
        /// </summary>
        /// <returns>true if archive process ran</returns>
        public virtual bool Archive()
        {
            bool lbR = false;
            string lsDataStorageName = this.Data.DataModel.DataStorageName;
            if (!lsDataStorageName.EndsWith("MaxArchive"))
            {
                this.SetProperties();
                //// Not all data models support archiving.  They need a creation method that takes a new storage name that can be used as the Archive name.
                MaxDataModel loDataModel = MaxFactry.Core.MaxFactryLibrary.Create(this.Data.DataModel.GetType(), lsDataStorageName + "MaxArchive") as MaxDataModel;
                if (null != loDataModel)
                {
                    lock (_oLock)
                    {
                        MaxData loDataArchive = this.Data.Clone();
                        try
                        {
                            bool lbDelete = false;
                            if (MaxBaseWriteRepository.Insert(loDataArchive))
                            {
                                lbDelete = true;
                            }
                            else
                            {
                                if (MaxBaseWriteRepository.Delete(loDataArchive))
                                {
                                    if (MaxBaseWriteRepository.Insert(loDataArchive))
                                    {
                                        lbDelete = true;
                                    }
                                }
                            }

                            if (lbDelete)
                            {
                                if (MaxBaseWriteRepository.Delete(this.Data))
                                {
                                    string lsCacheKey = this.GetCacheKey() + "Load*";
                                    MaxCacheRepository.Remove(this.GetType(), lsCacheKey);
                                    lbR = true;
                                }
                            }
                        }
                        catch (Exception loE)
                        {
                            MaxLogLibrary.Log(new MaxLogEntryStructure("EntityArchive", MaxEnumGroup.LogError, "Error archiving {Type}", loE, this.GetType()));
                        }
                    }
                }
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

        /// Sets the datamodel properties for any extended data
        /// </summary>
        protected override void SetProperties()
        {
            string[] laExtendedKey = this.Data.GetExtendedNameList();
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
