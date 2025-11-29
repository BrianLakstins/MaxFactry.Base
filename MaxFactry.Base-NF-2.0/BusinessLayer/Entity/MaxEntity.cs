// <copyright file="MaxEntity.cs" company="Lakstins Family, LLC">
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
// <change date="3/17/2014" author="Brian A. Lakstins" description="Initial Release">
// <change date="6/24/2014" author="Brian A. Lakstins" description="Add a method for getting sorting information.">
// <change date="6/26/2014" author="Brian A. Lakstins" description="Update default sorting method for MF43.">
// <change date="11/10/2014" author="Brian A. Lakstins" description="Move type specific get and set from MaxData in Daya Layer">
// <change date="11/11/2014" author="Brian A. Lakstins" description="Added getting copy of data.  Made Set protected.">
// <change date="11/27/2014" author="Brian A. Lakstins" description="Fix to string encryption functionality.">
// <change date="1/14/2015" author="Brian A. Lakstins" description="Add ability to override providers.">
// <change date="2/24/2015" author="Brian A. Lakstins" description="Added LoadAll method.">
// <change date="3/26/2015" author="Brian A. Lakstins" description="Moved to MaxFactry.Base">
// <change date="6/1/2015" author="Brian A. Lakstins" description="Allow retrieving storage key.">
// <change date="6/4/2015" author="Brian A. Lakstins" description="Adding ability to easily cache all entities.">
// <change date="6/8/2015" author="Brian A. Lakstins" description="Updated to include storage key in cached data.">
// <change date="1/6/2016" author="Brian A. Lakstins" description="Process each property like it might need stored in a stream.">
// <change date="3/8/2016" author="Brian A. Lakstins" description="Allow string to be cleared without being loaded first.">
// <change date="4/20/2016" author="Brian A. Lakstins" description="Centralize caching configuration.">
// <change date="5/10/2016" author="Brian A. Lakstins" description="Add ability to specify repository provider name.">
// <change date="7/14/2016" author="Brian A. Lakstins" description="Add reset method.">
// <change date="8/5/2016" author="Brian A. Lakstins" description="Made some methods virtual.">
// <change date="12/21/2016" author="Brian A. Lakstins" description="Updated data to lazy load and set the current storage key.">
// <change date="12/21/2016" author="Brian A. Lakstins" description="Updating getting a string or byte[] to also be able to get it from a stream.">
// <change date="4/20/2017" author="Brian A. Lakstins" description="Add base method for loading records from cache that match one property.  Integrate setting properties to data model before insert and update.">
// <change date="4/28/2017" author="Brian A. Lakstins" description="Fix issue getting a stream.  Reset the position to the start.">
// <change date="5/23/2017" author="Brian A. Lakstins" description="Updated to cache MaxData instead of MaxEntity">
// <change date="6/21/2017" author="Brian A. Lakstins" description="Fix issue with accessing stream after it is disposed.">
// <change date="6/21/2017" author="Brian A. Lakstins" description="Centralize Get and Set methods access to base data index.">
// <change date="10/24/2017" author="Brian A. Lakstins" description="Update cache clearing.  Remove unnecessary SetList command.">
// <change date="1/23/2018" author="Brian A. Lakstins" description="Add method to get a MaxIndex that represents the entity.">
// <change date="2/8/2019" author="Brian A. Lakstins" description="Add some events">
// <change date="7/30/2019" author="Brian A. Lakstins" description="Speed up GetString.  Set storageKey only when defined.">
// <change date="10/3/2019" author="Brian A. Lakstins" description="Add support for GetPropertyName using functional expressions for the frameworks that support functional expressions.">
// <change date="12/2/2019" author="Brian A. Lakstins" description="Added Archive method.">
// <change date="12/11/2019" author="Brian A. Lakstins" description="Update archive process for case of previous failed archive attempts.">
// <change date="5/19/2020" author="Brian A. Lakstins" description="Lock archive process to prevent multiple treads trying to archive the same data.">
// <change date="5/31/2020" author="Brian A. Lakstins" description="Update archive process run at most once per hour per storage key and to store last archive date in cache">
// <change date="8/13/2020" author="Brian A. Lakstins" description="Move SetObject and GetObject functionality from MaxBaseIdEntity">
// <change date="1/7/2021" author="Brian A. Lakstins" description="SetProperties was resetting new long text and breaking data.  Updated to clear changed indicator on load and do SetObject before SaveStream.  Fix loading object from stream.">
// <change date="1/13/2021" author="Brian A. Lakstins" description="Store archive date in persistent configuration so archive process does not need to run each time application restarts.  Archive process fixes.">
// <change date="1/16/2021" author="Brian A. Lakstins" description="Update definition of cache keys.">
// <change date="1/20/2021" author="Brian A. Lakstins" description="Fix loading by property">
// <change date="2/16/2021" author="Brian A. Lakstins" description="Fix issue when storing data objects that don't match properties. Add method to map properties to an index.">
// <change date="5/18/2021" author="Brian A. Lakstins" description="Pull records from archive if there are archived records.">
// <change date="5/28/2021" author="Brian A. Lakstins" description="Make a method work in all supported frameworks">
// <change date="6/9/2021" author="Brian A. Lakstins" description="Only get property values if they are going to be returned.  Cache strings stored in streams.">
// <change date="3/20/2024" author="Brian A. Lakstins" description="Happy birthday to my mom.  Sara Jean Lakstins (Cartwright) - 3/20/1944 to 3/14/2019.">
// <change date="3/22/2024" author="Brian A. Lakstins" description="Remove StorageKey.  Add dynamic KeyPropertyList and Key.  Change Repositories to Base versions.  Remove archiving.  Replace Key with DataName.  Add retries to data changing methods.  Update load methods to reuse other load methods when possible.">
// <change date="3/23/2024" author="Brian A. Lakstins" description="Updating support generic primary keys.  Renaming Reset method to Clear.">
// <change date="3/24/2024" author="Brian A. Lakstins" description="Updated for changes namespaces">
// <change date="3/26/2024" author="Brian A. Lakstins" description="Specify between Property Name and Data Name">
// <change date="3/30/2024" author="Brian A. Lakstins" description="Use DataKey as unique identifier.  Remove using SelectAll in repository because it ignores any filters including StorageKey and IsDeleted.  Use list of Streams DataNames for stream storage.">
// <change date="6/19/2024" author="Brian A. Lakstins" description="Fix getting object when binary or long string data cannot be loaded.">
// <change date="6/28/2024" author="Brian A. Lakstins" description="Change how mapindex works so it always returns requested properties.">
// <change date="8/26/2024" author="Brian A. Lakstins" description="Add methods for dynamic creation and property value setting.">
// <change date="9/16/2024" author="Brian A. Lakstins" description="Making sure Propertlist is not null.">
// <change date="9/24/2024" author="Brian A. Lakstins" description="Update filtering for when there is not a value.">
// <change date="10/23/2024" author="Brian A. Lakstins" description="Only include the condition if there is a next value.">
// <change date="3/22/2025" author="Brian A. Lakstins" description="Make insert retry configurable.">
// <change date="4/9/2025" author="Brian A. Lakstins" description="Add and integrate SetInitial method for setting initial values before inserting.">
// <change date="4/14/2025" author="Brian A. Lakstins" description="Only update streams when date has changed.">
// <change date="5/12/2025" author="Brian A. Lakstins" description="Make sure Guid type for DataKey filter is a Guid object.">
// <change date="5/23/2025" author="Brian A. Lakstins" description="Remove stream handling.  Should be part of DataContextLibraryProvider  Add multi record insert method.">
// <change date="6/3/2025" author="Brian A. Lakstins" description="Fix LoadAll methods to not include any key filtering data. Add method to define default DataQuery.">
// <change date="6/3/2025" author="Brian A. Lakstins" description="Clear changed data after successful insert.">
// <change date="6/3/2025" author="Brian A. Lakstins" description="Fix issue with null error.">
// <change date="6/9/2025" author="Brian A. Lakstins" description="Indicate success on update even when data has not changed, but log the info.">
// <change date="6/9/2025" author="Brian A. Lakstins" description="Fix index mapping to default to all properties.">
// <change date="6/9/2025" author="Brian A. Lakstins" description="Fix loading by filter to specify DataQuery correctly.">
// <change date="6/10/2025" author="Brian A. Lakstins" description="Update caching integration to include expire date and work consistently with Data and DataList">
// <change date="6/11/2025" author="Brian A. Lakstins" description="Use ApplicationKey for cache and allow override using StorageKey">
// <change date="6/12/2025" author="Brian A. Lakstins" description="Fix CacheKey for when there is not a StorageKey defined for the DataModel">
// <change date="6/12/2025" author="Brian A. Lakstins" description="Move CacheKey method into DataModel">
// <change date="6/17/2025" author="Brian A. Lakstins" description="Try to prevent exception when parsing a guid">
// <change date="6/18/2025" author="Brian A. Lakstins" description="Fix and add some cache clearing">
// <change date="6/23/2025" author="Brian A. Lakstins" description="Remove cache clearing that is not needed.">
// <change date="6/24/2025" author="Brian A. Lakstins" description="Clear all cache on update.">
// <change date="7/10/2025" author="Brian A. Lakstins" description="Make method virtual so it can be overridden.">
// <change date="9/9/2025" author="Brian A. Lakstins" description="Make sure DataNames that make up the PropertyName DataKey are included.">
// <change date="10/21/2025" author="Brian A. Lakstins" description="Change DataKey handling so it's always included.">
// <change date="11/6/2025" author="Brian A. Lakstins" description="Adding MapIndexList to allow an entity to generate a list based on property names.">
// <change date="11/7/2025" author="Brian A. Lakstins" description="Fix offset handling.">
// <change date="11/7/2025" author="Brian A. Lakstins" description="Fix null exception.">
// <change date="11/23/2025" author="Brian A. Lakstins" description="Handling per property filters and calculations">
// <change date="11/24/2025" author="Brian A. Lakstins" description="Sum multiple values for same index if numbers.  Add ABS function support.">
// <change date="11/24/2025" author="Brian A. Lakstins" description="Getting aggregate functions working.">
// <change date="11/29/2025" author="Brian A. Lakstins" description="Create single internal access to entity property list.  Cache Key property information.">
// </changelog>
#endregion

namespace MaxFactry.Base.BusinessLayer
{
    using System;
#if net4_52 || netcore1 || netstandard1_2
    using System.Linq.Expressions;
#endif
    using System.Globalization;
    using System.IO;
    using System.Reflection;
    using MaxFactry.Core;
    using MaxFactry.Base.DataLayer;
    using MaxFactry.Base.DataLayer.Library;
    using System.Text.RegularExpressions;
    using System.Collections.Generic;

    /// <summary>
    /// Base Business Layer Entity
    /// </summary>
    public abstract class MaxEntity
    {
        /// <summary>
        /// Locking object
        /// </summary>
        private static object _oLock = new object();

        /// <summary>
        /// Index of property names that make up the key for each entity type.
        /// </summary>
        private static Dictionary<Type, string[]> _oPropertyNameKeyListIndex = new Dictionary<Type, string[]>();

        /// <summary>
        /// Property Names that make up the key for the entity.
        /// </summary>
        private string[] _aPropertyNameKeyList = null;
        
        /// <summary>
        /// Index of property info for each entity type.
        /// </summary>
        private static Dictionary<Type, Dictionary<string, PropertyInfo>> _oPropertyIndexIndex = new Dictionary<Type, Dictionary<string, PropertyInfo>>();

        /// <summary>
        /// Index of peroperty info for this entity.
        /// </summary>
        private Dictionary<string, PropertyInfo> _oPropertyIndex = null;

        /// <summary>
        /// Index of objects that have been un serialized.
        /// </summary>
        private MaxIndex _oUnSerializedObjectIndex = new MaxIndex();

        /// <summary>
        /// Event before inserting a record 
        /// </summary>
        public static event EventHandler InsertBefore = delegate { };

        /// <summary>
        /// Event after inserting a record 
        /// </summary>
        public static event EventHandler InsertAfter = delegate { };

        /// <summary>
        /// Event when failing to insert a record 
        /// </summary>
        public static event EventHandler InsertFail = delegate { };

        /// <summary>
        /// Event before updating a record 
        /// </summary>
        public static event EventHandler UpdateBefore = delegate { };

        /// <summary>
        /// Event after updating a record 
        /// </summary>
        public static event EventHandler UpdateAfter = delegate { };

        /// <summary>
        /// Event when failing to update a record 
        /// </summary>
        public static event EventHandler UpdateFail = delegate { };

        /// <summary>
        /// Event before deleting a record 
        /// </summary>
        public static event EventHandler DeleteBefore = delegate { };

        /// <summary>
        /// Event after deleting a record 
        /// </summary>
        public static event EventHandler DeleteAfter = delegate { };

        /// <summary>
        /// Event when failing to delete a record 
        /// </summary>
        public static event EventHandler DeleteFail = delegate { };

        /// <summary>
        /// object to hold data for the entity
        /// </summary>
        private MaxData _oData = null;

