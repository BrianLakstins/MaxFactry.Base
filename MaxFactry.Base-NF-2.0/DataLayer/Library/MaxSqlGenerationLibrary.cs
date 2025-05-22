// <copyright file="MaxSqlGenerationLibrary.cs" company="Lakstins Family, LLC">
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
// <change date="2/5/2014" author="Brian A. Lakstins" description="Fixed from using wrong interface.">
// <change date="2/26/2014" author="Brian A. Lakstins" description="Update to use MaxDataQuery.">
// <change date="4/22/2016" author="Brian A. Lakstins" description="Addition of altering a table.">
// <change date="5/10/2016" author="Brian A. Lakstins" description="Fix issue when table names have spaces that end up in parameter names.">
// <change date="5/18/2016" author="Brian A. Lakstins" description="Fix issue when table names has underscore in it which should be fine, but was being removed.">
// <change date="5/22/2025" author="Brian A. Lakstins" description="Review and update for consistency">
// </changelog>
#endregion

namespace MaxFactry.Base.DataLayer.Library
{
	using System;
    using MaxFactry.Core;

    /// <summary>
    /// Generates Sql specific to any database
    /// </summary>
    public class MaxSqlGenerationLibrary : MaxByMethodFactory
	{
        /// <summary>
        /// Valid short code name characters.
        /// </summary>
        private static string _sParameterNameValid = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789_";

        /// <summary>
        /// Internal storage of single object
        /// </summary>
        private static MaxSqlGenerationLibrary _oInstance = null;

        /// <summary>
        /// Lock object for multi-threaded access.
        /// </summary>
        private static object _oLock = new object();

        /// <summary>
        /// Gets the single instance of this class.
        /// </summary>
        public static MaxSqlGenerationLibrary Instance
        {
            get
            {
                if (null == _oInstance)
                {
                    lock (_oLock)
                    {
                        if (null == _oInstance)
                        {
                            _oInstance = new MaxSqlGenerationLibrary();
                        }
                    }
                }

                return _oInstance;
            }
        }

		/// <summary>
		/// Parses the command text and makes it provider specific
		/// </summary>
        /// <param name="lsName">The Name used to determine the provider</param>
        /// <param name="loType">The Type used to determine the provider</param>
		/// <param name="lsBaseCommandText">Generic Sql Command text to parse</param>
		/// <returns>Sql specific for current provider</returns>
		public static string GetCommandText(string lsName, Type loType, string lsBaseCommandText)
		{
            IMaxProvider loProvider = Instance.GetProviderByName(lsName, loType);
            if (loProvider is IMaxSqlGenerationLibraryProvider)
            {
                return ((IMaxSqlGenerationLibraryProvider)loProvider).GetCommandText(lsBaseCommandText);
            }

            MaxByMethodFactory.HandleInterfaceNotImplemented(loType, loProvider, "MaxSqlGenerationLibrary", "IMaxSqlGenerationLibraryProvider");
            return null;
        }

        /// <summary>
        /// Gets Sql to initialize database
        /// </summary>
        /// <param name="lsName">The Name used to determine the provider</param>
        /// <param name="loType">The Type used to determine the provider</param>
        /// <returns>Sql to initialize database</returns>
        public static string GetDbInitialization(string lsName, Type loType)
        {
            IMaxProvider loProvider = Instance.GetProviderByName(lsName, loType);
            if (loProvider is IMaxSqlGenerationLibraryProvider)
            {
                return ((IMaxSqlGenerationLibraryProvider)loProvider).GetDbInitialization();
            }

            MaxByMethodFactory.HandleInterfaceNotImplemented(loType, loProvider, "MaxSqlGenerationLibrary", "IMaxSqlGenerationLibraryProvider");
            return null;
        }

		/// <summary>
		/// Gets Sql to check if a table exists
		/// </summary>
        /// <param name="lsName">The Name used to determine the provider</param>
        /// <param name="loType">The Type used to determine the provider</param>
		/// <returns>Sql to check if a table exists</returns>
        public static string GetTableExists(string lsName, Type loType)
		{
            IMaxProvider loProvider = Instance.GetProviderByName(lsName, loType);
            if (loProvider is IMaxSqlGenerationLibraryProvider)
            {
                return ((IMaxSqlGenerationLibraryProvider)loProvider).GetTableExists();
            }

            MaxByMethodFactory.HandleInterfaceNotImplemented(loType, loProvider, "MaxSqlGenerationLibrary", "IMaxSqlGenerationLibraryProvider");
            return null;
        }

