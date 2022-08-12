// <copyright file="MaxDataList.cs" company="Lakstins Family, LLC">
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
// <change date="2/22/2014" author="Brian A. Lakstins" description="Removed some Column naming conventions.">
// <change date="4/2/2014" author="Brian A. Lakstins" description="Added tracking changed columns.">
// <change date="5/19/2014" author="Brian A. Lakstins" description="Add ability to stored extended fields.">
// <change date="10/17/2014" author="Brian A. Lakstins" description="Updated to store MaxData in an Index instead of breaking it down into arrays.">
// <change date="5/25/2017" author="Brian A. Lakstins" description="Updated use string as key without padding to lower memory usage and speed up data access.">
// <change date="1/16/2021" author="Brian A. Lakstins" description="Use integer array get methods of Index.">
// </changelog>
#endregion

namespace MaxFactry.Base.DataLayer
{
	using System;
    using MaxFactry.Core;

    /// <summary>
    /// Stores a list of elements that can be turned into data indexes
    /// </summary>
    public class MaxDataList
	{
		/// <summary>
		/// Definition for the elements in the list
		/// </summary>
		private MaxDataModel _oDataModel = null;

        /// <summary>
        /// List of data stored.
        /// </summary>
        private MaxIndex _oDataList = new MaxIndex();

		/// <summary>
		/// Total count of records if more than current list
		/// </summary>
		private int _nTotalCount = 0;

		/// <summary>
		/// Initializes a new instance of the MaxDataList class
		/// </summary>
		public MaxDataList() : base()
		{
		}

		/// <summary>
		/// Initializes a new instance of the MaxDataList class
		/// </summary>
		/// <param name="loDataModel">The definition of the data indexes</param>
		public MaxDataList(MaxDataModel loDataModel)
		{
			this._oDataModel = loDataModel;
		}

		/// <summary>
		/// Gets or sets the Total Count of records if more than the current list
		/// </summary>
		public int TotalCount
		{
			get
			{
				return this._nTotalCount;
			}

            set
            {
				this._nTotalCount = value;
            }
		}

		/// <summary>
		/// Gets the number of elements in the list
		/// </summary>
		public int Count
		{
			get
			{
                return this._oDataList.Count;
			}
		}

		/// <summary>
		/// Gets the name used for the data in the list
		/// </summary>
		public string DataStorageName
		{
			get
			{
				return this._oDataModel.DataStorageName;
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

        /// <summary>
        /// Gets or sets the value of each item based on an integer key.
        /// This only works if items have all been added with incrementing integer keys.
        /// </summary>
        /// <param name="lnKey">Integer to match if possible.</param>
        /// <returns>Value for the item.</returns>
        public MaxData this[int lnKey]
        {
            get
            {
                object loObject = this._oDataList[lnKey];
                MaxData loR = null;
                if (null != loObject && loObject is MaxData)
                {
                    loR = (MaxData)loObject;
                }

                return loR;
            }
        }

		/// <summary>
		/// Gets the value in an index
		/// </summary>
		/// <param name="lsKey">Key to match</param>
		/// <param name="lnElement">Position of the index in the list</param>
		/// <returns>object in the specified index matching the key</returns>
		public object Get(string lsKey, int lnElement)
		{
			object loR = null;
			MaxData loData = this[lnElement];
			if (null != loData)
			{
				loR = loData.Get(lsKey);
			}

			return loR;
		}

		/// <summary>
		/// Adds an index to the list
		/// </summary>
		/// <param name="loData">The index to add</param>
		public void Add(MaxData loData)
		{
            this._oDataList.Add(loData);
        }
	}
}
