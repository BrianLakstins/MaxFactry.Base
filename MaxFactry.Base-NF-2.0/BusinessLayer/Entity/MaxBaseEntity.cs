// <copyright file="MaxBaseEntity.cs" company="Lakstins Family, LLC">
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
// <change date="3/19/2024" author="Brian A. Lakstins" description="Initial creation">
// <change date="3/20/2024" author="Brian A. Lakstins" description="Happy birthday to my mom.  Sara Jean Lakstins (Cartwright) - 3/20/1944 to 3/14/2019.">
// <change date="3/24/2024" author="Brian A. Lakstins" description="Updated for changes namespaces">
// <change date="3/26/2024" author="Brian A. Lakstins" description="Check to see if Data names are being used before using them">
// <change date="3/30/2024" author="Brian A. Lakstins" description="Use IsStored to determine if StorageKey needs set.">
// <change date="7/16/2024" author="Brian A. Lakstins" description="Add a way to set an attribute.">
// <change date="1/21/2025" author="Brian A. Lakstins" description="Added some type checking.">
// <change date="3/22/2025" author="Brian A. Lakstins" description="Integrate with changes to base insert.">
// <change date="4/9/2025" author="Brian A. Lakstins" description="Override SetInitial method insteading of altering Insert method.">
// <change date="6/3/2025" author="Brian A. Lakstins" description="Remove special handling of StorageKey. Make sure CreatedDate is unique.  Override GetDataQuery to filter out deleted records.">
// <change date="6/4/2025" author="Brian A. Lakstins" description="Fix IsDeleted filter">
// </changelog>
#endregion

namespace MaxFactry.Base.BusinessLayer
{
    using System;
    using MaxFactry.Base.DataLayer;
    using MaxFactry.Base.DataLayer.Library;

    /// <summary>
    /// Base Business Layer Entity with general properties and methods
    /// </summary>
    public abstract class MaxBaseEntity : MaxEntity
    {
        private string[] _aBaseKeyPropertyNameList = null;

        private static object _oLock = new object();

        private static long _nUniqueCreatedDateLast = 0;

        /// <summary>
        /// Initializes a new instance of the MaxBaseIdEntity class
        /// </summary>
        /// <param name="loData">object to hold data</param>
        public MaxBaseEntity(MaxData loData) : base(loData)
        {
        }

        /// <summary>
        /// Initializes a new instance of the MaxBaseIdEntity class.
        /// </summary>
        /// <param name="loDataModelType">Type of data model.</param>
        public MaxBaseEntity(Type loDataModelType)
            : base(loDataModelType)
        {
        }

        /// <summary>
        /// Gets the Storage Key for this entity
        /// </summary>
        public string StorageKey
        {
            get
            {
                return this.GetString(this.MaxBaseDataModel.StorageKey);
            }
        }

        /// <summary>
        /// Gets the date the entity was created
        /// </summary>
        public virtual DateTime CreatedDate
        {
            get
            {
                return this.GetDateTime(this.MaxBaseDataModel.CreatedDate);
            }
        }

