// <copyright file="MaxDataContextDefaultProvider.cs" company="Lakstins Family, LLC">
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
// <change date="2/19/2017" author="Brian A. Lakstins" description="Initial creation">
// <change date="6/4/2020" author="Brian A. Lakstins" description="Updated for change to base.">
// <change date="6/5/2020" author="Brian A. Lakstins" description="Remove using MaxDataLibrary to ge StorageKey because can cause stack overflow.">
// <change date="6/2/2021" author="Brian A. Lakstins" description="Update from MaxDataContextDataSetProvider so can remove it and just use this.">
// <change date="7/20/2023" author="Brian A. Lakstins" description="Replacing strings with constant properties.">
// </changelog>
#endregion

namespace MaxFactry.Base.DataLayer.Provider
{
	using System;
#if net2 || netcore2
    using System.Collections.Generic;
    using System.Data;
#endif
    using System.IO;
    using MaxFactry.Core;
    using MaxFactry.Base.DataLayer.Library;

    /// <summary>
    /// Provides static methods to manipulate storage of data
    /// </summary>
    public class MaxDataContextDefaultProvider : MaxProvider, IMaxDataContextProvider
	{

#if net2 || netcore2
        /// <summary>
        /// Private storage of Dataset on a per StorageKey basis.
        /// </summary>
        private static Dictionary<string, DataSet> _oDataSetIndex = new Dictionary<string, DataSet>();

        /// <summary>
        /// Lock for thread lock access to dataset
        /// </summary>
        private static object _oLock = new object();

        /// <summary>
        /// Folder to use for data set storage
        /// </summary>
        private string _sDataSetFolder = string.Empty;

        /// <summary>
        /// Configuration name for default context provider 
        /// </summary>
        public const string DefaultContextProviderConfigName = "DefaultContextProviderName";

        /// <summary>
        /// Configuration name for instance context provider
        /// </summary>
        public const string ContextProviderConfigName = "ContextProviderName";

        /// <summary>
        /// Initializes the provider
        /// </summary>
        /// <param name="lsName">Name of the provider</param>
        /// <param name="loConfig">Configuration information</param>
        public override void Initialize(string lsName, MaxIndex loConfig)
        {
            base.Initialize(lsName, loConfig);
            string lsFolder = this.GetConfigValue(loConfig, "DataSetFolder") as string;
            if (null != lsFolder)
            {
                this._sDataSetFolder = lsFolder;
            }
        }

        protected string DataSetFolder
        {
            get
            {
                return this._sDataSetFolder;
            }
        }

        /// <summary>
        /// Selects all data from the data storage name for the specified type.
        /// </summary>
        /// <param name="lsDataStorageName">Name of the data storage (table name).</param>
        /// <param name="laDataNameList">list of fields to return from select</param>
        /// <returns>List of data elements with a base data model.</returns>
        public virtual MaxDataList SelectAll(string lsDataStorageName, params string[] laDataNameList)
        {
            string lsStorageKey = MaxDataLibrary.GetStorageKey(null);
            if (!_oDataSetIndex.ContainsKey(lsStorageKey))
            {
                _oDataSetIndex.Add(lsStorageKey, new DataSet());
            }

            DataSet loDataSet = _oDataSetIndex[lsStorageKey];
            MaxDataModel loDataModel = new MaxDataModel();
            MaxDataList loDataList = new MaxDataList(loDataModel);
            if (loDataSet.Tables.Contains(lsDataStorageName))
            {
                foreach (DataRow loRow in loDataSet.Tables[lsDataStorageName].Rows)
                {
                    MaxData loDataNew = new MaxData(loDataModel);
                    foreach (DataColumn loColumn in loDataSet.Tables[lsDataStorageName].Columns)
                    {
                        loDataNew.Set(loColumn.ColumnName, loRow[loColumn]);
                    }

                    loDataList.Add(loDataNew);
                }
            }

            return loDataList;
        }

