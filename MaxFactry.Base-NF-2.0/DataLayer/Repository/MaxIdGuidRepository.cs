// <copyright file="MaxIdGuidRepository.cs" company="Lakstins Family, LLC">
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
// <change date="6/25/2014" author="Brian A. Lakstins" description="Based on MaxAppIdRepository">
// <change date="12/2/2014" author="Brian A. Lakstins" description="Add laFields.">
// <change date="12/18/2014" author="Brian A. Lakstins" description="Updated Provider and DataModel access pattern.">
// <change date="3/26/2015" author="Brian A. Lakstins" description="Moved to MaxFactry.Base">
// <change date="6/1/2015" author="Brian A. Lakstins" description="Updated to not need provider.">
// <change date="12/21/2016" author="Brian A. Lakstins" description="Updated to work with meta copy of MaxData">
// <change date="12/21/2016" author="Brian A. Lakstins" description="Updated to work with meta copy of MaxData">
// <change date="3/20/2024" author="Brian A. Lakstins" description="Happy birthday to my mom.  Sara Jean Lakstins (Cartwright) - 3/20/1944 to 3/14/2019.">
// <change date="3/22/2024" author="Brian A. Lakstins" description="Added SelectAllByProperty because no longer available in parent class.">
// </changelog>
#endregion

namespace MaxFactry.Base.DataLayer
{
	using System;
    using MaxFactry.Core;

	/// <summary>
    /// Provides static methods to manipulate storage of data using the MaxIdGuidDataModel
	/// </summary>
    public abstract class MaxIdGuidRepository : MaxBaseWriteRepository
	{
        /// <summary>
        /// Selects all based on a single property
        /// </summary>
        /// <param name="loData">Element with data used to determine the provider.</param>
        /// <param name="lsPropertyName">The name of the property used to select.</param>
        /// <param name="loPropertyValue">The value of the property used to select.</param>
        /// <param name="laDataNameList">list of fields to return from select</param>
        /// <returns>List of data from select</returns>
        protected static MaxDataList SelectAllByProperty(MaxData loData, string lsPropertyName, object loPropertyValue, params string[] laDataNameList)
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
        /// Selects entity matching the unique identifier
        /// </summary>
        /// <param name="loData">Data used to determine repository provider.</param>
        /// <param name="loId">Unique Identifier of the entity</param>
        /// <param name="laDataNameList">list of fields to return from select</param>
        /// <returns>Single entity that matches the Unique Id</returns>
        public static MaxData SelectById(MaxData loData, Guid loId, params string[] laDataNameList)
		{
            MaxIdGuidDataModel loDataModel = loData.DataModel as MaxIdGuidDataModel;
            if (null == loDataModel)
            {
                throw new MaxException("Error casting [" + loData.DataModel.GetType() + "] for DataModel");
            }

            MaxDataList loList = SelectAllByProperty(loData, loDataModel.Id, loId, laDataNameList);
            if (loList.Count.Equals(1))
            {
                return loList[0];
            }
            else if (loList.Count > 1)
            {
                throw new MaxException("Multiple records match the  Id [" + loId.ToString() + "]");
            }

            return null;
        }

        /// <summary>
        /// Selects entities created within the date range
        /// </summary>
        /// <param name="loData">Data used to determine repository provider.</param>
        /// <param name="ldStart">Start date of range</param>
        /// <param name="ldEnd">End date of range</param>
        /// <param name="laDataNameList">list of fields to return from select</param>
        /// <returns>List of data from select</returns>
        public static MaxDataList SelectAllByCreatedDateRange(MaxData loData, DateTime ldStart, DateTime ldEnd, params string[] laDataNameList)
        {
            MaxIdGuidDataModel loDataModel = loData.DataModel as MaxIdGuidDataModel;
            if (null == loDataModel)
            {
                throw new MaxException("Error casting [" + loData.DataModel.GetType() + "] for DataModel");
            }

            MaxData loDataFilter = new MaxData(loData);
            MaxDataQuery loDataQuery = new MaxDataQuery();
            loDataQuery.StartGroup();
            loDataQuery.AddFilter(loDataModel.CreatedDate, ">", ldStart);
            loDataQuery.AddCondition("AND");
            loDataQuery.AddFilter(loDataModel.CreatedDate, "<", ldEnd);
            loDataQuery.EndGroup();
            MaxDataList loDataList = Select(loDataFilter, loDataQuery, 0, 0, string.Empty, laDataNameList);
            return loDataList;
        }
	}
}