        /// <summary>
        /// Type of the data model.
        /// </summary>
        private Type _oDataModelType = null;

        /// <summary>
        /// Internal storage of EntityList this entity is a part of.
        /// </summary>
        private MaxEntityList _oEntityList = null;

        /// <summary>
        /// Initializes a new instance of the MaxEntity class
        /// </summary>
        /// <param name="loData">object to hold data</param>
        public MaxEntity(MaxData loData)
        {
            if (this.Load(loData))
            {
                this.Data.ClearChanged();
            }
        }

        /// <summary>
        /// Initializes a new instance of the MaxEntity class.
        /// </summary>
        /// <param name="loDataModelType">Type of data model.</param>
        public MaxEntity(Type loDataModelType)
        {
            this._oDataModelType = loDataModelType;
        }

        /// <summary>
        /// Gets a list of property names needed for the DataKey
        /// </summary>
        public virtual string[] PropertyNameKeyList
        {
            get
            {
                if (null == this._aPropertyNameKeyList)
                {
                    if (!_oPropertyNameKeyListIndex.ContainsKey(this.GetType()))
                    {
                        lock (_oLock)
                        {
                            if (!_oPropertyNameKeyListIndex.ContainsKey(this.GetType()))
                            {
                                MaxIndex loPropertyNameIndex = new MaxIndex();
                                foreach (string lsDataName in this.Data.DataModel.DataNameKeyList)
                                {
                                    foreach (string lsPropertyName in this.PropertyIndex.Keys)
                                    {
                                        //// The property name has to match the DataName for it to be considered part of the key.  
                                        //// This property can be overridden if that is not the case.
                                        if (lsDataName == this.GetDataName(this.Data.DataModel, lsPropertyName))
                                        {
                                            loPropertyNameIndex.Add(lsPropertyName, true);
                                        }
                                    }
                                }

                                _oPropertyNameKeyListIndex.Add(this.GetType(), loPropertyNameIndex.GetSortedKeyList());
                            }
                        }
                    }

                    this._aPropertyNameKeyList = _oPropertyNameKeyListIndex[this.GetType()];
                }

                return this._aPropertyNameKeyList;
            }
        }

        /// <summary>
        /// Gets the key for the data associated with the entity
        /// </summary>
        public virtual string DataKey
        {
            get
            {
                return this.Data.GetDataKey();
            }
        }

        /// <summary>
        /// Gets or sets the object holding the data
        /// </summary>
        protected virtual MaxData Data
        {
            get
            {
                if (null == this._oData)
                {
                    this._oData = new MaxData(MaxDataLibrary.GetDataModel(this.DataModelType));
                }

                return this._oData;
            }
        }

        /// <summary>
        /// Gets the type of the data model
        /// </summary>
        protected Type DataModelType
        {
            get
            {
                return this._oDataModelType;
            }
        }

        /// <summary>
        /// Gets the entity list this entity is part of.
        /// </summary>
        protected MaxEntityList EntityList
        {
            get
            {
                return this._oEntityList;
            }
        }

        /// <summary>
        /// Property indicating if any data in this entity has been changed
        /// </summary>
        protected virtual bool IsDataChanged
        {
            get
            {
                foreach (string lsDataName in this.Data.DataModel.DataNameList)
                {
                    if (this.Data.GetIsChanged(lsDataName))
                    {
                        return true;
                    }
                }

                return false;
            }
        }

        /// <summary>
        /// Gets the index of property info for this entity.
        /// </summary>
        protected Dictionary<string, PropertyInfo> PropertyIndex
        {
            get
            {
                if (null == _oPropertyIndex)
                {
                    if (!_oPropertyIndexIndex.ContainsKey(this.GetType()))
                    {
                        lock (_oLock)
                        {
                            if (!_oPropertyIndexIndex.ContainsKey(this.GetType()))
                            {
                                Dictionary<string, PropertyInfo> loR = new Dictionary<string, PropertyInfo>();
                                MaxIndex loPropertyList = MaxFactry.Core.MaxFactryLibrary.GetPropertyList(this);
                                string[] laPropertyNameIndex = loPropertyList.GetSortedKeyList();
                                foreach (string lsPropertyName in laPropertyNameIndex)
                                {
                                    object loProperty = loPropertyList[lsPropertyName];
                                    if (loProperty is PropertyInfo)
                                    {
                                        loR.Add(((PropertyInfo)loProperty).Name, (PropertyInfo)loProperty);
                                        loR.Add(this.GetType().ToString() + "." + ((PropertyInfo)loProperty).Name, (PropertyInfo)loProperty);
                                    }
                                }

                                _oPropertyIndexIndex.Add(this.GetType(), loR);
                            }
                        }
                    }

                    _oPropertyIndex = _oPropertyIndexIndex[this.GetType()];
                }

                return _oPropertyIndex;
            }
        }

        /// <summary>
        /// Clears the data associated with the entity to it's blank state
        /// </summary>
        public virtual void Clear()
        {
            this._oData = null;
        }

        /// <summary>
        /// Inserts a new record and removes any lists from cache
        /// </summary>
        /// <returns>true if inserted.  False if cannot be inserted.</returns>
        public virtual bool Insert()
        {
            return this.InsertRetry(5);
        }

        /// <summary>
        /// Inserts a new record and removes any lists from cache
        /// </summary>
        /// <param name="lnRetry">Number of times to retry if failure</param>
        /// <returns>true if inserted.  False if cannot be inserted.</returns>
        public bool InsertRetry(int lnRetry)
        {
            OnInsertBefore();
            this.SetInitial();
            this.SetProperties();
            int lnTry = 0;
            bool lbR = MaxBaseWriteRepository.Insert(this.Data);
            while (!lbR && lnTry <= lnRetry)
            {
                MaxLogLibrary.Log(new MaxLogEntryStructure(this.GetType(), "InsertRetry(Retry)", MaxEnumGroup.LogError, "Insert attempt {0} failed.", lnTry + 1));
                System.Threading.Thread.Sleep(100);
                lbR = MaxBaseWriteRepository.Insert(this.Data);
                lnTry++;
            }

            if (lbR)
            {
                this.Data.ClearChanged();
                //// Clear everything for this entity from the cache
                string lsCacheKey = this.GetCacheKey("LoadAll*");
                MaxCacheRepository.Remove(this.GetType(), lsCacheKey);
                OnInsertAfter();
            } 
            else
            {
                OnInsertFail();
            }

            return lbR;
        }

        /// <summary>
        /// Runs the hanlders for the InsertBefore event
        /// </summary>
        protected virtual void OnInsertBefore()
        {
            EventHandler loHandler = InsertBefore;
            if (loHandler != null)
            {
                loHandler(this, new MaxEntityEventArgs(this));
            }
        }

        /// <summary>
        /// Runs the hanlders for the InsertAfter event
        /// </summary>
        protected virtual void OnInsertAfter()
        {
            EventHandler loHandler = InsertAfter;
            if (loHandler != null)
            {
                loHandler(this, new MaxEntityEventArgs(this));
            }
        }

        /// <summary>
        /// Runs the hanlders for the InsertFail event
        /// </summary>
        protected virtual void OnInsertFail()
        {
            EventHandler loHandler = InsertFail;
            if (loHandler != null)
            {
                loHandler(this, new MaxEntityEventArgs(this));
            }
        }

        /// <summary>
        /// Updates an existing record and removes any lists from Cache.
        /// </summary>
        /// <returns>true if updated.  False if cannot be updated.</returns>
        public virtual bool Update()
        {
            bool lbR = false;
            OnUpdateBefore();
            this.SetProperties();
            if (!this.IsDataChanged)
            {
                MaxLogLibrary.Log(new MaxLogEntryStructure(this.GetType(), "Update", MaxEnumGroup.LogInfo, "No data has been changed for update."));
                lbR = true;
            }
            else if (string.IsNullOrEmpty(this.DataKey))
            {
                MaxLogLibrary.Log(new MaxLogEntryStructure(this.GetType(), "Update", MaxEnumGroup.LogError, "DataKey is not set for update."));
            }
            else
            {
                int lnLimit = 5;
                int lnTry = 0;
                lbR = MaxBaseWriteRepository.Update(this.Data);
                while (!lbR && lnTry < lnLimit)
                {
                    System.Threading.Thread.Sleep(100);
                    lbR = MaxBaseWriteRepository.Update(this.Data);
                    lnTry++;
                }

                if (lbR)
                {
                    this.Data.ClearChanged();
                    //// Clear everything for this entity from the cache
                    string lsCacheKey = this.GetCacheKey("LoadAll*");
                    MaxCacheRepository.Remove(this.GetType(), lsCacheKey);
                    if (null != this.DataKey && this.DataKey.Length > 0)
                    {
                        //// Clear everything that has this DataKey from cache
                        lsCacheKey = this.GetCacheKey("LoadByDataKey/" + this.DataKey + "*");
                        MaxCacheRepository.Remove(this.GetType(), lsCacheKey);
                    }

                    OnUpdateAfter();
                    return lbR;
                }
            }
            

            if (!lbR)
            {
                OnUpdateFail();
            }

            return lbR;
        }

        /// <summary>
        /// Runs the hanlders for the UpdateBefore event
        /// </summary>
        protected virtual void OnUpdateBefore()
        {
            EventHandler loHandler = UpdateBefore;
            if (loHandler != null)
            {
                loHandler(this, new MaxEntityEventArgs(this));
            }
        }

        /// <summary>
        /// Runs the hanlders for the UpdateAfter event
        /// </summary>
        protected virtual void OnUpdateAfter()
        {
            EventHandler loHandler = UpdateAfter;
            if (loHandler != null)
            {
                loHandler(this, new MaxEntityEventArgs(this));
            }
        }

        /// <summary>
        /// Runs the hanlders for the UpdateFail event
        /// </summary>
        protected virtual void OnUpdateFail()
        {
            EventHandler loHandler = UpdateFail;
            if (loHandler != null)
            {
                loHandler(this, new MaxEntityEventArgs(this));
            }
        }

        /// <summary>
        /// Marks a record as deleted  and removes any lists from Cache.
        /// </summary>
        /// <returns>true if marked as deleted.  False if it cannot be deleted.</returns>
        public virtual bool Delete()
        {
            bool lbR = false;
            OnDeleteBefore();
            if (!string.IsNullOrEmpty(this.DataKey))
            {
                string lsCacheKey = this.GetCacheKey("LoadByDataKey/" + this.DataKey + "*");
                lbR = MaxBaseWriteRepository.Delete(this.Data);
                if (lbR)
                {
                    this.Data.Clear();
                    MaxCacheRepository.Remove(this.GetType(), lsCacheKey);
                    lsCacheKey = this.GetCacheKey("LoadAll*");
                    MaxCacheRepository.Remove(this.GetType(), lsCacheKey);
                    OnDeleteAfter();
                    return lbR;
                }
            }

            OnDeleteFail();
            return lbR;
        }

        /// <summary>
        /// Runs the hanlders for the DeleteBefore event
        /// </summary>
        protected virtual void OnDeleteBefore()
        {
            EventHandler loHandler = DeleteBefore;
            if (loHandler != null)
            {
                loHandler(this, new MaxEntityEventArgs(this));
            }
        }

        /// <summary>
        /// Runs the hanlders for the DeleteAfter event
        /// </summary>
        protected virtual void OnDeleteAfter()
        {
            EventHandler loHandler = DeleteAfter;
            if (loHandler != null)
            {
                loHandler(this, new MaxEntityEventArgs(this));
            }
        }

        /// <summary>
        /// Runs the hanlders for the DeleteFail event
        /// </summary>
        protected virtual void OnDeleteFail()
        {
            EventHandler loHandler = DeleteFail;
            if (loHandler != null)
            {
                loHandler(this, new MaxEntityEventArgs(this));
            }
        }

        public virtual bool MatchesFilter(string lsFilter)
        {
            bool lbR = true;
            if (!string.IsNullOrEmpty(lsFilter))
            {
                //// TODO: Handle AND and OR conditions
                string[] laFilterPart = lsFilter.Split(new char[] { '&' }, StringSplitOptions.RemoveEmptyEntries);
                foreach (string lsFilterPart in laFilterPart)
                {
                    string[] laFilterProperty = lsFilterPart.Split(new char[] { '=' }, StringSplitOptions.RemoveEmptyEntries);
                    if (laFilterProperty.Length == 2)
                    {
                        MaxIndex loPropertyIndex = MaxFactry.Core.MaxFactryLibrary.GetPropertyList(this);
                        object loProperty = loPropertyIndex[laFilterProperty[0]];
                        if (loProperty is PropertyInfo)
                        {
                            object loValue = ((PropertyInfo)loProperty).GetValue(this, null);
                            if (loValue is double)
                            {
                                double lnCheckValue = MaxConvertLibrary.ConvertToDouble(typeof(object), laFilterProperty[1]);
                                if (lnCheckValue != (double)loValue)
                                {
                                    lbR = false;
                                }
                            }
                            else if (loValue is int)
                            {
                                int lnCheckValue = MaxConvertLibrary.ConvertToInt(typeof(object), laFilterProperty[1]);
                                if (lnCheckValue != (int)loValue)
                                {
                                    lbR = false;
                                }
                            }
                            else if (loValue is Guid)
                            {
                                Guid loCheckValue = MaxConvertLibrary.ConvertToGuid(typeof(object), laFilterProperty[1]);
                                if (loCheckValue != (Guid)loValue)
                                {
                                    lbR = false;
                                }
                            }
                        }
                    }
                }
            }

            return lbR;
        }

