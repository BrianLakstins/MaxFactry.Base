// <copyright file="MaxBaseIdBranchDataModel.cs" company="Lakstins Family, LLC">
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
// <change date="6/25/2014" author="Brian A. Lakstins" description="Based on MaxAppIdBaseIdDataModel without the AppId functionality.">
// <change date="7/17/2014" author="Brian A. Lakstins" description="Add InherityType so there can be different types of inheritance from a single base.">
// <change date="3/26/2015" author="Brian A. Lakstins" description="Moved to MaxFactry.Base">
// </changelog>
#endregion

namespace MaxFactry.Base.DataLayer
{
	using System;
    using MaxFactry.Base.DataLayer;

	/// <summary>
	/// Data structure to use for creating data that can have properties overridden.
    /// This relationship is referred to as the "Base / Branch" relationship where each
    /// inherited item is a new Branch of the Base.
	/// </summary>
	public abstract class MaxBaseIdBranchDataModel : MaxBaseIdDataModel
	{
		/// <summary>
		/// Unique Identifier of base item.
		/// </summary>
        public readonly string RootId = "RootId";

        /// <summary>
        /// Type of data inheritance.
        /// </summary>
        public readonly string BranchType = "BranchType";

        /// <summary>
        /// Flags used to indicate which properties are overridden
        /// </summary>
        public readonly string OverrideList = "OverrideList";

		/// <summary>
        /// Initializes a new instance of the MaxBaseIdBranchDataModel class
		/// </summary>
        public MaxBaseIdBranchDataModel()
			: base()
		{
            this.AddNullable(this.RootId, typeof(Guid));
            this.AddNullable(this.BranchType, typeof(MaxShortString));
            this.AddNullable(this.OverrideList, typeof(long));
        }
	}
}
