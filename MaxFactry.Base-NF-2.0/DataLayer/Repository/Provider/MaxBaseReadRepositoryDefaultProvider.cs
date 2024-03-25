// <copyright file="MaxBaseReadRepositoryDefaultProvider.cs" company="Lakstins Family, LLC">
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
// </changelog>
#endregion

namespace MaxFactry.Base.DataLayer.Provider
{
	using System;
    using System.IO;
	using MaxFactry.Core;
    using MaxFactry.Base.DataLayer.Library;

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
            string lsDefaultContextProviderName = this.GetConfigValue(loConfig, MaxDataContextDefaultProvider.DefaultContextProviderConfigName) as string;
            if (null != lsDefaultContextProviderName)
            {
                Type loDefaultContextProviderType = this.GetConfigValue(loConfig, lsDefaultContextProviderName) as Type;
                if (null != loDefaultContextProviderType)
                {
                    MaxFactry.Core.MaxLogLibrary.Log(new MaxLogEntryStructure(MaxEnumGroup.LogDebug, "Initialize {RepositoryProvider} default context provider set to {ContextProvider}", this.GetType(), loDefaultContextProviderType));
                    this.DefaultContextProviderType = loDefaultContextProviderType;
                }
            }

            string lsContextProviderName = this.GetConfigValue(loConfig, MaxDataContextDefaultProvider.ContextProviderConfigName) as string;
            if (null != lsContextProviderName)
            {
                this.ContextProviderName = lsContextProviderName;
            }
		}

        /// <summary>
        /// Selects all data from the data storage name for the specified type.
        /// </summary>
        /// <param name="lsDataStorageName">Name of the data storage (table name).</param>
        /// <param name="laDataNameList">list of fields to return from select</param>
        /// <returns>List of data elements with a base data model.</returns>
        public virtual MaxDataList SelectAll(MaxData loData, params string[] laDataNameList)
        {
            IMaxDataContextProvider loDataContextProvider = MaxDataLibrary.GetContextProvider(this, null);
            if (null == loDataContextProvider)
            {
                return new MaxDataList();
                //throw new MaxException("DataContextProvider was not found for [" + this.GetType().ToString() + "].  Check configuration for DataContextProvider.");
            }

            return loDataContextProvider.SelectAll(loData, laDataNameList);
        }

        /// <summary>
        /// Selects data from the database.
        /// </summary>
        /// <param name="loData">Element with data used in the filter.</param>
        /// <param name="loDataQuery">Query information to filter results.</param>
        /// <param name="lnPageIndex">Page to return.</param>
        /// <param name="lsOrderBy">Sorting information.</param>
        /// <param name="lnPageSize">Items per page.</param>
        /// <param name="lnTotal">Total items found.</param>
        /// <param name="laDataNameList">list of fields to return from select.</param>
        /// <returns>List of data from select.</returns>
        public virtual MaxDataList Select(MaxData loData, MaxDataQuery loDataQuery, int lnPageIndex, int lnPageSize, string lsOrderBy, out int lnTotal, params string[] laDataNameList)
        {
            IMaxDataContextProvider loDataContextProvider = MaxDataLibrary.GetContextProvider(this, loData);
            lnTotal = 0;
            if (null == loDataContextProvider)
            {
                return new MaxDataList(loData.DataModel);
               //throw new MaxException("DataContextProvider was not found for [" + this.GetType().ToString() + "].  Check configuration for DataContextProvider.");
            }

            return loDataContextProvider.Select(loData, loDataQuery, lnPageIndex, lnPageSize, lsOrderBy, out lnTotal, laDataNameList);
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
        /// Gets the mime-type of the file.
        /// </summary>
        /// <param name="lsName">File name</param>
        /// <returns>Data updated based on sending of message.</returns>
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
