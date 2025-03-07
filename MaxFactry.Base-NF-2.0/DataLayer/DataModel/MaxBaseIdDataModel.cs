﻿// <copyright file="MaxBaseIdDataModel.cs" company="Lakstins Family, LLC">
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
// <change date="6/25/2014" author="Brian A. Lakstins" description="Based on MaxAppDataModel without the AppId functionality.">
// <change date="10/17/2014" author="Brian A. Lakstins" description="Updated IsActive to be able to be null.">
// <change date="11/04/2014" author="Brian A. Lakstins" description="Added base option flag property.">
// <change date="3/26/2015" author="Brian A. Lakstins" description="Moved to MaxFactry.Base">
// <change date="11/29/2018" author="Brian A. Lakstins" description="Added property name for AttributeIndex to hold MaxIndex data.">
// <change date="12/2/2019" author="Brian A. Lakstins" description="Updated to reference base instantiation method.">
// <change date="3/20/2024" author="Brian A. Lakstins" description="Happy birthday to my mom.  Sara Jean Lakstins (Cartwright) - 3/20/1944 to 3/14/2019.">
// <change date="3/22/2024" author="Brian A. Lakstins" description="Change parent class.  Remove fields that are already in parent class.">
// <change date="3/26/2024" author="Brian A. Lakstins" description="Move logic for GetStreamPath from MaxData">
// <change date="1/21/2025" author="Brian A. Lakstins" description="Change base class">
// </changelog>
#endregion

namespace MaxFactry.Base.DataLayer
{
    using System;
    using System.Collections.Generic;
    using MaxFactry.Core;

    /// <summary>
    /// Defines base data model with some standard storage properties
    /// </summary>
	public abstract class MaxBaseIdDataModel : MaxBaseGuidKeyDataModel
	{
        /// <summary>
        /// Alternate Unique Identifier field
        /// </summary>
        public readonly string AlternateId = "AlternateId";

        /// <summary>
        /// Extension field used for internal storage of properties
        /// </summary>
        public readonly string Extension = "Extension";

        /// <summary>
        /// Initializes a new instance of the MaxBaseIdDataModel class
        /// </summary>
        public MaxBaseIdDataModel() : base()
		{
            this.AddNullable(this.AlternateId, typeof(MaxShortString));
            this.AddNullable(this.Extension, typeof(byte[]));
        }
    }
}
