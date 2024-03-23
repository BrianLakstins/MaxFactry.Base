// <copyright file="MaxSqlGenerationLibraryDefaultProvider.cs" company="Lakstins Family, LLC">
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
// <change date="2/5/2014" author="Brian A. Lakstins" description="Added tracking of order of replacements.">
// <change date="2/22/2014" author="Brian A. Lakstins" description="Updated checking for changed properties and primary keys.">
// <change date="2/25/2014" author="Brian A. Lakstins" description="Made all methods virtual.">
// <change date="2/25/2014" author="Brian A. Lakstins" description="Update to use MaxDataQuery.">
// <change date="4/2/2014" author="Brian A. Lakstins" description="Update because MaxDataModel does not need a DataStorageName property.">
// <change date="4/14/2014" author="Brian A. Lakstins" description="Allow the same field to be used as multiple query parameters.">
// <change date="7/3/2014" author="Brian A. Lakstins" description="Updated to filter fields that are stored.">
// <change date="1/14/2015" author="Brian A. Lakstins" description="Update to allow queries without where clause.">
// <change date="4/22/2016" author="Brian A. Lakstins" description="Addition of altering a table.">
// <change date="5/10/2016" author="Brian A. Lakstins" description="Fix issue when table names have spaces that end up in parameter names.">
// <change date="5/18/2016" author="Brian A. Lakstins" description="Add support for server generated id (autoincrement).">
// <change date="8/11/2020" author="Brian A. Lakstins" description="Add default queries for SQL server for adding columns to tables">
// <change date="3/20/2024" author="Brian A. Lakstins" description="Updated for changes to DataModel.">
// </changelog>
#endregion

namespace MaxFactry.Base.DataLayer.Library.Provider
{
	using System;
	using System.Text;
    using MaxFactry.Core;

    /// <summary>
    /// Generates Sql specific to any database
    /// </summary>
    public class MaxSqlGenerationLibraryDefaultProvider : MaxProvider, IMaxSqlGenerationLibraryProvider
	{
		/// <summary>
		/// Dictionary used to store replacement text
		/// </summary>
		private MaxIndex _oReplacementIndex = new MaxIndex();

		/// <summary>
		/// Dictionary used to store replacement text
		/// </summary>
		private MaxIndex _oReplacementOrderIndex = new MaxIndex();
		
		/// <summary>
        /// Initializes a new instance of the MaxSqlGenerationLibraryDefaultProvider class
		/// </summary>
		public MaxSqlGenerationLibraryDefaultProvider()
		{
			// CREATE LOGIN <name of Windows User> FROM WINDOWS; GO 
			// CREATE LOGIN <login name> WITH PASSWORD = '<password>' ; GO 
			// Create Database <dbName>
			// Use <dbName>; CREATE USER <new user name> FOR LOGIN <login name> ; GO 
		}

        /// <summary>
        /// Initializes the provider
        /// </summary>
        /// <param name="lsName">Name of the provider</param>
        /// <param name="loConfig">Configuration information</param>
        public override void Initialize(string lsName, MaxIndex loConfig)
        {
            base.Initialize(lsName, loConfig);
        }

        /// <summary>
        /// Gets Sql to initialize database
        /// </summary>
        /// <returns>Sql to initialize database</returns>
        public virtual string GetDbInitialization()
        {
            return string.Empty;
        }

		/// <summary>
		/// Gets Sql to check if a table exists
		/// </summary>
		/// <returns>Sql to check if a table exists</returns>
		public virtual string GetTableExists()
		{
			return "SELECT COUNT(*) AS [RecordCount] FROM #SchemaTable WHERE #TableNameField = @TableName #TableExistFilter";
		}

        /// <summary>
        /// Gets Sql to get a list of columns in a table
        /// </summary>
        /// <param name="lsTable">The table</param>
        /// <returns>Sql to list columns in a table</returns>
        public virtual string GetColumnList(string lsTable)
        {
            return "select COLUMN_NAME as name from information_schema.columns where table_name = '" + lsTable + "'";
        }

		/// <summary>
		/// Gets Sql to list all databases
		/// </summary>
		/// <returns>Sql to list all databases</returns>
        public virtual string GetDatabaseList()
		{
			return "#DatabaseList";
		}

		/// <summary>
		/// Gets Sql to create a database
		/// </summary>
		/// <param name="lsDatabase">Name of the database</param>
		/// <returns>Sql to create a database</returns>
        public virtual string GetCreateDatabase(string lsDatabase)
		{
			return string.Concat("CREATE DATABASE [", lsDatabase, "]");
		}

