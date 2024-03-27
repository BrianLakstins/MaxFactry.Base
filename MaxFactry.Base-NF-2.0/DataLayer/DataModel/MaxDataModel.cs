// <copyright file="MaxDataModel.cs" company="Lakstins Family, LLC">
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
// <change date="12/20/2013" author="Brian A. Lakstins" description="Initial Release">
// <change date="3/2/2014" author="Brian A. Lakstins" description="Updates to reduce amount of configuration needed.">
// <change date="3/22/2014" author="Brian A. Lakstins" description="Updated to no longer be abstract because used with MaxCRUD.">
// <change date="6/26/2014" author="Brian A. Lakstins" description="Added StorageKey. Added ability to remove keys.">
// <change date="7/3/2014" author="Brian A. Lakstins" description="Added support for default Stream repository provider.  Changed StorageKey to MaxShortString since it will be part of PK for SQL.">
// <change date="7/4/2014" author="Brian A. Lakstins" description="Added support for default Message repository provider.">
// <change date="9/16/2014" author="Brian A. Lakstins" description="Update AddAttribute to consume less memory (string.concat)">
// <change date="4/22/2016" author="Brian A. Lakstins" description="Added new initializer that can set data storage name for when only table names are known (not structure).">
// <change date="12/21/2016" author="Brian A. Lakstins" description="Added indicators for streams.  Added GetPrimaryKeySuffix to help index records for later query.">
// <change date="5/25/2017" author="Brian A. Lakstins" description="Updated to use new FindValue method of MaxIndex to reduce memory usage.">
// <change date="11/29/2018" author="Brian A. Lakstins" description="Updated methods to make it clearer that AttributeIndex is for each property so could add an AttributeIndex property.">
// <change date="7/30/2019" author="Brian A. Lakstins" description="Add method to check to key, especially StorageKey.">
// <change date="3/20/2024" author="Brian A. Lakstins" description="Happy birthday to my mom.  Sara Jean Lakstins (Cartwright) - 3/20/1944 to 3/14/2019.">
// <change date="3/22/2024" author="Brian A. Lakstins" description="Remove storagekey.  Rename Key and Property to DataName so not confused with Primary Key or Entity property.  Remove unused field _oKeyIndex. Add properies for Data Names.  Remove PrimaryKey suffix. Create some constants for reused strings.">
// <change date="3/23/2024" author="Brian A. Lakstins" description="Adding a field and some methods to support generic primary keys.">
// <change date="3/25/2024" author="Brian A. Lakstins" description="Add method to map content that may not be formatted for the data model.">
// <change date="3/26/2024" author="Brian A. Lakstins" description="Move logic for GetStreamPath from MaxData">
// </changelog>
#endregion

namespace MaxFactry.Base.DataLayer
{
    using System;
    using System.Collections.Generic;
    using System.Reflection;
    using MaxFactry.Base.DataLayer.Library;
    using MaxFactry.Core;

    /// <summary>
    /// Base class used to define data models.
    /// </summary>
    public class MaxDataModel
    {
        public const string AttributeIsPrimaryKey = "IsPrimaryKey";

        public const string AttributeIsAllowDBNull = "IsAllowDBNull";
        
        public const string AttributeIsEncrypted = "IsEncrypted";

        /// <summary>
        /// Indicates that the data for the string is stored as a stream
        /// </summary>
        public static readonly string StreamStringIndicator = "{SSI}";

        /// <summary>
        /// Indicates that the data for the byte[] is stored as a stream
        /// </summary>
        public static readonly byte[] StreamByteIndicator = System.Text.UTF8Encoding.UTF8.GetBytes("{SBI}");

        /// <summary>
        /// Index of data names and types
        /// </summary>
        private MaxIndex _oDataNameIndex = new MaxIndex();

		/// <summary>
		/// Index of attributes related to each data name
		/// Index of attributes related to each data name
		/// </summary>
		private MaxIndex _oDataNameAttributeIndex = new MaxIndex();

		/// <summary>
		/// Name to use to reference the storage of this data
		/// </summary>
		private string _sDataStorageName = string.Empty;

        /// <summary>
        /// Type to use as the repository.
        /// </summary>
        private Type _oRepositoryType = null;

        /// <summary>
        /// Type to use as the repository provider.
        /// </summary>
        private Type _oRepositoryProviderType = null;