        /// <summary>
        /// Gets Sql to get a list of columns in the table
        /// </summary>
        /// <param name="lsName">The Name used to determine the provider</param>
        /// <param name="loType">The Type used to determine the provider</param>
        /// <param name="lsTable">The name of the table to get columns for</param>
        /// <returns>Sql to get a list of columns for a table</returns>
        public static string GetColumnList(string lsName, Type loType, string lsTable)
        {
            IMaxProvider loProvider = Instance.GetProviderByName(lsName, loType);
            if (loProvider is IMaxSqlGenerationLibraryProvider)
            {
                return ((IMaxSqlGenerationLibraryProvider)loProvider).GetColumnList(lsTable);
            }

            MaxByMethodFactory.HandleInterfaceNotImplemented(loType, loProvider, "MaxSqlGenerationLibrary", "IMaxSqlGenerationLibraryProvider");
            return null;
        }

		/// <summary>
		/// Gets Sql to list all databases
		/// </summary>
        /// <param name="lsName">The Name used to determine the provider</param>
        /// <param name="loType">The Type used to determine the provider</param>
		/// <returns>Sql to list all databases</returns>
        public static string GetDatabaseList(string lsName, Type loType)
		{
            IMaxProvider loProvider = Instance.GetProviderByName(lsName, loType);
            if (loProvider is IMaxSqlGenerationLibraryProvider)
            {
                return ((IMaxSqlGenerationLibraryProvider)loProvider).GetDatabaseList();
            }

            MaxByMethodFactory.HandleInterfaceNotImplemented(loType, loProvider, "MaxSqlGenerationLibrary", "IMaxSqlGenerationLibraryProvider");
            return null;
        }

		/// <summary>
		/// Gets Sql to create a database
		/// </summary>
        /// <param name="lsName">The Name used to determine the provider</param>
        /// <param name="loType">The Type used to determine the provider</param>
		/// <param name="lsDatabase">Name of the database to create</param>
		/// <returns>Sql to create a database</returns>
        public static string GetCreateDatabase(string lsName, Type loType, string lsDatabase)
		{
            IMaxProvider loProvider = Instance.GetProviderByName(lsName, loType);
            if (loProvider is IMaxSqlGenerationLibraryProvider)
            {
                return ((IMaxSqlGenerationLibraryProvider)loProvider).GetCreateDatabase(lsDatabase);
            }

            MaxByMethodFactory.HandleInterfaceNotImplemented(loType, loProvider, "MaxSqlGenerationLibrary", "IMaxSqlGenerationLibraryProvider");
            return null;
        }

		/// <summary>
		/// Gets Sql to create a user
		/// </summary>
        /// <param name="lsName">The Name used to determine the provider</param>
        /// <param name="loType">The Type used to determine the provider</param>
		/// <param name="lsDatabase">Name of the database</param>
		/// <returns>Sql to create a user</returns>
        public static string GetCreateDboUser(string lsName, Type loType, string lsDatabase)
		{
            IMaxProvider loProvider = Instance.GetProviderByName(lsName, loType);
            if (loProvider is IMaxSqlGenerationLibraryProvider)
            {
                return ((IMaxSqlGenerationLibraryProvider)loProvider).GetCreateDboUser(lsDatabase);
            }

            MaxByMethodFactory.HandleInterfaceNotImplemented(loType, loProvider, "MaxSqlGenerationLibrary", "IMaxSqlGenerationLibraryProvider");
            return null;
        }

        /// <summary>
        /// Gets Sql to give read only access to a user
        /// </summary>
        /// <param name="lsName">The Name used to determine the provider</param>
        /// <param name="loType">The Type used to determine the provider</param>
        /// <param name="lsDatabase">Name of the database</param>
        /// <param name="lsUsername">Name of the user to grant read only access</param>
        /// <returns>Sql to create a user</returns>
        public static string GetAddReadOnlyUser(string lsName, Type loType, string lsDatabase, string lsUsername)
		{
            IMaxProvider loProvider = Instance.GetProviderByName(lsName, loType);
            if (loProvider is IMaxSqlGenerationLibraryProvider)
            {
                return ((IMaxSqlGenerationLibraryProvider)loProvider).GetAddReadOnlyUser(lsDatabase, lsUsername);
            }

            MaxByMethodFactory.HandleInterfaceNotImplemented(loType, loProvider, "MaxSqlGenerationLibrary", "IMaxSqlGenerationLibraryProvider");
            return null;
        }