        /// <summary>
        /// Creates and index of property information that can be easily serialized
        /// </summary>
        /// <param name="laPropertyNameList"></param>
        /// <returns></returns>
        public virtual MaxIndex MapIndex(params string[] laPropertyNameList)
        {
            MaxIndex loR = new MaxIndex();
            MaxIndex loPropertyIndex = MaxFactry.Core.MaxFactryLibrary.GetPropertyList(this);
            string[] laPropertyNameIndex = loPropertyIndex.GetSortedKeyList();
            foreach (string lsPropertyName in laPropertyNameIndex)
            {
                object loProperty = loPropertyIndex[lsPropertyName];
                if (loProperty is PropertyInfo)
                {
                    loPropertyIndex.Add(((PropertyInfo)loProperty).Name, loProperty);
                    loPropertyIndex.Add(this.GetType().ToString() + "." + ((PropertyInfo)loProperty).Name, loProperty);
                }
            }

            string[] laIncludedPropertyNameList = laPropertyNameList;
            if (null == laIncludedPropertyNameList || laIncludedPropertyNameList.Length == 0)
            {
                laIncludedPropertyNameList = laPropertyNameIndex;
            }

            foreach (string lsPropertyName in laIncludedPropertyNameList)
            {
                string lsName = lsPropertyName;
                object loProperty = loPropertyIndex[lsPropertyName];
                string lsFilter = string.Empty;
                if (lsPropertyName.Contains("."))
                {
                    lsName = lsPropertyName.Substring(lsPropertyName.LastIndexOf(".") + 1);
                }
                else if (lsPropertyName.Contains(":"))
                {
                    loProperty = loPropertyIndex[lsPropertyName.Substring(0, lsPropertyName.IndexOf(":"))];
                    lsName = lsPropertyName.Substring(lsPropertyName.IndexOf(":") + 1);
                    if (lsName.Contains("?"))
                    {
                        lsName = lsName.Substring(0, lsName.IndexOf("?"));
                        lsFilter = lsPropertyName.Substring(lsPropertyName.IndexOf("?") + 1);
                    }
                }

                if (this.MatchesFilter(lsFilter))
                {
                    if (loProperty is PropertyInfo)
                    {
                        object loValue = ((PropertyInfo)loProperty).GetValue(this, null);
                        if (loValue is MaxEntity)
                        {
                            loR.Add(lsName, ((MaxEntity)loValue).MapIndex(laIncludedPropertyNameList));
                        }
                        else if (loValue is double)
                        {
                            if (double.MinValue != (double)loValue)
                            {
                                loR.Add(lsName, loValue);
                            }
                        }
                        else if (loValue is int)
                        {
                            if (int.MinValue != (int)loValue)
                            {
                                loR.Add(lsName, loValue);
                            }
                        }
                        else if (loValue is Guid)
                        {
                            if (Guid.Empty != (Guid)loValue)
                            {
                                loR.Add(lsName, loValue);
                            }
                        }
                        else if (loValue is DateTime)
                        {
                            if ((DateTime)loValue > DateTime.MinValue)
                            {
                                //// Use same format as javascript date .toISOString()
                                loR.Add(lsName, MaxConvertLibrary.ConvertToDateTimeUtc(typeof(object), loValue).ToString("o", CultureInfo.InvariantCulture));
                            }
                        }
                        else if (loValue != null && !(loValue is Stream))
                        {
                            loR.Add(lsName, loValue);
                        }
                    }
                    else
                    {
                        loR.Add(lsName, string.Empty);
                    }
                }
            }

            return loR;
        }


        public virtual string[] ParseParamList(string lsParams)
        {
            List<string> loR = new List<string>();
            int lnFirstParenIndex = lsParams.IndexOf("(");
            int lnFirstSemicolonIndex = lsParams.IndexOf(";");
            string lsParam = lsParams;
            if (lnFirstParenIndex != -1 && (lnFirstParenIndex < lnFirstSemicolonIndex || lnFirstSemicolonIndex == -1))
            {
                int lnEndIndex = 0;
                int lnParenCount = 0;
                int lnCurrent = lnFirstParenIndex;
                while (lnEndIndex < lnFirstParenIndex && lnCurrent < lsParams.Length)
                {
                    if (lsParams[lnCurrent] == '(') lnParenCount++;
                    if (lsParams[lnCurrent] == ')') lnParenCount--;
                    if (lnParenCount == 0)
                    {
                        lnEndIndex = lnCurrent;
                    }

                    lnCurrent++;
                }

                lsParam = lsParams.Substring(0, lnEndIndex + 1);
            }
            else if (lnFirstSemicolonIndex != -1)
            {
                lsParam = lsParams.Substring(0, lnFirstSemicolonIndex);
            }

            if (!string.IsNullOrEmpty(lsParam))
            {
                loR.Add(lsParam);
                if (lsParams.Length > lsParam.Length + 1)
                {
                    string lsRemainingParams = lsParams.Substring(lsParam.Length + 1);
                    string[] laRemainingParams = this.ParseParamList(lsRemainingParams);
                    loR.AddRange(laRemainingParams);
                }
            }

            return loR.ToArray();
        }

        /// <summary>
        /// MULTIPLY(PropertyName1, PropertyName1)
        /// SUBTRACT(ADD(PropertyName1,PropertyName2,PropertyName3,PropertyName4),PropertyName5)
        /// MULTIPLY(SUBTRACT(PropertyName1, PropertyName2),PropertyName3)
        /// </summary>
        /// <param name="loIndex"></param>
        /// <param name="lsFunction"></param>
        /// <returns></returns>
        public virtual object EvaluateFunction(MaxIndex loIndex, string lsFunction)
        {
            object loR = null;
            List<string> loFunctionNameList = new List<string>(new string[] { "MULTIPLY", "SUBTRACT", "ADD", "ABS", "FORMAT" });
            string lsMethod = lsFunction.Substring(0, lsFunction.IndexOf("(")).ToUpper();
            string lsParams = lsFunction.Substring(lsFunction.IndexOf("(") + 1, lsFunction.Length - lsFunction.IndexOf("(") - 2);
            if (loFunctionNameList.Contains(lsMethod))
            {
                List<string> loParamList = new List<string>(this.ParseParamList(lsParams));
                if (loParamList.Count > 0)
                {
                    if (lsMethod == "MULTIPLY")
                    {
                        double lnR = 1;
                        foreach (string lsParam in loParamList)
                        {
                            object loValue = null;
                            if (lsParam.Contains("("))
                            {
                                loValue = this.EvaluateFunction(loIndex, lsParam);
                            }
                            else
                            {
                                loValue = loIndex[lsParam];
                            }

                            double lnValue = MaxConvertLibrary.ConvertToDouble(typeof(object), loValue);
                            lnR *= lnValue;
                        }

                        loR = lnR;
                    }
                    else if (lsMethod == "SUBTRACT")
                    {
                        object loValue1 = null;
                        if (loParamList[0].Contains("("))
                        {
                            loValue1 = this.EvaluateFunction(loIndex, loParamList[0]);
                        }
                        else
                        {
                            loValue1 = loIndex[loParamList[0]];
                        }

                        object loValue2 = null;
                        if (loParamList[1].Contains("("))
                        {
                            loValue2 = this.EvaluateFunction(loIndex, loParamList[1]);
                        }
                        else
                        {
                            loValue2 = loIndex[loParamList[1]];
                        }

                        //// todo: make sure not too small
                        double lnR = MaxConvertLibrary.ConvertToDouble(typeof(object), loValue1) - MaxConvertLibrary.ConvertToDouble(typeof(object), loValue2);
                        loR = lnR;
                    }
                    else if (lsMethod == "ADD")
                    {
                        double lnR = 0;
                        foreach (string lsParam in loParamList)
                        {
                            object loValue = null;
                            if (lsParam.Contains("("))
                            {
                                loValue = this.EvaluateFunction(loIndex, lsParam);
                            }
                            else
                            {
                                loValue = loIndex[lsParam];
                            }

                            double lnValue = MaxConvertLibrary.ConvertToDouble(typeof(object), loValue);
                            lnR += lnValue;
                        }

                        loR = lnR;
                    }
                    else if (lsMethod == "ABS")
                    {
                        double lnR = 0;
                        foreach (string lsParam in loParamList)
                        {
                            object loValue = null;
                            if (lsParam.Contains("("))
                            {
                                loValue = this.EvaluateFunction(loIndex, lsParam);
                            }
                            else
                            {
                                loValue = loIndex[lsParam];
                            }

                            double lnValue = MaxConvertLibrary.ConvertToDouble(typeof(object), loValue);
                            lnR = Math.Abs(lnValue);
                        }

                        loR = lnR;
                    }
                    else if (lsMethod == "FORMAT")
                    {
                        string lsR = string.Empty;
                        foreach (string lsParam in loParamList)
                        {
                            object loValue = null;
                            if (lsParam.Contains("("))
                            {
                                loValue = this.EvaluateFunction(loIndex, lsParam);
                            }
                            else 
                            {
                                string[] laParam = lsParam.Split(new char[] { '#' }, StringSplitOptions.RemoveEmptyEntries);
                                if (laParam.Length >= 2)
                                {
                                    string lsPropertyName = laParam[0];
                                    string lsFormat = laParam[1];
                                    string lsDataName = this.GetDataName(this.Data.DataModel, lsPropertyName);
                                    Type loType = this.Data.DataModel.GetValueType(lsDataName);
                                    if (loType == typeof(DateTime))
                                    {
                                        DateTime ldDateTime = MaxConvertLibrary.ConvertToDateTimeUtc(typeof(object), loIndex[lsPropertyName]);
                                        double lnOffset = 0;
                                        if (laParam.Length >= 3)
                                        {
                                            string lsOffsetParam = laParam[2];
                                            //// Use Regex to determine if offset is a number
                                            Regex loRegex = new Regex(@"^-?[0-9]?[0-9\.]*$");
                                            if (loRegex.IsMatch(lsOffsetParam))
                                            {
                                                lnOffset = MaxConvertLibrary.ConvertToDouble(typeof(object), lsOffsetParam);
                                            }
                                            else
                                            {
#if net4_52 || netcore2_1
                                                TimeZoneInfo loTZ = TimeZoneInfo.FindSystemTimeZoneById(lsOffsetParam);
                                                lnOffset = loTZ.GetUtcOffset(ldDateTime).TotalHours;
#else
                                        throw new MaxException("Time zone format not supported in this framework.");
#endif
                                            }

                                            ldDateTime = ldDateTime.AddHours(lnOffset);
                                        }

                                        loValue = ldDateTime.ToString(lsFormat);
                                    }
                                }
                            }

                            string lsValue = MaxConvertLibrary.ConvertToString(typeof(object), loValue);
                            lsR += lsValue;
                        }

                        loR = lsR;
                    }
                }
            }

            return loR;
        }