        /// <summary>
        /// Gets the date the entity was last updated
        /// </summary>
        public virtual DateTime LastUpdateDate
        {
            get
            {
                return this.GetDateTime(this.MaxBaseDataModel.LastUpdateDate);
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether this is active
        /// </summary>
        public virtual bool IsActive
        {
            get
            {
                return this.GetBoolean(this.MaxBaseDataModel.IsActive);
            }

            set
            {
                this.Set(this.MaxBaseDataModel.IsActive, value);
            }
        }

        /// <summary>
        /// Gets a value indicating whether this is deleted
        /// </summary>
        public virtual bool IsDeleted
        {
            get
            {
                return this.GetBoolean(this.MaxBaseDataModel.IsDeleted);
            }
        }

        /// <summary>
        /// Gets or sets an index of names and values that can be used in any way
        /// </summary>
        public virtual MaxIndex AttributeIndex
        {
            get
            {
                MaxIndex loR = new MaxIndex();
                object loCurrent = this.GetObject(this.MaxBaseDataModel.AttributeIndexText, typeof(MaxIndex));
                if (null != loCurrent && loCurrent is MaxIndex)
                {
                    loR = (MaxIndex)loCurrent;
                }
                else
                {
                    this.SetObject(this.MaxBaseDataModel.AttributeIndexText, loR);
                }

                return loR;
            }

            set
            {
                this.SetObject(this.MaxBaseDataModel.AttributeIndexText, value);
            }
        }

        /// <summary>
        /// Gets an index of property names and current values to use as a key for each record
        /// Ignore storage key, since it's internal to the data layer.
        /// </summary>
        public override string[] PropertyNameKeyList
        {
            get
            {
                if (null == this._aBaseKeyPropertyNameList)
                {
                    MaxIndex loPropertyNameIndex = new MaxIndex();
                    foreach (string lsKeyPropertyName in base.PropertyNameKeyList)
                    {
                        if (lsKeyPropertyName != this.MaxBaseDataModel.StorageKey)
                        {
                            loPropertyNameIndex.Add(lsKeyPropertyName, true);
                        }
                    }

                    this._aBaseKeyPropertyNameList = loPropertyNameIndex.GetSortedKeyList();
                }

                return this._aBaseKeyPropertyNameList;
            }
        }

        /// <summary>
        /// Gets the Data Model for this entity
        /// </summary>
        protected MaxBaseDataModel MaxBaseDataModel
        {
            get
            {
                return (MaxBaseDataModel)MaxDataLibrary.GetDataModel(this.DataModelType);
            }
        }

        /// <summary>
        /// Gets or sets the object holding the data
        /// </summary>
        protected override MaxData Data
        {
            get
            {
                MaxData loR = base.Data;
                if (loR.DataModel.IsStored(this.MaxBaseDataModel.StorageKey) &&
                    null == loR.Get(this.MaxBaseDataModel.StorageKey))
                {
                    loR.Set(this.MaxBaseDataModel.StorageKey, MaxDataLibrary.GetStorageKey(loR));
                }

                return loR;
            }
        }

        /// <summary>
        /// Leave out LastUpdateDate.  If it's the only one changed, then there was not anything really changed.
        /// </summary>
        protected override bool IsDataChanged
        {
            get
            {
                foreach (string lsDataName in this.Data.DataModel.DataNameList)
                {
                    if (lsDataName != this.MaxBaseDataModel.LastUpdateDate && this.Data.GetIsChanged(lsDataName))
                    {
                        return true;
                    }
                }

                return false;
            }
        }

        protected override string GetDataName(MaxDataModel loDataModel, string lsPropertyName)
        {
            string lsR = base.GetDataName(loDataModel, lsPropertyName);
            //// Force Data Name for AttributeIndex to be AttributeIndexText
            if (lsPropertyName == "AttributeIndex")
            {
                lsR = this.MaxBaseDataModel.AttributeIndexText;
            }

            return lsR;
        }

        protected override MaxDataQuery GetDataQuery()
        {
            MaxDataQuery loR = base.GetDataQuery();
            if (this.Data.DataModel.IsStored(this.MaxBaseDataModel.IsDeleted))
            {
                loR.StartGroup();
                loR.AddFilter(this.MaxBaseDataModel.IsDeleted, "=", false);
                loR.EndGroup();
            }

            return loR;
        }

        /// <summary>
        /// True if the passed option flag has been set.
        /// </summary>
        /// <param name="lnOption">Option flag from 0 to 63</param>
        /// <returns>value indicating whether the flag is set or not</returns>
        public bool IsOptionFlagSet(short lnOption)
        {
            return this.GetBit(this.MaxBaseDataModel.OptionFlagList, lnOption);
        }

        /// <summary>
        /// Sets the option to the Value parameter
        /// </summary>
        /// <param name="lnOption">Option flag from 0 to 63</param>
        /// <param name="lbValue">Value to set the option.</param>
        public void SetOptionFlag(short lnOption, bool lbValue)
        {
            this.SetBit(this.MaxBaseDataModel.OptionFlagList, lnOption, lbValue);
        }

        /// <summary>
        /// Sets the value of an attribute associated with the attribute index
        /// </summary>
        /// <param name="lsName">Name of the attribute</param>
        /// <param name="loValue">Value for the attribute</param>
        public void SetAttribute(string lsName, object loValue)
        {
            MaxIndex loAttributeIndex = this.AttributeIndex;
            loAttributeIndex.Add(lsName, loValue);
            this.AttributeIndex = loAttributeIndex;
        }

        /// <summary>
        /// Default is to sort by created date.
        /// </summary>
        /// <returns>Sortable created date.</returns>
        public override string GetDefaultSortString()
        {
            return MaxConvertLibrary.ConvertToSortString(typeof(object), this.CreatedDate) + base.GetDefaultSortString();
        }

        public virtual MaxEntityList LoadAllActive(params string[] laPropertyNameList)
        {
            MaxEntityList loR = base.LoadAllByProperty(this.MaxBaseDataModel.IsActive, true, laPropertyNameList);
            return loR;
        }

        public virtual MaxEntityList LoadAllActiveCache(params string[] laPropertyNameList)
        {
            MaxEntityList loR = base.LoadAllByPropertyCache(this.MaxBaseDataModel.IsActive, true, laPropertyNameList);
            return loR;
        }

        public virtual MaxEntityList LoadAllActiveByProperty(string lsPropertyName, object loValue, params string[] laPropertyNameList)
        {
            //// Add a Query 
            MaxDataQuery loDataQuery = this.GetDataQuery();
            loDataQuery.StartGroup();
            loDataQuery.AddFilter(this.MaxBaseDataModel.IsActive, "=", true);
            loDataQuery.AddAnd();
            loDataQuery.AddFilter(this.GetDataName(this.Data.DataModel, lsPropertyName), "=", loValue);
            loDataQuery.EndGroup();
            MaxData loData = this.Data.Clone();
            MaxEntityList loEntityList = this.LoadAllByPage(loData, 0, 0, string.Empty, loDataQuery, laPropertyNameList);
            return loEntityList;
        }

        /// <summary>
        /// Loads all entities of this type that have not been marked as deleted and updated since the date passed
        /// <param name="ldLastUpdate">Date to use to look up</param>
        /// <returns>List of entities</returns>
        public virtual MaxEntityList LoadAllSinceLastUpdateDate(DateTime ldLastUpdate, params string[] laPropertyNameList)
        {
            //// Add a Query 
            MaxDataQuery loDataQuery = this.GetDataQuery();
            loDataQuery.StartGroup();
            loDataQuery.AddFilter(this.MaxBaseDataModel.LastUpdateDate, ">", ldLastUpdate);
            loDataQuery.EndGroup();
            MaxData loData = this.Data.Clone();
            MaxEntityList loEntityList = this.LoadAllByPage(loData, 0, 0, string.Empty, loDataQuery, laPropertyNameList);
            return loEntityList;
        }

        public override string GetCacheKey()
        {
            string lsR = base.GetCacheKey();
            if (this.Data.DataModel.IsStored(this.MaxBaseDataModel.StorageKey))
            {
                lsR += this.StorageKey + "/";
            }

            return lsR;
        }

        /// <summary>
        /// Gets a unique time to use for a log entry so that no two events are logged at exactly the same time.
        /// </summary>
        /// <returns></returns>
        private static DateTime GetUniqueCreatedDate()
        {
            DateTime ldR = DateTime.MinValue;
            lock (_oLock)
            {
                ldR = DateTime.UtcNow;
                if (ldR.Ticks <= _nUniqueCreatedDateLast)
                {
                    ldR = new DateTime(_nUniqueCreatedDateLast + 1, DateTimeKind.Utc);
                }

                _nUniqueCreatedDateLast = ldR.Ticks;
            }

            return ldR;
        }

        /// <summary>
        /// Sets the initial values for the entity
        /// </summary>
        protected override void SetInitial()
        {
            base.SetInitial();
            if (this.Data.DataModel.IsStored(this.MaxBaseDataModel.LastUpdateDate))
            {
                this.Set(this.MaxBaseDataModel.LastUpdateDate, DateTime.UtcNow);
            }

            if (this.Data.DataModel.IsStored(this.MaxBaseDataModel.CreatedDate))
            {
                this.Set(this.MaxBaseDataModel.CreatedDate, GetUniqueCreatedDate());
            }
        }

        /// <summary>
        /// Sets IsDeleted to true if it's available.
        /// </summary>
        /// <returns></returns>
        public override bool Delete()
        {
            if (this.Data.DataModel.IsStored(this.MaxBaseDataModel.IsDeleted))
            {
                this.Set(this.MaxBaseDataModel.IsDeleted, true);
                return this.Update();
            }

            return base.Delete();
        }

        /// <summary>
        /// Deletes an existing record and deleted any related streams.
        /// </summary>
        /// <returns>true if deleted.  False if it cannot be deleted.</returns>
        public virtual bool RemoveFromStorage()
        {
            return base.Delete();
        }

        /// <summary>
        /// Sets the datamodel properties for any extended data
        /// </summary>
        protected override void SetProperties()
        {
            if (null == this.Get(this.MaxBaseDataModel.IsDeleted))
            {
                this.Set(this.MaxBaseDataModel.IsDeleted, false);
            }

            if (null == this.Get(this.MaxBaseDataModel.IsActive))
            {
                this.Set(this.MaxBaseDataModel.IsActive, false);
            }

            if (this.GetLong(this.MaxBaseDataModel.OptionFlagList) < 0)
            {
                this.Set(this.MaxBaseDataModel.OptionFlagList, 0);
            }

            if (this.Data.DataModel.IsStored(this.MaxBaseDataModel.LastUpdateDate))
            {
                this.Set(this.MaxBaseDataModel.LastUpdateDate, DateTime.UtcNow);
            }

            base.SetProperties();
        }
    }
}
