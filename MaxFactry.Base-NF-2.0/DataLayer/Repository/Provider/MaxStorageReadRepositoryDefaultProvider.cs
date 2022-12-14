// <copyright file="MaxStorageReadRepositoryDefaultProvider.cs" company="Lakstins Family, LLC">
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
// <change date="6/26/2014" author="Brian A. Lakstins" description="Based on MaxCRUDRepositoryProvider">
// <change date="7/17/2014" author="Brian A. Lakstins" description="Removed default setting of IsActive and IsDeleted.  Null defaults to false for both.">
// <change date="8/21/2014" author="Brian A. Lakstins" description="Add stream support.">
// <change date="8/24/2014" author="Brian A. Lakstins" description="Update to inherit from MaxProviderBase.">
// <change date="9/26/2014" author="Brian A. Lakstins" description="Add ability to override ContextProvider on a per provider basis through a central configuration.">
// <change date="10/22/2014" author="Brian A. Lakstins" description="Add ability to specify DataContext through MaxData instance.">
// <change date="11/10/2014" author="Brian A. Lakstins" description="Update to moving Get and Set methods from MaxData.">
// <change date="11/11/2014" author="Brian A. Lakstins" description="Added general Selecting by a single property that does not include records marked as deleted.">
// <change date="12/2/2014" author="Brian A. Lakstins" description="Update to match interface.  Add laFields.  Restrict to only those that are not marked as deleted.">
// <change date="1/14/2015" author="Brian A. Lakstins" description="Update to throw exception when providers cannot be found.">
// <change date="1/28/2015" author="Brian A. Lakstins" description="Add the context provider name.">
// <change date="7/2/2016" author="Brian A. Lakstins" description="Updated to access provider configuration using base provider methods.">
// <change date="9/9/2019" author="Brian A. Lakstins" description="Add method to get value of a field in a dataquery.">
// <change date="6/4/2020" author="Brian A. Lakstins" description="Updated for change to base.">
// </changelog>
#endregion

namespace MaxFactry.Base.DataLayer.Provider
{
	using System;
    using System.IO;
	using MaxFactry.Core;

	/// <summary>
	/// Provides base for creating Providers for Repositories that use a subclass of MaxDataModel for storage.
	/// </summary>
	public class MaxStorageReadRepositoryDefaultProvider : MaxProvider, IMaxStorageReadRepositoryProvider
	{
        /// <summary>
        /// Gets or sets the Default provider for the Data Context.
        /// </summary>
        public virtual Type DefaultContextProviderType { get; set; }

        /// <summary>
        /// Gets or sets the provider name for the Data Context.
        /// </summary>
        public virtual string ContextProviderName { get; set; }

		/// <summary>
		/// Initializes the provider.
		/// </summary>
		/// <param name="lsName">Name of the provider.</param>
		/// <param name="loConfig">Configuration information.</param>
		public override void Initialize(string lsName, MaxIndex loConfig)
		{
            base.Initialize(lsName, loConfig);
            string lsDefaultContextProviderName = this.GetConfigValue(loConfig, "DefaultContextProviderName") as string;
            if (null != lsDefaultContextProviderName)
            {
                Type loDefaultContextProviderType = this.GetConfigValue(loConfig, lsDefaultContextProviderName) as Type;
                if (null != loDefaultContextProviderType)
                {
                    MaxFactry.Core.MaxLogLibrary.Log(new MaxLogEntryStructure(MaxEnumGroup.LogDebug, "Initialize {RepositoryProvider} default context provider set to {ContextProvider}", this.GetType(), loDefaultContextProviderType));
                    this.DefaultContextProviderType = loDefaultContextProviderType;
                }
            }

            string lsContextProviderName = this.GetConfigValue(loConfig, "ContextProviderName") as string;
            if (null != lsContextProviderName)
            {
                this.ContextProviderName = lsContextProviderName;
            }
		}

        /// <summary>
        /// Selects all data from the data storage name for the specified type.
        /// </summary>
        /// <param name="lsDataStorageName">Name of the data storage (table name).</param>
        /// <param name="laFields">list of fields to return from select</param>
        /// <returns>List of data elements with a base data model.</returns>
        public virtual MaxDataList SelectAll(string lsDataStorageName, params string[] laFields)
        {
            IMaxDataContextProvider loDataContextProvider = MaxDataLibrary.GetContextProvider(this, null);
            if (null == loDataContextProvider)
            {
                return new MaxDataList();
                //throw new MaxException("DataContextProvider was not found for [" + this.GetType().ToString() + "].  Check configuration for DataContextProvider.");
            }

            return loDataContextProvider.SelectAll(lsDataStorageName, laFields);
        }

