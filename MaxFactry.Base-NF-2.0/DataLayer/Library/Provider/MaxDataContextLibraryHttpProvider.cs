// <copyright file="MaxDataContextLibraryDefaultProvider.cs" company="Lakstins Family, LLC">
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
// <change date="3/25/2024" author="Brian A. Lakstins" description="Initial creation">
// </changelog>
#endregion

namespace MaxFactry.Base.DataLayer.Library.Provider
{
    using System;
    using System.IO;
    using MaxFactry.Core;
    using MaxFactry.Base.DataLayer.Library;

    /// <summary>
    /// Provides static methods to manipulate storage of data
    /// </summary>
    public class MaxDataContextLibraryHttpProvider : MaxProvider, IMaxDataContextLibraryProvider
    {
        /// <summary>
        /// Initializes the provider
        /// </summary>
        /// <param name="lsName">Name of the provider</param>
        /// <param name="loConfig">Configuration information</param>
        public override void Initialize(string lsName, MaxIndex loConfig)
        {
            base.Initialize(lsName, loConfig);
        }

        /// <summary>
        /// Selects all data from the data storage name for the specified type.
        /// </summary>
        /// <param name="loData">Used for return data</param>
        /// <param name="laDataNameList">list of fields to return from select</param>
        /// <returns>List of data elements with a base data model.</returns>
        public virtual MaxDataList SelectAll(MaxData loData, params string[] laDataNameList)
        {
            MaxIndex loIndex = MaxHttpLibrary.GetResponse(loData, null, 0, 0, string.Empty, laDataNameList);
            MaxDataList loR = loData.DataModel.MapIndex(loIndex);

            return loR;
        }

        /// <summary>
        /// Selects data from the database.
        /// </summary>
        /// <param name="loData">Element with data used in the filter.</param>
        /// <param name="loDataQuery">Query information to filter results.</param>
        /// <param name="lnPageIndex">Page to return.</param>
        /// <param name="lnPageSize">Items per page.</param>
        /// <param name="lsOrderBy">Sort information.</param>
        /// <param name="lnTotal">Total items found.</param>
        /// <param name="laDataNameList">list of fields to return from select.</param>
        /// <returns>List of data from select.</returns>
        public virtual MaxDataList Select(MaxData loData, MaxDataQuery loDataQuery, int lnPageIndex, int lnPageSize, string lsOrderBy, out int lnTotal, params string[] laDataNameList)
        {
            MaxIndex loIndex = MaxHttpLibrary.GetResponse(loData, loDataQuery, lnPageIndex, lnPageSize, lsOrderBy, laDataNameList);
            MaxDataList loR = loData.DataModel.MapIndex(loIndex);
            lnTotal = loR.Count;
            return loR;
        }

        /// <summary>
        /// Gets the number of records that match the filter.
        /// </summary>
        /// <param name="loData">Element with data used in the filter.</param>
        /// <param name="loDataQuery">Query information to filter results.</param>
        /// <returns>number of records that match.</returns>
        public virtual int SelectCount(MaxData loData, MaxDataQuery loDataQuery)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Inserts a new data element.
        /// </summary>
        /// <param name="loDataList">The list of data objects to insert.</param>
        /// <returns>The data that was inserted.</returns>
        public virtual int Insert(MaxDataList loDataList)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Updates an existing data element.
        /// </summary>
        /// <param name="loDataList">The list of data objects to insert.</param>
        /// <returns>The data that was updated.</returns>
        public virtual int Update(MaxDataList loDataList)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Deletes an existing data element.
        /// </summary>
        /// <param name="loDataList">The list of data objects to insert.</param>
        /// <returns>true if deleted.</returns>
        public virtual int Delete(MaxDataList loDataList)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Writes stream data to storage.
        /// </summary>
        /// <param name="loData">The data index for the object</param>
        /// <param name="lsKey">Data element name to write</param>
        /// <returns>Number of bytes written to storage.</returns>
        public virtual bool StreamSave(MaxData loData, string lsKey)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Opens stream data in storage
        /// </summary>
        /// <param name="loData">The data index for the object</param>
        /// <param name="lsKey">Data element name to write</param>
        /// <returns>Stream that was opened.</returns>
        public virtual Stream StreamOpen(MaxData loData, string lsKey)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Deletes stream data in storage
        /// </summary>
        /// <param name="loData">The data index for the object</param>
        /// <param name="lsKey">Data element name to write</param>
        /// <returns>Stream that was opened.</returns>
        public virtual bool StreamDelete(MaxData loData, string lsKey)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Gets the Url of a saved stream.
        /// </summary>
        /// <param name="loData">The data index for the object</param>
        /// <param name="lsKey">Data element name</param>
        /// <returns>Url of stream if one can be provided.</returns>
        public virtual string GetStreamUrl(MaxData loData, string lsKey)
        {
            throw new NotImplementedException();
        }
    }
}