        /// <summary>
        /// MULTIPLY(PropertyName1, PropertyName1)
        /// SUBTRACT(ADD(PropertyName1,PropertyName2,PropertyName3,PropertyName4),PropertyName5)
        /// MULTIPLY(SUBTRACT(PropertyName1, PropertyName2),PropertyName3)
        /// </summary>
        /// <param name="loIndex"></param>
        /// <param name="lsFunction"></param>
        /// <returns></returns>
        public virtual object EvaluateAggregateFunction(MaxIndex loExistingIndex, string lsAlias, MaxIndex loIndex, string lsFunction)
        {
            object loR = null;
            List<string> loFunctionNameList = new List<string>(new string[] { "GROUPBY", "SUM", "AVG" });
            string lsMethod = lsFunction.Substring(0, lsFunction.IndexOf("(")).ToUpper();
            string lsParams = lsFunction.Substring(lsFunction.IndexOf("(") + 1, lsFunction.Length - lsFunction.IndexOf("(") - 2);
            if (loFunctionNameList.Contains(lsMethod))
            {
                List<string> loParamList = new List<string>(this.ParseParamList(lsParams));
                if (loParamList.Count > 0)
                {
                    if (lsMethod == "GROUPBY")
                    {
                        string lsR = string.Empty;
                        foreach (string lsParam in loParamList)
                        {
                            object loValue = null;
                            if (lsParam.Contains("("))
                            {
                                loValue = this.EvaluateFunction(loIndex, lsParam);
                            }
                            else
                            {
                                loValue = loIndex[lsParam];
                            }

                            string lsValue = MaxConvertLibrary.ConvertToString(typeof(object), loValue);
                            lsR += lsValue;
                        }

                        loR = lsR;
                    }
                    else if (lsMethod == "SUM")
                    {
                        double lnR = 0;
                        if (loExistingIndex.Contains(lsAlias))
                        {
                            object loValue = loExistingIndex[lsAlias];
                            if (loValue is double || (loValue is string && !string.IsNullOrEmpty((string)loValue)))
                            {
                                lnR += MaxConvertLibrary.ConvertToDouble(typeof(object), loValue);
                            }
                        }
                        
                        foreach (string lsParam in loParamList)
                        {
                            object loValue = null;
                            if (lsParam.Contains("("))
                            {
                                loValue = this.EvaluateFunction(loIndex, lsParam);
                            }
                            else
                            {
                                loValue = loIndex[lsParam];
                            }

                            if (loValue is double || (loValue is string && !string.IsNullOrEmpty((string)loValue)))
                            {
                                lnR += MaxConvertLibrary.ConvertToDouble(typeof(object), loValue);
                            }
                        }

                        loR = lnR;
                    }
                    else if (lsMethod == "AVG")
                    {
                        double lnR = 0;
                        int lnAggregateCount = 0;
                        if (loExistingIndex.Contains(lsAlias))
                        {
                            object loExistingValue = loExistingIndex[lsAlias];
                            if (loExistingValue is double || (loExistingValue is string && !string.IsNullOrEmpty((string)loExistingValue)))
                            {
                                lnR += MaxConvertLibrary.ConvertToDouble(typeof(object), loExistingValue);
                                lnAggregateCount = MaxConvertLibrary.ConvertToInt(typeof(object), loExistingIndex["_AggregateCount"]);
                            }                           
                        }

                        foreach (string lsParam in loParamList)
                        {
                            object loValue = null;
                            if (lsParam.Contains("("))
                            {
                                loValue = this.EvaluateFunction(loIndex, lsParam);
                            }
                            else
                            {
                                loValue = loIndex[lsParam];
                            }

                            if (loValue is double || (loValue is string && !string.IsNullOrEmpty((string)loValue)))
                            {
                                double lnValue = MaxConvertLibrary.ConvertToDouble(typeof(object), loValue);
                                lnR = ((lnR * lnAggregateCount) + lnValue) / (lnAggregateCount + 1);
                            }
                        }

                        loR = lnR;
                    }
                }
            }

            return loR;
        }

        /// <summary>
        /// Creates a list of indexes from a list of entities
        /// </summary>
        /// <param name="loList">List of entities</param>
        /// <param name="laPropertyNameList">List of property names to include</param>
        /// <returns></returns>
        public virtual MaxIndex[] MapIndexList(MaxEntityList loList, params string[] laPropertyNameList)
        {
            MaxIndex[] laR = new MaxIndex[0];
            if (null != laPropertyNameList && laPropertyNameList.Length > 0)
            {
                List<string> loPropertyNameList = new List<string>(laPropertyNameList);
                List<string> loDataNameKeyList = new List<string>(this.Data.DataModel.DataNameKeyList);
                //// Get all properties that have a filter and combine them into on row per key
                //// Filtered properties look like PropertyName:AliasName?FilterProperty1=value1&FilterProperty2=value2
                //// Add any properties used in filters to the Filtered Property list
                List<string> loFilteredPropertyList = new List<string>();
                foreach (string lsName in laPropertyNameList)
                {
                    if (lsName.Contains("?"))
                    {
                        string[] laName = lsName.Split(new char[] { '?' }, StringSplitOptions.RemoveEmptyEntries);
                        string lsNamePart = laName[0];
                        if (lsNamePart.Contains(":"))
                        {
                            lsNamePart = lsNamePart.Substring(0, lsNamePart.IndexOf(":"));
                        }

                        if (!loFilteredPropertyList.Contains(lsNamePart))
                        {
                            loFilteredPropertyList.Add(lsNamePart);
                        }

                        string[] laFilter = laName[1].Split(new char[] { '&', '|' }, StringSplitOptions.RemoveEmptyEntries);
                        foreach (string lsFilter in laFilter)
                        {
                            string[] laFilterParts = lsFilter.Split(new char[] { '=' }, StringSplitOptions.RemoveEmptyEntries);
                            if (laFilterParts.Length == 2)
                            {
                                if (!loFilteredPropertyList.Contains(laFilterParts[0]))
                                {
                                    loFilteredPropertyList.Add(laFilterParts[0]);
                                }
                            }
                        }
                    }
                }

                //// Add any properties that are part of the index but not part of the key or filtered properties
                List<string> loIndexPropertyList = new List<string>();
                foreach (string lsName in laPropertyNameList)
                {
                    if (!lsName.Contains("(") && !lsName.Contains("?") && !loFilteredPropertyList.Contains(lsName))
                    {
                        string lsDataName = this.GetDataName(this.Data.DataModel, lsName);
                        if (!loDataNameKeyList.Contains(lsDataName) && lsName != "PropertyNameKeyList" && lsName != "DataKey")
                        {
                            if (!loIndexPropertyList.Contains(lsName))
                            {
                                loIndexPropertyList.Add(lsName);
                            }
                        }
                    }
                }

                Dictionary<string, MaxIndex> loIndexDictionary = new Dictionary<string, MaxIndex>();
                //// Create an Index of indexes using filtered properties and key properties
                Regex loRegex = new Regex(@"^-?[0-9]?[0-9\.]*$");
                for (int lnE = 0; lnE < loList.Count; lnE++)
                {
                    MaxEntity loEntity = loList[lnE] as MaxEntity;
                    MaxIndex loIndex = loEntity.MapIndex(laPropertyNameList);
                    string lsIndexKey = loEntity.DataKey;
                    if (loFilteredPropertyList.Count > 0)
                    {
                        lsIndexKey = string.Empty;
                        foreach (string lsIndexProperty in loIndexPropertyList)
                        {
                            string lsValue = loIndex.GetValueString(lsIndexProperty);
                            lsIndexKey += lsValue;
                        }
                    }

                    if (!loIndexDictionary.ContainsKey(lsIndexKey))
                    {
                        loIndexDictionary.Add(lsIndexKey, loIndex);
                    }
                    else
                    {
                        MaxIndex loExistingIndex = loIndexDictionary[lsIndexKey];
                        foreach (string lsPropertyName in loIndex.GetSortedKeyList())
                        {   
                            object loValue = loIndex[lsPropertyName];
                            if (loExistingIndex.Contains(lsPropertyName))
                            {
                                string lsValue = MaxConvertLibrary.ConvertToString(typeof(object), loValue);
                                //// Use a Regular expression to determine if the value is numeric
                                if (!string.IsNullOrEmpty(lsValue) && loRegex.IsMatch(lsValue))
                                {
                                    double lnValue = MaxConvertLibrary.ConvertToDouble(typeof(object), loValue);
                                    double lnExistingValue = MaxConvertLibrary.ConvertToDouble(typeof(object), loExistingIndex[lsPropertyName]);
                                    loExistingIndex[lsPropertyName] = lnValue + lnExistingValue;
                                }
                            }
                            else if (null != loValue)
                            {
                                loExistingIndex.Add(lsPropertyName, loValue);
                            }
                        }
                    }
                }

                //// Process any function properties
                //// MULTIPLY(PropertyName1;PropertyName1):PropertyAlias
                //// SUBTRACT(ADD(PropertyName1;PropertyName2;PropertyName3;PropertyName4);PropertyName5):PropertyAlias
                bool lbHasAggregate = false;
                foreach (string lsKey in loIndexDictionary.Keys)
                {
                    MaxIndex loIndex = loIndexDictionary[lsKey];
                    foreach (string lsName in laPropertyNameList)
                    {
                        if (lsName.Contains("("))
                        {
                            string lsAlias = lsName;
                            string lsFunction = lsName;
                            if (lsName.Contains(":"))
                            {
                                lsAlias = lsName.Substring(lsName.IndexOf(":") + 1);
                                lsFunction = lsName.Substring(0, lsName.IndexOf(":"));
                            }

                            object loValue = this.EvaluateFunction(loIndex, lsFunction);
                            if (null != loValue)
                            {
                                loIndex.Add(lsAlias, loValue);
                            }

                            if (lsFunction.StartsWith("GROUPBY("))
                            {
                                lbHasAggregate = true;
                            }
                        }
                    }
                }

                if (lbHasAggregate)
                {
                    Dictionary<string, MaxIndex> loAggregateIndexDictionary = new Dictionary<string, MaxIndex>();
                    foreach (string lsKey in loIndexDictionary.Keys)
                    {
                        MaxIndex loIndex = loIndexDictionary[lsKey];
                        string lsAggregateKey = string.Empty;
                        foreach (string lsName in laPropertyNameList)
                        {
                            if (lsName.Contains("(") && lsName.Contains("GROUPBY("))
                            {
                                string lsAlias = lsName;
                                string lsFunction = lsName;
                                if (lsName.Contains(":"))
                                {
                                    lsAlias = lsName.Substring(lsName.IndexOf(":") + 1);
                                    lsFunction = lsName.Substring(0, lsName.IndexOf(":"));
                                }

                                object loValue = this.EvaluateAggregateFunction(null, lsAlias, loIndex, lsFunction);
                                if (null != loValue)
                                { 
                                    string lsValue = MaxConvertLibrary.ConvertToString(typeof(object), loValue);
                                    lsAggregateKey += lsValue;
                                    loIndex.Add(lsAlias, lsValue);
                                }
                            }
                        }

                        int lnAggregateCount = 1;
                        MaxIndex loExistingIndex = new MaxIndex();
                        loIndex.Add("_AggregateCount", lnAggregateCount);
                        if (!loAggregateIndexDictionary.ContainsKey(lsAggregateKey))
                        {
                            string[] laKey = loIndex.GetSortedKeyList();
                            foreach (string lsKeyPart in laKey)
                            {
                                loExistingIndex.Add(lsKeyPart, loIndex[lsKeyPart]);
                            }

                            loAggregateIndexDictionary.Add(lsAggregateKey, loExistingIndex);
                        }
                        else
                        {
                            loExistingIndex = loAggregateIndexDictionary[lsAggregateKey];
                            lnAggregateCount = MaxConvertLibrary.ConvertToInt(typeof(object), loExistingIndex["_AggregateCount"]);
                        }

                        foreach (string lsName in laPropertyNameList)
                        {
                            if (lsName.Contains("(") && !lsName.Contains("GROUPBY("))
                            {
                                string lsAlias = lsName;
                                string lsFunction = lsName;
                                if (lsName.Contains(":"))
                                {
                                    lsAlias = lsName.Substring(lsName.IndexOf(":") + 1);
                                    lsFunction = lsName.Substring(0, lsName.IndexOf(":"));
                                }

                                object loValue = this.EvaluateAggregateFunction(loExistingIndex, lsAlias, loIndex, lsFunction);
                                if (null != loValue)
                                {
                                    loExistingIndex.Add(lsAlias, loValue);
                                }
                            }
                        }

                        loExistingIndex["_AggregateCount"] = lnAggregateCount + 1;
                        
                    }

                    loIndexDictionary = loAggregateIndexDictionary;
                }

                laR = new MaxIndex[loIndexDictionary.Values.Count];
                loIndexDictionary.Values.CopyTo(laR, 0);
            }
            else
            {
                laR = new MaxIndex[loList.Count];
                for (int lnIndex = 0; lnIndex < loList.Count; lnIndex++)
                {
                    laR[lnIndex] = loList[lnIndex].MapIndex(laPropertyNameList);
                }
            }

            return laR;
        }

