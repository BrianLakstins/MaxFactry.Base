// <copyright file="MaxBaseWriteRepository.cs" company="Lakstins Family, LLC">
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
// <change date="3/22/2024" author="Brian A. Lakstins" description="Initial creation.  Based on MaxStorageWriteRepository.">
// </changelog>
#endregion

namespace MaxFactry.Base.DataLayer
{
    using MaxFactry.Core;

    /// <summary>
    /// Provides static methods to manipulate storage of data
    /// </summary>
    public class MaxBaseWriteRepository : MaxBaseReadRepository
	{
        /// <summary>
        /// Internal storage of single object
        /// </summary>
        private static MaxBaseWriteRepository _oInstance = null;

        /// <summary>
        /// Lock object for multi-threaded access.
        /// </summary>
        private static object _oLock = new object();

        /// <summary>
        /// Gets the single instance of this class.
        /// </summary>
        public static new MaxBaseWriteRepository Instance
        {
            get
            {
                if (null == _oInstance)
                {
                    lock (_oLock)
                    {
                        if (null == _oInstance)
                        {
                            _oInstance = new MaxBaseWriteRepository();
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
            IMaxBaseWriteRepositoryProvider loProvider = (IMaxBaseWriteRepositoryProvider)Instance.GetRepositoryProvider(loData);
            return loProvider.Insert(loData);
        }

		/// <summary>
		/// Updates an element of the specified type
		/// </summary>
		/// <param name="loData">The data index for the object</param>
		/// <returns>true if updated element</returns>
        public static bool Update(MaxData loData)
		{
            IMaxBaseWriteRepositoryProvider loProvider = (IMaxBaseWriteRepositoryProvider)Instance.GetRepositoryProvider(loData);
            return loProvider.Update(loData);
        }

		/// <summary>
		/// Deletes an element of the specified type
		/// </summary>
		/// <param name="loData">The data index for the object</param>
		/// <returns>true if deleted.  False if cannot be deleted.</returns>
        public static bool Delete(MaxData loData)
		{
            IMaxBaseWriteRepositoryProvider loProvider = (IMaxBaseWriteRepositoryProvider)Instance.GetRepositoryProvider(loData);
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
            IMaxBaseWriteRepositoryProvider loProvider = (IMaxBaseWriteRepositoryProvider)Instance.GetRepositoryProvider(loData);
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
            IMaxBaseWriteRepositoryProvider loProvider = (IMaxBaseWriteRepositoryProvider)Instance.GetRepositoryProvider(loData);
            bool lbR = loProvider.StreamDelete(loData, lsKey);
            return lbR;
        }

        public override IMaxBaseReadRepositoryProvider GetRepositoryProvider(MaxData loData)
        {
            IMaxProvider loProvider = base.GetRepositoryProvider(loData);
            IMaxBaseWriteRepositoryProvider loR = loProvider as IMaxBaseWriteRepositoryProvider;
            if (null == loR)
            {
                throw new MaxException("Error getting provider for interface IMaxBaseWriteRepositoryProvider");
            }

            return loR;
        }
	}
}