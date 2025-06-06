﻿// <copyright file="MaxBaseRelationGuidKeyDataModel.cs" company="Lakstins Family, LLC">
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
// <change date="3/20/2024" author="Brian A. Lakstins" description="Happy birthday to my mom.  Sara Jean Lakstins (Cartwright) - 3/20/1944 to 3/14/2019.">
// <change date="3/24/2024" author="Brian A. Lakstins" description="Initial creation.">
// <change date="6/4/2025" author="Brian A. Lakstins" description="Remove key specification">
// </changelog>
#endregion

namespace MaxFactry.Base.DataLayer
{
	using System;

	/// <summary>
	/// Defines base data model to relate data with unique identifiers.
	/// </summary>
	public abstract class MaxBaseRelationGuidKeyDataModel : MaxBaseGuidKeyDataModel
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
        /// Initializes a new instance of the MaxBaseRelationDataModel class
        /// </summary>
        public MaxBaseRelationGuidKeyDataModel()
			: base()
		{
            this.AddType(this.ParentId, typeof(Guid));
            this.AddType(this.ChildId, typeof(Guid));
            this.AddNullable(this.Name, typeof(string));
            this.AddNullable(this.RelativeOrder, typeof(int));
            this.AddNullable(this.RelationType, typeof(MaxShortString));
        }
	}
}