        /// <summary>
        /// Creates a list of indexes from a list of entities
        /// </summary>
        /// <param name="loList">List of entities</param>
        /// <param name="laPropertyNameList">List of property names to include</param>
        /// <returns></returns>
        public virtual MaxIndex[] MapIndexList2(MaxEntityList loList, params string[] laPropertyNameList)
        {
            MaxIndex[] laR = new MaxIndex[0];
            //// Check for properties that need special handling
            //// Function properties are indicated by @
            Dictionary<string, List<string>> loFunctionPropertyIndex = new Dictionary<string, List<string>>();
            //// Filter properties are indicated by :
            Dictionary<string, string> loFilterPropertyIndex = new Dictionary<string, string>();
            //// Index properties are any other properties that are not in the key
            List<string> loIndexPropertyList = new List<string>();
            List<string> loDataNameKeyList = new List<string>(this.Data.DataModel.DataNameKeyList);
            if (null != laPropertyNameList)
            {
                foreach (string lsPropertyName in laPropertyNameList)
                {
                    string[] laPropertyName = lsPropertyName.Split(new char[] { '@' }, StringSplitOptions.RemoveEmptyEntries);
                    if (laPropertyName.Length == 2)
                    {
                        if (!loFunctionPropertyIndex.ContainsKey(laPropertyName[0]))
                        {
                            loFunctionPropertyIndex.Add(laPropertyName[0], new List<string>());
                        }

                        loFunctionPropertyIndex[laPropertyName[0]].Add(laPropertyName[1]);
                    }
                    else
                    {
                        laPropertyName = lsPropertyName.Split(new char[] { ':' }, StringSplitOptions.RemoveEmptyEntries);
                        if (laPropertyName.Length == 2)
                        {
                            loFilterPropertyIndex.Add(laPropertyName[0], laPropertyName[1]);
                        }
                        else
                        {
                            string lsDataName = this.GetDataName(this.Data.DataModel, lsPropertyName);
                            if (!string.IsNullOrEmpty(lsDataName) && !loDataNameKeyList.Contains(lsDataName))
                            {
                                Type loType = this.Data.DataModel.GetValueType(lsDataName);
                                if (loType != typeof(MaxLongString))
                                {
                                    loIndexPropertyList.Add(lsPropertyName);
                                }
                            }
                        }
                    }
                }
            }

            if (loFunctionPropertyIndex.Count > 0)
            {
                Dictionary<string, MaxIndex> loIndexDictionary = new Dictionary<string, MaxIndex>();
                for (int lnE = 0; lnE < loList.Count; lnE++)
                {
                    MaxEntity loEntity = loList[lnE] as MaxEntity;
                    MaxIndex loIndex = loEntity.MapIndex(laPropertyNameList);
                    string lsIndexKey = string.Empty;
                    foreach (string lsIndexProperty in loIndexPropertyList)
                    {
                        if (!loFilterPropertyIndex.ContainsKey(lsIndexProperty) && !loFunctionPropertyIndex.ContainsKey(lsIndexProperty))
                        {
                            string lsValue = loIndex.GetValueString(lsIndexProperty);
                            lsIndexKey += lsValue;
                        }
                    }

                    foreach (string lsFilterProperty in loFilterPropertyIndex.Keys)
                    {
                        string lsValue = loIndex.GetValueString(lsFilterProperty);
                        string lsFormat = loFilterPropertyIndex[lsFilterProperty];
                        loIndex.Add(lsFilterProperty + ":" + lsFormat, lsValue);
                        string lsDataName = this.GetDataName(this.Data.DataModel, lsFilterProperty);
                        Type loType = this.Data.DataModel.GetValueType(lsDataName);
                        if (loType == typeof(DateTime))
                        {
                            DateTime ldDateTime = MaxConvertLibrary.ConvertToDateTimeUtc(typeof(object), lsValue);
                            double lnOffset = 0;
                            if (lsFormat.Contains(";"))
                            {
                                string[] laFormat = lsFormat.Split(new char[] { ';' }, StringSplitOptions.RemoveEmptyEntries);
                                if (laFormat.Length == 2)
                                {
                                    lsFormat = laFormat[0];
                                    //// Use Regex to determine if offset is a number
                                    Regex loRegex = new Regex(@"^-?[0-9]?[0-9\.]*$");
                                    if (loRegex.IsMatch(laFormat[1]))
                                    {
                                        lnOffset = MaxConvertLibrary.ConvertToDouble(typeof(object), laFormat[1]);
                                    }
                                    else
                                    {
#if net4_52 || netcore2_1
                                        TimeZoneInfo loTZ = TimeZoneInfo.FindSystemTimeZoneById(laFormat[1]);
                                        lnOffset = loTZ.GetUtcOffset(ldDateTime).TotalHours;
#else
                                        throw new MaxException("Time zone format not supported in this framework.");
#endif
                                    }

                                    ldDateTime = ldDateTime.AddHours(lnOffset);
                                }
                            }
                           
                            loIndex.Add(lsFilterProperty, ldDateTime.Date.AddHours(-1 * lnOffset).ToString("o", CultureInfo.InvariantCulture));
                            lsValue = ldDateTime.ToString(lsFormat);                            
                        }

                        loIndex.Add(lsFilterProperty + ":IndexKey", lsValue);
                        lsIndexKey += lsValue;
                    }

                    foreach (string lsFunctionProperty in loFunctionPropertyIndex.Keys)
                    {
                        foreach (string lsFunction in loFunctionPropertyIndex[lsFunctionProperty])
                        {
                            loIndex.Add(lsFunctionProperty + "@" + lsFunction, loIndex[lsFunctionProperty]);
                            if (lsFunction == "COUNT")
                            {
                                loIndex[lsFunctionProperty + "@" + lsFunction] = 1;
                            }
                        }
                    }

                    if (loIndexDictionary.ContainsKey(lsIndexKey))
                    {
                        foreach (string lsFunctionProperty in loFunctionPropertyIndex.Keys)
                        {
                            foreach (string lsFunction in loFunctionPropertyIndex[lsFunctionProperty])
                            {
                                if (lsFunction == "SUM" || lsFunction == "COUNT")
                                {
                                    double lnValue = MaxConvertLibrary.ConvertToDouble(typeof(object), loIndex[lsFunctionProperty + "@" + lsFunction]);
                                    double lnValueExisting = MaxConvertLibrary.ConvertToDouble(typeof(object), loIndexDictionary[lsIndexKey][lsFunctionProperty + "@" + lsFunction]);
                                    loIndexDictionary[lsIndexKey][lsFunctionProperty + "@" + lsFunction] = lnValueExisting + lnValue;
                                    if (lsFunction == "SUM")
                                    {
                                        loIndexDictionary[lsIndexKey][lsFunctionProperty] = lnValueExisting + lnValue;
                                    }
                                }
                            }

                            foreach (string lsFunction in loFunctionPropertyIndex[lsFunctionProperty])
                            {
                                if (lsFunction == "AVERAGE")
                                {
                                    double lnSum = MaxConvertLibrary.ConvertToDouble(typeof(object), loIndexDictionary[lsIndexKey][lsFunctionProperty + "@SUM"]);
                                    double lnCount = MaxConvertLibrary.ConvertToDouble(typeof(object), loIndexDictionary[lsIndexKey][lsFunctionProperty + "@COUNT"]);
                                    if (lnCount > 0)
                                    {
                                        loIndexDictionary[lsIndexKey][lsFunctionProperty + "@" + lsFunction] = lnSum / lnCount;
                                    }
                                    else
                                    {
                                        loIndexDictionary[lsIndexKey][lsFunctionProperty + "@" + lsFunction] = 0;
                                    }
                                }
                            }
                        }
                    }
                    else
                    {
                        loIndexDictionary.Add(lsIndexKey, loIndex);
                    }
                }

                laR = new MaxIndex[loIndexDictionary.Values.Count];
                loIndexDictionary.Values.CopyTo(laR, 0);
            }
            else
            {
                laR = new MaxIndex[loList.Count];
                for (int lnIndex = 0; lnIndex < loList.Count; lnIndex++)
                {
                    laR[lnIndex] = loList[lnIndex].MapIndex(laPropertyNameList);
                }
            }

            return laR;
        }

        /// <summary>
        /// Creates a new entity of the current type or base type that has a static "Create" method
        /// </summary>
        /// <returns>New Entity of the current type or null if no create method found</returns>
        public virtual MaxEntity CreateNew()
        {
            MaxEntity loR = null;
            Type loType = this.GetType();
            MethodInfo loCreateMethod = loType.GetMethod("Create");
            while (null == loCreateMethod && null != loType.BaseType)
            {
                loType = loType.BaseType;
                loCreateMethod = loType.GetMethod("Create");
            }

            if (null != loCreateMethod)
            {
                loR = loCreateMethod.Invoke(null, null) as MaxEntity;
            }

            return loR;
        }

        /// <summary>
        /// Creates a copy of the current max entity with all current data
        /// </summary>
        /// <returns></returns>
        public virtual MaxEntity Clone()
        {
            MaxEntity loR = this.CreateNew();
            if (null != loR)
            {
                loR.Load(this.GetData());
            }

            return loR;
        }

        public virtual void SetValue(PropertyInfo loProperty, string lsValue)
        {
            if (loProperty.CanWrite)
            {
                if (loProperty.PropertyType == typeof(double))
                {
                    loProperty.SetValue(this, MaxConvertLibrary.ConvertToDouble(typeof(object), lsValue), BindingFlags.Instance | BindingFlags.Public, null, null, null);
                }
                else if (loProperty.PropertyType == typeof(int))
                {
                    loProperty.SetValue(this, MaxConvertLibrary.ConvertToInt(typeof(object), lsValue), BindingFlags.Instance | BindingFlags.Public, null, null, null);
                }
                else if (loProperty.PropertyType == typeof(bool))
                {
                    loProperty.SetValue(this, MaxConvertLibrary.ConvertToBoolean(typeof(object), lsValue), BindingFlags.Instance | BindingFlags.Public, null, null, null);
                }
                else if (loProperty.PropertyType == typeof(string))
                {
                    loProperty.SetValue(this, MaxConvertLibrary.ConvertToString(typeof(object), lsValue), BindingFlags.Instance | BindingFlags.Public, null, null, null);
                }
                else if (loProperty.PropertyType == typeof(Guid))
                {
                    loProperty.SetValue(this, MaxConvertLibrary.ConvertToGuid(typeof(object), lsValue), BindingFlags.Instance | BindingFlags.Public, null, null, null);
                }
                else if (loProperty.PropertyType == typeof(DateTime))
                {
                    DateTime loDateTime = MaxConvertLibrary.ConvertToDateTimeUtc(typeof(object), lsValue);
                    loProperty.SetValue(this, loDateTime, BindingFlags.Instance | BindingFlags.Public, null, null, null);
                }
                else if (loProperty.PropertyType == typeof(MaxIndex))
                {
                    MaxIndex loMaxIndex = MaxConvertLibrary.DeserializeObject(typeof(object), lsValue, typeof(MaxIndex)) as MaxIndex;
                    if (null != loMaxIndex)
                    {
                        loProperty.SetValue(this, loMaxIndex, BindingFlags.Instance | BindingFlags.Public, null, null, null);
                    }
                }
                else
                {
                    loProperty.SetValue(this, lsValue, BindingFlags.Instance | BindingFlags.Public, null, null, null);
                }
            }
        }


        /// <summary>
        /// Checks to see if the property name is in the list
        /// </summary>
        /// <param name="laPropertyNameList"></param>
        /// <param name="lsPropertyName"></param>
        /// <returns></returns>
        protected virtual bool HasIndexPropertyName(string[] laPropertyNameList, string lsPropertyName)
        {
            bool lbR = false;
            //// Force naming of properties for mapping to prevent stack overflow
            if (null != laPropertyNameList && laPropertyNameList.Length > 0)
            {
                foreach (string lsPropertyNameCheck in laPropertyNameList)
                {
                    if (lsPropertyNameCheck == this.GetType().ToString() + "." + lsPropertyName ||
                        lsPropertyNameCheck == lsPropertyName)
                    {
                        lbR = true;
                    }
                }
            }

            return lbR;
        }

#if net4_52 || netcore1 || netstandard1_2

        /// <summary>
        /// Gets a property name when specified with a function to the property
        /// </summary>
        /// <typeparam name="T">Generic placeholder for the property type</typeparam>
        /// <param name="loFunction">Function that specifies the type</param>
        /// <returns>Name of the property</returns>
        public string GetPropertyName<T>(Expression<Func<T>> loFunction)
        {
            string lsR = string.Empty;
            if (null != loFunction)
            {
                MemberExpression loBody = loFunction.Body as MemberExpression;
                if (null != loBody)
                {
                    lsR = loBody.Member.Name;
                }
            }

            return lsR;
        }
#endif

        /// <summary>
        /// Gets the data name (column name for tables) that matches the property name
        /// </summary>
        /// <param name="loDataModel">Data model to use for mapping</param>
        /// <param name="lsPropertyName">Name of the property</param>
        /// <returns>Name of the data name.  If there are more than one, the names are separated by a tab character</returns>
        protected virtual string GetDataName(MaxDataModel loDataModel, string lsPropertyName)
        {
            string lsR = string.Empty;
            if (loDataModel.IsStored(lsPropertyName))
            {
                lsR = lsPropertyName;
            }
            else
            {
                FieldInfo[] laFieldInfo = loDataModel.GetType().GetFields(BindingFlags.Static | BindingFlags.Public);
                foreach (FieldInfo loFieldInfo in laFieldInfo)
                {
                    if (loFieldInfo.Name == lsPropertyName)
                    {
                        lsR = loFieldInfo.GetValue(loDataModel) as string;
                    }
                }
            }

            return lsR;
        }

        /// <summary>
        /// Maps the data names based on the property names
        /// </summary>
        /// <param name="loDataModel">DataModel to use for mapping</param>
        /// <param name="laPropertyNameList">Names of propterties needed</param>
        /// <returns></returns>
        protected virtual string[] GetDataNameList(MaxDataModel loDataModel, string[] laPropertyNameList)
        {
            MaxIndex loDataNameIndex = new MaxIndex();
            if (null != laPropertyNameList && laPropertyNameList.Length > 0)
            {
                foreach (string lsPropertyName in laPropertyNameList)
                {
                    string lsDataName = this.GetDataName(loDataModel, lsPropertyName);
                    if (!string.IsNullOrEmpty(lsDataName))
                    {
                        string[] laDataName = lsDataName.Split('\t');
                        foreach (string lsName in laDataName)
                        {
                            loDataNameIndex.Add(lsName, true);
                        }
                    }
                }

                //// Make sure everything that makes up the DataKey is also included so that Streams can be loaded
                foreach (string lsDataName in this.Data.DataModel.DataNameKeyList)
                {
                    if (!loDataNameIndex.Contains(lsDataName))
                    {
                        loDataNameIndex.Add(lsDataName, true);
                    }
                }
            }

            return loDataNameIndex.GetSortedKeyList();
        }

