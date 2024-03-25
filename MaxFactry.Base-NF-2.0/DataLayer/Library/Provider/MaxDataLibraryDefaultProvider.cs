// <copyright file="MaxDataLibraryDefaultProvider.cs" company="Lakstins Family, LLC">
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
// <change date="6/26/2014" author="Brian A. Lakstins" description="Initial Release">
// <change date="6/27/2014" author="Brian A. Lakstins" description="Add way to override StorageKey by setting on Data.">
// <change date="6/28/2014" author="Brian A. Lakstins" description="Add Application level storage key to object storage key.">
// <change date="6/29/2014" author="Brian A. Lakstins" description="Updated to update storage key.">
// <change date="7/3/2014" author="Brian A. Lakstins" description="Added support for getting stream provider.">
// <change date="7/4/2014" author="Brian A. Lakstins" description="Added support for getting message provider.">
// <change date="7/21/2014" author="Brian A. Lakstins" description="Removed MaxAppId dependency stuff.">
// <change date="7/26/2014" author="Brian A. Lakstins" description="Make sure StorageKey is not Guid.Empty.">
// <change date="8/13/2014" author="Brian A. Lakstins" description="Add diagnostic logging. Optimize getting StorageKey.">
// <change date="8/21/2014" author="Brian A. Lakstins" description="Remove stream support.">
// <change date="8/24/2014" author="Brian A. Lakstins" description="Update to inherit from MaxProviderBase.">
// <change date="9/16/2014" author="Brian A. Lakstins" description="Rename.">
// <change date="9/26/2014" author="Brian A. Lakstins" description="Add some logging.">
// <change date="10/22/2014" author="Brian A. Lakstins" description="Add ability to specify DataContext through MaxData instance.">
// <change date="11/10/2014" author="Brian A. Lakstins" description="Update to moving Get and Set methods from MaxData.">
// <change date="1/14/2015" author="Brian A. Lakstins" description="Update to remove MaxStorageKey string being used.">
// <change date="12/09/2015" author="Brian A. Lakstins" description="Update to change application storage key to no longer include DataModel object type since it's redundant.">
// <change date="1/28/2015" author="Brian A. Lakstins" description="Add ability to specify the name of the data context provider.">
// <change date="12/21/2016" author="Brian A. Lakstins" description="Removed including suffix.  ">
// <change date="7/30/2019" author="Brian A. Lakstins" description="Update handling of StorageKey.">
// <change date="8/2/2019" author="Brian A. Lakstins" description="Fix for when storagekey configuration is specified as a guid, or the configured value is not ready at app startup.">
// <change date="5/19/2020" author="Brian A. Lakstins" description="Add time detailed logging to getting storage key when it takes longer.">
// <change date="5/20/2020" author="Brian A. Lakstins" description="Update logging.">
// <change date="5/22/2020" author="Brian A. Lakstins" description="Refactor GetStorageKey into multiple methods so they can be prioritized in sub classes">
// <change date="8/3/2020" author="Brian A. Lakstins" description="Add conditional method for netstandard1_2 for not having StopWatch">
// <change date="3/20/2024" author="Brian A. Lakstins" description="Happy birthday to my mom.  Sara Jean Lakstins (Cartwright) - 3/20/1944 to 3/14/2019.">
// <change date="3/22/2024" author="Brian A. Lakstins" description="Add some methods that were in Repository proviers.  Remove some unused methods.">
// <change date="3/24/2024" author="Brian A. Lakstins" description="Update to namespace.">
// <change date="3/25/2024" author="Brian A. Lakstins" description="Remove handling of DataContext">
// </changelog>
#endregion

namespace MaxFactry.Base.DataLayer.Library.Provider
{
    using System;
    using System.Diagnostics;
    using MaxFactry.Core;

    /// <summary>
    /// Provides static methods to manipulate storage of data
    /// </summary>
    public class MaxDataLibraryDefaultProvider : MaxProvider, IMaxDataLibraryProvider
    {
        private static object _oLock = new object();

        /// <summary>
        /// Maps classes that use a provider to the class that is the provider
        /// </summary>
        private MaxIndex _oDataModelMapIndex = new MaxIndex();

        /// <summary>
        /// Default storage key
        /// </summary>
        private string _sDefaultStorageKey = null;