		/// <summary>
		/// Gets Sql to create a table
		/// </summary>
        /// <param name="lsName">The Name used to determine the provider</param>
        /// <param name="loType">The Type used to determine the provider</param>
		/// <param name="loDataModel">DataModel information used for select</param>
		/// <returns>Sql to create a table</returns>
        public static string GetTableCreate(string lsName, Type loType, MaxDataModel loDataModel)
		{
            IMaxProvider loProvider = Instance.GetProviderByName(lsName, loType);
            if (loProvider is IMaxSqlGenerationLibraryProvider)
            {
                return ((IMaxSqlGenerationLibraryProvider)loProvider).GetTableCreate(loDataModel);
            }

            MaxByMethodFactory.HandleInterfaceNotImplemented(loType, loProvider, "MaxSqlGenerationLibrary", "IMaxSqlGenerationLibraryProvider");
            return null;
        }

        /// <summary>
        /// Gets Sql to alter a table
        /// </summary>
        /// <param name="lsName">The Name used to determine the provider</param>
        /// <param name="loType">The Type used to determine the provider</param>
        /// <param name="loDataModel">DataModel information used for definition</param>
        /// <param name="loDataList">List of data to use to make sure table matches definition</param>
        /// <returns>Sql to alter a table</returns>
        public static string GetTableAlter(string lsName, Type loType, MaxDataModel loDataModel, MaxDataList loDataList)
        {
            IMaxProvider loProvider = Instance.GetProviderByName(lsName, loType);
            if (loProvider is IMaxSqlGenerationLibraryProvider)
            {
                return ((IMaxSqlGenerationLibraryProvider)loProvider).GetTableAlter(loDataModel, loDataList);
            }

            MaxByMethodFactory.HandleInterfaceNotImplemented(loType, loProvider, "MaxSqlGenerationLibrary", "IMaxSqlGenerationLibraryProvider");
            return null;
        }

        /// <summary>
        /// Gets Sql to insert records into a table
        /// </summary>
        /// <param name="lsName">The Name used to determine the provider</param>
        /// <param name="loType">The Type used to determine the provider</param>
        /// <param name="loDataList">List of data to insert</param>
        /// <returns>Sql to insert records</returns>
        public static string GetInsert(string lsName, Type loType, MaxDataList loDataList)
		{
            IMaxProvider loProvider = Instance.GetProviderByName(lsName, loType);
            if (loProvider is IMaxSqlGenerationLibraryProvider)
            {
                return ((IMaxSqlGenerationLibraryProvider)loProvider).GetInsert(loDataList);
            }

            MaxByMethodFactory.HandleInterfaceNotImplemented(loType, loProvider, "MaxSqlGenerationLibrary", "IMaxSqlGenerationLibraryProvider");
            return null;
        }

        /// <summary>
        /// Get Sql to update records
        /// </summary>
        /// <param name="lsName">The Name used to determine the provider</param>
        /// <param name="loType">The Type used to determine the provider</param>
        /// <param name="loDataList">List of data to use for updates</param>
        /// <returns>Sql to perform updates</returns>
        public static string GetUpdate(string lsName, Type loType, MaxDataList loDataList)
		{
            IMaxProvider loProvider = Instance.GetProviderByName(lsName, loType);
            if (loProvider is IMaxSqlGenerationLibraryProvider)
            {
                return ((IMaxSqlGenerationLibraryProvider)loProvider).GetUpdate(loDataList);
            }

            MaxByMethodFactory.HandleInterfaceNotImplemented(loType, loProvider, "MaxSqlGenerationLibrary", "IMaxSqlGenerationLibraryProvider");
            return null;
        }

        /// <summary>
        /// Get Sql to delete records
        /// </summary>
        /// <param name="lsName">The Name used to determine the provider</param>
        /// <param name="loType">The Type used to determine the provider</param>
        /// <param name="loDataList">List of data to use to delete</param>
        /// <returns>Sql to perform delete</returns>
        public static string GetDelete(string lsName, Type loType, MaxDataList loDataList)
		{
            IMaxProvider loProvider = Instance.GetProviderByName(lsName, loType);
            if (loProvider is IMaxSqlGenerationLibraryProvider)
            {
                return ((IMaxSqlGenerationLibraryProvider)loProvider).GetDelete(loDataList);
            }

            MaxByMethodFactory.HandleInterfaceNotImplemented(loType, loProvider, "MaxSqlGenerationLibrary", "IMaxSqlGenerationLibraryProvider");
            return null;
        }