        /// <summary>
        /// Gets sorting info for the data layer
        /// </summary>
        /// <param name="loDataModel"></param>
        /// <param name="lsPropertySort"></param>
        /// <returns></returns>
        protected virtual string GetOrderBy(MaxDataModel loDataModel, string lsPropertySort)
        {
            string lsR = string.Empty;
            if (!string.IsNullOrEmpty(lsPropertySort))
            {
                string[] laPropertySort = lsPropertySort.Split(',');
                foreach (string lsProperty in laPropertySort)
                {
                    string[] laProperty = lsProperty.Split(' ');
                    string lsDataName = this.GetDataName(loDataModel, laProperty[0]);
                    if (string.IsNullOrEmpty(lsDataName))
                    {
                        lsR = null;
                    }
                    else if (null != lsR)
                    {
                        string[] laDataName = lsDataName.Split('\t');
                        if (laDataName.Length > 1) 
                        {
                            lsR = null;
                        }
                        else
                        {
                            if (lsR.Length > 0)
                            {
                                lsR += ",";
                            }

                            lsR += lsDataName;
                            if (laProperty.Length > 1)
                            {
                                lsR += " " + laProperty[1];
                            }
                        }
                    }
                }
            }

            return lsR;
        }

        /// <summary>
        /// Sorts a list based on a property
        /// </summary>
        /// <param name="loList">List of entities to be stored</param>
        /// <param name="lsPropertySort">Property information used to sort</param>
        /// <param name="lsOrderBy">Order by query used to to get the list of entities</param>
        /// <returns></returns>
        protected virtual MaxEntityList GetSorted(MaxEntityList loList, string lsPropertySort)
        {
            if (string.IsNullOrEmpty(lsPropertySort))
            {
                return loList;
            }
            else
            {
                MaxEntityList loR = new MaxEntityList(loList.EntityType);
                string[] laPropertySort = lsPropertySort.Split(',');
                System.Collections.Generic.SortedList<string, MaxEntity> loSortedList = new System.Collections.Generic.SortedList<string, MaxEntity>();
                //// TODO: Allow for descending sorts
                for (int lnE = 0; lnE < loList.Count; lnE++)
                {
                    string lsSort = string.Empty;
                    foreach (string lsProperty in laPropertySort)
                    {
                        string[] laProperty = lsProperty.Split(' ');
                        PropertyInfo loPropertyInfo = loList[lnE].GetType().GetProperty(laProperty[0].Trim());
                        if (null != loPropertyInfo)
                        {
                            object loValue = loPropertyInfo.GetValue(loList[lnE], null);
                            lsSort += MaxConvertLibrary.ConvertToSortString(typeof(object), loValue);
                        }
                    }

                    lsSort += loList[lnE].GetDefaultSortString();
                    loSortedList.Add(lsSort, loList[lnE]);
                }

                foreach (MaxEntity loEntity in loSortedList.Values)
                {
                    loR.Add(loEntity);
                }

                return loR;
            }
        }

        /// <summary>
        /// Loads all entities of this type
        /// </summary>
        /// <returns>List of entities</returns>
        public virtual MaxEntityList LoadAll(params string[] laPropertyNameList)
        {
            string[] laDataNameList = this.GetDataNameList(this.Data.DataModel, laPropertyNameList);
            MaxData loData = new MaxData(this.Data.DataModel);
            MaxDataList loDataList = MaxBaseReadRepository.Select(loData, this.GetDataQuery(), 0, 0, string.Empty, laDataNameList);
            MaxEntityList loEntityList = MaxEntityList.Create(this.GetType(), loDataList);
            return loEntityList;
        }

        /// <summary>
        /// Loads all entities of this type into cache
        /// </summary>
        /// <returns>List of entities</returns>
        public virtual MaxEntityList LoadAllCache(params string[] laPropertyNameList)
        {
            MaxData loData = new MaxData(this.Data.DataModel);
            MaxDataQuery loDataQuery = this.GetDataQuery();
            return this.LoadAllByPageCache(loData, 0, 0, string.Empty, loDataQuery, laPropertyNameList);
        }

        /// <summary>
        /// Gets the default data query for this entity
        /// </summary>
        /// <returns>Data query for anything with this type</returns>
        protected virtual MaxDataQuery GetDataQuery()
        {
            MaxDataQuery loR = new MaxDataQuery();
            return loR;
        }

        /// <summary>
        /// Loads all entities for a particular page using a data query
        /// </summary>
        /// <param name="lnPageIndex"></param>
        /// <param name="lnPageSize"></param>
        /// <param name="lsPropertySort"></param>
        /// <param name="loDataQuery"></param>
        /// <param name="laPropertyNameList"></param>
        /// <returns></returns>
        protected virtual MaxEntityList LoadAllByPage(MaxData loData, int lnPageIndex, int lnPageSize, string lsPropertySort, MaxDataQuery loDataQuery, params string[] laPropertyNameList)
        {
            string lsOrderBy = this.GetOrderBy(loData.DataModel, lsPropertySort);
            string[] laDataNameList = this.GetDataNameList(loData.DataModel, laPropertyNameList);
            MaxDataList loDataList = MaxBaseReadRepository.Select(loData, loDataQuery, lnPageIndex, lnPageSize, lsOrderBy, laDataNameList);
            MaxEntityList loR = MaxEntityList.Create(this.GetType(), loDataList);
            if (string.IsNullOrEmpty(lsOrderBy))
            {
                loR = this.GetSorted(loR, lsPropertySort);
            }

            return loR;
        }

        /// <summary>
        /// Loads all entities for a particular page using a data query
        /// </summary>
        /// <param name="lnPageIndex"></param>
        /// <param name="lnPageSize"></param>
        /// <param name="lsPropertySort"></param>
        /// <param name="loDataQuery"></param>
        /// <param name="laPropertyNameList"></param>
        /// <returns></returns>
        protected virtual MaxEntityList LoadAllByPageCache(MaxData loData, int lnPageIndex, int lnPageSize, string lsPropertySort, MaxDataQuery loDataQuery, params string[] laPropertyNameList)
        {
            string lsCacheDataListKey = "LoadAllByPage";
            if (null != loData)
            {
                lsCacheDataListKey += "/DH=" + MaxEncryptionLibrary.GetHash(typeof(object), MaxEncryptionLibrary.MD5Hash, loData.ToString());
            }

            if (lnPageIndex > 0)
            {
                lsCacheDataListKey += "/PI=" + lnPageIndex.ToString();
            }

            if (lnPageSize > 0)
            {
                lsCacheDataListKey += "/PS=" + lnPageSize.ToString();
            }

            if (!string.IsNullOrEmpty(lsPropertySort))
            {
                lsCacheDataListKey += "/PSH=" + MaxEncryptionLibrary.GetHash(typeof(object), MaxEncryptionLibrary.MD5Hash, lsPropertySort);
            }

            if (null != laPropertyNameList && laPropertyNameList.Length > 0)
            {
                lsCacheDataListKey += "/PNLH=" + MaxEncryptionLibrary.GetHash(typeof(object), MaxEncryptionLibrary.MD5Hash, string.Concat(laPropertyNameList));
            }

            if (null != loDataQuery && loDataQuery.GetQuery().Length > 0)
            {
                lsCacheDataListKey += "/DQH=" + MaxEncryptionLibrary.GetHash(typeof(object), MaxEncryptionLibrary.MD5Hash, loDataQuery.ToString());
            }

            lsCacheDataListKey = this.GetCacheKey(lsCacheDataListKey);
            MaxDataList loDataList = MaxCacheRepository.Get(this.GetType(), lsCacheDataListKey, typeof(MaxDataList)) as MaxDataList;
            string lsOrderBy = this.GetOrderBy(loData.DataModel, lsPropertySort);
            if (null == loDataList)
            {
                string[] laDataNameList = this.GetDataNameList(loData.DataModel, laPropertyNameList);
                loDataList = MaxBaseReadRepository.Select(loData, loDataQuery, lnPageIndex, lnPageSize, lsOrderBy, laDataNameList);
                MaxCacheRepository.Set(this.GetType(), lsCacheDataListKey, loDataList, this.GetCacheExpire());
            }
            else
            {
                for (int lnD = 0; lnD < loDataList.Count; lnD++)
                {
                    loDataList[lnD].ClearChanged();
                }
            }

            MaxEntityList loR = MaxEntityList.Create(this.GetType(), loDataList);
            if (string.IsNullOrEmpty(lsOrderBy))
            {
                loR = this.GetSorted(loR, lsPropertySort);
            }

            return loR;
        }

        /// <summary>
        /// Loads all entities matching the property
        /// </summary>
        /// <param name="lsPropertyName">Name of the property</param>
        /// <param name="loPropertyValue">Value to match</param>
        /// <returns>List of matching entities</returns>
        public virtual MaxEntityList LoadAllByProperty(string lsPropertyName, object loPropertyValue, params string[] laPropertyNameList)
        {
            MaxEntityList loR = new MaxEntityList(this.GetType());
            string lsDataName = this.GetDataName(this.Data.DataModel, lsPropertyName);
            if (!string.IsNullOrEmpty(lsDataName))
            {
                MaxData loData = new MaxData(this.Data);
                MaxDataQuery loDataQuery = this.GetDataQuery();
                loDataQuery.StartGroup();
                loDataQuery.AddFilter(lsDataName, "=", loPropertyValue);
                loDataQuery.EndGroup();
                loR = this.LoadAllByPage(loData, 0, 0, string.Empty, loDataQuery, laPropertyNameList);
            }

            return loR;
        }

        /// <summary>
        /// Loads all entities matching the property and caches them
        /// </summary>
        /// <param name="lsPropertyName">Name of the property</param>
        /// <param name="loPropertyValue">Value to match</param>
        /// <returns>List of matching entities</returns>
        public virtual MaxEntityList LoadAllByPropertyCache(string lsPropertyName, object loPropertyValue, params string[] laPropertyNameList)
        {
            MaxEntityList loR = new MaxEntityList(this.GetType());
            string lsDataName = this.GetDataName(this.Data.DataModel, lsPropertyName);
            if (!string.IsNullOrEmpty(lsDataName))
            {
                MaxData loData = new MaxData(this.Data);
                MaxDataQuery loDataQuery = this.GetDataQuery();
                loDataQuery.StartGroup();
                loDataQuery.AddFilter(lsDataName, "=", loPropertyValue);
                loDataQuery.EndGroup();
                loR = this.LoadAllByPageCache(loData, 0, 0, string.Empty, loDataQuery, laPropertyNameList);
            }

            return loR;
        }

        /// <summary>
        /// Loads all entities for a particular page worth of data
        /// </summary>
        /// <param name="lnPageIndex">Page to return</param>
        /// <param name="lnPageSize">Items per page</param>
        /// <param name="lsPropertySort">Sort information</param>
        /// <param name="laPropertyNameList"></param>
        /// <returns>List of matching entities</returns>
        public virtual MaxEntityList LoadAllByPage(int lnPageIndex, int lnPageSize, string lsPropertySort, params string[] laPropertyNameList)
        {
            MaxData loData = new MaxData(this.Data);
            MaxEntityList loR = this.LoadAllByPage(loData, lnPageIndex, lnPageSize, lsPropertySort, this.GetDataQuery(), laPropertyNameList);
            return loR;
        }

        /// <summary>
        /// Loads all entities for a particular page using a filter
        /// </summary>
        /// <param name="lnPageIndex"></param>
        /// <param name="lnPageSize"></param>
        /// <param name="lsPropertySort"></param>
        /// <param name="loFilter"></param>
        /// <param name="laPropertyNameList"></param>
        /// <returns></returns>
        public virtual MaxEntityList LoadAllByPageFilter(int lnPageIndex, int lnPageSize, string lsPropertySort, MaxIndex loFilter, params string[] laPropertyNameList)
        {
            MaxDataQuery loDataQuery = this.GetDataQuery();
            if (loFilter.Count > 0)
            {
                string[] laKey = loFilter.GetSortedKeyList();
                if (laKey.Length > 0)
                {
                    MaxDataQuery loDataQueryFilter = new MaxDataQuery();
                    string lsNextCondition = string.Empty;
                    foreach (string lsKey in laKey)
                    {
                        MaxIndex loDetail = loFilter[lsKey] as MaxIndex;
                        string lsValue = loDetail.GetValueString("Value");
                        string lsPropertyName = loDetail.GetValueString("Name");
                        string lsDataName = this.GetDataName(this.Data.DataModel, lsPropertyName);
                        if (!string.IsNullOrEmpty(lsDataName) && !string.IsNullOrEmpty(lsValue))
                        {
                            if (!string.IsNullOrEmpty(lsNextCondition)) {
                                loDataQueryFilter.AddCondition(lsNextCondition);
                                lsNextCondition = string.Empty;
                            }

                            if (loDetail.Contains("StartGroup"))
                            {
                                loDataQueryFilter.StartGroup();
                            }

                            loDataQueryFilter.AddFilter(lsDataName, loDetail.GetValueString("Operator"), lsValue);
                            if (loDetail.Contains("EndGroup"))
                            {
                                loDataQueryFilter.EndGroup();
                            }

                            if (loDetail.Contains("Condition") && !string.IsNullOrEmpty(loDetail.GetValueString("Condition")))
                            {
                                lsNextCondition = loDetail.GetValueString("Condition");                                
                            }
                        }
                    }

                    Object[] laDataQueryFilter = loDataQueryFilter.GetQuery();
                    if (laDataQueryFilter.Length > 0)
                    {
                        loDataQuery.StartGroup();
                        foreach (object loQuery in laDataQueryFilter)
                        {
                            loDataQuery.Add(loQuery);
                        }

                        loDataQuery.EndGroup();
                    }
                }
            }

            MaxData loData = new MaxData(this.Data);
            MaxEntityList loR = this.LoadAllByPage(loData, lnPageIndex, lnPageSize, lsPropertySort, loDataQuery, laPropertyNameList);
            return loR;
        }