        /// <summary>
        /// Gets the context provider using the Repository provider for this type
        /// </summary>
        /// <param name="loType">Type of context provider to get</param>
        /// <returns>Context provider</returns>
        public virtual MaxDataModel GetDataModel(Type loType)
        {
            Type loDataModelType = loType;
            if (this._oDataModelMapIndex.Contains(loType.ToString()))
            {
                loDataModelType = (Type)this._oDataModelMapIndex[loType.ToString()];
            }

            object loDataModel = MaxFactryLibrary.CreateSingleton(loDataModelType);
            return loDataModel as MaxDataModel;
        }

        /// <summary>
        /// Maps a class that uses a specific provider to that provider
        /// </summary>
        /// <param name="loType">Class that uses a provider</param>
        /// <param name="loDataModelType">The provider to use</param>
        public virtual void RegisterDataModelProvider(Type loType, Type loDataModelType)
        {
            this._oDataModelMapIndex.Add(loType.ToString(), loDataModelType);
        }

        /// <summary>
        /// Gets the storage key used to separate the storage of data
        /// </summary>
        /// <param name="loData">The data to be stored using the storage key.</param>
        /// <returns>string used for the storage key</returns>
        public virtual string GetStorageKey(MaxData loData)
        {
            string lsR = this.GetStorageKeyFromProcess();
            MaxBaseDataModel loBaseDataModel = loData.DataModel as MaxBaseDataModel;
            if (null != loBaseDataModel && null != loData.Get(loBaseDataModel.StorageKey))
            {
                lsR = MaxConvertLibrary.ConvertToString(typeof(object), loData.Get(loBaseDataModel.StorageKey));   
            }
            else if (null == lsR || lsR.Length.Equals(0))
            {
                lsR = this.GetStorageKeyFromConfiguration();
            }

            if (lsR.Length == 0)
            {
                MaxFactry.Core.MaxLogLibrary.Log(new MaxLogEntryStructure("GetStorageKeyProvider", MaxEnumGroup.LogError, "GetStorageKey(MaxData loData) ended with blank storagekey."));
            }

            return lsR;
        }

        /// <summary>
        /// Gets the extension of a file name.
        /// </summary>
        /// <param name="lsName">Name of a file.</param>
        /// <returns>Extension of the file.</returns>
        public virtual string GetFileNameExtension(string lsName)
        {
            string lsR = string.Empty;
            if (lsName.IndexOf(".") > 0 && lsName.LastIndexOf('.') != lsName.Length - 1)
            {
                lsR = lsName.Substring(lsName.LastIndexOf('.') + 1);
            }

            return lsR;
        }

        /// <summary>
        /// Saves a data to a file
        /// </summary>
        /// <param name="lsDirectory"></param>
        /// <param name="loData"></param>
        /// <returns></returns>
        public virtual bool SaveAsFile(string lsDirectory, MaxData loData)
        {
            try
            {
                lock (_oLock)
                {
                    string lsFile = System.IO.Path.Combine(lsDirectory, loData.DataModel.DataStorageName + ".json");
                    MaxIndex loIndex = new MaxIndex();
                    MaxIndex loDataIndex = new MaxIndex();
                    foreach (string lsDataName in loData.DataModel.DataNameList)
                    {
                        loDataIndex.Add(lsDataName, loData.Get(lsDataName));
                    }

                    loIndex.Add(loDataIndex);
                    string lsJson = MaxConvertLibrary.SerializeObjectToString(loIndex);
                    System.IO.File.WriteAllText(lsFile, lsJson);
                }

                return true;
            }
            catch (Exception loE)
            {
                MaxLogLibrary.Log(new MaxLogEntryStructure(this.GetType(), "SaveAsFile", MaxEnumGroup.LogEmergency, "Exception saving data to file", loE));
            }

            return false;
        }

