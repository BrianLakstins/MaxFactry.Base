// <copyright file="MaxData.cs" company="Lakstins Family, LLC">
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
// <change date="1/22/2014" author="Brian A. Lakstins" description="Initial Release">
// <change date="2/22/2014" author="Brian A. Lakstins" description="Added checking for null in Set method.">
// <change date="2/22/2014" author="Brian A. Lakstins" description="Added more ways to convert to boolean.">
// <change date="2/25/2014" author="Brian A. Lakstins" description="Added more conversions.">
// <change date="3/11/2014" author="Brian A. Lakstins" description="Added turning strings into numbers.">
// <change date="4/2/2014" author="Brian A. Lakstins" description="Add constructor that includes data.  Update to not set when value is same.">
// <change date="5/19/2014" author="Brian A. Lakstins" description="Add ability to add extended fields.">
// <change date="6/16/2014" author="Brian A. Lakstins" description="Add support for bitflag storage.">
// <change date="7/2/2014" author="Brian A. Lakstins" description="Add minimal support for byte[] and Stream.">
// <change date="7/3/2014" author="Brian A. Lakstins" description="Removed converting a stored byte[] to a Stream.">
// <change date="7/17/2014" author="Brian A. Lakstins" description="Moved conversion to MaxConveryLibrary.">
// <change date="8/21/2014" author="Brian A. Lakstins" description="Fix warning in MF43.">
// <change date="9/26/2014" author="Brian A. Lakstins" description="Added a way to create a MaxData from another MaxData with a different model.">
// <change date="10/17/2014" author="Brian A. Lakstins" description="Removed array based initialization. Updated Set method to be faster.">
// <change date="10/22/2014" author="Brian A. Lakstins" description="Fix Extension issue.">
// <change date="11/10/2014" author="Brian A. Lakstins" description="Move type specific get and set to MaxEntity in Business Layer">
// <change date="1/6/2016" author="Brian A. Lakstins" description="Add a way to clear the changed status for each property.">
// <change date="1/10/2016" author="Brian A. Lakstins" description="Update comparison method for determining if data has been changed.">
// <change date="12/21/2016" author="Brian A. Lakstins" description="Added clone method.  Updated constructor to create a copy that only includes meta attributes.">
// <change date="7/30/2019" author="Brian A. Lakstins" description="Update handling of StorageKey.">
// <change date="12/2/2019" author="Brian A. Lakstins" description="Added clone method that allows changing DataModel.">
// <change date="12/12/2019" author="Brian A. Lakstins" description="Added reset method that allows clearing data.">
// <change date="8/26/2020" author="Brian A. Lakstins" description="Add logging of any errors in Set method (been getting some NullReferenceException errors)">
// <change date="8/26/2020" author="Brian A. Lakstins" description="Still got a null error, so wrapping the try up over more code.">
// <change date="9/1/2020" author="Brian A. Lakstins" description="Still got a null error, so changing it a bit.">
// <change date="1/16/2021" author="Brian A. Lakstins" description="Add a way to convert to and from a string.">
// <change date="1/18/2021" author="Brian A. Lakstins" description="Fix handling streams when converting to a string.">
// <change date="5/28/2021" author="Brian A. Lakstins" description="Handle stream path.">
// <change date="8/2/2023" author="Brian A. Lakstins" description="Fixed a deserialization issue by not having a contstructor without any arguements">
// <change date="3/20/2024" author="Brian A. Lakstins" description="Happy birthday to my mom.  Sara Jean Lakstins (Cartwright) - 3/20/1944 to 3/14/2019.">
// <change date="3/22/2024" author="Brian A. Lakstins" description="Rename Key to DataName.  Save original value when changed.  Consolidate Clone and MaxData initializer.  Remove Clone by datamodel (initializer can be used instead).  Rename Reset to Clear.  Add GetKey.  Add key to stream path.">
// <change date="3/23/2024" author="Brian A. Lakstins" description="Remove GetKey method (moving to DataModel so can be overridden in child classes if needed)">
// <change date="3/24/2024" author="Brian A. Lakstins" description="Updated for changes namespaces">
// <change date="3/26/2024" author="Brian A. Lakstins" description="Move logic for GetStreamPath.  Filter streams from extended data when converting to string.">
// <change date="3/30/2024" author="Brian A. Lakstins" description="Add DataKey as unique identifer for any record.  Update cloning proccess to keep track of changes.  Remove Streams when creating string from data.  Keep DataKey when initialized from another MaxData.">
// <change date="6/3/2025" author="Brian A. Lakstins" description="Update process to convert to and from string so it works with XML serializer">
// </changelog>
#endregion