        /// <summary>
        /// Loads all entities for a particular page using a filter
        /// </summary>
        /// <param name="loFilter"></param>
        /// <param name="laPropertyNameList"></param>
        /// <returns></returns>
        public virtual MaxEntityList LoadAllByFilterCache(MaxIndex loFilter, params string[] laPropertyNameList)
        {
            MaxDataQuery loDataQuery = this.GetDataQuery();
            if (loFilter.Count > 0)
            {
                string[] laKey = loFilter.GetSortedKeyList();
                if (laKey.Length > 0)
                {
                    MaxDataQuery loDataQueryFilter = new MaxDataQuery();
                    foreach (string lsKey in laKey)
                    {
                        MaxIndex loDetail = loFilter[lsKey] as MaxIndex;
                        if (loDetail.Contains("StartGroup"))
                        {
                            loDataQueryFilter.StartGroup();
                        }

                        string lsPropertyName = loDetail.GetValueString("Name");
                        string lsDataName = this.GetDataName(this.Data.DataModel, lsPropertyName);
                        if (!string.IsNullOrEmpty(lsDataName))
                        {
                            loDataQueryFilter.AddFilter(lsDataName, loDetail.GetValueString("Operator"), loDetail.GetValueString("Value"));
                        }

                        if (loDetail.Contains("EndGroup"))
                        {
                            loDataQueryFilter.EndGroup();
                        }

                        if (loDetail.Contains("Condition") && !string.IsNullOrEmpty(loDetail.GetValueString("Condition")))
                        {
                            loDataQueryFilter.AddCondition(loDetail.GetValueString("Condition"));
                        }
                    }

                    Object[] laDataQueryFilter = loDataQueryFilter.GetQuery();
                    if (laDataQueryFilter.Length > 0)
                    {
                        loDataQuery.StartGroup();
                        foreach (object loQuery in laDataQueryFilter)
                        {
                            loDataQuery.Add(loQuery);
                        }

                        loDataQuery.EndGroup();
                    }
                }
            }

            MaxData loData = new MaxData(this.Data);
            MaxEntityList loR = this.LoadAllByPageCache(loData, 0, 0, string.Empty, loDataQuery, laPropertyNameList);
            return loR;
        }

        /// <summary>
        /// Loads the entity that matches the key
        /// </summary>
        /// <param name="lsDataKey"></param>
        /// <param name="laPropertyNameList"></param>
        /// <returns></returns>
        public virtual bool LoadByDataKeyCache(string lsDataKey, params string[] laPropertyNameList)
        {
            bool lbR = false;
            if (!string.IsNullOrEmpty(lsDataKey))
            {
                string lsCacheDataKey = "LoadByDataKey/" + lsDataKey;
                if (null != laPropertyNameList && laPropertyNameList.Length > 0)
                {
                    lsCacheDataKey += "/PNLH=" + MaxEncryptionLibrary.GetHash(typeof(object), MaxEncryptionLibrary.MD5Hash, string.Concat(laPropertyNameList));
                }

                lsCacheDataKey = this.GetCacheKey(lsCacheDataKey);
                MaxData loData = MaxCacheRepository.Get(this.GetType(), lsCacheDataKey, typeof(MaxData)) as MaxData;
                if (null != loData)
                {
                    lbR = this.Load(loData);
                }
                else
                {
                    //// Set each DataName that makes up the DataKey
                    loData = new MaxData(this.Data);
                    string[] laDataKey = lsDataKey.Split(new string[] { this.Data.DataModel.KeySeparator }, StringSplitOptions.None);
                    if (laDataKey.Length == this.Data.DataModel.DataNameKeyList.Length)
                    {
                        for (int lnD = 0; lnD < this.Data.DataModel.DataNameKeyList.Length; lnD++)
                        {
                            string lsDataName = this.Data.DataModel.DataNameKeyList[lnD];
                            object loValue = laDataKey[lnD];
                            if (null != loValue)
                            {
                                if (this.Data.DataModel.GetValueType(lsDataName) == typeof(Guid) && loValue is string)
                                {
                                    string lsGuidPattern = @"^[0-9a-fA-F]{8}-[0-9a-fA-F]{4}-[0-9a-fA-F]{4}-[0-9a-fA-F]{4}-[0-9a-fA-F]{12}$";
                                    if (Regex.IsMatch((string)loValue, lsGuidPattern))
                                    {
                                        loValue = new Guid(((string)loValue));
                                    }
                                }

                                loData.Set(lsDataName, loValue);
                            }
                        }
                    }

                    MaxDataQuery loDataQuery = this.GetDataQuery();
                    string[] laDataNameList = this.GetDataNameList(loData.DataModel, laPropertyNameList);
                    MaxDataList loDataList = MaxBaseReadRepository.Select(loData, loDataQuery, 0, 0, string.Empty, laDataNameList);
                    if (loDataList.Count == 1)
                    {
                        lbR = this.Load(loDataList[0]);
                        MaxCacheRepository.Set(this.GetType(), lsCacheDataKey, loDataList[0], this.GetCacheExpire());
                    }
                }
            }

            return lbR;
        }

        /// <summary>
        /// Gets the time to expire the cache for this entity.
        /// </summary>
        /// <returns></returns>
        public virtual DateTime GetCacheExpire()
        {
            DateTime ldR = DateTime.UtcNow.AddHours(1);
            if (null != this.Data)
            {
                string lsMaxCacheExpire = this.Data.Get("_MaxCacheExpire") as string;                
                if (null != lsMaxCacheExpire)
                {
                    DateTime ldMaxCacheExpire = DateTime.MinValue;
                    if (DateTime.TryParse(lsMaxCacheExpire, out ldMaxCacheExpire))
                    {
                        ldR = ldMaxCacheExpire;
                    }
                }
            }

            return ldR;
        }

        /// <summary>
        /// Gets a copy of the data associated with this entity.
        /// </summary>
        /// <returns>Copy of data for this entity.</returns>
        public virtual MaxData GetData()
        {
            MaxData loR = this.Data.Clone();
            loR.ClearChanged();
            return loR;
        }

        /// <summary>
        /// Loads the Data for this entity
        /// </summary>
        /// <param name="loData">Data for the entity</param>
        /// <returns>True if loaded, false if not loaded</returns>
        public virtual bool Load(MaxData loData)
        {
            if (null != loData && null != loData.DataModel)
            {
                this._oData = loData;
                this._oDataModelType = loData.DataModel.GetType();
                this._oUnSerializedObjectIndex = new MaxIndex();
                return true;
            }

            return false;
        }

        /// <summary>
        /// Gets a string that can be used to sort a list of this entity
        /// </summary>
        /// <returns>Random string to use to prevent duplicates.</returns>
        public virtual string GetDefaultSortString()
        {
            return Guid.NewGuid().ToString();
        }

        /// <summary>
        /// Gets a key that can be used for caching this entity.
        /// </summary>
        /// <returns>a key to use for the cache</returns>
        public virtual string GetCacheKey(string lsKey)
        {
            string lsR = string.Empty;
            if (null != this.Data && null != this.Data.DataModel)
            {
                lsR = new MaxData(this.Data.DataModel).GetCacheKey(lsKey);
            }

            return lsR;
        }

        /// <summary>
        /// Sets the current list this entity is part of.
        /// </summary>
        /// <param name="loEntityList">Current list this entity is part of.</param>
        public virtual void SetList(MaxEntityList loEntityList)
        {
            this._oEntityList = loEntityList;
        }

        /// <summary>
        /// Sets the name of the repository provider
        /// </summary>
        /// <param name="lsName">Name of the repository provider</param>
        public virtual void SetRepositoryProviderName(string lsName)
        {
            this.Data.Set("MaxRepositoryName", lsName);
        }

        /// <summary>
        /// Exports the entity data to a JavaScript Object Notation text representation
        /// </summary>
        /// <returns>JavaScript Object Notation string representing data for entity</returns>
        public virtual string ExportToString()
        {
            MaxIndex loIndex = new MaxIndex();
            foreach (string lsDataName in this.Data.DataModel.DataNameList)
            {
                loIndex.Add(lsDataName, this.Get(lsDataName));
            }

            string lsR = MaxConvertLibrary.SerializeObjectToString(this.GetType(), loIndex);
            return lsR;
        }

        /// <summary>
        /// Loads JavaScript Object Notation data that was originally exported
        /// </summary>
        /// <param name="lsData">JavaScript Object Notation representation of entity</param>
        /// <returns>True if successful.</returns>
        public virtual bool Load(string lsData)
        {
            MaxIndex loIndex = MaxConvertLibrary.DeserializeObject(this.GetType(), lsData, typeof(MaxIndex)) as MaxIndex;
            if (null != loIndex)
            {
                string[] laKey = loIndex.GetSortedKeyList();
                foreach (string lsDataName in laKey)
                {
                    this.Set(lsDataName, loIndex[lsDataName]);
                }

                return true;
            }

            return false;
        }

        /// <summary>
        /// Sets the datamodel properties for any initial data before attemping an insert
        /// </summary>
        protected virtual void SetInitial()
        {
        }

        /// <summary>
        /// Sets the datamodel properties for any extended data
        /// </summary>
        protected virtual void SetProperties()
        {
            string[] laKey = this._oUnSerializedObjectIndex.GetSortedKeyList();
            foreach (string lsDataName in laKey)
            {
                this.SetObject(lsDataName, this._oUnSerializedObjectIndex[lsDataName]);
            }
        }

        /// <summary>
		/// Sets a value
		/// </summary>
		/// <param name="lsDataName">Key to track the value</param>
		/// <param name="loValue">The value to set</param>
        protected virtual void Set(string lsDataName, object loValue)
        {
            if (this.Data.DataModel.GetAttributeSetting(lsDataName, MaxDataModel.AttributeIsEncrypted) &&
                null != loValue && (loValue is string || loValue is byte[]))
            {
                // Convert to byte array so it can be encrypted
                if (loValue is string)
                {
                    loValue = MaxConvertLibrary.ConvertToByteArray(this.DataModelType, loValue);
                }

                //// Encrypt the value
                loValue = MaxEncryptionLibrary.Encrypt(this.GetType(), (byte[])loValue);

                //// Encode byte[] to string if this encrypted value is being stored as a string.
                if (this.Data.DataModel.GetValueType(lsDataName) == typeof(string) ||
                    this.Data.DataModel.GetValueType(lsDataName) == typeof(MaxShortString) ||
                    this.Data.DataModel.GetValueType(lsDataName) == typeof(MaxLongString))
                {
                    loValue = Convert.ToBase64String((byte[])loValue);
                }
            }
            else if (null == loValue)
            {
                if (this.Data.DataModel.GetValueType(lsDataName) == typeof(string) ||
                    this.Data.DataModel.GetValueType(lsDataName) == typeof(MaxShortString) ||
                    this.Data.DataModel.GetValueType(lsDataName) == typeof(MaxLongString))
                {
                    loValue = string.Empty;
                }
            }
            else if (this.Data.DataModel.GetValueType(lsDataName) == typeof(MaxLongString))
            {
                //// Run the GetString method to update the current data if it comes from a stream so that if needing to save it to a stream it can be compared and not marked as changed.
                this.GetString(lsDataName);
            }

            this.Data.Set(lsDataName, loValue);
        }

        /// <summary>
        /// Gets the value based on the data in the datamodel
        /// </summary>
        /// <param name="lsDataName">Key to track the value</param>
        /// <returns>Value for the data</returns>
        protected virtual object Get(string lsDataName)
        {
            return this.Data.Get(lsDataName);
        }

        /// <summary>
        /// Sets a bit flag to true or false.
        /// </summary>
        /// <param name="lsDataName">Key used to track the value.</param>
        /// <param name="lnFlag">The flag to set.  Flag 0 to Flag 64.</param>
        /// <param name="lbSetting">Setting for the flag.</param>
        protected void SetBit(string lsDataName, short lnFlag, bool lbSetting)
        {
            long lnValue = this.GetLong(lsDataName);
            if (lnValue == long.MinValue)
            {
                lnValue = 0;
            }

            long lnBit = (long)Math.Pow(2, lnFlag);
            if (lbSetting)
            {
                lnValue |= lnBit;
            }
            else
            {
                lnValue &= ~lnBit;
            }

            this.Set(lsDataName, lnValue);
        }

