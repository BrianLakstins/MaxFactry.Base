// <copyright file="MaxStorageWriteRepositoryDefaultProvider.cs" company="Lakstins Family, LLC">
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
	public class MaxStorageWriteRepositoryDefaultProvider : MaxStorageReadRepositoryDefaultProvider, IMaxStorageWriteRepositoryProvider
	{
        private static object _oLock = new object();

        /// <summary>
        /// Inserts a new data element.
        /// </summary>
        /// <param name="loData">The data for the element.</param>
        /// <returns>true if inserted.</returns>
        public virtual bool Insert(MaxData loData)
        {
            IMaxDataContextProvider loProvider = MaxDataLibrary.GetContextProvider(this, loData);
            if (null == loProvider)
            {
                throw new MaxException("DataContextProvider was not found for [" + this.GetType().ToString() + "].  Check configuration for DataContextProvider.");
            } 

            MaxDataList loDataList = new MaxDataList(loData.DataModel);
            loDataList.Add(loData);
            int lnCount = loProvider.Insert(loDataList);
            if (lnCount.Equals(1))
            {
                return true;
            }

            return false;
        }

		/// <summary>
		/// Updates an existing data element.
		/// </summary>
		/// <param name="loData">the data for the element.</param>
		/// <returns>true if updated.</returns>
        public virtual bool Update(MaxData loData)
        {
            IMaxDataContextProvider loProvider = MaxDataLibrary.GetContextProvider(this, loData);
            if (null == loProvider)
            {
                throw new MaxException("DataContextProvider was not found for [" + this.GetType().ToString() + "].  Check configuration for DataContextProvider.");
            } 

            MaxDataList loDataList = new MaxDataList(loData.DataModel);
            loDataList.Add(loData);
            int lnCount = loProvider.Update(loDataList);
            if (lnCount.Equals(1))
            {
                return true;
            }

            return false;
        }

		/// <summary>
		/// Deletes an existing data element.
		/// </summary>
		/// <param name="loData">the data for the element.</param>
		/// <returns>true if deleted.</returns>
        public virtual bool Delete(MaxData loData)
        {
            IMaxDataContextProvider loProvider = MaxDataLibrary.GetContextProvider(this, loData);
            if (null == loProvider)
            {
                throw new MaxException("DataContextProvider was not found for [" + this.GetType().ToString() + "].  Check configuration for DataContextProvider.");
            } 
            
            MaxDataList loDataList = new MaxDataList(loData.DataModel);
            loDataList.Add(loData);
            int lnCount = loProvider.Delete(loDataList);
            if (lnCount > 0)
            {
                return true;
            }

            return false;
        }

        /// <summary>
        /// Writes stream data to storage.
        /// </summary>
        /// <param name="loData">The data index for the object</param>
        /// <param name="lsKey">Data element name to write</param>
        /// <returns>Number of bytes written to storage.</returns>
        public virtual bool StreamSave(MaxData loData, string lsKey)
        {
            if (loData.DataModel is MaxBaseIdFileDataModel)
            {
                MaxBaseIdFileDataModel loDataModel = loData.DataModel as MaxBaseIdFileDataModel;
                if (null == loData.Get(loDataModel.Content))
                {
                    string lsFromFileName = loData.Get(loDataModel.FromFileName) as string;
                    if (!string.IsNullOrEmpty(lsFromFileName) && File.Exists(lsFromFileName))
                    {
                        FileInfo loFileInfo = new FileInfo(lsFromFileName);
                        loData.Set(loDataModel.Content, File.OpenRead(lsFromFileName));
                        loData.Set(loDataModel.ContentLength, loFileInfo.Length);
                        loData.Set(loDataModel.ContentName, loFileInfo.Name);
                        loData.Set(loDataModel.ContentDate, loFileInfo.CreationTimeUtc);
                        loData.Set(loDataModel.ContentType, MaxStorageReadRepository.GetMimeType(loData, lsFromFileName));
                        loData.Set(loDataModel.MimeType, MaxStorageReadRepository.GetMimeType(loData, lsFromFileName));
                    }
                }
            }

            IMaxDataContextProvider loProvider = MaxDataLibrary.GetContextProvider(this, loData);
            bool lbR = false;
            if (null != loProvider)
            {
                lbR = loProvider.StreamSave(loData, lsKey);
            }

            return lbR;
        }

        /// <summary>
        /// Removes stream from storage.
        /// </summary>
        /// <param name="loData">The data index for the object</param>
        /// <param name="lsKey">Data element name to remove</param>
        /// <returns>true if successful.</returns>
        public virtual bool StreamDelete(MaxData loData, string lsKey)
        {
            IMaxDataContextProvider loProvider = MaxDataLibrary.GetContextProvider(this, loData);
            bool lbR = false;
            if (null != loProvider)
            {
                lbR = loProvider.StreamDelete(loData, lsKey);
            }

            return lbR;
        }

        /// <summary>
        /// Saves a data to a file
        /// </summary>
        /// <param name="lsDirectory"></param>
        /// <param name="loData"></param>
        /// <returns></returns>
        protected virtual bool SaveAsFile(string lsDirectory, MaxData loData)
        {
            try
            {
                lock (_oLock)
                {
                    string lsFile = System.IO.Path.Combine(lsDirectory, loData.DataModel.DataStorageName + ".json");
                    MaxIndex loIndex = new MaxIndex();
                    string[] laKey = loData.DataModel.GetKeyList();

                    MaxIndex loDataIndex = new MaxIndex();
                    foreach (string lsKey in laKey)
                    {
                        loDataIndex.Add(lsKey, loData.Get(lsKey));
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
        protected virtual MaxDataList LoadFromFile(string lsDirectory, MaxData loData)
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
                                string[] laKey = loData.DataModel.GetKeyList();
                                if (loDataIndex != null)
                                {
                                    //// Load multiple records
                                    foreach (string lsIndexKey in laIndexKey)
                                    {
                                        loDataIndex = loIndex[lsIndexKey] as MaxIndex;
                                        if (loDataIndex != null)
                                        {
                                            MaxData loDataOut = new MaxData(loData.DataModel);
                                            foreach (string lsKey in laKey)
                                            {
                                                loDataOut.Set(lsKey, loDataIndex[lsKey]);
                                            }

                                            loR.Add(loDataOut);
                                        }
                                    }
                                }
                                else
                                {
                                    //// Load 1 record
                                    MaxData loDataOut = new MaxData(loData.DataModel);
                                    foreach (string lsKey in laKey)
                                    {
                                        loDataOut.Set(lsKey, loIndex[lsKey]);
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
    }
}