namespace MaxFactry.Base.DataLayer
{
    using System;
    using System.IO;
    using MaxFactry.Base.DataLayer.Library;
    using MaxFactry.Core;

    /// <summary>
    /// Stores data retrieved from external sources in some name value pair type of collections.
    /// </summary>
    public class MaxData
    {
        /// <summary>
        /// Information to define the type of value for each key
        /// </summary>
        private MaxDataModel _oDataModel = null;

        /// <summary>
        /// The index of values based on keys
        /// </summary>
        private MaxIndex _oIndex = new MaxIndex();

        /// <summary>
        /// The index of whether the value was changed
        /// </summary>
        private MaxIndex _oChangedIndex = new MaxIndex();

        /// <summary>
        /// The index of extended fields.
        /// </summary>
        private MaxIndex _oExtendedIndex = new MaxIndex();

        private string _sDataKey = null;

        /// <summary>
        /// Initializes a new instance of the MaxData class
        /// </summary>
        public MaxData()
        {
        }

        /// <summary>
        /// Initializes a new instance of the MaxData class
        /// </summary>
        /// <param name="loDataModel">Definition of the data</param>
        public MaxData(MaxDataModel loDataModel)
        {
            this._oDataModel = loDataModel;
        }

        public MaxData(string lsData)
        {
            MaxIndexItemStructure[] laData = MaxConvertLibrary.DeserializeObject(this.GetType(), lsData, typeof(MaxIndexItemStructure[])) as MaxIndexItemStructure[];
            MaxIndex loDataIndex = new MaxIndex(laData);
            MaxIndexItemStructure[] laIndex = MaxConvertLibrary.DeserializeObject(this.GetType(), loDataIndex["Index.MaxIndexItemStructure[]"] as string, typeof(MaxIndexItemStructure[])) as MaxIndexItemStructure[];
            this._oIndex = new MaxIndex(laIndex);
            MaxIndexItemStructure[] laExtendedIndex = MaxConvertLibrary.DeserializeObject(this.GetType(), loDataIndex["ExtendedIndex.MaxIndexItemStructure[]"] as string, typeof(MaxIndexItemStructure[])) as MaxIndexItemStructure[];
            this._oExtendedIndex = new MaxIndex(laExtendedIndex);
            this._sDataKey = loDataIndex["DataKey"] as string;
            string lsDataModelType = loDataIndex["DataModelType"] as string;
            this._oDataModel = MaxDataLibrary.GetDataModel(Type.GetType(lsDataModelType));
        }

        /// <summary>
        /// Initializes a new instance of the MaxData class that includes unstored attributes from the existing instance, but not the stored data
        /// </summary>
        /// <param name="loData">Data to base the new MaxData on</param>
        public MaxData(MaxData loData)
        {
            this._oDataModel = loData.DataModel;
            this._sDataKey = loData.DataKey;
            string[] laExtendedNameList = loData.GetExtendedNameList();
            foreach (string lsDataName in laExtendedNameList)
            {
                this.Set(lsDataName, loData.Get(lsDataName));
            }
        }

        /// <summary>
        /// Gets the definition for the data
        /// </summary>
        public MaxDataModel DataModel
        {
            get
            {
                return this._oDataModel;
            }
        }

