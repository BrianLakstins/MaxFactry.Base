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

        public const string DefaultContextProviderConfigName = "DefaultContextProviderName";

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

        public static MaxDataList SelectAll(IMaxRepositoryProvider loRepositoryProvider, MaxData loData, params string[] laDataNameList)
        {
            IMaxDataContextLibraryProvider loProvider = Instance.GetContextProvider(loRepositoryProvider, loData);
            MaxDataList loDataList = loProvider.SelectAll(loData, laDataNameList);
            loDataList.Total = loDataList.Count;
            return loDataList;
        }

        public static MaxDataList Select(IMaxRepositoryProvider loRepositoryProvider, MaxData loData, MaxDataQuery loDataQuery, int lnPageIndex, int lnPageSize, string lsOrderBy, params string[] laDataNameList)
        {
            IMaxDataContextLibraryProvider loProvider = Instance.GetContextProvider(loRepositoryProvider, loData);
            MaxDataList loDataList = loProvider.Select(loData, loDataQuery, lnPageIndex, lnPageSize, lsOrderBy, out int lnTotal, laDataNameList);
            loDataList.Total = lnTotal;
            return loDataList;
        }

        /// <summary>
        /// Selects data from the database
        /// </summary>
        /// <param name="loData">Element with data used to determine the provider and used for basic filtering</param>
        /// <param name="loDataQuery">Query information to use for advanced filter results.</param>
        /// <returns>Number of records that matches the data query</returns>
        public static int SelectCount(IMaxRepositoryProvider loRepositoryProvider, MaxData loData, MaxDataQuery loDataQuery)
        {
            IMaxDataContextLibraryProvider loProvider = Instance.GetContextProvider(loRepositoryProvider, loData);
            int lnR = loProvider.SelectCount(loData, loDataQuery);
            return lnR;
        }

        /// <summary>
        /// Opens stream data in storage
        /// </summary>
        /// <param name="loData">The data index for the object</param>
        /// <param name="lsKey">Data element name to write</param>
        /// <returns>Stream that was opened.</returns>
        public static Stream StreamOpen(IMaxRepositoryProvider loRepositoryProvider, MaxData loData, string lsKey)
        {
            IMaxDataContextLibraryProvider loProvider = Instance.GetContextProvider(loRepositoryProvider, loData);
            Stream loR = loProvider.StreamOpen(loData, lsKey);
            return loR;
        }

        /// <summary>
        /// Gets a Url to a stream.
        /// </summary>
        /// <param name="loData">The data index for the object</param>
        /// <param name="lsKey">Data element name</param>
        /// <returns>Url to stream.</returns>
        public static string GetStreamUrl(IMaxRepositoryProvider loRepositoryProvider, MaxData loData, string lsKey)
        {
            IMaxDataContextLibraryProvider loProvider = Instance.GetContextProvider(loRepositoryProvider, loData);
            string lsR = loProvider.GetStreamUrl(loData, lsKey);
            return lsR;
        }

        /// <summary>
        /// Inserts a new element of the specified type
        /// </summary>
        /// <param name="loData">The data index for the object</param>
        /// <returns>true if inserted</returns>
        public static bool Insert(IMaxRepositoryProvider loRepositoryProvider, MaxDataList loDataList)
        {
            IMaxDataContextLibraryProvider loProvider = Instance.GetContextProvider(loRepositoryProvider, loDataList);
            int lnTotal = loProvider.Insert(loDataList);
            if (lnTotal == loDataList.Count)
            {
                return true;
            }

            return false;
        }

        /// <summary>
        /// Updates an element of the specified type
        /// </summary>
        /// <param name="loData">The data index for the object</param>
        /// <returns>true if updated element</returns>
        public static bool Update(IMaxRepositoryProvider loRepositoryProvider, MaxDataList loDataList)
        {
            IMaxDataContextLibraryProvider loProvider = Instance.GetContextProvider(loRepositoryProvider, loDataList);
            int lnTotal = loProvider.Update(loDataList);
            if (lnTotal == loDataList.Count)
            {
                return true;
            }

            return false;
        }

        /// <summary>
        /// Deletes an element of the specified type
        /// </summary>
        /// <param name="loData">The data index for the object</param>
        /// <returns>true if deleted.  False if cannot be deleted.</returns>
        public static bool Delete(IMaxRepositoryProvider loRepositoryProvider, MaxDataList loDataList)
        {
            IMaxDataContextLibraryProvider loProvider = Instance.GetContextProvider(loRepositoryProvider, loDataList);
            int lnTotal = loProvider.Delete(loDataList);
            if (lnTotal == loDataList.Count)
            {
                return true;
            }

            return false;
        }

        /// <summary>
        /// Saves stream data to storage.
        /// </summary>
        /// <param name="loData">The data index for the object</param>
        /// <param name="lsKey">Data element name to write</param>
        /// <returns>Number of bytes written to storage.</returns>
        public static bool StreamSave(IMaxRepositoryProvider loRepositoryProvider, MaxData loData, string lsKey)
        {
            IMaxDataContextLibraryProvider loProvider = Instance.GetContextProvider(loRepositoryProvider, loData);
            bool lbR = loProvider.StreamSave(loData, lsKey);
            return lbR;
        }

        /// <summary>
        /// Removes stream from storage.
        /// </summary>
        /// <param name="loData">The data index for the object</param>
        /// <param name="lsKey">Data element name to remove</param>
        /// <returns>true if successful.</returns>
        public static bool StreamDelete(IMaxRepositoryProvider loRepositoryProvider, MaxData loData, string lsKey)
        {
            IMaxDataContextLibraryProvider loProvider = Instance.GetContextProvider(loRepositoryProvider, loData);
            bool lbR = loProvider.StreamDelete(loData, lsKey);
            return lbR;
        }

        /// <summary>
        /// Maps a class that uses a specific provider to that provider
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

        public virtual IMaxDataContextLibraryProvider GetContextProvider(IMaxRepositoryProvider loRepositoryProvider, MaxDataList loDataList)
        {
            return GetContextProvider(loRepositoryProvider, loDataList[0]);
        }
    }
}