        /// <summary>
        /// Selects data from the database.
        /// </summary>
        /// <param name="loData">Element with data used in the filter.</param>
        /// <param name="loDataQuery">Query information to filter results.</param>
        /// <param name="lnPageIndex">Page to return.</param>
        /// <param name="lnPageSize">Items per page.</param>
        /// <param name="lsOrderBy">Sort information.</param>
        /// <param name="lnTotal">Total items found.</param>
        /// <param name="laDataNameList">list of fields to return from select.</param>
        /// <returns>List of data from select.</returns>
        public virtual MaxDataList Select(MaxData loData, MaxDataQuery loDataQuery, int lnPageIndex, int lnPageSize, string lsOrderBy, out int lnTotal, params string[] laDataNameList)
        {
            this.CreateTable(loData);
            DataSet loDataSet = this.GetDataSet(loData);
            MaxDataList loDataList = new MaxDataList(loData.DataModel);
            string[] laKey = loData.DataModel.GetKeyList();
            int lnRows = 0;
            int lnStart = 0;
            int lnEnd = int.MaxValue;
            if (lnPageSize > 0 && lnPageIndex > 0)
            {
                lnStart = (lnPageIndex - 1) * lnPageSize;
                lnEnd = lnStart + lnPageSize;
            }

            string[] laKeyList = loData.DataModel.GetKeyList();
            List<string> loPrimaryKeyList = new List<string>();
            for (int lnK = 0; lnK < laKeyList.Length; lnK++)
            {
                string lsKey = laKeyList[lnK];
                if (this.IsStored(loData.DataModel, lsKey))
                {
                    bool lbIsPrimaryKey = loDataList.DataModel.GetPropertyAttributeSetting(lsKey, "IsPrimaryKey");
                    if (lbIsPrimaryKey)
                    {
                        if (null != loData.Get(lsKey))
                        {
                            loPrimaryKeyList.Add(lsKey);
                        }
                    }
                }
            }

            object[] laDataQuery = loDataQuery.GetQuery();
            string lsDataQuery = string.Empty;
            if (laDataQuery.Length > 0)
            {
                for (int lnDQ = 0; lnDQ < laDataQuery.Length; lnDQ++)
                {
                    object loStatement = laDataQuery[lnDQ];
                    if (loStatement is char)
                    {
                        // Group Start or end characters
                        lsDataQuery += (char)loStatement;
                    }
                    else if (loStatement is string)
                    {
                        // Comparison operators
                        lsDataQuery += " " + (string)loStatement + " ";
                    }
                    else if (loStatement is MaxDataFilter)
                    {
                        MaxDataFilter loDataFilter = (MaxDataFilter)loStatement;
                        lsDataQuery += "[" + loDataFilter.Name + "] " + loDataFilter.Operator + " '" + loDataFilter.Value + "'";
                    }
                }
            }

            DataView loView = new DataView(loDataSet.Tables[loData.DataModel.DataStorageName]);
            if (lsDataQuery.Length > 0)
            {
                loView.RowFilter = lsDataQuery;
            }

            foreach (DataRowView loRow in loView)
            {
                bool lbIsMatch = true;
                foreach (string lsKey in loPrimaryKeyList)
                {
                    if (loData.DataModel.GetValueType(lsKey) == typeof(Guid))
                    {
                        if ((Guid)loRow[lsKey] != MaxConvertLibrary.ConvertToGuid(loData.DataModel.GetType(), loData.Get(lsKey)))
                        {
                            lbIsMatch = false;
                        }
                    }
                    else if (loRow[lsKey] != loData.Get(lsKey))
                    {
                        lbIsMatch = false;
                    }
                }

                if (lbIsMatch)
                {
                    if (lnRows >= lnStart && lnRows < lnEnd)
                    {
                        MaxData loDataOut = new MaxData(loDataList.DataModel);
                        for (int lnR = 0; lnR < loView.Table.Columns.Count; lnR++)
                        {
                            object loValue = loRow[loView.Table.Columns[lnR].ColumnName];
                            if (loValue is DBNull)
                            {
                                loDataOut.Set(loView.Table.Columns[lnR].ColumnName, null);
                            }
                            else
                            {
                                loDataOut.Set(loView.Table.Columns[lnR].ColumnName, loValue);
                            }
                        }

                        loData.ClearChanged();
                        loDataList.Add(loDataOut);
                    }

                    lnRows++;
                }
            }

            lnTotal = 0;
            return loDataList;
        }

