// <copyright file="IMaxStorageWriteRepositoryProvider.cs" company="Lakstins Family, LLC">
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
// <change date="6/25/2014" author="Brian A. Lakstins" description="Based on IMaxAppRepositoryProvider">
// <change date="8/21/2014" author="Brian A. Lakstins" description="Add stream management.">
// <change date="11/11/2014" author="Brian A. Lakstins" description="Added general Selecting by a single property that does not include records marked as deleted.">
// <change date="12/2/2014" author="Brian A. Lakstins" description="Add laFields.">
// </changelog>
#endregion

namespace MaxFactry.Base.DataLayer
{
	using System;
    using System.IO;

	/// <summary>
	/// Provides methods to manipulate storage of data
	/// </summary>
    public interface IMaxStorageWriteRepositoryProvider : IMaxStorageReadRepositoryProvider
	{
		/// <summary>
		/// Inserts a new data element
		/// </summary>
		/// <param name="loData">The data for the element</param>
		/// <returns>true if inserted</returns>
		bool Insert(MaxData loData);

		/// <summary>
		/// Updates an existing data element
		/// </summary>
		/// <param name="loData">the data for the element</param>
		/// <returns>true if updated</returns>
        bool Update(MaxData loData);

		/// <summary>
		/// Deletes an existing data element
		/// </summary>
		/// <param name="loData">the data for the element</param>
		/// <returns>true if deleted</returns>
		bool Delete(MaxData loData);

        /// <summary>
        /// Writes stream data to storage.
        /// </summary>
        /// <param name="loData">The data index for the object</param>
        /// <param name="lsKey">Data element name to write</param>
        /// <returns>Number of bytes written to storage.</returns>
        bool StreamSave(MaxData loData, string lsKey);

        /// <summary>
        /// Removes stream from storage.
        /// </summary>
        /// <param name="loData">The data index for the object</param>
        /// <param name="lsKey">Data element name to remove</param>
        /// <returns>true if successful.</returns>
        bool StreamDelete(MaxData loData, string lsKey);
	}
}
