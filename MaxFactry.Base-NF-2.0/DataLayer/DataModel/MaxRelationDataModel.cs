// <copyright file="MaxRelationDataModel.cs" company="Lakstins Family, LLC">
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
// <change date="6/25/2014" author="Brian A. Lakstins" description="Based on MaxAppRelationDataModel without the AppId functionality.">
// <change date="3/26/2015" author="Brian A. Lakstins" description="Moved to MaxFactry.Base">
// </changelog>
#endregion

namespace MaxFactry.Base.DataLayer
{
	using System;

	/// <summary>
	/// Defines base data model to relate data with unique identifiers.
	/// </summary>
	public abstract class MaxRelationDataModel : MaxDataModel
	{
		/// <summary>
		/// Unique Identifier if the parent
		/// </summary>
        public readonly string ParentId = "ParentId";

		/// <summary>
		/// Unique Identifier if the child
		/// </summary>
        public readonly string ChildId = "ChildId";

        /// <summary>
        /// Name of the relationship
        /// </summary>
        public readonly string Name = "Name";
        
        /// <summary>
		/// Relative order of the relationship
		/// </summary>
        public readonly string RelativeOrder = "RelativeOrder";

        /// <summary>
        /// Type of the relationship
        /// </summary>
        public readonly string RelationType = "RelationType";

		/// <summary>
        /// Initializes a new instance of the MaxRelationDataModel class
		/// </summary>
        public MaxRelationDataModel()
			: base()
		{
            this.AddKey(this.ParentId, typeof(Guid));
            this.AddKey(this.ChildId, typeof(Guid));
            this.AddNullable(this.Name, typeof(string));
            this.AddNullable(this.RelativeOrder, typeof(int));
            this.AddNullable(this.RelationType, typeof(MaxShortString));
        }
	}
}