		/// <summary>
		/// Gets Sql to create a user
		/// </summary>
		/// <param name="lsDatabase">Name of the database</param>
		/// <returns>Sql to create a database</returns>
        public virtual string GetCreateDboUser(string lsDatabase)
		{
			return string.Concat("CREATE LOGIN [", lsDatabase, "] WITH PASSWORD = @Password;USE [", lsDatabase, "];CREATE USER [", lsDatabase, "] FOR LOGIN [", lsDatabase, "]; EXEC sp_addrolemember N'db_owner', N'", lsDatabase, "';");
		}

		/// <summary>
		/// Gets Sql to give read only access to a user
		/// </summary>
		/// <param name="lsDatabase">Name of the database</param>
		/// <param name="lsUsername">Name of the user to grant read only access</param>
		/// <returns>Sql to add user access</returns>
        public virtual string GetAddReadOnlyUser(string lsDatabase, string lsUsername)
		{
			return string.Concat("USE [", lsDatabase, "];CREATE USER [", lsUsername, "] FOR LOGIN [", lsUsername, "]; EXEC sp_addrolemember N'db_datareader', N'", lsUsername, "';");
		}

		/// <summary>
		/// Gets Sql to create a table
		/// </summary>
		/// <param name="loDataModel">DataModel information used for select</param>
		/// <returns>Sql to create a table</returns>
        public virtual string GetTableCreate(MaxDataModel loDataModel)
		{
			StringBuilder loCommandText = new StringBuilder("CREATE TABLE [" + loDataModel.DataStorageName + "] (");
            int lnAdded = 0;
			foreach (string lsDataName in loDataModel.DataNameList)
			{
                if (loDataModel.IsStored(lsDataName))
                {
                    if (lnAdded != 0)
                    {
                        loCommandText.Append(",");
                    }

                    string lsType = string.Concat("MaxDefinitionType.", loDataModel.GetValueType(lsDataName), ".");
                    loCommandText.Append(string.Concat("[", lsDataName, "] ", lsType));

                    bool lbIsAutoIncrement = loDataModel.GetAttributeSetting(lsDataName, "IsAutoIncrement");

                    if (lbIsAutoIncrement)
                    {
                        loCommandText.Append(" AUTOINCREMENT");
                    }

                    bool lbIsAllowDBNull = loDataModel.GetAttributeSetting(lsDataName, "IsAllowDBNull");

                    if (!lbIsAllowDBNull)
                    {
                        loCommandText.Append(" NOT NULL");
                    }

                    lnAdded++;
                }
			}

			loCommandText.Append(");");
			return loCommandText.ToString();
		}

        /// <summary>
        /// Gets Sql to alter a table
        /// </summary>
        /// <param name="loDataModel">DataModel information</param>
        /// <param name="loDataList">Column information of current table structure</param>
        /// <returns>Sql to alter a table</returns>
        public virtual string GetTableAlter(MaxDataModel loDataModel, MaxDataList loDataList)
        {
            StringBuilder loCommandText = new StringBuilder();
            int lnAdded = 0;
            foreach (string lsDataName in loDataModel.DataNameList)
            {
                if (loDataModel.IsStored(lsDataName))
                {
                    bool lbExists = false;
                    for (int lnD = 0; lnD < loDataList.Count; lnD++)
                    {
                        MaxData loData = loDataList[lnD];
                        if (loData.Get("name").ToString() == lsDataName)
                        {
                            lbExists = true;
                        }
                    }

                    if (!lbExists)
                    {
                        string lsType = string.Concat("MaxDefinitionType.", loDataModel.GetValueType(lsDataName), ".");
                        loCommandText.Append(string.Concat("ALTER TABLE [", loDataModel.DataStorageName, "] ADD [", lsDataName, "] ", lsType, ";"));
                        lnAdded++;
                    }
                }
            }

            if (lnAdded > 0)
            {
                return loCommandText.ToString();
            }

            return string.Empty;
        }

