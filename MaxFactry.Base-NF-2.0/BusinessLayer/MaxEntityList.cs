// <copyright file="MaxEntityList.cs" company="Lakstins Family, LLC">
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
// <change date="12/16/2013" author="Brian A. Lakstins" description="Initial Release">
// <change date="10/24/2017" author="Brian A. Lakstins" description="Update to allow exporting to a string and loading from a string">
// <change date="1/23/2018" author="Brian A. Lakstins" description="Use method on MaxEntity so that it could be overridden on other classes.">
// <change date="5/31/2020" author="Brian A. Lakstins" description="Make sure data is not null before loading it.">
// <change date="3/20/2024" author="Brian A. Lakstins" description="Happy birthday to my mom.  Sara Jean Lakstins (Cartwright) - 3/20/1944 to 3/14/2019.">
// <change date="3/22/2024" author="Brian A. Lakstins" description="Set Total from DataList when created from DataList.  Remove unused string conversion methods.">
// </changelog>
#endregion

namespace MaxFactry.Base.BusinessLayer
{
    using System;
    using MaxFactry.Core;
    using MaxFactry.Base.DataLayer;

    /// <summary>
    /// Stores a list of elements that can be turned into data indexes
    /// </summary>
    public class MaxEntityList
    {
        /// <summary>
        /// Stores elements that can be turned into data indexes.
        /// Indexed by an incrementing integer.
        /// Values are object arrays.
        /// </summary>
        private MaxIndex _oEntityIndex = new MaxIndex();

        /// <summary>
        /// Type of entities in this list.
        /// </summary>
        private Type _oEntityType = null;

        /// <summary>
        /// Total when this represents a page
        /// </summary>
        private int _nTotal = int.MinValue;

        /// <summary>
        /// Initializes a new instance of the MaxEntityList class
        /// </summary>
        /// <param name="loType">Type of entity stored in this list.</param>
        public MaxEntityList(Type loType) : base()
        {
            this._oEntityType = loType;
        }

        /// <summary>
        /// Gets the number of elements in the list
        /// </summary>
        public int Count
        {
            get
            {
                return this._oEntityIndex.Count;
            }
        }

        /// <summary>
        /// Gets the type of entity in this list.
        /// </summary>
        public Type EntityType
        {
            get
            {
                return this._oEntityType;
            }
        }

        public int Total
        {
            get
            {
                if (this._nTotal >= 0)
                {
                    return this._nTotal;
                }

                return this.Count;
            }

            set
            {
                this._nTotal = value;
            }
        }

		/// <summary>
		/// Gets or sets the value of each item based on an integer key
		/// This only works if items have all been added with incrementing integer keys
		/// </summary>
		/// <param name="lnKey">Integer to match if possible</param>
		/// <returns>Value for the item</returns>
		public MaxEntity this[int lnKey]
		{
			get
			{
				return (MaxEntity)this._oEntityIndex[lnKey];
			}
		}

        /// <summary>
        /// Dynamically creates a new instance of the class configured for MaxEntityList
        /// </summary>
        /// <param name="loEntityType">Type of entity to create.</param>
        /// <returns>dynamically created class</returns>
        public static MaxEntityList Create(Type loEntityType)
        {
            return Create(loEntityType, null);
        }

        /// <summary>
        /// Dynamically creates a new instance of the class configured for MaxEntityList
        /// </summary>
        /// <param name="loEntityType">Type of entity to create.</param>
        /// <param name="loDataList">Data list to populate the entity list</param>
        /// <returns>dynamically created class</returns>
        public static MaxEntityList Create(Type loEntityType, MaxDataList loDataList)
        {
            MaxEntityList loEntityList = MaxFactryLibrary.Create(typeof(MaxEntityList), loEntityType) as MaxEntityList;
            if (null == loEntityList)
            {
                throw new MaxException("Error creating MaxEntityList");
            }

            if (null != loDataList)
            {
                loEntityList.Total = loDataList.Total;
                for (int lnD = 0; lnD < loDataList.Count; lnD++)
                {
                    MaxData loData = loDataList[lnD];
                    if (null != loData)
                    {
                        MaxEntity loEntity = MaxBusinessLibrary.GetEntity(loEntityType, loData);
                        loEntityList.Add(loEntity);
                    }
                }
            }

            return loEntityList;
        }

        /// <summary>
        /// Adds an item to the index generating an incrementing key
        /// </summary>
        /// <param name="loEntity">entity to add</param>
        public void Add(MaxEntity loEntity)
        {
            this._oEntityIndex.Add(loEntity);
            loEntity.SetList(this);
        }
    }
}