        /// <summary>
        /// Gets the number of records that match the filter.
        /// </summary>
        /// <param name="loData">Element with data used in the filter.</param>
        /// <param name="loDataQuery">Query information to filter results.</param>
        /// <returns>number of records that match.</returns>
        public virtual int SelectCount(MaxData loData, MaxDataQuery loDataQuery)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Inserts a new data element.
        /// </summary>
        /// <param name="loDataList">The list of data objects to insert.</param>
        /// <returns>The data that was inserted.</returns>
        public virtual int Insert(MaxDataList loDataList)
        {
            int lnR = 0;
            lock (_oLock)
            {
                for (int lnD = 0; lnD < loDataList.Count; lnD++)
                {
                    MaxData loData = loDataList[lnD];
                    this.CreateTable(loData);
                    DataSet loDataSet = this.GetDataSet(loData);
                    DataRow loRow = loDataSet.Tables[loDataList.DataModel.DataStorageName].NewRow();
                    string[] laKeyList = loData.DataModel.GetKeyList();
                    for (int lnK = 0; lnK < laKeyList.Length; lnK++)
                    {
                        string lsKey = laKeyList[lnK];
                        if (this.IsStored(loData.DataModel, lsKey))
                        {
                            object loValue = loData.Get(lsKey);
                            if (null != loValue)
                            {
                                loRow[lsKey] = loValue;
                            }
                            else if (loData.DataModel.GetValueType(lsKey) == typeof(bool))
                            {
                                loRow[lsKey] = false;
                            }
                        }
                    }

                    loDataSet.Tables[loDataList.DataModel.DataStorageName].Rows.Add(loRow);
                    lnR++;
                }

                this.SaveToFile();
            }

            return lnR;
        }

        /// <summary>
        /// Updates an existing data element.
        /// </summary>
        /// <param name="loDataList">The list of data objects to insert.</param>
        /// <returns>The data that was updated.</returns>
        public virtual int Update(MaxDataList loDataList)
        {
            int lnR = 0;
            lock (_oLock)
            {
                for (int lnD = 0; lnD < loDataList.Count; lnD++)
                {
                    MaxData loData = loDataList[lnD];
                    this.CreateTable(loData);
                    DataSet loDataSet = this.GetDataSet(loData);
                    string[] laKeyList = loData.DataModel.GetKeyList();
                    List<string> loPrimaryKeyList = new List<string>();
                    for (int lnK = 0; lnK < laKeyList.Length; lnK++)
                    {
                        string lsKey = laKeyList[lnK];
                        if (this.IsStored(loData.DataModel, lsKey))
                        {
                            bool lbIsPrimaryKey = loDataList.DataModel.GetPropertyAttributeSetting(lsKey, "IsPrimaryKey");
                            if (lbIsPrimaryKey)
                            {
                                if (null != loData.Get(laKeyList[lnK]))
                                {
                                    loPrimaryKeyList.Add(laKeyList[lnK]);
                                }
                            }
                        }
                    }

                    foreach (DataRow loRow in loDataSet.Tables[loData.DataModel.DataStorageName].Rows)
                    {
                        bool lbIsMatch = true;
                        foreach (string lsKey in loPrimaryKeyList)
                        {
                            if (loData.DataModel.GetValueType(lsKey) == typeof(Guid))
                            {
                                if ((Guid)loRow[lsKey] != MaxConvertLibrary.ConvertToGuid(loData.DataModel.GetType(), loData.Get(lsKey)))
                                {
                                    lbIsMatch = false;
                                }
                            }
                            else if (loRow[lsKey] != loData.Get(lsKey))
                            {
                                lbIsMatch = false;
                            }
                        }

                        if (lbIsMatch)
                        {
                            for (int lnK = 0; lnK < laKeyList.Length; lnK++)
                            {
                                string lsKey = laKeyList[lnK];
                                if (this.IsStored(loData.DataModel, lsKey))
                                {
                                    if (loData.GetIsChanged(lsKey))
                                    {
                                        object loValue = loData.Get(lsKey);
                                        if (null == loValue)
                                        {
                                            loRow[lsKey] = DBNull.Value;
                                        }
                                        else
                                        {
                                            loRow[lsKey] = loValue;
                                        }
                                    }
                                }
                            }

                            lnR++;
                        }
                    }
                }

                this.SaveToFile();
            }

            return lnR;
        }

