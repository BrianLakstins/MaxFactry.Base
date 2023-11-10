// <copyright file="MaxBaseIdRepositoryDefaultProvider.cs" company="Lakstins Family, LLC">
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
// <change date="6/25/2014" author="Brian A. Lakstins" description="Based on MaxAppIdRepositoryProvider">
// <change date="12/2/2014" author="Brian A. Lakstins" description="Update to match interface.  Add laFields.">
// <change date="3/26/2015" author="Brian A. Lakstins" description="Moved to MaxFactry.Base">
// <change date="12/21/2016" author="Brian A. Lakstins" description="Added central method to filter by deleted property.">
// </changelog>
#endregion

namespace MaxFactry.Base.DataLayer.Provider
{
	using System;
	using MaxFactry.Core;
    using MaxFactry.Base.DataLayer;

	/// <summary>
    /// Provider for MaxBaseIdRepository
	/// </summary>
    public abstract class MaxBaseIdRepositoryDefaultProvider : MaxFactry.Base.DataLayer.Provider.MaxStorageWriteRepositoryDefaultProvider, IMaxBaseIdRepositoryProvider
	{
        /// <summary>
        /// Selects data from the database.
        /// </summary>
        /// <param name="loData">Element with data used in the filter.</param>
        /// <param name="loDataQuery">Query information to filter results.</param>
        /// <param name="lnPageIndex">Page to return.</param>
        /// <param name="lnPageSize">Items per page.</param>
        /// <param name="lnTotal">Total items found.</param>
        /// <param name="laFields">list of fields to return from select.</param>
        /// <returns>List of data from select.</returns>
        public override MaxDataList Select(MaxData loData, MaxDataQuery loDataQuery, int lnPageIndex, int lnPageSize, string lsSort, out int lnTotal, params string[] laFields)
        {
            ////Update DataQuery to include filter by IsDeleted if one is not already there.
            if (loData.DataModel is MaxBaseIdDataModel)
            {
                bool lbHasIsDeleted = false;
                if (null != loDataQuery)
                {
                    object[] laQuery = loDataQuery.GetQuery();
                    foreach (object loQuery in laQuery)
                    {
                        if (loQuery is MaxDataFilter)
                        {
                            if (((MaxDataFilter)loQuery).Name == ((MaxBaseIdDataModel)loData.DataModel).IsDeleted)
                            {
                                lbHasIsDeleted = true;
                            }
                        }
                    }

                    if (!lbHasIsDeleted && loData.DataModel.HasKey(((MaxBaseIdDataModel)loData.DataModel).IsDeleted))
                    {
                        if (laQuery.Length > 0)
                        {
                            loDataQuery.AddCondition("AND");
                        }

                        loDataQuery.StartGroup();
                        loDataQuery.AddFilter(((MaxBaseIdDataModel)loData.DataModel).IsDeleted, "=", false);
                        loDataQuery.EndGroup();
                    }
                }
            }

            return base.Select(loData, loDataQuery, lnPageIndex, lnPageSize, lsSort, out lnTotal, laFields);
        }
	}
}
