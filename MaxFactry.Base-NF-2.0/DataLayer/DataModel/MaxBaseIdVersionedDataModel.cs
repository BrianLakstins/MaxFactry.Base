// <copyright file="MaxBaseIdVersionedDataModel.cs" company="Lakstins Family, LLC">
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
// <change date="11/10/2014" author="Brian A. Lakstins" description="Based on MaxAppIdDataModel without the AppId functionality.">
// <change date="3/26/2015" author="Brian A. Lakstins" description="Moved to MaxFactry.Base">
// <change date="6/3/2025" author="Brian A. Lakstins" description="Change base class. Update to use DataKey and StorageKey correctly">
// </changelog>
#endregion

namespace MaxFactry.Base.DataLayer
{
	using System;
    using System.IO;
    using MaxFactry.Base.DataLayer;

	/// <summary>
    /// Defines base data model for data with a unique identifier that is versioned
	/// </summary>
    public class MaxBaseIdVersionedDataModel : MaxBaseDataModel
    {
        /// <summary>
        /// Name used as the key for the versioned data.
        /// </summary>
        public readonly string Name = "Name";

        /// <summary>
        /// The version number of this data.
        /// </summary>
        public readonly string Version = "Version";

		/// <summary>
        /// Initializes a new instance of the MaxBaseIdVersionedDataModel class
		/// </summary>
        public MaxBaseIdVersionedDataModel()
			: base()
		{
            this.SetDataStorageName("MaxBaseIdVersioned");
            this.AddDataKey(this.Name, typeof(string));
            this.AddAttribute(this.Name, MaxDataModel.AttributeIsStorageKey, "true");
            this.AddDataKey(this.Version, typeof(int));
		}
	}
}