        /// <summary>
        /// Deletes an existing data element.
        /// </summary>
        /// <param name="loDataList">The list of data objects to insert.</param>
        /// <returns>true if deleted.</returns>
        public virtual int Delete(MaxDataList loDataList)
        {
            int lnR = 0;
            lock (_oLock)
            {
                for (int lnD = 0; lnD < loDataList.Count; lnD++)
                {
                    MaxData loData = loDataList[lnD];
                    this.CreateTable(loData);
                    DataSet loDataSet = this.GetDataSet(loData);
                    string[] laKeyList = loData.DataModel.GetKeyList();
                    List<string> loPrimaryKeyList = new List<string>();
                    for (int lnK = 0; lnK < laKeyList.Length; lnK++)
                    {
                        string lsKey = laKeyList[lnK];
                        if (this.IsStored(loData.DataModel, lsKey))
                        {
                            bool lbIsPrimaryKey = loDataList.DataModel.GetPropertyAttributeSetting(lsKey, "IsPrimaryKey");
                            if (lbIsPrimaryKey)
                            {
                                if (null != loData.Get(lsKey))
                                {
                                    loPrimaryKeyList.Add(lsKey);
                                }
                            }
                        }
                    }

                    for (int lnRow = loDataSet.Tables[loData.DataModel.DataStorageName].Rows.Count - 1; lnRow >= 0; lnRow--)
                    {
                        bool lbIsMatch = true;
                        DataRow loRow = loDataSet.Tables[loData.DataModel.DataStorageName].Rows[lnRow];
                        foreach (string lsKey in loPrimaryKeyList)
                        {
                            if (loData.DataModel.GetValueType(lsKey) == typeof(Guid))
                            {
                                if ((Guid)loRow[lsKey] != MaxConvertLibrary.ConvertToGuid(loData.DataModel.GetType(), loData.Get(lsKey)))
                                {
                                    lbIsMatch = false;
                                }
                            }
                            else if (loRow[lsKey] != loData.Get(lsKey))
                            {
                                lbIsMatch = false;
                            }
                        }

                        if (lbIsMatch)
                        {
                            loDataSet.Tables[loData.DataModel.DataStorageName].Rows.RemoveAt(lnRow);
                        }
                    }
                }

                this.SaveToFile();
            }

            return lnR;
        }

        /// <summary>
        /// Writes stream data to storage.
        /// </summary>
        /// <param name="loData">The data index for the object</param>
        /// <param name="lsKey">Data element name to write</param>
        /// <returns>Number of bytes written to storage.</returns>
        public virtual bool StreamSave(MaxData loData, string lsKey)
        {
            return MaxDataContextStreamLibrary.StreamSave(loData, lsKey);
        }

        /// <summary>
        /// Opens stream data in storage
        /// </summary>
        /// <param name="loData">The data index for the object</param>
        /// <param name="lsKey">Data element name to write</param>
        /// <returns>Stream that was opened.</returns>
        public virtual Stream StreamOpen(MaxData loData, string lsKey)
        {
            return MaxDataContextStreamLibrary.StreamOpen(loData, lsKey);
        }

        /// <summary>
        /// Deletes stream data in storage
        /// </summary>
        /// <param name="loData">The data index for the object</param>
        /// <param name="lsKey">Data element name to write</param>
        /// <returns>Stream that was opened.</returns>
        public virtual bool StreamDelete(MaxData loData, string lsKey)
        {
            return MaxDataContextStreamLibrary.StreamDelete(loData, lsKey);
        }

        /// <summary>
        /// Gets the Url of a saved stream.
        /// </summary>
        /// <param name="loData">The data index for the object</param>
        /// <param name="lsKey">Data element name</param>
        /// <returns>Url of stream if one can be provided.</returns>
        public virtual string GetStreamUrl(MaxData loData, string lsKey)
        {
            return MaxDataContextStreamLibrary.GetStreamUrl(loData, lsKey);
        }

