// <copyright file="MaxBaseIdIndexDataModel.cs" company="Lakstins Family, LLC">
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
// <change date="6/25/2014" author="Brian A. Lakstins" description="Based on MaxAppIdIndexDataModel without the AppId functionality.">
// <change date="7/9/2014" author="Brian A. Lakstins" description="Updated to work as base Key/Value store for any type of data.">
// <change date="3/26/2015" author="Brian A. Lakstins" description="Moved to MaxFactry.Base">
// <change date="12/9/2015" author="Brian A. Lakstins" description="Updated to include indexId as part of storage id.">
// <change date="12/21/2016" author="Brian A. Lakstins" description="Updated to use AddKey method.  Added override for GetPrimaryKeySuffix.">
// <change date="3/20/2024" author="Brian A. Lakstins" description="Happy birthday to my mom.  Sara Jean Lakstins (Cartwright) - 3/20/1944 to 3/14/2019.">
// <change date="3/22/2024" author="Brian A. Lakstins" description="Remove unused method.">
// </changelog>
#endregion

namespace MaxFactry.Base.DataLayer
{
	using System;
    using MaxFactry.Base.DataLayer;

	/// <summary>
	/// Defines base data model for hash table like data with a unique identifier
	/// </summary>
    public abstract class MaxBaseIdIndexDataModel : MaxBaseIdDataModel
	{
        /// <summary>
        /// IndexId for the Index
        /// </summary>
        public readonly string IndexId = "IndexId";

        /// <summary>
		/// Name for value
		/// </summary>
		public readonly string Name = "Name";

		/// <summary>
		/// Value stored based on the Name and current IndexId
		/// </summary>
		public readonly string Value = "Value";

		/// <summary>
        /// Initializes a new instance of the MaxBaseIdIndexDataModel class
		/// </summary>
        public MaxBaseIdIndexDataModel()
            : base()
		{
            this.RepositoryProviderType = typeof(MaxFactry.Base.DataLayer.Provider.MaxBaseIdIndexRepositoryDefaultProvider);
            this.RepositoryType = typeof(MaxBaseIdIndexRepository);
            this.AddDataKey(this.IndexId, typeof(Guid));
            this.AddDataKey(this.Name, typeof(MaxShortString));
            this.AddType(this.Value, typeof(string));
		}
	}
}
