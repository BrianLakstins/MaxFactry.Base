// <copyright file="MaxBaseWriteRepositoryDefaultProvider.cs" company="Lakstins Family, LLC">
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
// <change date="3/22/2024" author="Brian A. Lakstins" description="Initial creation.  Based on MaxStorageWriteRepositoryDefaultProvider.">
// <change date="3/24/2024" author="Brian A. Lakstins" description="Updated for changes namespaces">
// <change date="3/25/2024" author="Brian A. Lakstins" description="Use DataContextLibrary">
// </changelog>
#endregion

namespace MaxFactry.Base.DataLayer.Provider
{
	using MaxFactry.Core;
    using MaxFactry.Base.DataLayer.Library;

    /// <summary>
    /// Provides base for creating Providers for Repositories that use a subclass of MaxDataModel for storage.
    /// </summary>
    public class MaxBaseWriteRepositoryDefaultProvider : MaxBaseReadRepositoryDefaultProvider, IMaxBaseWriteRepositoryProvider
	{
        private static object _oLock = new object();

        /// <summary>
        /// Inserts a new data element.
        /// </summary>
        /// <param name="loData">The data for the element.</param>
        /// <returns>true if inserted.</returns>
        public virtual bool Insert(MaxData loData)
        {
            MaxDataList loDataList = new MaxDataList(loData.DataModel);
            loDataList.Add(loData);
            return MaxDataContextLibrary.Insert(this, loDataList);
        }

		/// <summary>
		/// Updates an existing data element.
		/// </summary>
		/// <param name="loData">the data for the element.</param>
		/// <returns>true if updated.</returns>
        public virtual bool Update(MaxData loData)
        {
            MaxDataList loDataList = new MaxDataList(loData.DataModel);
            loDataList.Add(loData);
            return MaxDataContextLibrary.Update(this, loDataList);
        }

		/// <summary>
		/// Deletes an existing data element.
		/// </summary>
		/// <param name="loData">the data for the element.</param>
		/// <returns>true if deleted.</returns>
        public virtual bool Delete(MaxData loData)
        {
            MaxDataList loDataList = new MaxDataList(loData.DataModel);
            loDataList.Add(loData);
            return MaxDataContextLibrary.Delete(this, loDataList);
        }

        /// <summary>
        /// Writes stream data to storage.
        /// </summary>
        /// <param name="loData">The data index for the object</param>
        /// <param name="lsKey">Data element name to write</param>
        /// <returns>Number of bytes written to storage.</returns>
        public virtual bool StreamSave(MaxData loData, string lsKey)
        {
            return MaxDataContextLibrary.StreamSave(this, loData, lsKey);
        }

        /// <summary>
        /// Removes stream from storage.
        /// </summary>
        /// <param name="loData">The data index for the object</param>
        /// <param name="lsKey">Data element name to remove</param>
        /// <returns>true if successful.</returns>
        public virtual bool StreamDelete(MaxData loData, string lsKey)
        {
            return MaxDataContextLibrary.StreamDelete(this, loData, lsKey);
        }
    }
}