		/// <summary>
		/// Parses the command text and makes it provider specific
		/// </summary>
		/// <param name="lsBaseCommandText">Generic Sql Command text to parse</param>
		/// <returns>Sql specific for current provider</returns>
        public virtual string GetCommandText(string lsBaseCommandText)
		{
			string lsCommandText = lsBaseCommandText;
			string[] laKeyList = this._oReplacementOrderIndex.GetSortedKeyList();
			foreach (string lsKeyId in laKeyList)
			{
				string lsKey = this._oReplacementOrderIndex[lsKeyId].ToString();
				while (lsCommandText.IndexOf(lsKey) > -1)
				{
					int lnStart = lsCommandText.IndexOf(lsKey);
					int lnEnd = lnStart + lsKey.Length;
					lsCommandText = string.Concat(
						lsCommandText.Substring(0, lnStart),
						this._oReplacementIndex[lsKey].ToString(),
						lsCommandText.Substring(lnEnd, lsCommandText.Length - lnEnd));
				}
			}

			return lsCommandText;
		}

		/// <summary>
		/// Gets Sql to insert records into a table
		/// </summary>
		/// <param name="loDataList">table of data to use for record inserts</param>
		/// <returns>Sql to insert records</returns>
        public virtual string GetInsert(MaxDataList loDataList)
		{
			if (!this.IsGoodIndexList(loDataList))
			{
				return string.Empty;
			}

			StringBuilder loSqlInsertClause = new StringBuilder();
			StringBuilder loSqlValueClause = new StringBuilder();
			StringBuilder loSqlSelectClause = new StringBuilder();
			StringBuilder loSqlQuery = new StringBuilder();
			string lsSqlFinal = string.Empty;
			bool lbUseSelect = false;
			for (int lnL = 0; lnL < loDataList.Count; lnL++)
			{
				MaxData loData = loDataList[lnL];
                foreach (string lsDataName in loDataList.DataModel.DataNameList)
                {
                    object loValue = loData.Get(lsDataName);
                    Type loDataType = loData.DataModel.GetValueType(lsDataName);
                    bool lbIsServerId = loDataList.DataModel.GetAttributeSetting(lsDataName, "IsServerId");

                    if (loData.DataModel.IsStored(lsDataName) && (null != loValue || typeof(bool).Equals(loDataType)) && !lbIsServerId)
                    {
                        if (0 == loSqlValueClause.Length)
                        {
                            loSqlValueClause.Append(" VALUES( ");
                        }
                        else
                        {
                            loSqlValueClause.Append(",");
                        }

                        if (0 == loSqlSelectClause.Length)
                        {
                            loSqlSelectClause.Append(" SELECT ");
                        }
                        else
                        {
                            loSqlSelectClause.Append(",");
                        }

                        bool lbIsClientId = loDataList.DataModel.GetAttributeSetting(lsDataName, "IsClientId");
                        if (lbIsClientId &&
                            loDataList.DataModel.GetValueType(lsDataName) != typeof(Guid))
                        {
                            loSqlSelectClause.Append(string.Concat("10 * FLOOR((isnull(max(", lsDataName, "),0)) / 10) + 10 + round(9 * rand(),0)"));
                            lsSqlFinal = string.Concat("SELECT max(", lsDataName, ") from [", loDataList.DataStorageName, "];");
                            lbUseSelect = true;
                        }
                        else
                        {
                            loSqlValueClause.Append(string.Concat("@", MaxSqlGenerationLibrary.GetParameterName(lsDataName, lnL)));
                            loSqlSelectClause.Append(string.Concat("@", MaxSqlGenerationLibrary.GetParameterName(lsDataName, lnL)));
                        }

                        if (0 == loSqlInsertClause.Length)
                        {
                            loSqlInsertClause.Append(string.Concat("INSERT INTO [", loDataList.DataStorageName, "] ("));
                        }
                        else
                        {
                            loSqlInsertClause.Append(", ");
                        }

                        loSqlInsertClause.Append(string.Concat("[", lsDataName, "]"));
                    }
				}

				if (lbUseSelect)
				{
					loSqlQuery.Append(string.Concat(loSqlInsertClause.ToString(), ") ", loSqlSelectClause.ToString(), " FROM ", loDataList.DataStorageName, ";", lsSqlFinal));
				}
				else
				{
					loSqlQuery.Append(string.Concat(loSqlInsertClause.ToString(), ") ", loSqlValueClause.ToString(), ");"));
				}

				loSqlInsertClause.Remove(0, loSqlInsertClause.Length);
				loSqlValueClause.Remove(0, loSqlValueClause.Length);
				loSqlInsertClause.Remove(0, loSqlInsertClause.Length);
			}

			return loSqlQuery.ToString();
		}