        /// <summary>
        /// Selects data from the database.
        /// </summary>
        /// <param name="lsName">The Name used to determine the provider</param>
        /// <param name="loType">The Type used to determine the provider</param>
        /// <param name="loData">Element with data used in the filter.</param>
        /// <param name="loDataQuery">Query information to filter results.</param>
        /// <param name="laDataNameList">list of fields to return from select.</param>
        /// <returns>Sql to perform select</returns>
        public static string GetSelect(string lsName, Type loType, MaxData loData, MaxDataQuery loDataQuery, params string[] laDataNameList)
		{
            IMaxProvider loProvider = Instance.GetProviderByName(lsName, loType);
            if (loProvider is IMaxSqlGenerationLibraryProvider)
            {
                return ((IMaxSqlGenerationLibraryProvider)loProvider).GetSelect(loData, loDataQuery, laDataNameList);
            }

            MaxByMethodFactory.HandleInterfaceNotImplemented(loType, loProvider, "MaxSqlGenerationLibrary", "IMaxSqlGenerationLibraryProvider");
            return null;
        }

        /// <summary>
        /// Selects data from the database.
        /// </summary>
        /// <param name="lsName">The Name used to determine the provider</param>
        /// <param name="loType">The Type used to determine the provider</param>
        /// <param name="lsDataStorageName">Data storage to get results from.</param>
        /// <param name="laDataNameList">list of fields to return from select.</param>
        /// <returns>Sql to perform select</returns>
        public static string GetSelect(string lsName, Type loType, string lsDataStorageName, params string[] laDataNameList)
        {
            IMaxProvider loProvider = Instance.GetProviderByName(lsName, loType);
            if (loProvider is IMaxSqlGenerationLibraryProvider)
            {
                return ((IMaxSqlGenerationLibraryProvider)loProvider).GetSelect(lsDataStorageName, laDataNameList);
            }

            MaxByMethodFactory.HandleInterfaceNotImplemented(loType, loProvider, "MaxSqlGenerationLibrary", "IMaxSqlGenerationLibraryProvider");
            return null;
        }

        /// <summary>
        /// Get Sql used to query the database to get a count of matching records
        /// </summary>
        /// <param name="lsName">The Name used to determine the provider</param>
        /// <param name="loType">The Type used to determine the provider</param>
        /// <param name="loData">Data used to create Sql</param>
        /// <param name="loDataQuery">Query information to filter results.</param>
        /// <returns>Sql to perform select</returns>
        public static string GetSelectCount(string lsName, Type loType, MaxData loData, MaxDataQuery loDataQuery)
		{
            IMaxProvider loProvider = Instance.GetProviderByName(lsName, loType);
            if (loProvider is IMaxSqlGenerationLibraryProvider)
            {
                return ((IMaxSqlGenerationLibraryProvider)loProvider).GetSelectCount(loData, loDataQuery);
            }

            MaxByMethodFactory.HandleInterfaceNotImplemented(loType, loProvider, "MaxSqlGenerationLibrary", "IMaxSqlGenerationLibraryProvider");
            return null;
		}

        /// <summary>
        /// Gets the name for a parameter making sure that the characters are valid
        /// </summary>
        /// <param name="lsFieldName">Field name that matches the parameter</param>
        /// <param name="lnFieldNumber">Number of the record</param>
        /// <returns>Name for a parameter</returns>
        public static string GetParameterName(string lsFieldName, int lnFieldNumber)
        {
            string lsParamName = string.Empty;
            for (int lnF = 0; lnF < lsFieldName.Length; lnF++)
            {
                char lsChar = lsFieldName[lnF];
                if (_sParameterNameValid.IndexOf(lsChar) >= 0)
                {
                    lsParamName += lsChar.ToString();
                }
            }

            if (lnFieldNumber >= 0)
            {
                return lsParamName + "$" + lnFieldNumber.ToString();
            }

            return lsParamName;
        }
	}
}
