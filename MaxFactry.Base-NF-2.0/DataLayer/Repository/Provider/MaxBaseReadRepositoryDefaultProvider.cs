﻿// <copyright file="MaxBaseReadRepositoryDefaultProvider.cs" company="Lakstins Family, LLC">
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
// <change date="3/22/2024" author="Brian A. Lakstins" description="Initial creation.  Based on MaxStorageReadRepositoryDefaultProvider.">
// <change date="3/24/2024" author="Brian A. Lakstins" description="Updated for changes namespaces">
// <change date="3/25/2024" author="Brian A. Lakstins" description="Use DataContextLibrary">
// <change date="5/22/2025" author="Brian A. Lakstins" description="Remove stream handling.">
// <change date="6/3/2025" author="Brian A. Lakstins" description="Add a default context provider.">
// </changelog>
#endregion

namespace MaxFactry.Base.DataLayer.Provider
{
	using System;
    using System.IO;
	using MaxFactry.Core;
    using MaxFactry.Base.DataLayer.Library;
    using MaxFactry.Base.DataLayer.Library.Provider;

    /// <summary>
    /// Provides base for creating Providers for Repositories that use a subclass of MaxDataModel for storage.
    /// </summary>
    public class MaxBaseReadRepositoryDefaultProvider : MaxProvider, IMaxBaseReadRepositoryProvider
	{
        /// <summary>
        /// An index to hold extensions and mime types.
        /// </summary>
        private static MaxIndex _oMimeTypeIndex = null;

        /// <summary>
        /// Lock to protect threads.
        /// </summary>
        private static object _oLock = new object();

        /// <summary>
        /// Gets an index to hold extensions and mime types.
        /// </summary>
        protected static MaxIndex MimeTypeIndex
        {
            get
            {
                if (null == _oMimeTypeIndex)
                {
                    lock (_oLock)
                    {
                        if (null == _oMimeTypeIndex)
                        {
                            _oMimeTypeIndex = new MaxIndex();
                            string lsContent = MaxFactryLibrary.GetStringResource(typeof(MaxBaseReadRepositoryDefaultProvider), "mimetypes");
                            if (lsContent.Contains("\n") && lsContent.Contains("\r"))
                            {
                                lsContent = lsContent.Replace('\r', ' ');
                            }
                            else if (lsContent.Contains("\r"))
                            {
                                lsContent = lsContent.Replace('\r', '\n');
                            }

                            string[] laContent = lsContent.Split('\n');
                            foreach (string lsLine in laContent)
                            {
                                if (lsLine.Length > 1 && !lsLine.Substring(0, 1).Equals("#") && lsLine.Contains("\t"))
                                {
                                    string[] laLine = lsLine.Split('\t');
                                    string lsMimeType = laLine[0];
                                    if (lsMimeType.Length > 0)
                                    {
                                        for (int lnM = 1; lnM < laLine.Length; lnM++)
                                        {
                                            string lsExtensionList = laLine[lnM].Trim();
                                            if (null != lsExtensionList && lsExtensionList.Length > 0)
                                            {
                                                string[] laExtensionList = lsExtensionList.Split(' ');
                                                for (int lnE = 0; lnE < laExtensionList.Length; lnE++)
                                                {
                                                    string lsExtensionPossible = laExtensionList[lnE].Trim().ToLower();
                                                    if (!_oMimeTypeIndex.Contains(lsExtensionPossible))
                                                    {
                                                        _oMimeTypeIndex.Add(lsExtensionPossible, lsMimeType);
                                                    }
                                                    else if (!(MaxConvertLibrary.ConvertToString(typeof(object), _oMimeTypeIndex[lsExtensionPossible]).ToLower().IndexOf(lsExtensionPossible) >= 0) &&
                                                        lsMimeType.ToLower().IndexOf(lsExtensionPossible) >= 0)
                                                    {
                                                        _oMimeTypeIndex[lsExtensionPossible] = lsMimeType;
                                                    }
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }

                return _oMimeTypeIndex;
            }
        }

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
            this.DefaultContextProviderType = typeof(MaxDataContextLibraryDefaultProvider);
            string lsDefaultContextProviderName = this.GetConfigValue(loConfig, MaxDataContextLibrary.DefaultContextProviderConfigName) as string;
            if (null != lsDefaultContextProviderName)
            {
                Type loDefaultContextProviderType = this.GetConfigValue(loConfig, lsDefaultContextProviderName) as Type;
                if (null != loDefaultContextProviderType)
                {
                    MaxFactry.Core.MaxLogLibrary.Log(new MaxLogEntryStructure(MaxEnumGroup.LogDebug, "Initialize {RepositoryProvider} default context provider set to {ContextProvider}", this.GetType(), loDefaultContextProviderType));
                    this.DefaultContextProviderType = loDefaultContextProviderType;
                }
            }

            string lsContextProviderName = this.GetConfigValue(loConfig, MaxDataContextLibrary.ContextProviderConfigName) as string;
            if (null != lsContextProviderName)
            {
                this.ContextProviderName = lsContextProviderName;
            }
		}

        /// <summary>
        /// Selects all data
        /// </summary>
        /// <param name="loData">Data to use as definition</param>
        /// <param name="laDataNameList">Names of fields to return</param>
        /// <returns>List of data that is stored</returns>
        public virtual MaxDataList SelectAll(MaxData loData, params string[] laDataNameList)
        {
            return MaxDataContextLibrary.SelectAll(this, loData, laDataNameList);
        }

        /// <summary>
        /// Selects data
        /// </summary>
        /// <param name="loData">Data to use as definition</param>
        /// <param name="loDataQuery">Filter for the query</param>
        /// <param name="lnPageIndex">Page number of the data</param>
        /// <param name="lnPageSize">Size of the page</param>
        /// <param name="lsOrderBy">Data field used to sort</param>
        /// <param name="laDataNameList">Names of fields to return</param>
        /// <returns>List of data that matches the query parameters</returns>
        public virtual MaxDataList Select(MaxData loData, MaxDataQuery loDataQuery, int lnPageIndex, int lnPageSize, string lsOrderBy, params string[] laDataNameList)
        {
            return MaxDataContextLibrary.Select(this, loData, loDataQuery, lnPageIndex, lnPageSize, lsOrderBy, laDataNameList);
        }

        /// <summary>
        /// Selects a count of records
        /// </summary>
        /// <param name="loData">Data to use as definition</param>
        /// <param name="loDataQuery">Filter for the query</param>
        /// <returns>Count that matches the query parameters</returns>
        public virtual int SelectCount(MaxData loData, MaxDataQuery loDataQuery)
        {
            return MaxDataContextLibrary.SelectCount(this, loData, loDataQuery);
        }

        /// <summary>
        /// Gets the mime-type of the file.
        /// </summary>
        /// <param name="lsName">File name</param>
        /// <returns>mime type of the file</returns>
        public virtual string GetMimeType(string lsName)
        {
            string lsR = "application/octet-stream";
            string lsExtension = MaxDataLibrary.GetFileNameExtension(lsName);
            if (null != lsExtension && lsExtension.Length > 0)
            {
                if (MimeTypeIndex.Contains(lsExtension.ToLower()))
                {
                    lsR = MimeTypeIndex[lsExtension.ToLower()].ToString();
                }
            }

            return lsR;
        }
    }
}