        public string DataKey
        {
            get
            {
                return this._sDataKey;
            }
        }

        /// <summary>
        /// Checks to see if two objects have different values
        /// </summary>
        /// <param name="loCurrent">Current object</param>
        /// <param name="loNew">New object</param>
        /// <returns>True if different</returns>
        public static bool IsChanged(object loCurrent, object loNew)
        {
            if (null == loCurrent && null == loNew)
            {
                return false;
            }
            else if (null == loCurrent || null == loNew)
            {
                return true;
            }
            else if (!loCurrent.GetType().Equals(loNew.GetType()))
            {
                return true;
            }
            else if (loCurrent.Equals(loNew))
            {
                return false;
            }
            else if (loCurrent is byte[] && loNew is byte[])
            {
                byte[] loCurrentByte = (byte[])loCurrent;
                byte[] loNewByte = (byte[])loNew;
                if (loCurrentByte.Length != loNewByte.Length)
                {
                    return true;
                }

                for (int lnB = 0; lnB < loCurrentByte.Length; lnB++)
                {
                    if (loCurrentByte[lnB] != loNewByte[lnB])
                    {
                        return true;
                    }
                }

                return false;
            }

            return true;
        }

        /// <summary>
        /// Gets the original value of something that changed
        /// </summary>
        /// <param name="lsDataName">Name of data</param>
        /// <returns></returns>
        public object GetOriginal(string lsDataName)
        {
            if (this._oChangedIndex.Contains(lsDataName))
            {
                return this._oChangedIndex[lsDataName];
            }

            return null;
        }

