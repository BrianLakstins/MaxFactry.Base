// <copyright file="IMaxDataContextProvider.cs" company="Lakstins Family, LLC">
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
// </changelog>
#endregion

namespace MaxFactry.Base.DataLayer
{
	using System;
    using System.IO;
    using MaxFactry.Core;

    /// <summary>
    /// Provides methods to manipulate storage of data
    /// </summary>
    public interface IMaxDataContextProvider : IMaxProvider
	{
        /// <summary>
        /// Selects all data from the data storage name for the specified type.
        /// </summary>
        /// <param name="lsDataStorageName">Name of the data storage (table name).</param>
        /// <param name="laFields">list of fields to return from select</param>
        /// <returns>List of data elements with a base data model.</returns>
        MaxDataList SelectAll(string lsDataStorageName, params string[] laFields);

        /// <summary>
        /// Selects data from the database
        /// </summary>
        /// <param name="loData">Element with data used in the filter</param>
        /// <param name="loDataQuery">Query information to filter results.</param>
        /// <param name="lnPageIndex">Page to return</param>
        /// <param name="lnPageSize">Items per page</param>
        /// <param name="lsSort">Sorting information</param>
        /// <param name="lnTotal">Total items found</param>
        /// <param name="laFields">list of fields to return from select</param>
        /// <returns>List of data from select</returns>
        MaxDataList Select(MaxData loData, MaxDataQuery loDataQuery, int lnPageIndex, int lnPageSize, string lsSort, out int lnTotal, params string[] laFields);

        /// <summary>
        /// Gets the number of records that match the filter.
        /// </summary>
        /// <param name="loData">Element with data used in the filter.</param>
        /// <param name="loDataQuery">Query information to filter results.</param>
        /// <returns>number of records that match.</returns>
        int SelectCount(MaxData loData, MaxDataQuery loDataQuery);

		/// <summary>
		/// Inserts a list of data objects.
		/// </summary>
		/// <param name="loDataList">The list of data objects to insert.</param>
		/// <returns>The count affected.</returns>
		int Insert(MaxDataList loDataList);

		/// <summary>
		/// Updates a list of data objects.
		/// </summary>
		/// <param name="loDataList">The list of data objects to insert.</param>
		/// <returns>The count affected.</returns>
		int Update(MaxDataList loDataList);

		/// <summary>
		/// Deletes a list of data objects.
		/// </summary>
		/// <param name="loDataList">The list of data objects to insert.</param>
		/// <returns>The count affected.</returns>
		int Delete(MaxDataList loDataList);

        /// <summary>
        /// Writes stream data to storage.
        /// </summary>
        /// <param name="loData">The data index for the object</param>
        /// <param name="lsKey">Data element name to write</param>
        /// <returns>True if stream saved.</returns>
        bool StreamSave(MaxData loData, string lsKey);

        /// <summary>
        /// Opens stream data in storage
        /// </summary>
        /// <param name="loData">The data index for the object</param>
        /// <param name="lsKey">Data element name to write</param>
        /// <returns>Stream that was opened.</returns>
        Stream StreamOpen(MaxData loData, string lsKey);

        /// <summary>
        /// Removes stream from storage.
        /// </summary>
        /// <param name="loData">The data index for the object</param>
        /// <param name="lsKey">Data element name to remove</param>
        /// <returns>true if successful.</returns>
        bool StreamDelete(MaxData loData, string lsKey);

        /// <summary>
        /// Gets the Url of a saved stream.
        /// </summary>
        /// <param name="loData">The data index for the object</param>
        /// <param name="lsKey">Data element name</param>
        /// <returns>Url of stream if one can be provided.</returns>
        string GetStreamUrl(MaxData loData, string lsKey);
	}
}
