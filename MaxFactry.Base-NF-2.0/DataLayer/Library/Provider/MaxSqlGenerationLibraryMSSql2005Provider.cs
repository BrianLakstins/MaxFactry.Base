// <copyright file="MaxSqlGenerationLibraryMSSql2005Provider.cs" company="Lakstins Family, LLC">
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
// <change date="3/19/2014" author="Brian A. Lakstins" description="Update to create Max length text columns.">
// <change date="3/26/2014" author="Brian A. Lakstins" description="Added short string support.">
// </changelog>
#endregion

namespace MaxFactry.Base.DataLayer.Library.Provider
{
	using System;

	/// <summary>
	/// Generates Sql specific to any database
	/// </summary>
	public class MaxSqlGenerationLibraryMSSql2005Provider : MaxSqlGenerationLibraryDefaultProvider
	{
		/// <summary>
        /// Initializes a new instance of the MaxSqlGenerationLibraryMSSql2005Provider class
		/// </summary>
		public MaxSqlGenerationLibraryMSSql2005Provider()
		{
			this.AddReplacement("#SchemaTable", "INFORMATION_SCHEMA.TABLES");
			this.AddReplacement("#TableNameField", "TABLE_NAME");
			this.AddReplacement("#DatabaseList", "sp_databases");
			this.AddReplacement("#TableExistFilter", " AND TABLE_TYPE='BASE TABLE'");

            this.AddReplacement(string.Concat("MaxDefinitionType.", typeof(byte[]), "."), "IMAGE");
			this.AddReplacement(string.Concat("MaxDefinitionType.", typeof(bool), "."), "TINYINT");
#if !MF_FRAMEWORK_VERSION_V4_3
			this.AddReplacement(string.Concat("MaxDefinitionType.", typeof(decimal), "."), "MONEY");
#endif
			this.AddReplacement(string.Concat("MaxDefinitionType.", typeof(DateTime), "."), "DATETIME");
			this.AddReplacement(string.Concat("MaxDefinitionType.", typeof(double), "."), "FLOAT");
			this.AddReplacement(string.Concat("MaxDefinitionType.", typeof(Guid), "."), "UNIQUEIDENTIFIER");
			this.AddReplacement(string.Concat("MaxDefinitionType.", typeof(short), "."), "SMALLINT");
			this.AddReplacement(string.Concat("MaxDefinitionType.", typeof(int), "."), "INT");
			this.AddReplacement(string.Concat("MaxDefinitionType.", typeof(long), "."), "BIGINT");
			this.AddReplacement(string.Concat("MaxDefinitionType.", typeof(float), "."), "FLOAT");
			this.AddReplacement(string.Concat("MaxDefinitionType.", typeof(string), "."), "NVARCHAR(MAX)");
            this.AddReplacement(string.Concat("MaxDefinitionType.", typeof(MaxShortString), "."), "NVARCHAR(512)");
            this.AddReplacement(string.Concat("MaxDefinitionType.", typeof(MaxLongString), "."), "NVARCHAR(4000)");

			this.AddReplacement("AUTOINCREMENT", "Identity(1,1)");

			this.AddReplacement("[", "\"");
			this.AddReplacement("]", "\"");
		}

        /// <summary>
        /// Gets Sql to create a table
        /// </summary>
        /// <param name="loDataModel">DataModel information used for select</param>
        /// <returns>Sql to create a table</returns>
        public override string GetTableCreate(MaxDataModel loDataModel)
        {
            string lsR = base.GetTableCreate(loDataModel);

            lsR += "CREATE CLUSTERED INDEX [idx_" + loDataModel.DataStorageName + "] ON [" + loDataModel.DataStorageName + "](";
            string[] laKeyList = loDataModel.GetKeyList();
            string lsPK = string.Empty;
            for (int lnK = 0; lnK < laKeyList.Length; lnK++)
            {
                bool lbIsPrimaryKey = loDataModel.GetPropertyAttributeSetting(laKeyList[lnK], "IsPrimaryKey");
                if (lbIsPrimaryKey)
                {
                    if (lsPK.Length > 0)
                    {
                        lsPK += ", ";
                    }

                    lsPK += "[" + laKeyList[lnK] + "] ASC";
                }
            }

            lsR += lsPK + ");";
            return lsR;
        }
	}
}
