// <copyright file="MaxStorageReadRepository.cs" company="Lakstins Family, LLC">
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
// <change date="6/26/2014" author="Brian A. Lakstins" description="Based on MaxCRUDRepository">
// <change date="6/27/2014" author="Brian A. Lakstins" description="Fix references to interfaces.">
// <change date="7/3/2014" author="Brian A. Lakstins" description="Added support for managing streams.">
// <change date="7/4/2014" author="Brian A. Lakstins" description="Added support for sending messages.">
// <change date="8/21/2014" author="Brian A. Lakstins" description="Add url support.">
// <change date="10/22/2014" author="Brian A. Lakstins" description="Add method to copy all data from currently configure provider to another provider.">
// <change date="12/2/2014" author="Brian A. Lakstins" description="Add laFields.">
// <change date="12/18/2014" author="Brian A. Lakstins" description="Updated Provider and DataModel access pattern.">
// <change date="1/14/2015" author="Brian A. Lakstins" description="Update to work without any deleted field.">
// <change date="3/26/2015" author="Brian A. Lakstins" description="Restructured.">
// <change date="6/1/2015" author="Brian A. Lakstins" description="Updated to set storage key so that Providers can just use it from MaxData that is sent.">
// <change date="5/10/2016" author="Brian A. Lakstins" description="Add ability to specify repository provider name.">
// <change date="8/29/2016" author="Brian A. Lakstins" description="Fix issue with select running from provider instead of static class.">
// <change date="12/21/2016" author="Brian A. Lakstins" description="Updated to work with meta copy of MaxData">
// <change date="12/21/2016" author="Brian A. Lakstins" description="Updated to centralize Provider usage.">
// <change date="10/8/2020" author="Brian A. Lakstins" description="Update order of determination for provider.">
// </changelog>
#endregion

namespace MaxFactry.Base.DataLayer
{
	using System;
    using System.IO;
    using MaxFactry.Core;

    /// <summary>
    /// Provides static methods to manipulate storage of data
    /// </summary>
    public class MaxStorageReadRepository : MaxByMethodFactory
	{
        /// <summary>
        /// Internal storage of single object
        /// </summary>
        private static MaxStorageReadRepository _oInstance = null;

        /// <summary>
        /// Lock object for multi-threaded access.
        /// </summary>
        private static object _oLock = new object();

        /// <summary>
        /// Gets the single instance of this class.
        /// </summary>
        public static MaxStorageReadRepository Instance
        {
            get
            {
                if (null == _oInstance)
                {
                    lock (_oLock)
                    {
                        if (null == _oInstance)
                        {
                            _oInstance = new MaxStorageReadRepository();
                        }
                    }
                }

                return _oInstance;
            }
        }

        /// <summary>
        /// Selects data from the database
        /// </summary>
        /// <param name="loData">Element with data used to determine the provider and used for the filter</param>
        /// <param name="loDataQuery">Query information to filter results.</param>
        /// <param name="lnPageIndex">Page to return</param>
        /// <param name="lnPageSize">Items per page</param>
        /// <param name="lsSort">Sort information</param>
        /// <param name="lnTotal">Total items found</param>
        /// <param name="laFields">list of fields to return from select</param>
        /// <returns>List of data from select</returns>
        public static MaxDataList Select(MaxData loData, MaxDataQuery loDataQuery, int lnPageIndex, int lnPageSize, string lsSort, out int lnTotal, params string[] laFields)
        {
            MaxData loDataFilter = loData.Clone();
            IMaxStorageReadRepositoryProvider loProvider = Instance.GetStorageReadRepositoryProvider(loDataFilter);
            MaxDataList loDataList = loProvider.Select(loDataFilter, loDataQuery, lnPageIndex, lnPageSize, lsSort, out lnTotal, laFields);
            return loDataList;
        }

