// <copyright file="MaxStorageWriteRepository.cs" company="Lakstins Family, LLC">
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
// <change date="3/26/2015" author="Brian A. Lakstins" description="Restructured.">
// <change date="6/2/2015" author="Brian A. Lakstins" description="Updated to set StorageKey so that Providers don't have to.">
// <change date="12/21/2016" author="Brian A. Lakstins" description="Updated to work with meta copy of MaxData">
// </changelog>
#endregion

namespace MaxFactry.Base.DataLayer
{
	using System;
    using System.IO;
    using MaxFactry.Core;

    /// <summary>
    /// Provides static methods to manipulate storage of data
    /// </summary>
    public class MaxStorageWriteRepository : MaxStorageReadRepository
	{
        /// <summary>
        /// Internal storage of single object
        /// </summary>
        private static MaxStorageWriteRepository _oInstance = null;

        /// <summary>
        /// Lock object for multi-threaded access.
        /// </summary>
        private static object _oLock = new object();

        /// <summary>
        /// Gets the single instance of this class.
        /// </summary>
        public static new MaxStorageWriteRepository Instance
        {
            get
            {
                if (null == _oInstance)
                {
                    lock (_oLock)
                    {
                        if (null == _oInstance)
                        {
                            _oInstance = new MaxStorageWriteRepository();
                        }
                    }
                }

                return _oInstance;
            }
        }

		/// <summary>
		/// Inserts a new element of the specified type
		/// </summary>
		/// <param name="loData">The data index for the object</param>
		/// <returns>true if inserted</returns>
        public static bool Insert(MaxData loData)
		{
            loData.SetChanged();
            IMaxStorageWriteRepositoryProvider loProvider = Instance.GetStorageWriteRepositoryProvider(loData);
            return loProvider.Insert(loData);
        }

		/// <summary>
		/// Updates an element of the specified type
		/// </summary>
		/// <param name="loData">The data index for the object</param>
		/// <returns>true if updated element</returns>
        public static bool Update(MaxData loData)
		{
            IMaxStorageWriteRepositoryProvider loProvider = Instance.GetStorageWriteRepositoryProvider(loData);
            return loProvider.Update(loData);
        }

		/// <summary>
		/// Deletes an element of the specified type
		/// </summary>
		/// <param name="loData">The data index for the object</param>
		/// <returns>true if deleted.  False if cannot be deleted.</returns>
        public static bool Delete(MaxData loData)
		{
            IMaxStorageWriteRepositoryProvider loProvider = Instance.GetStorageWriteRepositoryProvider(loData);
            bool lbR = loProvider.Delete(loData);
            return lbR;
        }

        /// <summary>
        /// Saves stream data to storage.
        /// </summary>
        /// <param name="loData">The data index for the object</param>
        /// <param name="lsKey">Data element name to write</param>
        /// <returns>Number of bytes written to storage.</returns>
        public static bool StreamSave(MaxData loData, string lsKey)
        {
            IMaxStorageWriteRepositoryProvider loProvider = Instance.GetStorageWriteRepositoryProvider(loData);
            bool lbR = loProvider.StreamSave(loData, lsKey);
            return lbR;
        }

        /// <summary>
        /// Removes stream from storage.
        /// </summary>
        /// <param name="loData">The data index for the object</param>
        /// <param name="lsKey">Data element name to remove</param>
        /// <returns>true if successful.</returns>
        public static bool StreamDelete(MaxData loData, string lsKey)
        {
            IMaxStorageWriteRepositoryProvider loProvider = Instance.GetStorageWriteRepositoryProvider(loData);
            bool lbR = loProvider.StreamDelete(loData, lsKey);
            return lbR;
        }

        /// <summary>
        /// Gets the provider based on the type of object being stored
        /// </summary>
        /// <param name="loData">Data used to determine provider to use.</param>
        /// <returns>A provider for storing an object of that specified type</returns>
        public virtual IMaxStorageWriteRepositoryProvider GetStorageWriteRepositoryProvider(MaxData loData)
        {
            IMaxProvider loProvider = GetStorageReadRepositoryProvider(loData);
            IMaxStorageWriteRepositoryProvider loR = loProvider as IMaxStorageWriteRepositoryProvider;
            if (null == loR)
            {
                throw new MaxException("Error casting [" + loR.GetType() + "] for Provider");
            }

            return loR;
        }
	}
}