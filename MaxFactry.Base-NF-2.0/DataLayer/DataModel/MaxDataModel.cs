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
// </changelog>
#endregion

namespace MaxFactry.Base.DataLayer
{
    using System;
    using MaxFactry.Core;

    /// <summary>
    /// Base class used to define data models.
    /// </summary>
    public class MaxDataModel
    {
        /// <summary>
        /// Indicates that the data for the string is stored as a stream
        /// </summary>
        public static readonly string StreamStringIndicator = "{SSI}";

        /// <summary>
        /// Indicates that the data for the byte[] is stored as a stream
        /// </summary>
        public static readonly byte[] StreamByteIndicator = System.Text.UTF8Encoding.UTF8.GetBytes("{SBI}");

        /// <summary>
        /// Storage Key for use by a Repository provider to break records up in some manner.
        /// </summary>
        public readonly string StorageKey = "StorageKey";

        /// <summary>
        /// Index of column names and types
        /// </summary>
        private MaxIndex _oKeyIndex = new MaxIndex();

		/// <summary>
		/// Index of attributes related to each property
		/// </summary>
		private MaxIndex _oPropertyAttributeIndex = new MaxIndex();

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

		/// <summary>
		/// Initializes a new instance of the MaxDataModel class
		/// </summary>
		public MaxDataModel()
        {
            this.AddKey(this.StorageKey, typeof(MaxShortString));
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

		/// <summary>
		/// Gets the type of the value matching the key
		/// </summary>
		/// <param name="lsKey">Key to check</param>
		/// <returns>The defined type of the value matching the key</returns>
		public Type GetValueType(string lsKey)
		{
			Type loR = null;
			if (this._oKeyIndex.Contains(lsKey) && this._oKeyIndex[lsKey] is Type)
			{
				loR = (Type)this._oKeyIndex[lsKey];
			}

			return loR;
		}

        /// <summary>
        /// Gets a suffix for the primary key based on the data to speed up future queries
        /// </summary>
        /// <param name="loData">Data to use to create the suffix</param>
        /// <returns>String to use as suffix for primary key</returns>
        public virtual string GetPrimaryKeySuffix(MaxData loData)
        {
            object loValue = loData.Get("_PrimaryKeySuffix");
            if (null != loValue)
            {
                return loValue.ToString();
            }

            return string.Empty;
        }

		/// <summary>
		/// Gets the text of the attribute matching the key
		/// </summary>
		/// <param name="lsKey">Key to check</param>
		/// <param name="lsAttribute">Attribute to get</param>
		/// <returns>The defined type of the value matching the key</returns>
		public string GetPropertyAttribute(string lsKey, string lsAttribute)
		{
            object loR = this._oPropertyAttributeIndex.FindValue(lsKey, "|", lsAttribute);
            if (loR is Guid && this._oPropertyAttributeIndex.NotFoundId.Equals((Guid)loR))
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
        /// <param name="lsKey">Name of the key</param>
        /// <param name="lsAttribute">Attribute related to the key</param>
        /// <returns>True if the attribute is set to true (true, yes, 1).</returns>
        public bool GetPropertyAttributeSetting(string lsKey, string lsAttribute)
        {
            bool lbR = false;
            string lsR = this.GetPropertyAttribute(lsKey, lsAttribute);
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
        /// Gets a list of the keys
        /// </summary>
        /// <returns>list of the keys</returns>
        public string[] GetKeyList()
        {
            string[] laR = new string[this._oKeyIndex.Count];
            laR = this._oKeyIndex.GetSortedKeyList();
            return laR;
        }

        /// <summary>
        /// Checks to see if the DataModel has the key
        /// </summary>
        /// <param name="lsKey">Key name to look up</param>
        /// <returns>true if it has the key, false otherwise</returns>
        public bool HasKey(string lsKey)
        {
            return this._oKeyIndex.Contains(lsKey);
        }

        /// <summary>
        /// Adds a type to the definition
        /// </summary>
        /// <param name="lsKey">Name of the key</param>
        /// <param name="loType">Type of the value matching the key</param>
        /// <returns>true if added.  False if already exists.</returns>
        protected bool AddType(string lsKey, Type loType)
        {
            if (!this._oKeyIndex.Contains(lsKey))
            {
                this._oKeyIndex.Add(lsKey, loType);
                return true;
            }

            return false;
        }

        /// <summary>
        /// Adds a type to the definition
        /// </summary>
        /// <param name="lsKey">Name of the key</param>
        /// <param name="loType">Type of the value matching the key</param>
        /// <returns>true if added.  False if already exists.</returns>
        protected bool AddKey(string lsKey, Type loType)
        {
            if (!this._oKeyIndex.Contains(lsKey))
            {
                this._oKeyIndex.Add(lsKey, loType);
                this.AddPropertyAttribute(lsKey, "IsPrimaryKey", "true");
                return true;
            }

            return false;
        }

        /// <summary>
        /// Adds a type to the definition that could have a null value
        /// </summary>
        /// <param name="lsKey">Name of the key</param>
        /// <param name="loType">Type of the value matching the key</param>
        /// <returns>true if added.  False if already exists.</returns>
        protected bool AddNullable(string lsKey, Type loType)
        {
            if (!this._oKeyIndex.Contains(lsKey))
            {
                this._oKeyIndex.Add(lsKey, loType);
                this.AddPropertyAttribute(lsKey, "IsAllowDBNull", "true");
                return true;
            }

            return false;
        }

        /// <summary>
        /// Removes a key from the definition.
        /// </summary>
        /// <param name="lsKey">Name of the key</param>
        /// <returns>true if removed.  False if does not exist.</returns>
        protected bool RemoveKey(string lsKey)
        {
            if (this._oKeyIndex.Contains(lsKey))
            {
                this._oKeyIndex.Remove(lsKey);
                string[] laAttributeKeyList = this._oPropertyAttributeIndex.GetSortedKeyList();
                foreach (string lsAttributeKey in laAttributeKeyList)
                {
                    if (lsAttributeKey.Length > lsKey.Length + 1 && lsAttributeKey.Substring(0, lsKey.Length + 1).Equals(lsKey + "|"))
                    {
                        this._oPropertyAttributeIndex.Remove(lsAttributeKey);
                    }
                }

                return true;
            }

            return false;
        }

		/// <summary>
		/// Adds a type to the definition
		/// </summary>
		/// <param name="lsKey">Name of the key</param>
		/// <param name="lsAttribute">Name of the attribute associated with the key</param>
		/// <param name="lsValue">Value for the attribute</param>
		/// <returns>true if added.  False if already exists.</returns>
		protected bool AddPropertyAttribute(string lsKey, string lsAttribute, string lsValue)
		{
            string lsAttributeKey = lsKey + "|" + lsAttribute;
            if (!this._oPropertyAttributeIndex.Contains(lsAttributeKey))
			{
                this._oPropertyAttributeIndex.Add(lsAttributeKey, lsValue);
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
	}
}