        /// <summary>
        /// Selects all not marked as deleted
        /// </summary>
        /// <param name="loData">Element with data used to determine the provider.</param>
        /// <param name="lnPageIndex">Page to return</param>
        /// <param name="lnPageSize">Items per page</param>
        /// <param name="lsSort">Sort information</param>
        /// <param name="lnTotal">Total items found</param>
        /// <param name="laFields">list of fields to return from select</param>
        /// <returns>List of data from select</returns>
        public static MaxDataList SelectAll(MaxData loData, int lnPageIndex, int lnPageSize, string lsSort, out int lnTotal, params string[] laFields)
        {
            MaxData loDataFilter = new MaxData(loData);
            MaxDataQuery loDataQuery = new MaxDataQuery();
            MaxDataList loDataList = Select(loDataFilter, loDataQuery, lnPageIndex, lnPageSize, lsSort, out lnTotal, laFields);
            return loDataList;
        }
        
        /// <summary>
        /// Selects all not marked as deleted
        /// </summary>
        /// <param name="loData">Element with data used to determine the provider.</param>
        /// <param name="laFields">list of fields to return from select</param>
        /// <returns>List of data from select</returns>
        public static MaxDataList SelectAll(MaxData loData, params string[] laFields)
        {
            MaxData loDataFilter = new MaxData(loData);
            MaxDataQuery loDataQuery = new MaxDataQuery();
            int lnTotal = 0;
            MaxDataList loDataList = Select(loDataFilter, loDataQuery, 0, 0, string.Empty, out lnTotal, laFields);
            return loDataList;
        }

        /// <summary>
        /// Selects all based on a single property
        /// </summary>
        /// <param name="loData">Element with data used to determine the provider.</param>
        /// <param name="lsPropertyName">The name of the property used to select.</param>
        /// <param name="loPropertyValue">The value of the property used to select.</param>
        /// <param name="laFields">list of fields to return from select</param>
        /// <returns>List of data from select</returns>
        public static MaxDataList SelectAllByProperty(MaxData loData, string lsPropertyName, object loPropertyValue, params string[] laFields)
        {
            //// Set the property in case it is used on a PrimaryKey suffix
            loData.Set(lsPropertyName, loPropertyValue);
            MaxData loDataFilter = new MaxData(loData);
            //// Add a Query 
            MaxDataQuery loDataQuery = new MaxDataQuery();
            loDataQuery.StartGroup();
            loDataQuery.AddFilter(lsPropertyName, "=", loPropertyValue);
            loDataQuery.EndGroup();
            int lnTotal = 0;
            MaxDataList loDataList = Select(loDataFilter, loDataQuery, 0, 0, string.Empty, out lnTotal, laFields);
            return loDataList;
        }

        /// <summary>
        /// Selects all based on a compare to a single property
        /// </summary>
        /// <param name="loData">Element with data used to determine the provider.</param>
        /// <param name="lsPropertyName">The name of the property used to select.</param>
        /// <param name="loPropertyValue">The value of the property used to select.</param>
        /// <param name="lsPropertyComparer">Indicator on how to compare the property to the value.</param>
        /// <param name="laFields">list of fields to return from select</param>
        /// <returns>List of data from select</returns>
        public static MaxDataList SelectAllByPropertyCompare(MaxData loData, string lsPropertyName, object loPropertyValue, string lsPropertyComparer, params string[] laFields)
        {
            MaxData loDataFilter = new MaxData(loData);
            MaxDataQuery loDataQuery = new MaxDataQuery();
            loDataQuery.StartGroup();
            loDataQuery.AddFilter(lsPropertyName, lsPropertyComparer, loPropertyValue);
            loDataQuery.EndGroup();
            int lnTotal = 0;
            MaxDataList loDataList = Select(loDataFilter, loDataQuery, 0, 0, string.Empty, out lnTotal, laFields);
            return loDataList;
        }

        /// <summary>
        /// Selects all data from the data storage name for the specified type.
        /// </summary>
        /// <param name="loType">Type to use to determine the provider.</param>
        /// <param name="lsDataStorageName">Name of the data storage (table name).</param>
        /// <param name="laFields">list of fields to return from select</param>
        /// <returns>List of data elements with a base data model.</returns>
        public static MaxDataList SelectAll(Type loType, string lsDataStorageName, params string[] laFields)
        {
            IMaxStorageReadRepositoryProvider loProvider = Instance.GetProvider(loType) as IMaxStorageReadRepositoryProvider;
            if (null == loProvider)
            {
                throw new MaxException("Error casting [" + loProvider.GetType() + "] for Provider");
            }

            MaxDataList loDataList = loProvider.SelectAll(lsDataStorageName, laFields);
            return loDataList;
        }

