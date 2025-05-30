// <copyright file="MaxBaseDataModel.cs" company="Lakstins Family, LLC">
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
// <change date="3/26/2024" author="Brian A. Lakstins" description="Move logic for GetStreamPath from MaxData">
// <change date="3/30/2024" author="Brian A. Lakstins" description="Filter StorageKey from list of keys so it can be handled internally.">
// </changelog>
#endregion

namespace MaxFactry.Base.DataLayer
{
    using System;
    using MaxFactry.Core;
    using System.Collections.Generic;
    using MaxFactry.Base.DataLayer.Library;

    /// <summary>
    /// Defines base data model with some standard storage properties
    /// </summary>
    public abstract class MaxBaseDataModel : MaxDataModel
    {
        /// <summary>
        /// Storage Key for use by a Repository provider to break records up in some manner.
        /// </summary>
        public readonly string StorageKey = "StorageKey";

        /// <summary>
        /// Time and date this record was created
        /// </summary>
        public readonly string CreatedDate = "CreatedDate";

        /// <summary>
        /// Last Update Date field
        /// </summary>
        public readonly string LastUpdateDate = "LastUpdateDate";

        /// <summary>
        /// Is Active field
        /// </summary>
        public readonly string IsActive = "IsActive";

        /// <summary>
        /// Is Deleted field
        /// </summary>
        public readonly string IsDeleted = "IsDeleted";

        /// <summary>
        /// True / False flags that are all stored in one field.  Up to 63 options.
        /// </summary>
        public readonly string OptionFlagList = "OptionFlagList";

        /// <summary>
        /// Name for property used to store name value pair information related to the record that may not be queryable
        /// </summary>
        public readonly string AttributeIndexText = "AttributeIndexText";

        /// <summary>
        /// Initializes a new instance of the MaxBaseDataModel class
        /// </summary>
        public MaxBaseDataModel() : base()
        {
            this.AddStorageKey(this.StorageKey, typeof(MaxShortString));
            this.AddType(this.CreatedDate, typeof(DateTime));
            this.AddType(this.LastUpdateDate, typeof(DateTime));
            this.AddType(this.IsActive, typeof(bool));
            this.AddType(this.IsDeleted, typeof(bool));
            this.AddNullable(this.OptionFlagList, typeof(long));
            this.AddNullable(this.AttributeIndexText, typeof(MaxLongString));
            this.RepositoryType = typeof(MaxBaseRepository);
            this.RepositoryProviderType = typeof(Provider.MaxBaseRepositoryDefaultProvider);
        }

        public override string[] GetStreamPath(MaxData loData)
        {
            List<string> loR = new List<string>();
            if (this.IsStored(this.StorageKey))
            {
                string lsStorageKey = MaxConvertLibrary.ConvertToString(typeof(object), loData.Get(this.StorageKey));
                if (string.IsNullOrEmpty(lsStorageKey))
                {
                    lsStorageKey = MaxDataLibrary.GetStorageKey(loData);
                }

                loR.Add(lsStorageKey);
            }

            string lsDataStorageName = this.DataStorageName;
            if (lsDataStorageName.EndsWith("MaxArchive"))
            {
                lsDataStorageName = lsDataStorageName.Substring(0, lsDataStorageName.Length - "MaxArchive".Length);
            }

            loR.Add(lsDataStorageName);

            return loR.ToArray();
        }

        public override string[] DataNameKeyList
        {
            get
            {
                MaxIndex loR = new MaxIndex();
                foreach (string lsDataNameKey in base.DataNameKeyList)
                {
                    if (lsDataNameKey != this.StorageKey)
                    {
                        loR.Add(lsDataNameKey, true);
                    }
                }

                return loR.GetSortedKeyList();
            }
        }
    }
}
