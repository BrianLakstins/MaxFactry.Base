// <copyright file="MaxBaseKeyLongDataModel.cs" company="Lakstins Family, LLC">
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
// <change date="4/9/2025" author="Brian A. Lakstins" description="Initial creation">
// <change date="6/3/2025" author="Brian A. Lakstins" description="Add generic DataKey support">
// </changelog>
#endregion

namespace MaxFactry.Base.DataLayer
{
    using System;
    using MaxFactry.Core;
    using System.Collections.Generic;

    /// <summary>
    /// Defines base data model for data with a guid identifier.  Designed to replace MaxBaseIdDataModel.
    /// </summary>
    public abstract class MaxBaseKeyLongDataModel : MaxBaseDataModel
    {
        /// <summary>
        /// 64 bit integer generated on the client when creating new records
        /// </summary>
        public readonly string Id = "Id";

        /// <summary>
        /// Initializes a new instance of the MaxBaseGuidDataModel class
        /// </summary>
        public MaxBaseKeyLongDataModel()
            : base()
        {
            this.AddDataKey(this.Id, typeof(long));
        }

        public override string[] GetStreamPath(MaxData loData)
        {
            List<string> loR = new List<string>(base.GetStreamPath(loData));
            loR.Add(MaxConvertLibrary.ConvertToString(typeof(object), loData.Get(this.Id)));
            return loR.ToArray();
        }
    }
}
