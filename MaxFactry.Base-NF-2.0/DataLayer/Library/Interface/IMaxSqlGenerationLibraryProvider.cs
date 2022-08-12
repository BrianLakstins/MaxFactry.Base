// <copyright file="IMaxSqlGenerationLibraryProvider.cs" company="Lakstins Family, LLC">
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
// <change date="2/26/2014" author="Brian A. Lakstins" description="Update to use MaxDataQuery.">
// <change date="4/22/2016" author="Brian A. Lakstins" description="Addition of altering a table.">
// </changelog>
#endregion

namespace MaxFactry.Base.DataLayer.Library
{
	using System;
    using MaxFactry.Core;

    /// <summary>
    /// Defines an interface for interacting with a Sql database
    /// </summary>
    public interface IMaxSqlGenerationLibraryProvider : IMaxProvider
	{
		/// <summary>
		/// Gets the command text based on base command text
		/// </summary>
		/// <param name="lsBaseCommandText">The base command text</param>
		/// <returns>The command text to use</returns>
		string GetCommandText(string lsBaseCommandText);

        /// <summary>
        /// Gets Sql to initialize database
        /// </summary>
        /// <returns>Sql to initialize database</returns>
        string GetDbInitialization();
        
        /// <summary>
		/// Gets Sql to check if a table exists
		/// </summary>
		/// <returns>Sql to check if a table exists</returns>
		string GetTableExists();

        /// <summary>
		/// Gets Sql to get a list of columns in the table
		/// </summary>
        /// <param name="lsTable">Name of the table to get columns for</param>
        /// <returns>Sql to get a list of columns for a table</returns>
        string GetColumnList(string lsTable);        

		/// <summary>
		/// Gets Sql to list all databases
		/// </summary>
		/// <returns>Sql to list all databases</returns>
		string GetDatabaseList();

		/// <summary>
		/// Gets Sql to create a database
		/// </summary>
		/// <param name="lsDatabase">Name of the database to create</param>
		/// <returns>Sql to create a database</returns>
		string GetCreateDatabase(string lsDatabase);

		/// <summary>
		/// Gets Sql to create a user
		/// </summary>
		/// <param name="lsDatabase">Name of the database</param>
		/// <returns>Sql to create a database</returns>
		string GetCreateDboUser(string lsDatabase);

		/// <summary>
		/// Gets Sql to give read only access to a user
		/// </summary>
		/// <param name="lsDatabase">Name of the database</param>
		/// <param name="lsUsername">Name of the user to grant read only access</param>
		/// <returns>Sql to create a database</returns>
		string GetAddReadOnlyUser(string lsDatabase, string lsUsername);

		/// <summary>
		/// Gets Sql to create a table
		/// </summary>
		/// <param name="loDataModel">definition of the table to create</param>
		/// <returns>Sql to create a table</returns>
		string GetTableCreate(MaxDataModel loDataModel);

        /// <summary>
        /// Gets Sql to alter a table
        /// </summary>
        /// <param name="loDataModel">definition of the table to alter</param>
        /// <param name="loDataList">results of query of current table structure</param>
        /// <returns>Sql to alter a table</returns>
        string GetTableAlter(MaxDataModel loDataModel, MaxDataList loDataList);

		/// <summary>
		/// Gets Sql to insert records into a table
		/// </summary>
		/// <param name="loIndexList">data to use for record inserts</param>
		/// <returns>Sql to insert records</returns>
		string GetInsert(MaxDataList loIndexList);

		/// <summary>
		/// Get Sql to update records
		/// </summary>
		/// <param name="loIndexList">table of data to use for record updates</param>
		/// <returns>Sql to perform updates</returns>
		string GetUpdate(MaxDataList loIndexList);

		/// <summary>
		/// Get Sql to delete records
		/// </summary>
		/// <param name="loIndexList">table of data to use to delete records</param>
		/// <returns>Sql to perform delete</returns>
		string GetDelete(MaxDataList loIndexList);

		/// <summary>
		/// Get Sql used to query the database
		/// </summary>
        /// <param name="loData">Element with data used in the filter.</param>
        /// <param name="loDataQuery">Query information to filter results.</param>
        /// <param name="laFields">list of fields to return from select.</param>
		/// <returns>Sql to perform query</returns>
		string GetSelect(MaxData loData, MaxDataQuery loDataQuery, params string[] laFields);

        /// <summary>
        /// Get Sql used to query the database
        /// </summary>
        /// <param name="lsDataStorageName">Data storage to get results from.</param>
        /// <param name="laFields">list of fields to return from select.</param>
        /// <returns>Sql to perform query</returns>
        string GetSelect(string lsDataStorageName, params string[] laFields);

		/// <summary>
		/// Get Sql used to query the database to get a count of matching records
		/// </summary>
		/// <param name="loData">Data used to create Sql</param>
        /// <param name="loDataQuery">Query information to filter results.</param>
        /// <returns>Sql to perform query</returns>
        string GetSelectCount(MaxData loData, MaxDataQuery loDataQuery);
	}
}