        /// <summary>
        /// Gets the value for a key as a date time
        /// Initialized to DateTime.MinValue if null.
        /// </summary>
        /// <param name="lsDataName">Key for the index</param>
        /// <returns>Value matching the key as a date and time</returns>
        protected DateTime GetDateTime(string lsDataName)
        {
            object loValue = this.Get(lsDataName);
            return MaxConvertLibrary.ConvertToDateTime(this.DataModelType, loValue);
        }

        /// <summary>
        /// Gets the value for a key as a Unique Identifier.
        /// Initialized to Empty if null.
        /// </summary>
        /// <param name="lsDataName">Key for the index</param>
        /// <returns>Value matching the key as a Unique Identifier.</returns>
        protected Guid GetGuid(string lsDataName)
        {
            object loValue = this.Get(lsDataName);
            return MaxConvertLibrary.ConvertToGuid(this.DataModelType, loValue);
        }

        /// <summary>
        /// Gets the value for a key as a string.
        /// Initialized to String.Empty if null.
        /// Initialized to String.Empty if null.
        /// </summary>
        /// <param name="lsDataName">Key for the index</param>
        /// <returns>Value matching the key cast as a string.</returns>
        protected string GetString(string lsDataName)
        {
            object loValue = this.Get(lsDataName);
            string lsR = string.Empty;
            if (loValue is string)
            {
                lsR = (string)loValue;
            }
            else
            {
                lsR = MaxConvertLibrary.ConvertToString(this.DataModelType, loValue);
            }

            bool lbIsStream = false;
            if (lsR.Equals(MaxDataModel.StreamStringIndicator))
            {
                Stream loStream = MaxStreamLibrary.StreamOpen(this.Data, lsDataName);
                try
                {
                    if (null != loStream)
                    {
                        StreamReader loReader = new StreamReader(loStream);
                        try
                        {
                            loValue = loReader.ReadToEnd();
                        }
                        finally
                        {
                            loReader.Dispose();
                        }
                    }
                }
                finally
                {
                    if (null != loStream)
                    {
                        loStream.Dispose();
                    }
                }                

                if (null != loValue)
                {
                    this.Data.Set(lsDataName, loValue);
                    this.Data.ClearChanged(lsDataName);
                    lbIsStream = true;
                }
            }

            bool lbIsEncrypted = false;
            if (this.Data.DataModel.GetAttributeSetting(lsDataName, MaxDataModel.AttributeIsEncrypted))
            {
                //// Decode string to byte[] if this encrypted value is being stored as a string.
                if (loValue is string)
                {
                    loValue = Convert.FromBase64String((string)loValue);
                }

                //// Decrypt the value to a byte array
                loValue = MaxEncryptionLibrary.Decrypt(this.GetType(), (byte[])loValue);
                lbIsEncrypted = true;
            }

            if (lbIsStream || lbIsEncrypted)
            {
                lsR = MaxConvertLibrary.ConvertToString(this.DataModelType, loValue);
            }

            return lsR;
        }

        /// <summary>
        /// Gets the value for a key as a boolean.
        /// Initialized to false if null.
        /// </summary>
        /// <param name="lsDataName">Key for the index</param>
        /// <returns>Value matching the key cast as a boolean.</returns>
        protected bool GetBoolean(string lsDataName)
        {
            object loValue = this.Get(lsDataName);
            return MaxConvertLibrary.ConvertToBoolean(this.DataModelType, loValue);
        }

        /// <summary>
        /// Gets the value for a key as an integer
        /// Initialized to false if null.
        /// </summary>
        /// <param name="lsDataName">Key for the index</param>
        /// <returns>Value matching the key cast as an integer</returns>
        protected int GetInt(string lsDataName)
        {
            object loValue = this.Get(lsDataName);
            return MaxConvertLibrary.ConvertToInt(this.DataModelType, loValue);
        }

        /// <summary>
        /// Gets the value for a key as an integer
        /// Initialized to false if null.
        /// </summary>
        /// <param name="lsDataName">Key for the index</param>
        /// <returns>Value matching the key cast as an integer</returns>
        protected double GetDouble(string lsDataName)
        {
            object loValue = this.Get(lsDataName);
            return MaxConvertLibrary.ConvertToDouble(this.DataModelType, loValue);
        }

        /// <summary>
        /// Gets the value for a key as an integer
        /// Initialized to false if null.
        /// </summary>
        /// <param name="lsDataName">Key for the index</param>
        /// <returns>Value matching the key cast as an integer</returns>
        protected long GetLong(string lsDataName)
        {
            object loValue = this.Get(lsDataName);
            return MaxConvertLibrary.ConvertToLong(this.DataModelType, loValue);
        }

        /// <summary>
        /// Gets a value indicating whether the bit is set to true or false
        /// </summary>
        /// <param name="lsDataName">Key to track the value</param>
        /// <param name="lnFlag">The flag to set.  Flag 0 to Flag 64.</param>
        /// <returns>The value of the bit.</returns>
        protected bool GetBit(string lsDataName, short lnFlag)
        {
            long lnValue = this.GetLong(lsDataName);
            if (lnValue == long.MinValue)
            {
                lnValue = 0;
            }

            long lnBit = (long)Math.Pow(2, lnFlag);
            return !((lnValue & lnBit) == 0);
        }

        /// <summary>
        /// Gets a stream of binary data
        /// </summary>
        /// <param name="lsDataName">Key to track the value</param>
        /// <returns>The stream of binary data.</returns>
        protected Stream GetStream(string lsDataName)
        {
            Stream loR = null;
            object loValue = this.Get(lsDataName);
            if (loValue is Stream && ((Stream)loValue).CanRead)
            {
                loR = (Stream)loValue;
                try
                {
                    if (loR.Position != 0)
                    {
                        if (loR.CanSeek)
                        {
                            loR.Position = 0;
                        }
                        else
                        {
                            loR = null;
                        }
                    }
                }
                catch
                {
                    loR = null;
                }
            }

            if (null == loR || !loR.CanRead)
            {
                loR = MaxStreamLibrary.StreamOpen(this.Data, lsDataName);
                if (null != loR)
                {
                    this.Data.Set(lsDataName, loR);
                    this.Data.ClearChanged(lsDataName);
                }
            }

            return loR;
        }

        /// <summary>
        /// Gets a byte array of binary data
        /// </summary>
        /// <param name="lsDataName">Key to track the value</param>
        /// <returns>The stream of binary data.</returns>
        protected byte[] GetByte(string lsDataName)
        {
            object loValue = this.Get(lsDataName);
            if (loValue is byte[] && ((byte[])loValue).Length == MaxDataModel.StreamByteIndicator.Length)
            {
                bool lbIsMatch = true;
                for (int lnV = 0; lnV < MaxDataModel.StreamByteIndicator.Length; lnV++)
                {
                    if (((byte[])loValue)[lnV] != MaxDataModel.StreamByteIndicator[lnV])
                    {
                        lbIsMatch = false;
                    }
                }

                if (lbIsMatch)
                {
                    loValue = MaxStreamLibrary.StreamOpen(this.Data, lsDataName);
                }
            }

            byte[] laR = null;
            if (loValue is Stream)
            {
                //// Allocating a new byte[long] generates a warning for MF43: "opcode 'conv.ovf.i' -- overflow will not throw exception"
                //// Cast down to a 32 bit integer if we can so we don't allocate an array that is too large for MF43.
                if (((Stream)loValue).Length < (long)int.MaxValue)
                {
                    try
                    {
                        int lnLength = (int)((Stream)loValue).Length;
                        laR = new byte[lnLength];
                        ((Stream)loValue).Read(laR, 0, lnLength);
                        this.Data.Set(lsDataName, laR);
                        this.Data.ClearChanged(lsDataName);
                    }
                    finally
                    {
                        if (null != loValue)
                        {
                            ((Stream)loValue).Dispose();
                        }
                    }
                }
            }
            else if (loValue is byte[])
            {
                laR = (byte[])loValue;
            }

            if (this.Data.DataModel.GetAttributeSetting(lsDataName, MaxDataModel.AttributeIsEncrypted))
            {
                //// Decrypt the value 
                laR = MaxEncryptionLibrary.Decrypt(this.GetType(), laR);
            }

            return laR;
        }

        /// <summary>
        /// Sets the Data Context Provider that will be used to manage this data
        /// </summary>
        /// <param name="loType">Type of the DataContext provider</param>
        protected void SetDataContextProviderType(Type loType)
        {
            this.Data.Set("IMaxDataContextProvider", loType);
        }

        /// <summary>
        /// Sets the Repository Provider that will be used to manage this data.
        /// </summary>
        /// <param name="loType">Type of the Repository provider</param>
        protected void SetRepositoryProviderType(Type loType)
        {
            this.Data.Set("IMaxRepositoryProvider", loType);
        }

        /// <summary>
        /// Gets an object that may be stored serialized.
        /// </summary>
        /// <param name="lsDataName">Key to use to look up the data.</param>
        /// <param name="loType">Type of object to get.</param>
        /// <returns>object related to key.</returns>
        protected virtual object GetObject(string lsDataName, Type loType)
        {
            if (this._oUnSerializedObjectIndex.Contains(lsDataName))
            {
                return this._oUnSerializedObjectIndex[lsDataName];
            }

            object loValue = this.Get(lsDataName);
            object loValueType = this.Data.DataModel.GetValueType(lsDataName);
            if (null != loValueType)
            {
                if (loValueType.Equals(typeof(byte[])) &&
                    loValue is byte[])
                {
                    loValue = null;
                    byte[] laValue = this.GetByte(lsDataName);
                    if (null != laValue &&
                        laValue != MaxDataModel.StreamByteIndicator)
                    {
                        loValue = MaxConvertLibrary.DeserializeObject(this.Data.DataModel.GetType(), laValue, loType);
                    }
                }
                else if ((loValueType.Equals(typeof(string)) ||
                    loValueType.Equals(typeof(MaxLongString))) &&
                    loValue is string)
                {
                    loValue = null;
                    string lsValue = this.GetString(lsDataName);
                    if (null != lsValue && 
                        lsValue != MaxDataModel.StreamStringIndicator) 
                    {
                        loValue = MaxConvertLibrary.DeserializeObject(this.Data.DataModel.GetType(), lsValue, loType);
                    }
                }
            }

            this._oUnSerializedObjectIndex.Add(lsDataName, loValue);
            if (null != loValue)
            {
                this.SetObject(lsDataName, loValue);
                this.Data.ClearChanged(lsDataName);
            }           

            return loValue;
        }

        /// <summary>
        /// Serializes an object and saves the value.
        /// </summary>
        /// <param name="lsDataName">Key to use to save the data.</param>
        /// <param name="loValue">Value of the object.</param>
        protected virtual void SetObject(string lsDataName, object loValue)
        {
            this._oUnSerializedObjectIndex.Add(lsDataName, loValue);
            object loValueType = this.Data.DataModel.GetValueType(lsDataName);
            if (null != loValueType && loValueType.Equals(typeof(byte[])))
            {
                this.Set(lsDataName, MaxConvertLibrary.SerializeObjectToBinary(this.Data.DataModel.GetType(), loValue));
            }
            else if (null != loValueType && (loValueType.Equals(typeof(string)) ||
                loValueType.Equals(typeof(MaxLongString))))
            {
                this.Set(lsDataName, MaxConvertLibrary.SerializeObjectToString(this.Data.DataModel.GetType(), loValue));
            }
            else
            {
                this.Set(lsDataName, loValue);
            }
        }

        public static int Insert(MaxEntityList loEntityList)
        {
            int lnR = 0;
            try
            {
                if (loEntityList.Count > 0)
                {
                    MaxEntity loEntity = null;
                    MaxData loData = loEntityList[0].GetData();
                    MaxDataList loDataList = new MaxDataList(loData.DataModel);
                    for (int lnE = 0; lnE < loEntityList.Count; lnE++)
                    {
                        loEntity = loEntityList[lnE];
                        loEntity.SetInitial();
                        loEntity.SetProperties();
                        loData = loEntity.GetData();
                        loDataList.Add(loData);
                    }

                    lnR = MaxBaseWriteRepository.Insert(loDataList);
                    if (null != loEntity)
                    {
                        //// Clear everything for this entity from the cache
                        string lsCacheKey = loEntity.GetCacheKey("LoadAll*");
                        MaxCacheRepository.Remove(loEntity.GetType(), lsCacheKey);
                    }
                }
            }
            catch (Exception loE)
            {
                MaxLogLibrary.Log(new MaxLogEntryStructure(typeof(MaxEntity), "Insert", MaxEnumGroup.LogError, "Error inserting List: {Message}", loE.Message));
                lnR = lnR |= 1;
            }

            return lnR;
        }
    }
}
