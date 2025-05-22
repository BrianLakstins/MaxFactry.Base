// <copyright file="MaxDataContextLibrary.cs" company="Lakstins Family, LLC">
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
// <change date="3/25/2024" author="Brian A. Lakstins" description="Initial creation.">
// <change date="5/21/2025" author="Brian A. Lakstins" description="Remove stream handling.  Return flag based status codes. Always handle a list.  Review and update for consistency.">
// </changelog>
#endregion

namespace MaxFactry.Base.DataLayer.Library
{
    using System;
    using System.IO;
    using MaxFactry.Core;

    /// <summary>
    /// Provides static methods to manipulate storage of data
    /// </summary>
    public class MaxDataContextLibrary : MaxByMethodFactory
    {
        /// <summary>
        /// Maps classes that use a provider to the class that is the provider
        /// </summary>
        private MaxIndex _oContextProviderMapIndex = new MaxIndex();

        /// <summary>
        /// Internal storage of single object
        /// </summary>
        private static MaxDataContextLibrary _oInstance = null;

        /// <summary>
        /// Lock object for multi-threaded access.
        /// </summary>
        private static object _oLock = new object();

        /// <summary>
        /// Name of the configuration element for the default context provider
        /// </summary>
        public const string DefaultContextProviderConfigName = "DefaultContextProviderName";

        /// <summary>
        /// Name of the configuration element for the context provider
        /// </summary>
        public const string ContextProviderConfigName = "ContextProviderName";

        /// <summary>
        /// Gets the single instance of this class.
        /// </summary>
        public static MaxDataContextLibrary Instance
        {
            get
            {
                if (null == _oInstance)
                {
                    lock (_oLock)
                    {
                        if (null == _oInstance)
                        {
                            _oInstance = new MaxDataContextLibrary();
                        }
                    }
                }

                return _oInstance;
            }
        }

        /// <summary>
        /// Selects all data
        /// </summary>
        /// <param name="loRepositoryProvider">The repository provider used to determine the context provider</param>
        /// <param name="loData">Data to use as definition</param>
        /// <param name="laDataNameList">Names of fields to return</param>
        /// <returns>List of data that is stored</returns>
        public static MaxDataList SelectAll(IMaxRepositoryProvider loRepositoryProvider, MaxData loData, params string[] laDataNameList)
        {
            IMaxDataContextLibraryProvider loProvider = Instance.GetContextProvider(loRepositoryProvider, loData);
            MaxDataList loDataList = loProvider.SelectAll(loData, laDataNameList);
            loDataList.Total = loDataList.Count;
            return loDataList;
        }

        /// <summary>
        /// Selects data
        /// </summary>
        /// <param name="loRepositoryProvider">The repository provider used to determine the context provider</param>
        /// <param name="loData">Data to use as definition</param>
        /// <param name="loDataQuery">Filter for the query</param>
        /// <param name="lnPageIndex">Page number of the data</param>
        /// <param name="lnPageSize">Size of the page</param>
        /// <param name="lsOrderBy">Data field used to sort</param>
        /// <param name="laDataNameList">Names of fields to return</param>
        /// <returns>List of data that matches the query parameters</returns>
        public static MaxDataList Select(IMaxRepositoryProvider loRepositoryProvider, MaxData loData, MaxDataQuery loDataQuery, int lnPageIndex, int lnPageSize, string lsOrderBy, params string[] laDataNameList)
        {
            IMaxDataContextLibraryProvider loProvider = Instance.GetContextProvider(loRepositoryProvider, loData);
            MaxDataList loDataList = loProvider.Select(loData, loDataQuery, lnPageIndex, lnPageSize, lsOrderBy, out int lnTotal, laDataNameList);
            loDataList.Total = lnTotal;
            return loDataList;
        }

        /// <summary>
        /// Selects a count of records
        /// </summary>
        /// <param name="loRepositoryProvider">The repository provider used to determine the context provider</param>
        /// <param name="loData">Data to use as definition</param>
        /// <param name="loDataQuery">Filter for the query</param>
        /// <returns>Count that matches the query parameters</returns>
        public static int SelectCount(IMaxRepositoryProvider loRepositoryProvider, MaxData loData, MaxDataQuery loDataQuery)
        {
            IMaxDataContextLibraryProvider loProvider = Instance.GetContextProvider(loRepositoryProvider, loData);
            int lnR = loProvider.SelectCount(loData, loDataQuery);
            return lnR;
        }