        /// <summary>
        /// Loads data from a file
        /// </summary>
        /// <param name="lsDirectory"></param>
        /// <param name="loData"></param>
        /// <returns></returns>
        public virtual MaxDataList LoadFromFile(string lsDirectory, MaxData loData)
        {
            MaxDataList loR = new MaxDataList(loData.DataModel);
            try
            {
                lock (_oLock)
                {
                    string lsFile = System.IO.Path.Combine(lsDirectory, loData.DataModel.DataStorageName + ".json");
                    if (System.IO.File.Exists(lsFile))
                    {
                        string lsJson = System.IO.File.ReadAllText(lsFile);
                        MaxIndex loIndex = MaxConvertLibrary.DeserializeObject(lsJson, typeof(MaxIndex)) as MaxIndex;
                        if (loIndex != null)
                        {
                            string[] laIndexKey = loIndex.GetSortedKeyList();
                            if (laIndexKey.Length > 0)
                            {
                                MaxIndex loDataIndex = loIndex[laIndexKey[0]] as MaxIndex;
                                if (loDataIndex != null)
                                {
                                    //// Load multiple records
                                    foreach (string lsIndexKey in laIndexKey)
                                    {
                                        loDataIndex = loIndex[lsIndexKey] as MaxIndex;
                                        if (loDataIndex != null)
                                        {
                                            MaxData loDataOut = new MaxData(loData.DataModel);
                                            foreach (string lsDataName in loData.DataModel.DataNameList)
                                            {
                                                loDataOut.Set(lsDataName, loDataIndex[lsDataName]);
                                            }

                                            loR.Add(loDataOut);
                                        }
                                    }
                                }
                                else
                                {
                                    //// Load 1 record
                                    MaxData loDataOut = new MaxData(loData.DataModel);
                                    foreach (string lsDataName in loData.DataModel.DataNameList)
                                    {
                                        loDataOut.Set(lsDataName, loIndex[lsDataName]);
                                    }

                                    loR.Add(loDataOut);
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception loE)
            {
                MaxLogLibrary.Log(new MaxLogEntryStructure(this.GetType(), "LoadFromFile", MaxEnumGroup.LogEmergency, "Exception loading data from a file", loE));
            }

            return loR;
        }

        /// <summary>
        /// Gets the value of a field used in a DataQuery
        /// </summary>
        /// <param name="loDataQuery">DataQuery to use</param>
        /// <param name="lsDataName">Field name to get value</param>
        /// <returns>The current value in the query that matches the field.  Null if no match.</returns>
        public virtual object GetValue(MaxDataQuery loDataQuery, string lsDataName)
        {
            object loR = null;
            if (null != loDataQuery)
            {
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
                            if (loDataFilter.Name == lsDataName && loDataFilter.Operator == "=")
                            {
                                loR = loDataFilter.Value;
                            }
                        }
                    }
                }
            }

            return loR;
        }

        protected virtual string GetStorageKeyFromProcess()
        {
            string lsR = MaxConfigurationLibrary.GetValue(MaxEnumGroup.ScopeProcess, MaxFactryLibrary.MaxStorageKeyName) as string;
            return lsR;
        }


#if netstandard1_2
        protected virtual string GetStorageKeyFromConfigurationConditional()
        {
            string lsR = string.Empty;
            string lsLog = string.Empty;
            string lsDefaultStorageKey = new Guid("7AAFA5DF-B2A9-462F-918A-C8A4242DC348").ToString().ToLower();
            if (null == this._sDefaultStorageKey || this._sDefaultStorageKey == lsDefaultStorageKey)
            {
                //// Without the storagekey defined in the data, default to the storagekey for the application
                this._sDefaultStorageKey = MaxConvertLibrary.ConvertToString(typeof(object), MaxConfigurationLibrary.GetValue(MaxEnumGroup.ScopeApplication, MaxFactryLibrary.MaxStorageKeyName)).ToLower();
                if (null == this._sDefaultStorageKey || this._sDefaultStorageKey.Length == 0)
                {
                    this._sDefaultStorageKey = lsDefaultStorageKey;
                }
            }

            object loStorageKey = MaxConfigurationLibrary.GetValue(MaxEnumGroup.ScopeSession, MaxFactryLibrary.MaxStorageKeyName);
            if (null == loStorageKey || loStorageKey.ToString().Trim().Length.Equals(0) || (loStorageKey is Guid && Guid.Empty.Equals((Guid)loStorageKey)))
            {
                loStorageKey = MaxConfigurationLibrary.GetValue(MaxEnumGroup.ScopeProfile, MaxFactryLibrary.MaxStorageKeyName);
                if (null == loStorageKey || loStorageKey.ToString().Trim().Length.Equals(0) || (loStorageKey is Guid && Guid.Empty.Equals((Guid)loStorageKey)))
                {
                    loStorageKey = this._sDefaultStorageKey;
                }

                MaxConfigurationLibrary.SetValue(MaxEnumGroup.ScopeSession, MaxFactryLibrary.MaxStorageKeyName, loStorageKey);
            }

            if (null != loStorageKey && loStorageKey.ToString().Length > 0)
            {
                lsR = loStorageKey.ToString();
                MaxConfigurationLibrary.SetValue(MaxEnumGroup.ScopeProcess, MaxFactryLibrary.MaxStorageKeyName, lsR);
            }

            return lsR;
        }
#else
        protected virtual string GetStorageKeyFromConfigurationConditional()
        {
            string lsR = string.Empty;
            string lsLog = string.Empty;
            Stopwatch loWatch = Stopwatch.StartNew();
            string lsDefaultStorageKey = new Guid("7AAFA5DF-B2A9-462F-918A-C8A4242DC348").ToString().ToLower();
            if (null == this._sDefaultStorageKey || this._sDefaultStorageKey == lsDefaultStorageKey)
            {
                //// Without the storagekey defined in the data, default to the storagekey for the application
                this._sDefaultStorageKey = MaxConvertLibrary.ConvertToString(typeof(object), MaxConfigurationLibrary.GetValue(MaxEnumGroup.ScopeApplication, MaxFactryLibrary.MaxStorageKeyName)).ToLower();
                if (null == this._sDefaultStorageKey || this._sDefaultStorageKey.Length == 0)
                {
                    this._sDefaultStorageKey = lsDefaultStorageKey;
                    lsLog += "|_sDefaultStorageKey|" + loWatch.ElapsedTicks;
                }
            }

            object loStorageKey = MaxConfigurationLibrary.GetValue(MaxEnumGroup.ScopeSession, MaxFactryLibrary.MaxStorageKeyName);
            lsLog += "|ScopeSession|" + loWatch.ElapsedTicks;
            if (null == loStorageKey || loStorageKey.ToString().Trim().Length.Equals(0) || (loStorageKey is Guid && Guid.Empty.Equals((Guid)loStorageKey)))
            {
                loStorageKey = MaxConfigurationLibrary.GetValue(MaxEnumGroup.ScopeProfile, MaxFactryLibrary.MaxStorageKeyName);
                lsLog += "|ScopeProfile|" + loWatch.ElapsedTicks;
                if (null == loStorageKey || loStorageKey.ToString().Trim().Length.Equals(0) || (loStorageKey is Guid && Guid.Empty.Equals((Guid)loStorageKey)))
                {
                    loStorageKey = this._sDefaultStorageKey;
                    lsLog += "|Default|" + loWatch.ElapsedTicks;
                }

                MaxConfigurationLibrary.SetValue(MaxEnumGroup.ScopeSession, MaxFactryLibrary.MaxStorageKeyName, loStorageKey);
                lsLog += "|SetSession|" + loWatch.ElapsedTicks;
            }

            if (null != loStorageKey && loStorageKey.ToString().Length > 0)
            {
                lsR = loStorageKey.ToString();
                MaxConfigurationLibrary.SetValue(MaxEnumGroup.ScopeProcess, MaxFactryLibrary.MaxStorageKeyName, lsR);
                lsLog += "|SetProcess|" + loWatch.ElapsedTicks;
            }

            if (lsR.Length > 0)
            {
                long lnDuration = loWatch.ElapsedTicks;
                if (lnDuration > 100000)
                {
                    MaxFactry.Core.MaxLogLibrary.Log(new MaxLogEntryStructure("GetStorageKeyFromConfiguration", MaxEnumGroup.LogWarning, "GetStorageKeyFromConfiguration took {lnDuration} ticks for storage key {lsStorageKey} with log {lsLog}.", lnDuration, lsR, lsLog));
                }
                else if (lnDuration > 10000)
                {
                    MaxFactry.Core.MaxLogLibrary.Log(new MaxLogEntryStructure("GetStorageKeyFromConfiguration", MaxEnumGroup.LogInfo, "GetStorageKeyFromConfiguration took {lnDuration} ticks for storage key {lsStorageKey} with log {lsLog}.", lnDuration, lsR, lsLog));
                }
                else
                {
                    MaxFactry.Core.MaxLogLibrary.Log(new MaxLogEntryStructure("GetStorageKeyFromConfiguration", MaxEnumGroup.LogDebug, "GetStorageKeyFromConfiguration took {lnDuration} ticks for storage key {lsStorageKey} with log {lsLog}.", lnDuration, lsR, lsLog));
                }
            }

            return lsR;
        }
#endif

        protected virtual string GetStorageKeyFromConfiguration()
        {
            return this.GetStorageKeyFromConfigurationConditional();
        }
    }
}
