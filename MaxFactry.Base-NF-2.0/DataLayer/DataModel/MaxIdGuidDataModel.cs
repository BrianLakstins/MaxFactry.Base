// <copyright file="MaxIdGuidDataModel.cs" company="Lakstins Family, LLC">
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
// <change date="4/16/2015" author="Brian A. Lakstins" description="Initial creation">
// <change date="3/26/2024" author="Brian A. Lakstins" description="Move logic for GetStreamPath from MaxData">
// </changelog>
#endregion

namespace MaxFactry.Base.DataLayer
{
    using System;
    using System.Collections.Generic;
    using MaxFactry.Core;

    /// <summary>
    /// Defines base data model for data with an integer identifier
    /// </summary>
    public abstract class MaxIdGuidDataModel : MaxDataModel
    {
        /// <summary>
        /// Auto incrementing integer that uses some external rules to increment.
        /// Should be unique for each StorageKey
        /// </summary>
        public readonly string Id = "Id";

        /// <summary>
        /// Alternate Unique Identifier field
        /// </summary>
        public readonly string AlternateId = "AlternateId";

        /// <summary>
        /// Time and date this record was created
        /// </summary>
        public readonly string CreatedDate = "CreatedDate";

        /// <summary>
        /// Initializes a new instance of the MaxIdGuidDataModel class
        /// </summary>
        public MaxIdGuidDataModel()
            : base()
        {
            this.AddKey(this.Id, typeof(Guid));
            this.AddNullable(this.AlternateId, typeof(MaxShortString));
            this.AddType(this.CreatedDate, typeof(DateTime));
        }

        public override string[] GetStreamPath(MaxData loData)
        {
            List<string> loR = new List<string>();
            string lsDataStorageName = this.DataStorageName;
            if (lsDataStorageName.EndsWith("MaxArchive"))
            {
                lsDataStorageName = lsDataStorageName.Substring(0, lsDataStorageName.Length - "MaxArchive".Length);
            }

            loR.Add(lsDataStorageName);
            loR.Add(MaxConvertLibrary.ConvertToString(typeof(object), loData.Get(this.Id)));
            return loR.ToArray();
        }
    }
}