		/// <summary>
		/// Get Sql to update records
		/// </summary>
		/// <param name="loDataList">table of data to use for record updates</param>
		/// <returns>Sql to perform updates</returns>
        public virtual string GetUpdate(MaxDataList loDataList)
		{
			if (!this.IsGoodIndexList(loDataList))
			{
				return string.Empty;
			}

			StringBuilder loSqlUpdateClause = new StringBuilder();
			StringBuilder loSqlWhereClause = new StringBuilder();
			string lsSql = string.Empty;

			for (int lnL = 0; lnL < loDataList.Count; lnL++)
			{
				MaxData loData = loDataList[lnL];
				foreach (string lsDataName in loDataList.DataModel.DataNameList)
				{
                    if (loData.DataModel.IsStored(lsDataName))
                    {
                        if (loDataList.DataModel.IsPrimaryKey(lsDataName))
                        {
                            if (0 == loSqlWhereClause.Length)
                            {
                                loSqlWhereClause.Append(" WHERE ");
                            }
                            else
                            {
                                loSqlWhereClause.Append(" AND ");
                            }

                            loSqlWhereClause.Append(string.Concat("[", lsDataName, "] = @", lsDataName, "$", lnL.ToString()));
                        }
                        else if (loData.GetIsChanged(lsDataName))
                        {
                            if (0 == loSqlUpdateClause.Length)
                            {
                                loSqlUpdateClause.Append(string.Concat("UPDATE [", loDataList.DataStorageName, "] SET "));
                            }
                            else
                            {
                                loSqlUpdateClause.Append(", ");
                            }

                            loSqlUpdateClause.Append(string.Concat("[", lsDataName, "] = @", lsDataName, "$", lnL.ToString()));
                        }
                    }
				}

				if (loSqlUpdateClause.Length > 0)
				{
					if (lsSql.Length > 0)
					{
						lsSql += ";\r\n";
					}

					lsSql += loSqlUpdateClause.ToString() + loSqlWhereClause.ToString();
					loSqlUpdateClause.Remove(0, loSqlUpdateClause.Length);
					loSqlWhereClause.Remove(0, loSqlWhereClause.Length);
				}
			}

			return lsSql;
		}

		/// <summary>
		/// Get Sql to delete records
		/// </summary>
		/// <param name="loDataList">table of data to use to delete records</param>
		/// <returns>Sql to perform delete</returns>
        public virtual string GetDelete(MaxDataList loDataList)
		{
			if (!this.IsGoodIndexList(loDataList))
			{
				return string.Empty;
			}

			StringBuilder loSqlDeleteClause = new StringBuilder();
			for (int lnL = 0; lnL < loDataList.Count; lnL++)
			{
				MaxData loData = loDataList[lnL];
				foreach (string lsDataName in loDataList.DataModel.DataNameList)
				{
					if (loData.DataModel.IsStored(lsDataName) && loDataList.DataModel.IsPrimaryKey(lsDataName))
					{
						if (0 == loSqlDeleteClause.Length)
						{
							loSqlDeleteClause.Append(string.Concat("DELETE FROM [", loDataList.DataStorageName, "] WHERE "));
						}
						else
						{
							loSqlDeleteClause.Append(" AND ");
						}

						loSqlDeleteClause.Append(string.Concat("[", lsDataName, "] = @", lsDataName, "$", lnL.ToString()));
					}
				}
			}

			return loSqlDeleteClause.ToString();
		}