        /// <summary>
        /// Checks to see if this data key is stored by this provider.
        /// </summary>
        /// <param name="loDataModel">Data model used to get the type of the key.</param>
        /// <param name="lsKey">The key to check to see if it is stored.</param>
        /// <returns>True if it should be stored.  False otherwise.</returns>
        protected virtual bool IsStored(MaxDataModel loDataModel, string lsKey)
        {
            if (lsKey.Equals(loDataModel.StorageKey))
            {
                return false;
            }
            else if (loDataModel.GetValueType(lsKey).Equals(typeof(MaxShortString)))
            {
                return true;
            }
            else if (loDataModel.GetValueType(lsKey).Equals(typeof(string)))
            {
                return true;
            }
            else if (loDataModel.GetValueType(lsKey).Equals(typeof(Guid)))
            {
                return true;
            }
            else if (loDataModel.GetValueType(lsKey).Equals(typeof(int)))
            {
                return true;
            }
            else if (loDataModel.GetValueType(lsKey).Equals(typeof(long)))
            {
                return true;
            }
            else if (loDataModel.GetValueType(lsKey).Equals(typeof(double)))
            {
                return true;
            }
            else if (loDataModel.GetValueType(lsKey).Equals(typeof(byte[])))
            {
                return true;
            }
            else if (loDataModel.GetValueType(lsKey).Equals(typeof(bool)))
            {
                return true;
            }
            else if (loDataModel.GetValueType(lsKey).Equals(typeof(DateTime)))
            {
                return true;
            }
            else if (loDataModel.GetValueType(lsKey).Equals(typeof(MaxLongString)))
            {
                return true;
            }

            return false;
        }

        private void SaveToFile()
        {
            foreach (string lsKey in _oDataSetIndex.Keys)
            {
                if (!lsKey.EndsWith("Stored"))
                {
                    string lsText = MaxConvertLibrary.SerializeObjectToString(typeof(object), _oDataSetIndex[lsKey]);
                    if (!Directory.Exists(this.DataSetFolder))
                    {
                        Directory.CreateDirectory(this.DataSetFolder);
                    }

                    string lsFile = Path.Combine(this.DataSetFolder, "DataSet-" + lsKey + ".txt");
                    File.WriteAllText(lsFile, lsText);
                }
            }
        }

        private void LoadFromFile(string lsKey)
        {
            string lsFile = Path.Combine(this.DataSetFolder, "DataSet-" + lsKey + ".txt");
            if (File.Exists(lsFile))
            {
                string lsText = File.ReadAllText(lsFile);
                _oDataSetIndex.Add(lsKey + "Stored", MaxConvertLibrary.DeserializeObject(typeof(object), lsText, typeof(DataSet)) as DataSet);
            }
        }

        /// <summary>
        /// Gets the data set associated with this data
        /// </summary>
        /// <param name="loData">The data to use to determine the data set</param>
        /// <returns>A data set.</returns>
        private DataSet GetDataSet(MaxData loData)
        {
            string lsStorageKey = MaxDataLibrary.GetStorageKey(loData);
            if (!_oDataSetIndex.ContainsKey(lsStorageKey))
            {
                _oDataSetIndex.Add(lsStorageKey, new DataSet());
                this.LoadFromFile(lsStorageKey);
            }

            return _oDataSetIndex[lsStorageKey];
        }

