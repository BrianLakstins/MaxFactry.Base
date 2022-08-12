// <copyright file="MaxBaseIdDataModel.cs" company="Lakstins Family, LLC">
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
// </changelog>
#endregion

namespace MaxFactry.Base.DataLayer
{
	using System;
    using MaxFactry.Base.DataLayer;

	/// <summary>
	/// Defines base data model with some standard storage properties
	/// </summary>
	public abstract class MaxBaseIdDataModel : MaxIdGuidDataModel
	{
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
		/// Extension field used for internal storage of properties
		/// </summary>
		public readonly string Extension = "Extension";

        /// <summary>
        /// Options flags associated with this data.
        /// </summary>
        public readonly string OptionFlagList = "OptionFlagList";

        /// <summary>
        /// Name for property used to store name value pair information
        /// </summary>
        public readonly string AttributeIndex = "AttributeIndex";

        /// <summary>
        /// Initializes a new instance of the MaxBaseIdDataModel class
        /// </summary>
        public MaxBaseIdDataModel() : base()
		{
            this.AddType(this.LastUpdateDate, typeof(DateTime));
            this.AddType(this.IsActive, typeof(bool));
            this.AddType(this.IsDeleted, typeof(bool));
            this.AddNullable(this.Extension, typeof(byte[]));
            this.AddNullable(this.OptionFlagList, typeof(long));
            this.AddNullable(this.AttributeIndex, typeof(byte[]));
        }
    }
}