        /// <summary>
        /// Type to use as the repository provider for streams.
        /// </summary>
        private Type _oRepositoryStreamProviderType = null;

        /// <summary>
        /// Type to use as the repository provider for messages.
        /// </summary>
        private Type _oRepositoryMessageProviderType = null;

        private string[] _aDataNameList = null;

        private string[] _aDataNameKeyList = null;

		/// <summary>
		/// Initializes a new instance of the MaxDataModel class
		/// </summary>
		public MaxDataModel()
        {
        }

        /// <summary>
        /// Initializes a new instance of the MaxDataModel class
        /// </summary>
        /// <param name="lsDataStorageName">Name of the data storage</param>
        public MaxDataModel(string lsDataStorageName) : this()
        {
            this._sDataStorageName = lsDataStorageName;
        }

		/// <summary>
		/// Gets the name to use to store this type of data
		/// </summary>
		public string DataStorageName
		{
			get
			{
				return this._sDataStorageName;
			}
		}

        /// <summary>
        /// Gets or sets the type used as the Repository provider
        /// </summary>
        public Type RepositoryType
        {
            get
            {
                return this._oRepositoryType;
            }

            set
            {
                this._oRepositoryType = value;
            }
        }

        /// <summary>
        /// Gets or sets the type used as the Repository provider
        /// </summary>
        public Type RepositoryProviderType
        {
            get
            {
                return this._oRepositoryProviderType;
            }

            set
            {
                this._oRepositoryProviderType = value;
            }
        }

        /// <summary>
        /// Gets or sets the type used as the Repository provider for streams
        /// </summary>
        public Type RepositoryStreamProviderType
        {
            get
            {
                return this._oRepositoryStreamProviderType;
            }

            set
            {
                this._oRepositoryStreamProviderType = value;
            }
        }

        /// <summary>
        /// Gets or sets the type used as the Repository provider for messages
        /// </summary>
        public Type RepositoryMessageProviderType
        {
            get
            {
                return this._oRepositoryMessageProviderType;
            }

            set
            {
                this._oRepositoryMessageProviderType = value;
            }
        }

        public virtual string KeySeparator
        {
            get
            {
                return "/";
            }
        }

        /// <summary>
        /// Gets a list of the data names
        /// </summary>
        public virtual string[] DataNameList
        {
            get
            {
                if (this._aDataNameList == null)
                {
                    string[] laDataNameList = this._oDataNameIndex.GetSortedKeyList();
                    MaxIndex loR = new MaxIndex();
                    foreach (string lsDataName in laDataNameList)
                    {
                        if (this.IsStored(lsDataName))
                        {
                            loR.Add(lsDataName, true);
                        }
                    }

                    this._aDataNameList = loR.GetSortedKeyList();
                }

                return this._aDataNameList;
            }
        }

        /// <summary>
        /// Gets a list of primary keys
        /// </summary>
        public virtual string[] DataNameKeyList
        {
            get
            {
                if (null == this._aDataNameKeyList)
                {
                    MaxIndex loR = new MaxIndex();
                    foreach (string lsDataName in this.DataNameList)
                    {
                        if (this.IsPrimaryKey(lsDataName))
                        {
                            loR.Add(lsDataName, true);
                        }
                    }

                    this._aDataNameKeyList = loR.GetSortedKeyList();
                }

                return this._aDataNameKeyList;
            }
        }

        public virtual string GetDataNameKey(MaxData loData)
        {
            string lsR = string.Empty;
            foreach (string lsDataName in this.DataNameKeyList)
            {
                string lsValue = MaxConvertLibrary.ConvertToString(typeof(object), loData.Get(lsDataName));
                if (string.IsNullOrEmpty(lsValue))
                {
                    lsR = null;
                }
                else if (null != lsR)
                {
                    if (lsR.Length > 0)
                    {
                        lsR += this.KeySeparator;
                    }

                    lsR += lsValue;
                }
            }

            return lsR;
        }

        /// <summary>
        /// Gets the type of the value matching the data name
        /// </summary>
        /// <param name="lsDataName">Name to check</param>
        /// <returns>The defined type of the value matching the data name</returns>
        public Type GetValueType(string lsDataName)
		{
			Type loR = null;
			if (this._oDataNameIndex.Contains(lsDataName) && this._oDataNameIndex[lsDataName] is Type)
			{
				loR = (Type)this._oDataNameIndex[lsDataName];
			}

			return loR;
		}