        /// <summary>
        /// Inserts a new list of elements
        /// </summary>
        /// <param name="loRepositoryProvider">The repository provider used to determine the context provider</param>
        /// <param name="loDataList">The list of elements</param>
        /// <returns>Flag based status code indicating level of success.</returns>
        public static int Insert(IMaxRepositoryProvider loRepositoryProvider, MaxDataList loDataList)
        {
            int lnR = 0;
            if (loDataList.Count > 0)
            {
                IMaxDataContextLibraryProvider loProvider = Instance.GetContextProvider(loRepositoryProvider, loDataList[0]);
                lnR = loProvider.Insert(loDataList);
            }
            else
            {
                lnR |= 2;
                MaxLogLibrary.Log(new MaxLogEntryStructure(typeof(MaxDataContextLibrary), "Insert", MaxEnumGroup.LogInfo, "No data to insert."));
            }

            return lnR;
        }

        /// <summary>
        /// Updates a list of elements
        /// </summary>
        /// <param name="loRepositoryProvider">The repository provider used to determine the context provider</param>
        /// <param name="loDataList">The list of elements</param>
        /// <returns>Flag based status code indicating level of success.</returns>
        public static int Update(IMaxRepositoryProvider loRepositoryProvider, MaxDataList loDataList)            
        {
            int lnR = 0;
            if (loDataList.Count > 0)
            {
                IMaxDataContextLibraryProvider loProvider = Instance.GetContextProvider(loRepositoryProvider, loDataList[0]);
                lnR = loProvider.Update(loDataList);
            }
            else
            {
                lnR |= 2;
                MaxLogLibrary.Log(new MaxLogEntryStructure(typeof(MaxDataContextLibrary), "Update", MaxEnumGroup.LogInfo, "No data to update."));
            }
            
            return lnR;
        }

        /// <summary>
        /// Deletes a list of elements
        /// </summary>
        /// <param name="loRepositoryProvider">The repository provider used to determine the context provider</param>
        /// <param name="loDataList">The list of elements</param>
        /// <returns>Flag based status code indicating level of success.</returns>
        public static int Delete(IMaxRepositoryProvider loRepositoryProvider, MaxDataList loDataList)
        {
            int lnR = 0;
            if (loDataList.Count > 0)
            {
                IMaxDataContextLibraryProvider loProvider = Instance.GetContextProvider(loRepositoryProvider, loDataList[0]);
                lnR = loProvider.Delete(loDataList);
            }
            else
            {
                lnR |= 2;
                MaxLogLibrary.Log(new MaxLogEntryStructure(typeof(MaxDataContextLibrary), "Delete", MaxEnumGroup.LogInfo, "No data to delete."));
            }

            return lnR;
        }

        /// <summary>
        /// Registers a class that uses a specific provider to that provider
        /// </summary>
        /// <param name="loType">Class that uses a provider</param>
        /// <param name="loProviderType">The provider to use</param>
        public virtual void RegisterProvider(Type loType, Type loProviderType)
        {
            this._oContextProviderMapIndex.Add(loType.ToString(), loProviderType);
        }

        /// <summary>
        /// Gets the context provider using the Repository provider for this type
        /// </summary>
        /// <param name="loRepositoryProvider">The object calling the context provider.</param>
        /// <param name="loData">Data being acted upon by the context provider.</param>
        /// <returns>Context provider</returns>
        public virtual IMaxDataContextLibraryProvider GetContextProvider(IMaxRepositoryProvider loRepositoryProvider, MaxData loData)
        {
            // Try using the type of the caller as the type of the provider.  The Repository provider may also implement the IMaxDataContextLibraryProvider interface
            Type loProviderType = loRepositoryProvider.GetType();

            // See if a data context type has been configured for the provider.
            if (null != loRepositoryProvider.DefaultContextProviderType)
            {
                loProviderType = loRepositoryProvider.DefaultContextProviderType;
            }

            // See if a context provider was sent with the data
            if (null != loData && null != loData.Get("IMaxDataContextLibraryProvider") && loData.Get("IMaxDataContextLibraryProvider") is Type)
            {
                loProviderType = (Type)loData.Get("IMaxDataContextLibraryProvider");
            }

            // Use the mapping of one exists to override the specified provider.
            if (this._oContextProviderMapIndex.Contains(loProviderType.ToString()))
            {
                loProviderType = (Type)this._oContextProviderMapIndex[loProviderType.ToString()];
            }

            string lsName = loRepositoryProvider.Name;
            if (null != loRepositoryProvider.ContextProviderName && loRepositoryProvider.ContextProviderName.Length > 0)
            {
                lsName = loRepositoryProvider.ContextProviderName;
            }

            IMaxDataContextLibraryProvider loR = MaxFactryLibrary.CreateProvider(lsName, loProviderType) as IMaxDataContextLibraryProvider;
            return loR;
        }
    }
}