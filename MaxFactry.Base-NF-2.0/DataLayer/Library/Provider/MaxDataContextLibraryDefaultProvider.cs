// <copyright file="MaxDataContextLibraryDefaultProvider.cs" company="Lakstins Family, LLC">
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
// <change date="3/20/2024" author="Brian A. Lakstins" description="Happy birthday to my mom.  Sara Jean Lakstins (Cartwright) - 3/20/1944 to 3/14/2019.">
// <change date="3/23/2024" author="Brian A. Lakstins" description="Updated for changes to DataModel">
// <change date="3/24/2024" author="Brian A. Lakstins" description="Updated for changes namespaces">
// <change date="3/25/2024" author="Brian A. Lakstins" description="Moved from MaxFactry.Base.DataLayer namespace and renamed from MaxDataContextDefaultProvider">
// <change date="3/30/2024" author="Brian A. Lakstins" description="Updated for changes to MaxData.  Changed variable names to be consistent.">
// <change date="5/21/2025" author="Brian A. Lakstins" description="Remove stream handling methods and integrate stream handling using StreamLibrary">
// </changelog>
#endregion

namespace MaxFactry.Base.DataLayer.Library.Provider
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
    public class MaxDataContextLibraryDefaultProvider : MaxProvider, IMaxDataContextLibraryProvider
	{

        protected int _nMaxParameterCount = 900;

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
        /// Selects all data
        /// </summary>
        /// <param name="loData">Data to use as definition</param>
        /// <param name="laDataNameList">Names of fields to return</param>
        /// <returns>List of data that is stored</returns>
        public virtual MaxDataList SelectAll(MaxData loData, params string[] laDataNameList)
        {
            string lsStorageKey = MaxDataLibrary.GetStorageKey(loData);
            if (!_oDataSetIndex.ContainsKey(lsStorageKey))
            {
                _oDataSetIndex.Add(lsStorageKey, new DataSet());
            }

            DataSet loDataSet = _oDataSetIndex[lsStorageKey];
            MaxDataList loDataList = new MaxDataList(loData.DataModel);
            if (loDataSet.Tables.Contains(loData.DataModel.DataStorageName))
            {
                foreach (DataRow loRow in loDataSet.Tables[loData.DataModel.DataStorageName].Rows)
                {
                    MaxData loDataOut = new MaxData(loData.DataModel);
                    foreach (DataColumn loColumn in loDataSet.Tables[loData.DataModel.DataStorageName].Columns)
                    {
                        loDataOut.Set(loColumn.ColumnName, loRow[loColumn]);
                    }

                    loDataOut.ClearChanged();
                    loDataList.Add(loDataOut);
                }
            }

            return loDataList;
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
        public virtual MaxDataList Select(MaxData loData, MaxDataQuery loDataQuery, int lnPageIndex, int lnPageSize, string lsOrderBy, out int lnTotal, params string[] laDataNameList)
        {
            this.CreateTable(loData);
            DataSet loDataSet = this.GetDataSet(loData);
            MaxDataList loDataList = new MaxDataList(loData.DataModel);
            int lnRows = 0;
            int lnStart = 0;
            int lnEnd = int.MaxValue;
            if (lnPageSize > 0 && lnPageIndex > 0)
            {
                lnStart = (lnPageIndex - 1) * lnPageSize;
                lnEnd = lnStart + lnPageSize;
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
                //// Checking for matching row based on Data Key
                foreach (string lsDataName in loData.DataModel.DataNameKeyList)
                {
                    if (loData.DataModel.GetValueType(lsDataName) == typeof(Guid))
                    {
                        if ((Guid)loRow[lsDataName] != MaxConvertLibrary.ConvertToGuid(loData.DataModel.GetType(), loData.Get(lsDataName)))
                        {
                            lbIsMatch = false;
                        }
                    }
                    else if (loRow[lsDataName] != loData.Get(lsDataName))
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

                        loDataOut.ClearChanged();
                        loDataList.Add(loDataOut);
                    }

                    lnRows++;
                }
            }

            lnTotal = 0;
            return loDataList;
        }

        /// <summary>
        /// Selects a count of records
        /// </summary>
        /// <param name="loData">Data to use as definition</param>
        /// <param name="loDataQuery">Filter for the query</param>
        /// <returns>Count that matches the query parameters</returns>
        public virtual int SelectCount(MaxData loData, MaxDataQuery loDataQuery)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Inserts a new list of elements
        /// </summary>
        /// <param name="loDataList">The list of elements</param>
        /// <returns>Flag based status code indicating level of success.</returns>
        public virtual int Insert(MaxDataList loDataList)
        {
            int lnR = 0;
            lock (_oLock)
            {
                try
                {
                    for (int lnD = 0; lnD < loDataList.Count && lnR == 0; lnD++)
                    {
                        MaxData loData = loDataList[lnD];
                        foreach (string lsDataName in loData.DataModel.DataNameStreamList)
                        {
                            int lnReturn = MaxStreamLibrary.StreamSave(loData, lsDataName);
                            if ((lnReturn & 1) != 0)
                            {
                                lnR |= 2; //// Error saving stream
                            }
                        }

                        if (lnR == 0)
                        {
                            this.CreateTable(loData);
                            DataSet loDataSet = this.GetDataSet(loData);
                            DataRow loRow = loDataSet.Tables[loDataList.DataModel.DataStorageName].NewRow();
                            foreach (string lsDataName in loData.DataModel.DataNameList)
                            {
                                if (loData.DataModel.IsStored(lsDataName))
                                {
                                    object loValue = loData.Get(lsDataName);
                                    if (null != loValue)
                                    {
                                        loRow[lsDataName] = loValue;
                                    }
                                    else if (loData.DataModel.GetValueType(lsDataName) == typeof(bool))
                                    {
                                        loRow[lsDataName] = false;
                                    }
                                }
                            }

                            loDataSet.Tables[loDataList.DataModel.DataStorageName].Rows.Add(loRow);
                        }
                    }

                    this.SaveToFile();
                }
                catch (Exception loE)
                {
                    MaxLogLibrary.Log(new MaxLogEntryStructure(this.GetType(), "Insert", MaxEnumGroup.LogError, "Exception inserting {Count} data elements", loE, loDataList.Count));
                    lnR |= 1; //// Exception inserting
                }
            }

            return lnR;
        }

        /// <summary>
        /// Updates a list of elements
        /// </summary>
        /// <param name="loDataList">The list of elements</param>
        /// <returns>Flag based status code indicating level of success.</returns>
        public virtual int Update(MaxDataList loDataList)
        {
            int lnR = 0;
            lock (_oLock)
            {
                try
                {
                    for (int lnD = 0; lnD < loDataList.Count && lnR == 0; lnD++)
                    {
                        MaxData loData = loDataList[lnD];
                        foreach (string lsDataName in loData.DataModel.DataNameStreamList)
                        {
                            int lnReturn = MaxStreamLibrary.StreamSave(loData, lsDataName);
                            if ((lnReturn & 1) != 0 && (lnR & 2) != 0)
                            {
                                lnR |= 2; //// Error saving stream
                            }
                        }
                        if (lnR == 0)
                        {
                            this.CreateTable(loData);
                            DataSet loDataSet = this.GetDataSet(loData);
                            foreach (DataRow loRow in loDataSet.Tables[loData.DataModel.DataStorageName].Rows)
                            {
                                bool lbIsMatch = true;
                                foreach (string lsKey in loData.DataModel.DataNameKeyList)
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
                                    foreach (string lsDataName in loDataList.DataModel.DataNameList)
                                    {
                                        if (loDataList.DataModel.IsStored(lsDataName))
                                        {
                                            if (loData.GetIsChanged(lsDataName))
                                            {
                                                object loValue = loData.Get(lsDataName);
                                                if (null == loValue)
                                                {
                                                    loRow[lsDataName] = DBNull.Value;
                                                }
                                                else
                                                {
                                                    loRow[lsDataName] = loValue;
                                                }
                                            }
                                        }
                                    }

                                    lnR++;
                                }
                            }
                        }
                    }

                    this.SaveToFile();
                }
                catch (Exception loE)
                {
                    MaxLogLibrary.Log(new MaxLogEntryStructure(this.GetType(), "Update", MaxEnumGroup.LogError, "Exception updating {Count} data elements", loE, loDataList.Count));
                    lnR |= 1; //// Exception updating
                }
            }

            return lnR;
        }


        /// <summary>
        /// Deletes a list of elements
        /// </summary>
        /// <param name="loDataList">The list of elements</param>
        /// <returns>Flag based status code indicating level of success.</returns>
        public virtual int Delete(MaxDataList loDataList)
        {
            int lnR = 0;
            lock (_oLock)
            {
                try
                {
                    for (int lnD = 0; lnD < loDataList.Count; lnD++)
                    {
                        MaxData loData = loDataList[lnD];
                        foreach (string lsDataName in loData.DataModel.DataNameStreamList)
                        {
                            int lnReturn = MaxStreamLibrary.StreamDelete(loData, lsDataName);
                            if ((lnReturn & 1) != 0)
                            {
                                lnR |= 2; //// Error deleting stream
                            }
                        }

                        if (lnR == 0)
                        {
                            this.CreateTable(loData);
                            DataSet loDataSet = this.GetDataSet(loData);
                            List<string> loPrimaryKeyList = new List<string>(loDataList.DataModel.DataNameKeyList);
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
                    }

                    this.SaveToFile();
                }
                catch (Exception loE)
                {
                    MaxLogLibrary.Log(new MaxLogEntryStructure(this.GetType(), "Delete", MaxEnumGroup.LogError, "Exception deleting {Count} data elements", loE, loDataList.Count));
                    lnR |= 1; //// Exception deleting
                }
            }

            return lnR;
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
                        List<DataColumn> loPrimaryKeyList = new List<DataColumn>();
                        for (int lnK = 0; lnK < loData.DataModel.DataNameList.Length; lnK++)
                        {
                            string lsColumName = loData.DataModel.DataNameList[lnK];
                            Type loType = loData.DataModel.GetValueType(lsColumName);
                            if (loType == typeof(MaxShortString) || loType == typeof(MaxLongString))
                            {
                                loType = typeof(string);
                            }

                            DataColumn loColumn = new DataColumn(lsColumName, loType);
                            if (loData.DataModel.GetAttributeSetting(lsColumName, MaxDataModel.AttributeIsAllowDBNull))
                            {
                                loColumn.AllowDBNull = true;
                            }

                            if (loData.DataModel.GetAttributeSetting(lsColumName, MaxDataModel.AttributeIsDataKey))
                            {
                                loPrimaryKeyList.Add(loColumn);
                            }

                            loTable.Columns.Add(loColumn);
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
#endif
    }
}