        /// <summary>
        /// Creates a table
        /// </summary>
        /// <param name="loData">Data to be stored.</param>
        private void CreateTable(MaxData loData)
        {
            DataSet loDataSet = this.GetDataSet(loData);
            if (!loDataSet.Tables.Contains(loData.DataModel.DataStorageName))
            {
                lock (_oLock)
                {
                    if (!loDataSet.Tables.Contains(loData.DataModel.DataStorageName))
                    {
                        DataTable loTable = new DataTable();
                        loTable.TableName = loData.DataModel.DataStorageName;
                        string[] laKeyList = loData.DataModel.GetKeyList();
                        List<DataColumn> loPrimaryKeyList = new List<DataColumn>();
                        for (int lnK = 0; lnK < laKeyList.Length; lnK++)
                        {
                            string lsColumName = laKeyList[lnK];
                            if (!loData.DataModel.StorageKey.Equals(lsColumName))
                            {
                                Type loType = loData.DataModel.GetValueType(lsColumName);
                                if (loType == typeof(MaxShortString) || loType == typeof(MaxLongString))
                                {
                                    loType = typeof(string);
                                }

                                DataColumn loColumn = new DataColumn(lsColumName, loType);
                                if (loData.DataModel.GetPropertyAttributeSetting(lsColumName, "IsAllowDBNull"))
                                {
                                    loColumn.AllowDBNull = true;
                                }

                                if (loData.DataModel.GetPropertyAttributeSetting(lsColumName, "IsPrimaryKey"))
                                {
                                    loPrimaryKeyList.Add(loColumn);
                                }

                                loTable.Columns.Add(loColumn);
                            }
                        }

                        loTable.PrimaryKey = loPrimaryKeyList.ToArray();

                        string lsStorageKey = MaxDataLibrary.GetStorageKey(loData);
                        if (_oDataSetIndex.ContainsKey(lsStorageKey + "Stored"))
                        {
                            DataSet loStored = _oDataSetIndex[lsStorageKey + "Stored"];
                            if (loStored.Tables.Contains(loData.DataModel.DataStorageName))
                            {
                                DataTable loTableStored = loStored.Tables[loData.DataModel.DataStorageName];
                                foreach (DataRow loRowStored in loTableStored.Rows)
                                {
                                    DataRow loRow = loTable.NewRow();
                                    foreach (DataColumn loColumn in loTable.Columns)
                                    {
                                        if (loTableStored.Columns.Contains(loColumn.ColumnName))
                                        {
                                            loRow[loColumn.ColumnName] = loRowStored[loColumn.ColumnName];
                                        }
                                        else
                                        {
                                            loRow[loColumn.ColumnName] = DBNull.Value;
                                        }
                                    }

                                    loTable.Rows.Add(loRow);
                                }
                            }
                        }

                        loDataSet.Tables.Add(loTable);
                    }
                }
            }
        }
#else
        /// <summary>
        /// Selects all data from the data storage name for the specified type.
        /// </summary>
        /// <param name="lsDataStorageName">Name of the data storage (table name).</param>
        /// <param name="laFields">list of fields to return from select</param>
        /// <returns>List of data elements with a base data model.</returns>
        public MaxDataList SelectAll(string lsDataStorageName, params string[] laDataNameList)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Selects data from the database
        /// </summary>
        /// <param name="loData">Element with data used in the filter</param>
        /// <param name="loDataQuery">Query information to filter results.</param>
        /// <param name="lnPageIndex">Page to return</param>
        /// <param name="lnPageSize">Items per page</param>
        /// <param name="lnTotal">Total items found</param>
        /// <param name="laFields">list of fields to return from select</param>
        /// <returns>List of data from select</returns>
        public MaxDataList Select(MaxData loData, MaxDataQuery loDataQuery, int lnPageIndex, int lnPageSize, out int lnTotal, params string[] laDataNameList)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Gets the number of records that match the filter.
        /// </summary>
        /// <param name="loData">Element with data used in the filter.</param>
        /// <param name="loDataQuery">Query information to filter results.</param>
        /// <returns>number of records that match.</returns>
        public int SelectCount(MaxData loData, MaxDataQuery loDataQuery)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Inserts a list of data objects.
        /// </summary>
        /// <param name="loDataList">The list of data objects to insert.</param>
        /// <returns>The count affected.</returns>
        public int Insert(MaxDataList loDataList)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Updates a list of data objects.
        /// </summary>
        /// <param name="loDataList">The list of data objects to insert.</param>
        /// <returns>The count affected.</returns>
        public int Update(MaxDataList loDataList)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Deletes a list of data objects.
        /// </summary>
        /// <param name="loDataList">The list of data objects to insert.</param>
        /// <returns>The count affected.</returns>
        public int Delete(MaxDataList loDataList)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Writes stream data to storage.
        /// </summary>
        /// <param name="loData">The data index for the object</param>
        /// <param name="lsKey">Data element name to write</param>
        /// <returns>True if stream saved.</returns>
        public bool StreamSave(MaxData loData, string lsKey)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Opens stream data in storage
        /// </summary>
        /// <param name="loData">The data index for the object</param>
        /// <param name="lsKey">Data element name to write</param>
        /// <returns>Stream that was opened.</returns>
        public Stream StreamOpen(MaxData loData, string lsKey)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Removes stream from storage.
        /// </summary>
        /// <param name="loData">The data index for the object</param>
        /// <param name="lsKey">Data element name to remove</param>
        /// <returns>true if successful.</returns>
        public bool StreamDelete(MaxData loData, string lsKey)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Gets the Url of a saved stream.
        /// </summary>
        /// <param name="loData">The data index for the object</param>
        /// <param name="lsKey">Data element name</param>
        /// <returns>Url of stream if one can be provided.</returns>
        public string GetStreamUrl(MaxData loData, string lsKey)
        {
            throw new NotImplementedException();
        }
#endif
    }
}