		/// <summary>
		/// Get Sql used to query the database
		/// </summary>
		/// <param name="loData">Data used to create Sql</param>
        /// <param name="loDataQuery">Query information to filter results.</param>
		/// <param name="laDataNameList">List of fields to pull from database</param>
		/// <returns>Sql to perform query</returns>
        public virtual string GetSelect(MaxData loData, MaxDataQuery loDataQuery, params string[] laDataNameList)
        {
            if (!this.IsGoodIndex(loData))
            {
                return string.Empty;
            }

            string lsR = string.Empty;
            string lsSqlWhere = this.GetWhere(loData, loDataQuery);
            MaxIndex loRequestedList = new MaxIndex();
            if (null != laDataNameList)
            {
                foreach (string lsDataName in laDataNameList)
                {
                    if (lsDataName.Length > 0)
                    {
                        loRequestedList.Add(lsDataName, true);
                    }
                }
            }

            StringBuilder loSqlSelectClause = new StringBuilder();
            foreach (string lsDataName in loData.DataModel.DataNameList)
            {
                if (loData.DataModel.IsStored(lsDataName))
                {
                    if (loRequestedList.Count == 0 || loRequestedList.Contains(lsDataName))
                    {
                        if (0 == loSqlSelectClause.Length)
                        {
                            loSqlSelectClause.Append("SELECT ");
                        }
                        else
                        {
                            loSqlSelectClause.Append(", ");
                        }

                        loSqlSelectClause.Append(string.Concat("[", lsDataName, "]"));
                        if (loRequestedList.Contains(lsDataName))
                        {
                            loRequestedList.Remove(lsDataName);
                        }
                    }
                }
            }

            if (null != laDataNameList)
            {
                foreach (string lsDataName in laDataNameList)
                {
                    if (loRequestedList.Contains(lsDataName) && this.IsFunction(lsDataName))
                    {
                        if (0 == loSqlSelectClause.Length)
                        {
                            loSqlSelectClause.Append("SELECT ");
                        }
                        else
                        {
                            loSqlSelectClause.Append(", ");
                        }

                        loSqlSelectClause.Append(lsDataName);
                        loRequestedList.Remove(lsDataName);
                    }
                } }

            lsR = string.Concat(loSqlSelectClause.ToString(), " FROM [", loData.DataModel.DataStorageName, "] ");
            if (lsSqlWhere.Length > 0)
            {
                lsR = string.Concat(lsR, lsSqlWhere);
            }

            return lsR;
        }

        /// <summary>
        /// Checks a requested field to determine if it's a function
        /// </summary>
        /// <param name="lsFunction"></param>
        /// <returns></returns>
        private bool IsFunction(string lsFunction)
        {
            if (lsFunction.Contains("(") && lsFunction.Contains(")"))
            {
                return true;
            }

            return false;
        }

        /// <summary>
        /// Get Sql used to query the database
        /// </summary>
        /// <param name="lsDataStorageName">Data storage to get results from.</param>
        /// <param name="laDataNameList">List of fields to pull from database</param>
        /// <returns>Sql to perform query</returns>
        public virtual string GetSelect(string lsDataStorageName, params string[] laDataNameList)
        {
            string lsR = string.Empty;
            MaxIndex loFieldsList = new MaxIndex();
            if (null != laDataNameList)
            {
                foreach (string lsField in laDataNameList)
                {
                    if (lsField.Length > 0)
                    {
                        loFieldsList.Add(lsField);
                    }
                }
            }

            StringBuilder loSqlSelectClause = new StringBuilder();

            MaxIndex loUsedList = new MaxIndex();
            string[] laFieldsKey = loFieldsList.GetSortedKeyList();
            for (int lnK = 0; lnK < laFieldsKey.Length; lnK++)
            {
                loUsedList.Add(laFieldsKey[lnK]);
            }

            for (int lnK = 0; lnK < laFieldsKey.Length; lnK++)
            {
                string lsKey = laFieldsKey[lnK];
                if (loUsedList.Contains(lsKey))
                {
                    if (0 == loSqlSelectClause.Length)
                    {
                        loSqlSelectClause.Append("SELECT ");
                    }
                    else
                    {
                        loSqlSelectClause.Append(", ");
                    }

                    loSqlSelectClause.Append(lsKey);
                    loUsedList.Remove(lsKey);
                }
            }

            if (0 == loSqlSelectClause.Length)
            {
                loSqlSelectClause.Append("SELECT *");
            }

            lsR = string.Concat(loSqlSelectClause.ToString(), " FROM [", lsDataStorageName, "] ");
            return lsR;
        }

		/// <summary>
		/// Get Sql used to query the database to get a count of matching records
		/// </summary>
		/// <param name="loData">Data used to create Sql</param>
        /// <param name="loDataQuery">Query information to filter results.</param>
        /// <returns>Sql to perform query</returns>
        public virtual string GetSelectCount(MaxData loData, MaxDataQuery loDataQuery)
		{
			if (!this.IsGoodIndex(loData))
			{
				return string.Empty;
			}

            string lsSqlWhere = this.GetWhere(loData, loDataQuery);

            return string.Concat("SELECT #COUNT FROM ", loData.DataModel.DataStorageName, " ", lsSqlWhere);
		}

