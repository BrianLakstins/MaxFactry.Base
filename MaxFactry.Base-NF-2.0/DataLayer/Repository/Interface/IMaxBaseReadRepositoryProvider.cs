﻿// <copyright file="IMaxBaseReadRepositoryProvider.cs" company="Lakstins Family, LLC">
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
// <change date="3/20/2024" author="Brian A. Lakstins" description="Happy birthday to my mom.  Sara Jean Lakstins (Cartwright) - 3/20/1944 to 3/14/2019.">
// <change date="3/22/2024" author="Brian A. Lakstins" description="Initial creation.  Based on IMaxStorageReadRepositoryProvider.">
// <change date="3/25/2024" author="Brian A. Lakstins" description="Removing passing Total">
// </changelog>
#endregion

namespace MaxFactry.Base.DataLayer
{
    using System.IO;

    /// <summary>
    /// Provides methods to manipulate storage of data
    /// </summary>
    public interface IMaxBaseReadRepositoryProvider : IMaxRepositoryProvider
    {
        /// <summary>
        /// Selects all data from the data storage name for the specified type.
        /// </summary>
        /// <param name="loData">Data with a Data Model</param>
        /// <param name="laDataNameList">list of fields to return from select</param>
        /// <returns>List of data elements with a base data model.</returns>
        MaxDataList SelectAll(MaxData loData, params string[] laDataNameList);

        /// <summary>
        /// Selects data from the database
        /// </summary>
        /// <param name="loData">Element with data used in the filter</param>
        /// <param name="loDataQuery">Query information to filter results.</param>
        /// <param name="lnPageIndex">Page to return</param>
        /// <param name="lnPageSize">Items per page</param>
        /// <param name="lsOrderBy">Sort information</param>
        /// <param name="laDataNameList">list of fields to return from select</param>
        /// <returns>List of data from select</returns>
        MaxDataList Select(MaxData loData, MaxDataQuery loDataQuery, int lnPageIndex, int lnPageSize, string lsOrderBy, params string[] laDataNameList);

        /// <summary>
        /// Selects data from the database.
        /// </summary>
        /// <param name="loData">Element with data used in the filter.</param>
        /// <param name="loDataQuery">Query information to filter results.</param>
        /// <returns>List of data from select.</returns>
        int SelectCount(MaxData loData, MaxDataQuery loDataQuery);

        /// <summary>
        /// Opens stream data in storage
        /// </summary>
        /// <param name="loData">The data index for the object</param>
        /// <param name="lsKey">Data element name to write</param>
        /// <returns>Stream that was opened.</returns>
        Stream StreamOpen(MaxData loData, string lsKey);

        /// <summary>
        /// Gets a Url to a stream.
        /// </summary>
        /// <param name="loData">The data index for the object</param>
        /// <param name="lsKey">Data element name</param>
        /// <returns>Url to stream.</returns>
        string GetStreamUrl(MaxData loData, string lsKey);

        /// <summary>
        /// Gets the mime-type of the file.
        /// </summary>
        /// <param name="lsName">File name</param>
        /// <returns>Data updated based on sending of message.</returns>
        string GetMimeType(string lsName);
    }
}