        /// <summary>
        /// Opens stream data in storage
        /// </summary>
        /// <param name="loData">The data index for the object</param>
        /// <param name="lsKey">Data element name to write</param>
        /// <returns>Stream that was opened.</returns>
        public static Stream StreamOpen(MaxData loData, string lsKey)
        {
            IMaxStorageReadRepositoryProvider loProvider = Instance.GetStorageReadRepositoryProvider(loData);
            Stream loR = loProvider.StreamOpen(loData, lsKey);
            return loR;
        }

        /// <summary>
        /// Gets a Url to a stream.
        /// </summary>
        /// <param name="loData">The data index for the object</param>
        /// <param name="lsKey">Data element name</param>
        /// <returns>Url to stream.</returns>
        public static string GetStreamUrl(MaxData loData, string lsKey)
        {
            IMaxStorageReadRepositoryProvider loProvider = Instance.GetStorageReadRepositoryProvider(loData);
            string lsR = loProvider.GetStreamUrl(loData, lsKey);
            return lsR;
        }

        /// <summary>
        /// Gets the mime-type of the file.
        /// </summary>
        /// <param name="loData">The data index for the object</param>
        /// <param name="lsName">File name</param>
        /// <returns>Data updated based on sending of message.</returns>
        public static string GetMimeType(MaxData loData, string lsName)
        {
            IMaxStorageReadRepositoryProvider loProvider = Instance.GetStorageReadRepositoryProvider(loData);
            string lsR = loProvider.GetMimeType(lsName);
            return lsR;
        }

        /// <summary>
        /// Gets the provider based on the type of object being stored
        /// Use MaxStorageRepositoryName for a name of a provider to use to override
        /// Or override based on RepositoryProviderType 
        /// </summary>
        /// <param name="loData">Data used to determine provider to use.</param>
        /// <returns>A provider for storing an object of that specified type</returns>
        public virtual IMaxProvider GetProvider(MaxData loData)
        {
            IMaxProvider loR = null;
            string lsName = loData.Get("MaxStorageRepositoryName") as string;
            if (null != lsName && lsName.Length > 0)
            {
                //// Try to create the provider specific to this data and type
                loR = this.GetProviderByName(lsName, loData.DataModel.GetType());
                if (null == loR)
                {
                    if (null != loData.DataModel.RepositoryProviderType)
                    {
                        loR = this.GetProviderByName(lsName, loData.DataModel.RepositoryProviderType);
                    }
                    else if (null != loData.DataModel.RepositoryType)
                    {
                        loR = this.GetProviderByName(lsName, loData.DataModel.RepositoryType);
                    }
                }
            }

            if (null == loR)
            {
                //// Try to create the provider specific to the Data Model.
                if (null != loData.DataModel.RepositoryProviderType)
                {
                    loR = this.GetProvider(loData.DataModel.RepositoryProviderType);
                }
                else if (null != loData.DataModel.RepositoryType)
                {
                    loR = this.GetProvider(loData.DataModel.RepositoryType);
                }
               
                if (null == loR)
                {
                    //// This should fall back to object and always create something
                    loR = this.GetProvider(loData.DataModel.GetType());
                }
            }

            return loR;
        }

        /// <summary>
        /// Gets the provider based on the type of object being stored
        /// Use IMaxRepositoryProvider to in data to override
        /// </summary>
        /// <param name="loData">Data used to determine provider to use.</param>
        /// <returns>A provider for storing an object of that specified type</returns>
        public virtual IMaxStorageReadRepositoryProvider GetStorageReadRepositoryProvider(MaxData loData)
        {
            IMaxProvider loProvider = null;
            Type loDataProvider = loData.Get("IMaxRepositoryProvider") as Type;
            if (null != loDataProvider)
            {
                loProvider = this.GetProvider(loDataProvider);
            }

            if (null == loProvider)
            {
                loProvider = this.GetProvider(loData);
            }

            IMaxStorageReadRepositoryProvider loR = loProvider as IMaxStorageReadRepositoryProvider;
            if (null == loR)
            {
                throw new MaxException("Error casting [" + loR.GetType() + "] for Provider");
            }

            return loR;
        }
	}
}