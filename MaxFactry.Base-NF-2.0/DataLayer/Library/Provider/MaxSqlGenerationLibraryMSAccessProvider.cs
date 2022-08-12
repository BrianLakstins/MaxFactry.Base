// <copyright file="MaxSqlGenerationLibraryMSAccessProvider.cs" company="Lakstins Family, LLC">
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
// <change date="1/22/2014" author="Brian A. Lakstins" description="Initial Release">
// </changelog>
#endregion

namespace MaxFactry.Base.DataLayer.Library.Provider
{
	using System;

	/// <summary>
	/// Generates Sql specific to any database
	/// </summary>
	public class MaxSqlGenerationLibraryMSAccessProvider : MaxSqlGenerationLibraryDefaultProvider
	{
		/// <summary>
        /// Initializes a new instance of the MaxSqlGenerationLibraryMSAccessProvider class
		/// </summary>
		public MaxSqlGenerationLibraryMSAccessProvider()
		{
			this.AddReplacement("#SchemaTable", "MSysObjects");
			this.AddReplacement("#TableNameField", "Name");
			this.AddReplacement("#TableExistFilter", " AND [Type]=1");
			this.AddReplacement("#COUNT", "COUNT(*)");

            this.AddReplacement(string.Concat("MaxDefinitionType.", typeof(byte[]), "."), "IMAGE");
			this.AddReplacement(string.Concat("MaxDefinitionType.", typeof(bool), "."), "BIT");
			this.AddReplacement(string.Concat("MaxDefinitionType.", typeof(DateTime), "."), "DATETIME");
			this.AddReplacement(string.Concat("MaxDefinitionType.", typeof(double), "."), "FLOAT");
			this.AddReplacement(string.Concat("MaxDefinitionType.", typeof(Guid), "."), "UNIQUEIDENTIFIER");
			this.AddReplacement(string.Concat("MaxDefinitionType.", typeof(short), "."), "SMALLINT");
			this.AddReplacement(string.Concat("MaxDefinitionType.", typeof(int), "."), "INTEGER");
			this.AddReplacement(string.Concat("MaxDefinitionType.", typeof(long), "."), "DOUBLE");
			this.AddReplacement(string.Concat("MaxDefinitionType.", typeof(float), "."), "REAL");
            this.AddReplacement(string.Concat("MaxDefinitionType.", typeof(string), "."), "MEMO");
            this.AddReplacement(string.Concat("MaxDefinitionType.", typeof(MaxShortString), "."), "TEXT(255)");
            this.AddReplacement(string.Concat("MaxDefinitionType.", typeof(MaxLongString), "."), "MEMO");
#if !MF_FRAMEWORK_VERSION_V4_3
            this.AddReplacement(string.Concat("MaxDefinitionType.", typeof(decimal), "."), "CURRENCY");
#endif

            this.AddReplacement("INTEGER AUTOINCREMENT", "AUTOINCREMENT");

			this.AddReplacement("%", "*");
		}
	}
}
