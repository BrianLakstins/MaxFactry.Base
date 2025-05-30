// <copyright file="MaxBaseRelationDateDataModel.cs" company="Lakstins Family, LLC">
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
// <change date="10/28/2022" author="Brian A. Lakstins" description="Initial creation.">
// <change date="3/12/2024" author="Brian A. Lakstins" description="Make the end date nullable.">
// </changelog>
#endregion

namespace MaxFactry.Base.DataLayer
{
	using System;

	/// <summary>
	/// Defines base data model to relate data with dates
	/// </summary>
	public abstract class MaxBaseRelationDateDataModel : MaxBaseRelationDataModel
	{
		/// <summary>
		/// StartTime
		/// </summary>
        public readonly string StartDate = "StartDate";

		/// <summary>
		/// Unique Identifier if the child
		/// </summary>
        public readonly string EndDate = "EndDate";

		/// <summary>
		/// Initializes a new instance of the MaxBaseRelationDateDataModel class
		/// </summary>
		public MaxBaseRelationDateDataModel()
			: base()
		{
            this.AddDataKey(this.StartDate, typeof(DateTime));
            this.AddNullable(this.EndDate, typeof(DateTime));
        }
	}
}
