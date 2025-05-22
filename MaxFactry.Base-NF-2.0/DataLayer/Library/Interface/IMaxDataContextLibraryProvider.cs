// <copyright file="IMaxDataContextLibraryProvider.cs" company="Lakstins Family, LLC">
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
// <change date="2/25/2014" author="Brian A. Lakstins" description="Add select methods.">
// <change date="8/21/2014" author="Brian A. Lakstins" description="Add stream management methods.">
// <change date="12/2/2014" author="Brian A. Lakstins" description="Add laFields.">
// <change date="3/20/2024" author="Brian A. Lakstins" description="Happy birthday to my mom.  Sara Jean Lakstins (Cartwright) - 3/20/1944 to 3/14/2019.">
// <change date="3/23/2024" author="Brian A. Lakstins" description="Updated SelectAll to use information from MaxData">
// <change date="3/25/2024" author="Brian A. Lakstins" description="Moved from MaxFactry.Base.DataLayer namespace and renamed from IMaxDataContextProvider">
// <change date="5/21/2025" author="Brian A. Lakstins" description="Remove stream handling.  Return flag based status codes. Always handle a list.  Review and update for consistency.">
// </changelog>
#endregion

namespace MaxFactry.Base.DataLayer.Library
{
	using System;
    using System.IO;
    using MaxFactry.Core;

    /// <summary>
    /// Provides methods to manipulate storage of data
    /// </summary>
    public interface IMaxDataContextLibraryProvider : IMaxProvider
	{
        /// <summary>
        /// Selects all data
        /// </summary>
        /// <param name="loData">Data to use as definition</param>
        /// <param name="laDataNameList">Names of fields to return</param>
        /// <returns>List of data that is stored</returns>
        MaxDataList SelectAll(MaxData loData, params string[] laDataNameList);

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
        MaxDataList Select(MaxData loData, MaxDataQuery loDataQuery, int lnPageIndex, int lnPageSize, string lsOrderBy, out int lnTotal, params string[] laDataNameList);

        /// <summary>
        /// Selects a count of records
        /// </summary>
        /// <param name="loData">Data to use as definition</param>
        /// <param name="loDataQuery">Filter for the query</param>
        /// <returns>Count that matches the query parameters</returns>
        int SelectCount(MaxData loData, MaxDataQuery loDataQuery);

        /// <summary>
        /// Inserts a new list of elements
        /// </summary>
        /// <param name="loDataList">The list of elements</param>
        /// <returns>Flag based status code indicating level of success.</returns>
        int Insert(MaxDataList loDataList);

        /// <summary>
        /// Updates a list of elements
        /// </summary>
        /// <param name="loDataList">The list of elements</param>
        /// <returns>Flag based status code indicating level of success.</returns>
		int Update(MaxDataList loDataList);

        /// <summary>
        /// Deletes a list of elements
        /// </summary>
        /// <param name="loDataList">The list of elements</param>
        /// <returns>Flag based status code indicating level of success.</returns>
		int Delete(MaxDataList loDataList);
    }
}