		/// <summary>
		/// Parses data to create the "WHERE" clause
		/// </summary>
		/// <param name="loData">Data index with table definition</param>
        /// <param name="loDataQuery">Query information to filter results.</param>
        /// <returns>Sql for Where clause</returns>
        protected virtual string GetWhere(MaxData loData, MaxDataQuery loDataQuery)
		{
			StringBuilder loSqlWhereClause = new StringBuilder();
            //// Check the data to see if parts of the primary key are provided
			foreach (string lsDataName in loData.DataModel.DataNameKeyList)
			{
				if (null != loData.Get(lsDataName))
				{
					if (0 == loSqlWhereClause.Length)
					{
						loSqlWhereClause.Append(" WHERE ");
					}
					else
					{
						loSqlWhereClause.Append(" AND ");
					}

					loSqlWhereClause.Append(this.GetField(lsDataName));
				}
			}

            //// Add anything in a data query
            if (null != loDataQuery)
            {
                object[] laDataQuery = loDataQuery.GetQuery();
                if (laDataQuery.Length > 0)
                {
                    string lsDataQuery = string.Empty;
                    for (int lnDQ = 0; lnDQ < laDataQuery.Length; lnDQ++)
                    {
                        object loStatement = laDataQuery[lnDQ];
                        if (loStatement is char)
                        {
                            // Group Start or end characters
                            lsDataQuery += (char)loStatement;
                        }
                        else if (loStatement is string)
                        {
                            // Comparison operators
                            lsDataQuery += " " + (string)loStatement + " ";
                        }
                        else if (loStatement is MaxDataFilter)
                        {
                            MaxDataFilter loDataFilter = (MaxDataFilter)loStatement;
                            if (loDataFilter.Value == null)
                            {
                                lsDataQuery += "[" + loDataFilter.Name + "] " + loDataFilter.Operator;
                            }
                            else
                            {
                                lsDataQuery += "[" + loDataFilter.Name + "] " + loDataFilter.Operator + " @DQ" + lnDQ.ToString() + MaxSqlGenerationLibrary.GetParameterName(loDataFilter.Name, int.MinValue);
                            }
                        }
                    }

                    if (lsDataQuery.Length > 0)
                    {
                        if (0 == loSqlWhereClause.Length)
                        {
                            loSqlWhereClause.Append(" WHERE ");
                        }
                        else
                        {
                            loSqlWhereClause.Append(" AND ");
                        }

                        loSqlWhereClause.Append(lsDataQuery);
                    }
                }
            }

			return loSqlWhereClause.ToString();
		}

		/// <summary>
		/// Adds to the dictionary
		/// </summary>
		/// <param name="lsName">Sql to replace</param>
		/// <param name="lsReplacement">New Sql to use</param>
		protected void AddReplacement(string lsName, string lsReplacement)
		{
			this._oReplacementIndex.Add(lsName, lsReplacement);
			this._oReplacementOrderIndex.Add(lsName);
		}

		/// <summary>
		/// Gets a field and operator in a filter format
		/// </summary>
		/// <param name="lsFieldName">Column name</param>
		/// <param name="lsOperator">Comparative operation</param>
		/// <returns>Sql text for matching the field to a parameter</returns>
		private string GetField(string lsFieldName, string lsOperator)
		{
			return string.Concat("[", lsFieldName, "] ", lsOperator, " @", MaxSqlGenerationLibrary.GetParameterName(lsFieldName, int.MinValue));
		}
	
		/// <summary>
		/// Gets a field and operator in a filter format
		/// </summary>
		/// <param name="lsFieldName">Column name</param>
		/// <returns>Sql text for matching the field to a parameter</returns>
		private string GetField(string lsFieldName)
		{
			return this.GetField(lsFieldName, "=");
		}

		/// <summary>
		/// Checks a table that is sent to make sure it is usable
		/// </summary>
		/// <param name="loDataList">List of data to check</param>
		/// <returns>true if it can be used</returns>
		private bool IsGoodIndexList(MaxDataList loDataList)
		{
			if (null == loDataList)
			{
				return false;
			}

			if (0 == loDataList.DataStorageName.Length)
			{
				return false;
			}

			if (loDataList.DataModel.DataNameList.Length == 0)
			{
				return false;
			}

			return true;
		}

		/// <summary>
		/// Checks a table that is sent to make sure it is usable
		/// </summary>
		/// <param name="loData">Data to check</param>
		/// <returns>true if it can be used</returns>
		private bool IsGoodIndex(MaxData loData)
		{
			if (null == loData)
			{
				return false;
			}

            if (0 == loData.DataModel.DataStorageName.Length)
			{
				return false;
			}

			if (loData.DataModel.DataNameList.Length == 0)
			{
				return false;
			}
           
			return true;
		}
	}
}
