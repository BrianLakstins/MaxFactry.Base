// <copyright file="MaxBaseReadRepository.cs" company="Lakstins Family, LLC">
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
// <change date="3/22/2024" author="Brian A. Lakstins" description="Initial creation.  Based on MaxStorageReadRepository.">
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
    public class MaxBaseReadRepository : MaxByMethodFactory
	{
        /// <summary>
        /// Internal storage of single object
        /// </summary>
        private static MaxBaseReadRepository _oInstance = null;

        /// <summary>
        /// Lock object for multi-threaded access.
        /// </summary>
        private static object _oLock = new object();

        /// <summary>
        /// Gets the single instance of this class.
        /// </summary>
        public static MaxBaseReadRepository Instance
        {
            get
            {
                if (null == _oInstance)
                {
                    lock (_oLock)
                    {
                        if (null == _oInstance)
                        {
                            _oInstance = new MaxBaseReadRepository();
                        }
                    }
                }

                return _oInstance;
            }
        }

        /// <summary>
        /// Selects all not marked as deleted
        /// </summary>
        /// <param name="loData">Element with data used to determine the provider.</param>
        /// <param name="laDataNameList">list of fields to return from select</param>
        /// <returns>List of data from select</returns>
        public static MaxDataList SelectAll(MaxData loData, params string[] laDataNameList)
        {
            IMaxBaseReadRepositoryProvider loProvider = Instance.GetRepositoryProvider(loData);
            MaxDataList loDataList = loProvider.SelectAll(loData, laDataNameList);
            loDataList.Total = loDataList.Count;
            return loDataList;
        }

        /// <summary>
        /// Selects data from the database
        /// </summary>
        /// <param name="loData">Element with data used to determine the provider and used for basic filtering</param>
        /// <param name="loDataQuery">Query information to use for advanced filter results.</param>
        /// <param name="lnPageIndex">Page to return</param>
        /// <param name="lnPageSize">Items per page</param>
        /// <param name="lsOrderBy">Sort information</param>
        /// <param name="laDataNameList">list of fields to return from select</param>
        /// <returns>List of data from select</returns>
        public static MaxDataList Select(MaxData loData, MaxDataQuery loDataQuery, int lnPageIndex, int lnPageSize, string lsOrderBy, params string[] laDataNameList)
        {
            IMaxBaseReadRepositoryProvider loProvider = Instance.GetRepositoryProvider(loData);
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
        public static int SelectCount(MaxData loData, MaxDataQuery loDataQuery)
        {
            IMaxBaseReadRepositoryProvider loProvider = Instance.GetRepositoryProvider(loData);
            int lnR = loProvider.SelectCount(loData, loDataQuery);
            return lnR;
        }

        /// <summary>
        /// Opens stream data in storage
        /// </summary>
        /// <param name="loData">The data index for the object</param>
        /// <param name="lsKey">Data element name to write</param>
        /// <returns>Stream that was opened.</returns>
        public static Stream StreamOpen(MaxData loData, string lsKey)
        {
            IMaxBaseReadRepositoryProvider loProvider = Instance.GetRepositoryProvider(loData);
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
            IMaxBaseReadRepositoryProvider loProvider = Instance.GetRepositoryProvider(loData);
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
            IMaxBaseReadRepositoryProvider loProvider = Instance.GetRepositoryProvider(loData);
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
            string lsName = loData.Get("MaxRepositoryName") as string;
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
        /// Use IMaxRepositoryProvider as extended data to override
        /// </summary>
        /// <param name="loData">Data used to determine provider to use.</param>
        /// <returns>A provider for storing an object of that specified type</returns>
        public virtual IMaxBaseReadRepositoryProvider GetRepositoryProvider(MaxData loData)
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

            IMaxBaseReadRepositoryProvider loR = loProvider as IMaxBaseReadRepositoryProvider;
            if (null == loR)
            {
                MaxException loE = new MaxException("Error getting provider for interface IMaxBaseReadRepositoryProvider");
                if (null != loProvider)
                {
                    loE = new MaxException("Error casting [" + loProvider.GetType() + "] for Provider of type IMaxBaseReadRepositoryProvider", loE);
                }

                loE.Data.Add("MaxDataString", loData.ToString());
                throw loE;
            }

            return loR;
        }
	}
}