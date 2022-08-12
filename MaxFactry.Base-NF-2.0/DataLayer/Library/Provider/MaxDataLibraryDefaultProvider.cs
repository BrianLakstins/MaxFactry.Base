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
// </changelog>
#endregion

namespace MaxFactry.Base.DataLayer.Provider
{
    using System;
    using System.Diagnostics;
    using MaxFactry.Core;

    /// <summary>
    /// Provides static methods to manipulate storage of data
    /// </summary>
    public class MaxDataLibraryDefaultProvider : MaxProvider, IMaxDataLibraryProvider
    {
        /// <summary>
        /// Maps classes that use a provider to the class that is the provider
        /// </summary>
        private MaxIndex _oContextProviderMapIndex = new MaxIndex();

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
        /// <param name="loRepositoryProvider">The object calling the context provider.</param>
        /// <param name="loData">Data being acted upon by the context provider.</param>
        /// <returns>Context provider</returns>
        public virtual IMaxDataContextProvider GetContextProvider(IMaxRepositoryProvider loRepositoryProvider, MaxData loData)
        {
            // Try using the type of the caller as the type of the provider.
            Type loProviderType = loRepositoryProvider.GetType();

            // See if a data context type has been configured for the provider.
            if (null != loRepositoryProvider.DefaultContextProviderType)
            {
                loProviderType = loRepositoryProvider.DefaultContextProviderType;
            }

            // See if a context provider was sent with the data
            if (null != loData && null != loData.Get("IMaxDataContextProvider") && loData.Get("IMaxDataContextProvider") is Type)
            {
                loProviderType = (Type)loData.Get("IMaxDataContextProvider");
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

            IMaxProvider loProvider = MaxFactryLibrary.CreateProvider(lsName, loProviderType);
            if (loProvider is IMaxDataContextProvider)
            {
                return (IMaxDataContextProvider)loProvider;
            }

            return null;
        }

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
            return (MaxDataModel)loDataModel;
        }

        /// <summary>
        /// Gets a data list for the data model based on the type
        /// </summary>
        /// <param name="loType">Type of data list to get</param>
        /// <returns>DataList for the data model specified by the type</returns>
        public virtual MaxDataList GetDataList(Type loType)
        {
            MaxDataModel loDataModel = this.GetDataModel(loType);
            return this.GetDataList(loDataModel);
        }

        /// <summary>
        /// Gets a data list for the data model based on the type
        /// </summary>
        /// <param name="loDataModel">Data model to be used in the list</param>
        /// <returns>DataList for the data model specified by the type</returns>
        public virtual MaxDataList GetDataList(MaxDataModel loDataModel)
        {
            object loObject = MaxFactryLibrary.Create(typeof(MaxDataList), loDataModel);
            if (loObject is MaxDataList)
            {
                return (MaxDataList)loObject;
            }

            return null;
        }

        /// <summary>
        /// Maps a class that uses a specific provider to that provider
        /// </summary>
        /// <param name="loType">Class that uses a provider</param>
        /// <param name="loProviderType">The provider to use</param>
        public virtual void RegisterContextProvider(Type loType, Type loProviderType)
        {
            this._oContextProviderMapIndex.Add(loType.ToString(), loProviderType);
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
            string lsR = this.GetStorageKeyFromData(loData);
            //// Only check the configuration for the central storage key if one was not found in the data
            if (null == lsR || lsR.Length.Equals(0))
            {
                lsR = this.GetStorageKeyFromProcess();
                if (null == lsR || lsR.Length.Equals(0))
                {
                    lsR = this.GetStorageKeyFromConfiguration();
                }
            }

            if (lsR.Length == 0)
            {
                MaxFactry.Core.MaxLogLibrary.Log(new MaxLogEntryStructure("GetStorageKeyProvider", MaxEnumGroup.LogError, "GetStorageKey(MaxData loData) ended with blank storagekey."));
            }

            return lsR;
        }

        protected virtual string GetStorageKeyFromData(MaxData loData)
        {
            string lsR = string.Empty;
            if (null != loData && null != loData.DataModel)
            {
                //// Get the value of the StorageKey property if it's defined
                if (loData.DataModel.HasKey(loData.DataModel.StorageKey))
                {
                    lsR = loData.Get(loData.DataModel.StorageKey) as string;
                }
            }

            return lsR;
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