        /// <summary>
        /// Gets a value indicating if any property has changed.
        /// </summary>
        /// <returns>True if changed.</returns>
        public bool GetIsChanged()
        {
            foreach (string lsDataName in this.DataModel.DataNameList)
            {
                if (this.GetIsChanged(lsDataName))
                {
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// Gets a value indicating if the property for this key has changed.
        /// </summary>
        /// <param name="lsKey">Name of the key</param>
        /// <returns>True if changed.</returns>
        public bool GetIsChanged(string lsDataName)
        {
            return this._oChangedIndex.Contains(lsDataName);
        }

        /// <summary>
        /// Clears the status of properties being changed
        /// </summary>
        public void ClearChanged()
        {
            this._sDataKey = this.DataModel.GetDataKey(this);
            this._oChangedIndex = new MaxIndex();
        }

        /// <summary>
        /// Clears the status of a property being changed
        /// </summary>
        /// <param name="lsDataName">Name to use for matching data</param>
        public void ClearChanged(string lsDataName)
        {
            if (this._oChangedIndex.Contains(lsDataName))
            {
                this._oChangedIndex.Remove(lsDataName);
            }
        }

        /// <summary>
        /// Gets a value based on the key
        /// </summary>
        /// <param name="lsDataName">Name to use for matching data</param>
        /// <returns>The value matching the key.</returns>
        public object Get(string lsDataName)
        {
            object loR = this._oIndex[lsDataName];
            if (this._oExtendedIndex.Contains(lsDataName))
            {
                loR = this._oExtendedIndex[lsDataName];
            }

            return loR;
        }

        /// <summary>
        /// Sets a value
        /// </summary>
        /// <param name="lsDataName">Name to use for matching data</param>
        /// <param name="loValue">The value to set</param>
        public void Set(string lsDataName, object loValue)
        {
            try
            {
                object loCurrent = this.Get(lsDataName);
                if (null != loCurrent || null != loValue)
                {
                    if (!_oChangedIndex.Contains(lsDataName) && IsChanged(loCurrent, loValue))
                    {
                        this._oChangedIndex.Add(lsDataName, loCurrent);
                    }

                    if (this.DataModel.HasDataName(lsDataName))
                    {
                        this._oIndex.Add(lsDataName, loValue);
                    }
                    else
                    {
                        this._oExtendedIndex.Add(lsDataName, loValue);
                    }
                }
            }
            catch (Exception loE)
            {
                MaxLogLibrary.Log(new MaxLogEntryStructure("MaxData.Set", MaxEnumGroup.LogError, "Error in MaxData.Set for {lsKey}, {loValue}, {DataModel}", loE, lsDataName, loValue, this.DataModel));
            }
        }

        /// <summary>
        /// Gets a list of the extended keys.
        /// </summary>
        /// <returns>list of the keys</returns>
        public string[] GetExtendedNameList()
        {
            string[] laR = this._oExtendedIndex.GetSortedKeyList();
            return laR;
        }

        /// <summary>
        /// Sets all properties that are not null to be considered changed.
        /// </summary>
        public void SetChanged()
        {
            foreach (string lsDataName in this.DataModel.DataNameList)
            {
                this._oChangedIndex.Add(lsDataName, this.Get(lsDataName));
            }
        }

        /// <summary>
        /// Creates a copy of the current MaxData object
        /// </summary>
        /// <returns>A copy including all properties and attributes</returns>
        public MaxData Clone()
        {
            MaxData loR = new MaxData(this);
            //// First, set only original values to cloned data
            foreach (string lsDataName in this.DataModel.DataNameList)
            {
                if (null != this.GetOriginal(lsDataName))
                {
                    loR._oIndex.Add(lsDataName, this.GetOriginal(lsDataName));
                }
                else
                {
                    loR.Set(lsDataName, this.Get(lsDataName));
                }
            }

            foreach (string lsDataName in this.DataModel.DataNameStreamList)
            {
                loR.Set(lsDataName, this.Get(lsDataName));
            }

            this.ClearChanged();
            //// Second, set all data that was changed to the new data
            foreach (string lsDataName in this.DataModel.DataNameList)
            {
                if (null != this.GetOriginal(lsDataName))
                {
                    loR.Set(lsDataName, this.Get(lsDataName));
                }
            }

            return loR;
        }

        /// <summary>
        /// Clears the current Data
        /// </summary>
        public void Clear()
        {
            this._oChangedIndex = new MaxIndex();
            this._oExtendedIndex = new MaxIndex();
            this._oIndex = new MaxIndex();
        }

        public override string ToString()
        {
            MaxIndex loDataIndex = new MaxIndex();
            MaxIndex loIndex = new MaxIndex();
            string[] laDataName = this._oIndex.GetSortedKeyList();
            foreach (string lsDataName in laDataName)
            {
                //// Convert streams to just the text "stream"
                object loValue = this._oIndex[lsDataName];
                if (loValue is Stream)
                {
                    loValue = "stream";
                }

                loIndex.Add(lsDataName, loValue);
            }

            loDataIndex.Add("Index.MaxIndexItemStructure[]", MaxConvertLibrary.SerializeObjectToString(this.GetType(), loIndex.GetSortedList()));

            loIndex = new MaxIndex();
            laDataName = this._oExtendedIndex.GetSortedKeyList();
            foreach (string lsDataName in laDataName)
            {
                //// Convert streams to just the text "stream"
                object loValue = this._oExtendedIndex[lsDataName];
                if (loValue is Stream)
                {
                    loValue = "stream";
                }

                loIndex.Add(lsDataName, loValue);
            }

            loDataIndex.Add("ExtendedIndex.MaxIndexItemStructure[]", MaxConvertLibrary.SerializeObjectToString(this.GetType(), loIndex.GetSortedList()));
            loDataIndex.Add("DataModelType", this.DataModel.GetType().AssemblyQualifiedName);
            loDataIndex.Add("DataKey", this.DataKey);
            string lsR = MaxConvertLibrary.SerializeObjectToString(this.GetType(), loDataIndex.GetSortedList());
            return lsR;
        }

        public string[] GetStreamPath()
        {
            return this.DataModel.GetStreamPath(this);
        }
    }
}
