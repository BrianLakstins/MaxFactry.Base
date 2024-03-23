// <copyright file="MaxBaseIdIndexRepository.cs" company="Lakstins Family, LLC">
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
// <change date="6/26/2014" author="Brian A. Lakstins" description="Based on MaxAppIdIndexRepository">
// <change date="7/9/2014" author="Brian A. Lakstins" description="Updated to use MaxData for provider selection.">
// <change date="12/2/2014" author="Brian A. Lakstins" description="Add laFields.">
// <change date="12/18/2014" author="Brian A. Lakstins" description="Updated Provider and DataModel access pattern.">
// <change date="3/26/2015" author="Brian A. Lakstins" description="Moved to MaxFactry.Base">
// <change date="12/21/2016" author="Brian A. Lakstins" description="Updated to work consistently with updated MaxData.">
// <change date="3/20/2024" author="Brian A. Lakstins" description="Happy birthday to my mom.  Sara Jean Lakstins (Cartwright) - 3/20/1944 to 3/14/2019.">
// <change date="3/23/2024" author="Brian A. Lakstins" description="Removed passing Total to methods.">
// </changelog>
#endregion

namespace MaxFactry.Base.DataLayer
{
	using System;
    using MaxFactry.Core;
    using MaxFactry.Base.DataLayer;

	/// <summary>
	/// Repository for basic application information
	/// </summary>
    public class MaxBaseIdIndexRepository : MaxBaseIdRepository
	{
        /// <summary>
        /// Selects all not marked as deleted based on the name
        /// </summary>
        /// <param name="loData">Data information used for select</param>
        /// <param name="lsName">The name for the value</param>
        /// <param name="laDataNameList">list of fields to return from select</param>
        /// <returns>List of information matching the application name</returns>
        public static MaxDataList SelectAllByName(MaxData loData, string lsName, params string[] laDataNameList)
        {
            MaxBaseIdIndexDataModel loDataModel = loData.DataModel as MaxBaseIdIndexDataModel;
            if (null == loDataModel)
            {
                throw new MaxException("Error casting [" + loData.DataModel.GetType() + "] for DataModel");
            }

            loData.Set(loDataModel.Name, lsName);
            MaxData loDataFilter = new MaxData(loData);
            loDataFilter.Set(loDataModel.Name, lsName);
            MaxDataQuery loDataQuery = new MaxDataQuery();
            MaxDataList loDataList = Select(loDataFilter, loDataQuery, 0, 0, string.Empty, laDataNameList);
            return loDataList;
        }

        /// <summary>
        /// Selects all not marked as deleted based on the IndexId
        /// </summary>
        /// <param name="loData">Data information used for select</param>
        /// <param name="loIndexId">Id of the Index.</param>
        /// <param name="laDataNameList">list of fields to return from select</param>
        /// <returns>List of information matching the application name</returns>
        public static MaxDataList SelectAllByIndexId(MaxData loData, Guid loIndexId, params string[] laDataNameList)
        {
            MaxBaseIdIndexDataModel loDataModel = loData.DataModel as MaxBaseIdIndexDataModel;
            if (null == loDataModel)
            {
                throw new MaxException("Error casting [" + loData.DataModel.GetType() + "] for DataModel");
            }

            loData.Set(loDataModel.IndexId, loIndexId);
            MaxData loDataFilter = new MaxData(loData);
            loDataFilter.Set(loDataModel.IndexId, loIndexId);
            MaxDataQuery loDataQuery = new MaxDataQuery();
            MaxDataList loDataList = Select(loDataFilter, loDataQuery, 0, 0, string.Empty, laDataNameList);
            return loDataList;
        }

        /// <summary>
        /// Selects all not marked as deleted based on the name and IndexId
        /// </summary>
        /// <param name="loData">Data information used for select</param>
        /// <param name="lsName">The name for the value</param>
        /// <param name="loIndexId">Id of the Index.</param>
        /// <param name="laDataNameList">list of fields to return from select</param>
        /// <returns>List of information matching the application name</returns>
        public static MaxDataList SelectAllByNameIndexId(MaxData loData, string lsName, Guid loIndexId, params string[] laDataNameList)
        {
            MaxBaseIdIndexDataModel loDataModel = loData.DataModel as MaxBaseIdIndexDataModel;
            if (null == loDataModel)
            {
                throw new MaxException("Error casting [" + loData.DataModel.GetType() + "] for DataModel");
            }

            loData.Set(loDataModel.Name, lsName);
            loData.Set(loDataModel.IndexId, loIndexId);
            MaxData loDataFilter = new MaxData(loData);
            loDataFilter.Set(loDataModel.IndexId, loIndexId);
            loDataFilter.Set(loDataModel.Name, lsName);
            MaxDataQuery loDataQuery = new MaxDataQuery();
            MaxDataList loDataList = Select(loDataFilter, loDataQuery, 0, 0, string.Empty, laDataNameList);
            return loDataList;
        }
	}
}