        /// <summary>
        /// Checks to see if this data name has a type that is stored
        /// </summary>
        /// <param name="lsDataName">The data name to check to see if it is stored.</param>
        /// <returns>True if it should be stored.  False otherwise.</returns>
        public virtual bool IsStored(string lsDataName)
        {
            Type loValueType = this.GetValueType(lsDataName);
            if (null != loValueType)
            {
                if (loValueType.Equals(typeof(MaxShortString)))
                {
                    return true;
                }
                else if (loValueType.Equals(typeof(string)))
                {
                    return true;
                }
                else if (loValueType.Equals(typeof(Guid)))
                {
                    return true;
                }
                else if (loValueType.Equals(typeof(int)))
                {
                    return true;
                }
                else if (loValueType.Equals(typeof(long)))
                {
                    return true;
                }
                else if (loValueType.Equals(typeof(double)))
                {
                    return true;
                }
                else if (loValueType.Equals(typeof(byte[])))
                {
                    return true;
                }
                else if (loValueType.Equals(typeof(bool)))
                {
                    return true;
                }
                else if (loValueType.Equals(typeof(DateTime)))
                {
                    return true;
                }
                else if (loValueType.Equals(typeof(MaxLongString)))
                {
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// Gets the text of the attribute matching the data name
        /// </summary>
        /// <param name="lsDataName">data name to check</param>
        /// <param name="lsAttribute">Attribute to get</param>
        /// <returns>The defined type of the value matching the data name</returns>
        public string GetAttribute(string lsDataName, string lsAttribute)
		{
            object loR = this._oDataNameAttributeIndex.FindValue(lsDataName, "|", lsAttribute);
            if (loR is Guid && this._oDataNameAttributeIndex.NotFoundId.Equals((Guid)loR))
            {
                return string.Empty;
            }
            else if (null != loR)
            {
                return loR.ToString();
            }

            return string.Empty;
        }

        /// <summary>
        /// Gets a boolean value indicating whether an attribute set to true.
        /// </summary>
        /// <param name="lsDataName">Name of the data name</param>
        /// <param name="lsAttribute">Attribute related to the data name</param>
        /// <returns>True if the attribute is set to true (true, yes, 1).</returns>
        public bool GetAttributeSetting(string lsDataName, string lsAttribute)
        {
            bool lbR = false;
            string lsR = this.GetAttribute(lsDataName, lsAttribute);
            if (null != lsR)
            {
                if (lsR == "1" || lsR.ToLower() == "true" || lsR.ToLower() == "yes")
                {
                    lbR = true;
                }
            }

            return lbR;
        }

        /// <summary>
        /// Checks to see if the DataModel has the data name
        /// </summary>
        /// <param name="lsDataName">Name to look up</param>
        /// <returns>true if it has the data name, false otherwise</returns>
        public bool HasDataName(string lsDataName)
        {
            return this._oDataNameIndex.Contains(lsDataName);
        }

        /// <summary>
        /// Checks to see if this data name is part of the primary key
        /// </summary>
        /// <param name="lsDataName"></param>
        /// <returns></returns>
        public bool IsPrimaryKey(string lsDataName)
        {
            return this.GetAttributeSetting(lsDataName, AttributeIsPrimaryKey);
        }

        /// <summary>
        /// Maps data that does not already match the data model to the data model
        /// </summary>
        /// <param name="loIndex">Data to map</param>
        /// <returns>List of data</returns>
        public virtual MaxDataList MapIndex(MaxIndex loIndex)
        {
            MaxDataList loR = new MaxDataList(this);
            string[] laKey = loIndex.GetSortedKeyList();
            MaxData loDataSingle = new MaxData(this);
            foreach (string lsKey in laKey)
            {
                object loValue = loIndex[lsKey];
                if (loValue is MaxIndex)
                {
                    MaxIndex loDataIndex = loValue as MaxIndex;
                    MaxData loData = new MaxData(this);
                    foreach (string lsDataName in this.DataNameList)
                    {
                        loData.Set(lsDataName, loDataIndex[lsDataName]);
                    }

                    loR.Add(loData);
                }
                else
                {
                    loDataSingle.Set(lsKey, loValue);
                }
            }

            return loR;
        }

        /// <summary>
        /// Adds a type to the definition
        /// </summary>
        /// <param name="lsDataName">Name of the data name</param>
        /// <param name="loType">Type of the value matching the data name</param>
        /// <returns>true if added.  False if already exists.</returns>
        protected bool AddType(string lsDataName, Type loType)
        {
            if (!this._oDataNameIndex.Contains(lsDataName))
            {
                this._oDataNameIndex.Add(lsDataName, loType);
                return true;
            }

            return false;
        }

        /// <summary>
        /// Adds a type to the definition
        /// </summary>
        /// <param name="lsDataName">Name of the data name</param>
        /// <param name="loType">Type of the value matching the data name</param>
        /// <returns>true if added.  False if already exists.</returns>
        protected bool AddKey(string lsDataName, Type loType)
        {
            if (!this._oDataNameIndex.Contains(lsDataName))
            {
                this._oDataNameIndex.Add(lsDataName, loType);
                this.AddAttribute(lsDataName, AttributeIsPrimaryKey, "true");
                return true;
            }

            return false;
        }

        /// <summary>
        /// Adds a type to the definition that could have a null value
        /// </summary>
        /// <param name="lsDataName">Name of the data name</param>
        /// <param name="loType">Type of the value matching the data name</param>
        /// <returns>true if added.  False if already exists.</returns>
        protected bool AddNullable(string lsDataName, Type loType)
        {
            if (!this._oDataNameIndex.Contains(lsDataName))
            {
                this._oDataNameIndex.Add(lsDataName, loType);
                this.AddAttribute(lsDataName, AttributeIsAllowDBNull, "true");
                return true;
            }

            return false;
        }

        /// <summary>
        /// Removes a type from the definition.
        /// </summary>
        /// <param name="lsDataName">Name of the data</param>
        /// <returns>true if removed.  False if does not exist.</returns>
        protected bool RemoveType(string lsDataName)
        {
            if (this._oDataNameIndex.Contains(lsDataName))
            {
                this._oDataNameIndex.Remove(lsDataName);
                string[] laAttributeKeyList = this._oDataNameAttributeIndex.GetSortedKeyList();
                foreach (string lsAttributeKey in laAttributeKeyList)
                {
                    if (lsAttributeKey.Length > lsDataName.Length + 1 && lsAttributeKey.Substring(0, lsDataName.Length + 1).Equals(lsDataName + "|"))
                    {
                        this._oDataNameAttributeIndex.Remove(lsAttributeKey);
                    }
                }

                return true;
            }

            return false;
        }

		/// <summary>
		/// Adds a type to the definition
		/// </summary>
		/// <param name="lsDataName">Name of the data</param>
		/// <param name="lsAttribute">Name of the attribute associated with the data</param>
		/// <param name="lsValue">Value for the attribute</param>
		/// <returns>true if added.  False if already exists.</returns>
		protected bool AddAttribute(string lsDataName, string lsAttribute, string lsValue)
		{
            string lsAttributeKey = lsDataName + "|" + lsAttribute;
            if (!this._oDataNameAttributeIndex.Contains(lsAttributeKey))
			{
                this._oDataNameAttributeIndex.Add(lsAttributeKey, lsValue);
				return true;
			}

			return false;
		}

		/// <summary>
		/// Sets the data storage name.  Should be done during initialize
		/// </summary>
		/// <param name="lsDataStorageName">Name to use to reference the data storage</param>
		protected void SetDataStorageName(string lsDataStorageName)
		{
			this._sDataStorageName = lsDataStorageName;
		}

        public virtual string[] GetStreamPath(MaxData loData)
        {
            List<string> loR = new List<string>();
            string lsDataStorageName = this.DataStorageName;
            if (lsDataStorageName.EndsWith("MaxArchive"))
            {
                lsDataStorageName = lsDataStorageName.Substring(0, lsDataStorageName.Length - "MaxArchive".Length);
            }

            loR.Add(lsDataStorageName);
            string lsKey = this.GetDataNameKey(loData);
            if (!string.IsNullOrEmpty(lsKey))
            {
                string[] laStreamKey = lsKey.Split(new string[] { this.KeySeparator }, StringSplitOptions.RemoveEmptyEntries);
                foreach (string lsStreamKey in laStreamKey)
                {
                    loR.Add(lsStreamKey);
                }
            }

            return loR.ToArray();
        }
    }
}
