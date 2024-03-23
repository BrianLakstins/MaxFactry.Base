// <copyright file="MaxBaseIdRepository.cs" company="Lakstins Family, LLC">
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
// <change date="6/26/2014" author="Brian A. Lakstins" description="Based on MaxCRUDRepository">
// <change date="6/27/2014" author="Brian A. Lakstins" description="Fix references to interfaces.">
// <change date="7/3/2014" author="Brian A. Lakstins" description="Added support for managing streams.">
// <change date="7/4/2014" author="Brian A. Lakstins" description="Added support for sending messages.">
// <change date="8/21/2014" author="Brian A. Lakstins" description="Add url support.">
// <change date="10/22/2014" author="Brian A. Lakstins" description="Add method to copy all data from currently configure provider to another provider.">
// <change date="12/2/2014" author="Brian A. Lakstins" description="Add laFields.">
// <change date="12/18/2014" author="Brian A. Lakstins" description="Updated Provider and DataModel access pattern.">
// <change date="1/14/2015" author="Brian A. Lakstins" description="Update to work without any deleted field.">
// <change date="3/26/2015" author="Brian A. Lakstins" description="Moved to MaxFactry.Base">
// <change date="12/21/2016" author="Brian A. Lakstins" description="Updated to work consistently with updated MaxData.">
// <change date="3/20/2024" author="Brian A. Lakstins" description="Happy birthday to my mom.  Sara Jean Lakstins (Cartwright) - 3/20/1944 to 3/14/2019.">
// <change date="3/22/2024" author="Brian A. Lakstins" description="Change parent class.  Use MaxData without making a copy.  Add SelectAllByProperty because it no longer exists in the parent class.">
// </changelog>
#endregion

namespace MaxFactry.Base.DataLayer
{
	using System;
    using MaxFactry.Core;

	/// <summary>
	/// Provides static methods to manipulate storage of data
	/// </summary>
	public abstract class MaxBaseIdRepository : MaxBaseRepository
	{
        /// <summary>
        /// Selects all based on a single property
        /// </summary>
        /// <param name="loData">Element with data used to determine the provider.</param>
        /// <param name="lsPropertyName">The name of the property used to select.</param>
        /// <param name="loPropertyValue">The value of the property used to select.</param>
        /// <param name="laDataNameList">list of fields to return from select</param>
        /// <returns>List of data from select</returns>
        public static MaxDataList SelectAllByProperty(MaxData loData, string lsPropertyName, object loPropertyValue, params string[] laDataNameList)
        {
            //// Set the property in case it is used on a PrimaryKey suffix
            loData.Set(lsPropertyName, loPropertyValue);
            MaxData loDataFilter = new MaxData(loData);
            //// Add a Query 
            MaxDataQuery loDataQuery = new MaxDataQuery();
            loDataQuery.StartGroup();
            loDataQuery.AddFilter(lsPropertyName, "=", loPropertyValue);
            loDataQuery.EndGroup();
            MaxDataList loDataList = Select(loDataFilter, loDataQuery, 0, 0, string.Empty, laDataNameList);
            return loDataList;
        }

        /// <summary>
        /// Selects all active not marked as deleted based on a single property
        /// </summary>
        /// <param name="loData">Element with data used to determine the provider.</param>
        /// <param name="lsPropertyName">The name of the property used to select.</param>
        /// <param name="loPropertyValue">The value of the property used to select.</param>
        /// <param name="laDataNameList">list of fields to return from select</param>
        /// <returns>List of data from select</returns>
        public static MaxDataList SelectAllActiveByProperty(MaxData loData, string lsPropertyName, object loPropertyValue, params string[] laDataNameList)
        {
            MaxBaseDataModel loDataModel = loData.DataModel as MaxBaseDataModel;
            if (null == loDataModel)
            {
                throw new MaxException("Error casting [" + loData.DataModel.GetType() + "] for DataModel");
            }

            MaxDataQuery loDataQuery = new MaxDataQuery();
            loDataQuery.StartGroup();
            loDataQuery.AddFilter(lsPropertyName, "=", loPropertyValue);
            loDataQuery.AddCondition("AND");
            loDataQuery.AddFilter(loDataModel.IsActive, "=", true);
            loDataQuery.EndGroup();
            MaxDataList loDataList = Select(loData, loDataQuery, 0, 0, string.Empty, laDataNameList);
            return loDataList;
        }

        /// <summary>
        /// Selects entities created within the date range
        /// </summary>
        /// <param name="loData">Data used to determine repository provider.</param>
        /// <param name="ldStart">Start date of range</param>
        /// <param name="ldEnd">End date of range</param>
        /// <param name="laDataNameList">list of fields to return from select</param>
        /// <returns>List of data from select</returns>
        public static MaxDataList SelectAllByLastUpdateDateRange(MaxData loData, DateTime ldStart, DateTime ldEnd, params string[] laDataNameList)
        {
            MaxBaseIdDataModel loDataModel = loData.DataModel as MaxBaseIdDataModel;
            if (null == loDataModel)
            {
                throw new MaxException("Error casting [" + loData.DataModel.GetType() + "] for DataModel");
            }

            MaxDataQuery loDataQuery = new MaxDataQuery();
            loDataQuery.StartGroup();
            loDataQuery.AddFilter(loDataModel.LastUpdateDate, ">=", ldStart);
            loDataQuery.AddCondition("AND");
            loDataQuery.AddFilter(loDataModel.LastUpdateDate, "<", ldEnd);
            loDataQuery.EndGroup();
            MaxDataList loDataList = Select(loData, loDataQuery, 0, 0, string.Empty, laDataNameList);
            return loDataList;
        }
	}
}