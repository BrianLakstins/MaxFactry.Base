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

    /// <summary>
    /// Base Business Layer Entity
    /// </summary>
    public abstract class MaxEntity
    {
        private string[] _aPropertyNameKeyList = null;

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
        /// Internal storage of base cache key
        /// </summary>
        private string _sCacheKey = null;

        /// <summary>
        /// Initializes a new instance of the MaxEntity class
        /// </summary>
        /// <param name="loData">object to hold data</param>
        public MaxEntity(MaxData loData)
        {
            loData.ClearChanged();
            this.Load(loData);
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
        /// Gets an index of property names and current values to use as a key for each record
        /// </summary>
        public virtual string[] PropertyNameKeyList
        {
            get
            {
                if (null == this._aPropertyNameKeyList)
                {
                    MaxIndex loPropertyNameIndex = new MaxIndex();
                    PropertyInfo[] laPropertyInfo = this.GetType().GetProperties(BindingFlags.Instance | BindingFlags.Public);
                    foreach (string lsDataName in this.Data.DataModel.DataNameKeyList)
                    {
                        foreach (PropertyInfo loPropertyInfo in laPropertyInfo)
                        {
                            //// The property name has to match the DataName for it to be considered part of the key.  
                            //// This property can be overridden if that is not the case.
                            if (lsDataName == this.GetDataName(this.Data.DataModel, loPropertyInfo.Name))
                            {
                                loPropertyNameIndex.Add(loPropertyInfo.Name, true);
                            }
                        }
                    }

                    this._aPropertyNameKeyList = loPropertyNameIndex.GetSortedKeyList();
                }

                return this._aPropertyNameKeyList;
            }
        }

        /// <summary>
        /// Gets a key for the entity
        /// </summary>
        public virtual string Key
        {
            get
            {
                string lsR = this.Data.DataModel.GetKey(this.Data);
                return lsR;
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
            OnInsertBefore();
            this.SetProperties();
            int lnLimit = 5;
            int lnTry = 0;
            bool lbR = MaxBaseWriteRepository.Insert(this.Data);
            while (!lbR && lnTry < lnLimit)
            {
                System.Threading.Thread.Sleep(100);
                lbR = MaxBaseWriteRepository.Insert(this.Data);
                lnTry++;
            }

            if (lbR)
            {
                string lsCacheKey = this.GetCacheKey() + "LoadByKey/" + this.Key;
                MaxCacheRepository.Set(this.GetType(), lsCacheKey, this.Data);
                lsCacheKey = this.GetCacheKey() + "LoadAll*";
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
            if (this.IsDataChanged && !string.IsNullOrEmpty(this.Key))
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
                    string lsCacheKey = this.GetCacheKey() + "LoadByKey/" + this.Key;
                    MaxCacheRepository.Set(this.GetType(), lsCacheKey, this.Data);
                    lsCacheKey = this.GetCacheKey() + "LoadAll*";
                    MaxCacheRepository.Remove(this.GetType(), lsCacheKey);
                    OnUpdateAfter();
                    return lbR;
                }
            }

            OnUpdateFail();
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
            if (!string.IsNullOrEmpty(this.Key))
            {
                lbR = MaxBaseWriteRepository.Delete(this.Data);
                if (lbR)
                {
                    foreach (string lsDataName in this.Data.DataModel.DataNameList)
                    {
                        if (this.Data.DataModel.GetValueType(lsDataName).Equals(typeof(Stream)))
                        {
                            MaxBaseWriteRepository.StreamDelete(this.Data, lsDataName);
                        }
                    }

                    string lsCacheKey = this.GetCacheKey() + "LoadAll*";
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
            foreach (string lsPropertyNameIndex in laPropertyNameIndex)
            {
                if (this.HasIndexPropertyName(laPropertyNameList, lsPropertyNameIndex))
                {
                    object loValue = null;
                    object loProperty = loPropertyIndex[lsPropertyNameIndex];
                    if (loProperty is PropertyInfo)
                    {
                        loValue = ((PropertyInfo)loProperty).GetValue(this, null);
                    }

                    if (loValue is MaxEntity)
                    {
                        loR.Add(lsPropertyNameIndex, ((MaxEntity)loValue).MapIndex(laPropertyNameList));
                    }
                    else if (loValue is double)
                    {
                        if (double.MinValue != (double)loValue)
                        {
                            loR.Add(lsPropertyNameIndex, loValue);
                        }
                        else
                        {
                            loR.Add(lsPropertyNameIndex, string.Empty);
                        }
                    }
                    else if (loValue is int)
                    {
                        if (int.MinValue != (int)loValue)
                        {
                            loR.Add(lsPropertyNameIndex, loValue);
                        }
                        else
                        {
                            loR.Add(lsPropertyNameIndex, string.Empty);
                        }
                    }
                    else if (loValue is Guid)
                    {
                        if (Guid.Empty != (Guid)loValue)
                        {
                            loR.Add(lsPropertyNameIndex, loValue);
                        }
                        else
                        {
                            loR.Add(lsPropertyNameIndex, string.Empty);
                        }
                    }
                    else if (loValue is DateTime)
                    {
                        if ((DateTime)loValue > DateTime.MinValue)
                        {
                            //// Use same format as javascript date .toISOString()
                            loR.Add(lsPropertyNameIndex, MaxConvertLibrary.ConvertToDateTimeUtc(typeof(object), loValue).ToString("o", CultureInfo.InvariantCulture));
                        }
                        else
                        {
                            loR.Add(lsPropertyNameIndex, string.Empty);
                        }
                    }
                    else if (loValue != null && !(loValue is Stream))
                    {
                        loR.Add(lsPropertyNameIndex, loValue);
                    }
                }
            }

            return loR;
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
                        foreach (string lsName in  laDataName)
                        {
                            loDataNameIndex.Add(lsName, true);
                        }
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
            MaxData loData = new MaxData(this.Data);
            MaxDataList loDataList = MaxBaseReadRepository.SelectAll(loData, laDataNameList);
            MaxEntityList loEntityList = MaxEntityList.Create(this.GetType(), loDataList);
            return loEntityList;
        }

        /// <summary>
        /// Loads all entities of this type into cache
        /// </summary>
        /// <returns>List of entities</returns>
        public virtual MaxEntityList LoadAllCache(params string[] laPropertyNameList)
        {
            string lsCacheDataKey = this.GetCacheKey() + "LoadAll";
            if (null != laPropertyNameList && laPropertyNameList.Length > 0)
            {
                lsCacheDataKey += "/PropertyNameHash=" + MaxEncryptionLibrary.GetHash(typeof(object), MaxEncryptionLibrary.MD5Hash, string.Concat(laPropertyNameList));
            }

            MaxDataList loDataList = MaxCacheRepository.Get(this.GetType(), lsCacheDataKey, typeof(MaxDataList)) as MaxDataList;
            if (null == loDataList)
            {
                string[] laDataNameList = this.GetDataNameList(this.Data.DataModel, laPropertyNameList);
                loDataList = MaxBaseReadRepository.SelectAll(this.Data, laDataNameList);
                MaxCacheRepository.Set(this.GetType(), lsCacheDataKey, loDataList);
            }

            MaxEntityList loEntityList = MaxEntityList.Create(this.GetType(), loDataList);
            return loEntityList;
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
            string[] laDataNameList = this.GetDataNameList(loData.DataModel, laPropertyNameList);
            string lsOrderBy = this.GetOrderBy(loData.DataModel, lsPropertySort);
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
            string lsCacheDataKey = this.GetCacheKey() + "LoadAllByPage/" + lnPageIndex.ToString() + "/" + lnPageSize + "/" + lsPropertySort;
            if (null != loData)
            {
                lsCacheDataKey += "/DataHash=" + MaxEncryptionLibrary.GetHash(typeof(object), MaxEncryptionLibrary.MD5Hash, loData.ToString());
            }

            if (null != loDataQuery)
            {
                lsCacheDataKey += "/DataQueryHash=" + MaxEncryptionLibrary.GetHash(typeof(object), MaxEncryptionLibrary.MD5Hash, loDataQuery.ToString());
            }

            if (null != laPropertyNameList && laPropertyNameList.Length > 0)
            {
                lsCacheDataKey += "/PropertyNameHash=" + MaxEncryptionLibrary.GetHash(typeof(object), MaxEncryptionLibrary.MD5Hash, string.Concat(laPropertyNameList));
            }

            MaxDataList loDataList = MaxCacheRepository.Get(this.GetType(), lsCacheDataKey, typeof(MaxDataList)) as MaxDataList;
            string lsOrderBy = this.GetOrderBy(loData.DataModel, lsPropertySort);
            if (null == loDataList)
            {
                string[] laDataNameList = this.GetDataNameList(loData.DataModel, laPropertyNameList);
                loDataList = MaxBaseReadRepository.Select(loData, loDataQuery, lnPageIndex, lnPageSize, lsOrderBy, laDataNameList);
                MaxCacheRepository.Set(this.GetType(), lsCacheDataKey, loDataList);
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
                MaxDataQuery loDataQuery = new MaxDataQuery();
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
                MaxDataQuery loDataQuery = new MaxDataQuery();
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
            MaxEntityList loR = this.LoadAllByPage(loData, lnPageIndex, lnPageSize, lsPropertySort, null, laPropertyNameList);
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
            MaxDataQuery loDataQuery = new MaxDataQuery();
            if (loFilter.Count > 0)
            {
                string[] laKey = loFilter.GetSortedKeyList();
                if (laKey.Length > 0)
                {
                    loDataQuery.StartGroup();
                    foreach (string lsKey in laKey)
                    {
                        MaxIndex loDetail = loFilter[lsKey] as MaxIndex;
                        if (loDetail.Contains("StartGroup"))
                        {
                            loDataQuery.StartGroup();
                        }

                        string lsPropertyName = loDetail.GetValueString("Name");
                        string lsDataName = this.GetDataName(this.Data.DataModel, lsPropertyName);
                        if (!string.IsNullOrEmpty(lsDataName))
                        {
                            loDataQuery.AddFilter(lsDataName, loDetail.GetValueString("Operator"), loDetail.GetValueString("Value"));
                        }

                        if (loDetail.Contains("EndGroup"))
                        {
                            loDataQuery.EndGroup();
                        }

                        if (loDetail.Contains("Condition") && !string.IsNullOrEmpty(loDetail.GetValueString("Condition")))
                        {
                            loDataQuery.AddCondition(loDetail.GetValueString("Condition"));
                        }
                    }

                    loDataQuery.EndGroup();
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
            MaxDataQuery loDataQuery = new MaxDataQuery();
            if (loFilter.Count > 0)
            {
                string[] laKey = loFilter.GetSortedKeyList();
                if (laKey.Length > 0)
                {
                    loDataQuery.StartGroup();
                    foreach (string lsKey in laKey)
                    {
                        MaxIndex loDetail = loFilter[lsKey] as MaxIndex;
                        if (loDetail.Contains("StartGroup"))
                        {
                            loDataQuery.StartGroup();
                        }

                        string lsPropertyName = loDetail.GetValueString("Name");
                        string lsDataName = this.GetDataName(this.Data.DataModel, lsPropertyName);
                        if (!string.IsNullOrEmpty(lsDataName))
                        {
                            loDataQuery.AddFilter(lsDataName, loDetail.GetValueString("Operator"), loDetail.GetValueString("Value"));
                        }

                        if (loDetail.Contains("EndGroup"))
                        {
                            loDataQuery.EndGroup();
                        }

                        if (loDetail.Contains("Condition") && !string.IsNullOrEmpty(loDetail.GetValueString("Condition")))
                        {
                            loDataQuery.AddCondition(loDetail.GetValueString("Condition"));
                        }
                    }

                    loDataQuery.EndGroup();
                }
            }

            MaxData loData = new MaxData(this.Data);
            MaxEntityList loR = this.LoadAllByPageCache(loData, 0, 0, string.Empty, loDataQuery, laPropertyNameList);
            return loR;
        }

        /// <summary>
        /// Loads the entity that matches the key
        /// </summary>
        /// <param name="lsKey"></param>
        /// <param name="laPropertyNameList"></param>
        /// <returns></returns>
        public bool LoadByKeyCache(string lsKey, params string[] laPropertyNameList)
        {
            bool lbR = false;
            string[] laKey = lsKey.Split(new string[] { this.Data.DataModel.KeySeparator }, StringSplitOptions.None);
            MaxDataQuery loDataQuery = new MaxDataQuery();
            if (laKey.Length == this.PropertyNameKeyList.Length)
            {
                loDataQuery.StartGroup();
                for (int lnK = 0; lnK < this.PropertyNameKeyList.Length; lnK++)
                {
                    string lsDataName = this.GetDataName(this.Data.DataModel, this.PropertyNameKeyList[lnK]);
                    loDataQuery.AddFilter(lsDataName, "=", this.Data.Get(lsDataName));
                    if (lnK < this.PropertyNameKeyList.Length - 1)
                    {
                        loDataQuery.AddAnd();
                    }
                }

                loDataQuery.EndGroup();
            }

            MaxData loData = new MaxData(this.Data);
            MaxEntityList loList = this.LoadAllByPageCache(loData, 0, 0, string.Empty, loDataQuery, laPropertyNameList);
            if (loList.Count == 1)
            {
                lbR = this.Load(loList[0].GetData());
            }

            return lbR;
        }

        /// <summary>
        /// Gets a copy of the data associated with this entity.
        /// </summary>
        /// <returns>Copy of data for this entity.</returns>
        public virtual MaxData GetData()
        {
            return this.Data.Clone();
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
        public virtual string GetCacheKey()
        {
            if (null == this._sCacheKey)
            {
                this._sCacheKey = this.GetType().ToString() + "/";
            }

            return this._sCacheKey;
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
        /// Sets the datamodel properties for any extended data
        /// </summary>
        protected virtual void SetProperties()
        {
            string[] laKey = this._oUnSerializedObjectIndex.GetSortedKeyList();
            foreach (string lsDataName in laKey)
            {
                this.SetObject(lsDataName, this._oUnSerializedObjectIndex[lsDataName]);
            }

            foreach (string lsDataName in this.Data.DataModel.DataNameList)
            {
                if (MaxBaseWriteRepository.StreamSave(this.Data, lsDataName))
                {
                    //// Clear Cache
                    string[] laStreamPath = this.Data.GetStreamPath();
                    string lsStreamPath = laStreamPath[0];
                    for (int lnP = 1; lnP < laStreamPath.Length; lnP++)
                    {
                        lsStreamPath += "/" + laStreamPath[lnP];
                    }

                    lsStreamPath += "/" + lsDataName;
                    string lsCacheDataKey = this.GetCacheKey() + "GetString/" + lsStreamPath;
                    MaxCacheRepository.Remove(this.GetType(), lsCacheDataKey);
                }
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
                string[] laStreamPath = this.Data.GetStreamPath();
                string lsStreamPath = laStreamPath[0];
                for (int lnP = 1; lnP < laStreamPath.Length; lnP++)
                {
                    lsStreamPath += "/" + laStreamPath[lnP];
                }

                lsStreamPath += "/" + lsDataName;
                string lsCacheDataKey = this.GetCacheKey() + "GetString/" + lsStreamPath;
                loValue = MaxCacheRepository.Get(this.GetType(), lsCacheDataKey, typeof(string)) as string;
                if (null == loValue)
                {
                    Stream loStream = MaxBaseReadRepository.StreamOpen(this.Data, lsDataName);
                    try
                    {
                        if (null != loStream)
                        {
                            StreamReader loReader = new StreamReader(loStream);
                            try
                            {
                                loValue = loReader.ReadToEnd();
                                MaxCacheRepository.Set(this.GetType(), lsCacheDataKey, loValue);
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
                loR = MaxBaseReadRepository.StreamOpen(this.Data, lsDataName);
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
                    loValue = MaxBaseReadRepository.StreamOpen(this.Data, lsDataName);
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
                    loValue = this.GetByte(lsDataName);
                    loValue = MaxConvertLibrary.DeserializeObject(this.Data.DataModel.GetType(), (byte[])loValue, loType);
                }
                else if ((loValueType.Equals(typeof(string)) ||
                    loValueType.Equals(typeof(MaxLongString))) &&
                    loValue is string)
                {
                    loValue = this.GetString(lsDataName);
                    loValue = MaxConvertLibrary.DeserializeObject(this.Data.DataModel.GetType(), (string)loValue, loType);
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
    }
}