        /// <summary>
        /// Selects data from the database.
        /// </summary>
        /// <param name="loData">Element with data used in the filter.</param>
        /// <param name="loDataQuery">Query information to filter results.</param>
        /// <param name="lnPageIndex">Page to return.</param>
        /// <param name="lsSort">Sorting information.</param>
        /// <param name="lnPageSize">Items per page.</param>
        /// <param name="lnTotal">Total items found.</param>
        /// <param name="laFields">list of fields to return from select.</param>
        /// <returns>List of data from select.</returns>
        public virtual MaxDataList Select(MaxData loData, MaxDataQuery loDataQuery, int lnPageIndex, int lnPageSize, string lsSort, out int lnTotal, params string[] laFields)
        {
            IMaxDataContextProvider loDataContextProvider = MaxDataLibrary.GetContextProvider(this, loData);
            lnTotal = 0;
            if (null == loDataContextProvider)
            {
                return new MaxDataList(loData.DataModel);
               //throw new MaxException("DataContextProvider was not found for [" + this.GetType().ToString() + "].  Check configuration for DataContextProvider.");
            }

            return loDataContextProvider.Select(loData, loDataQuery, lnPageIndex, lnPageSize, lsSort, out lnTotal, laFields);
        }

        /// <summary>
        /// Selects data from the database.
        /// </summary>
        /// <param name="loData">Element with data used in the filter.</param>
        /// <param name="loDataQuery">Query information to filter results.</param>
        /// <returns>List of data from select.</returns>
        public virtual int SelectCount(MaxData loData, MaxDataQuery loDataQuery)
        {
            IMaxDataContextProvider loDataContextProvider = MaxDataLibrary.GetContextProvider(this, loData);
            if (null == loDataContextProvider)
            {
                return 0;
                //throw new MaxException("DataContextProvider was not found for [" + this.GetType().ToString() + "].  Check configuration for DataContextProvider.");
            } 
            
            return loDataContextProvider.SelectCount(loData, loDataQuery);
        }

        /// <summary>
        /// Opens stream data in storage
        /// </summary>
        /// <param name="loData">The data index for the object</param>
        /// <param name="lsKey">Data element name to write</param>
        /// <returns>Stream that was opened.</returns>
        public virtual Stream StreamOpen(MaxData loData, string lsKey)
        {
            IMaxDataContextProvider loProvider = MaxDataLibrary.GetContextProvider(this, loData);
            if (null != loProvider)
            {
                Stream loStream = loProvider.StreamOpen(loData, lsKey);
                return loStream;
            }

            return null;
        }

        /// <summary>
        /// Removes stream from storage.
        /// </summary>
        /// <param name="loData">The data index for the object</param>
        /// <param name="lsKey">Data element name to remove</param>
        /// <returns>true if successful.</returns>
        public virtual string GetStreamUrl(MaxData loData, string lsKey)
        {
            IMaxDataContextProvider loProvider = MaxDataLibrary.GetContextProvider(this, loData);
            string lsR = string.Empty;
            if (null != loProvider)
            {
                lsR = loProvider.GetStreamUrl(loData, lsKey);
            }

            return lsR;
        }

        /// <summary>
        /// Gets the value of a field used in a DataQuery
        /// </summary>
        /// <param name="loDataQuery">DataQuery to use</param>
        /// <param name="lsFieldName">Field name to get value</param>
        /// <returns>The current value in the query that matches the field.  Null if no match.</returns>
        protected virtual object GetValue(MaxDataQuery loDataQuery, string lsFieldName)
        {
            object loR = null;
            object[] laDataQuery = loDataQuery.GetQuery();
            if (laDataQuery.Length > 0)
            {
                string lsDataQuery = string.Empty;
                for (int lnDQ = 0; lnDQ < laDataQuery.Length; lnDQ++)
                {
                    object loStatement = laDataQuery[lnDQ];
                    if (loStatement is MaxDataFilter)
                    {
                        MaxDataFilter loDataFilter = (MaxDataFilter)loStatement;
                        if (loDataFilter.Name == lsFieldName && loDataFilter.Operator == "=")
                        {
                            loR = loDataFilter.Value;
                        }
                    }
                }
            }

            return loR;
        }
    }
}
