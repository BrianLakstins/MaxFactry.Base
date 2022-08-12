// <copyright file="MaxCacheRepository.cs" company="Lakstins Family, LLC">
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
// <change date="9/15/2014" author="Brian A. Lakstins" description="Based on MaxCRUDRepository">
// <change date="12/18/2014" author="Brian A. Lakstins" description="Updated Provider and DataModel access pattern.">
// <change date="3/26/2015" author="Brian A. Lakstins" description="Restructured.">
// </changelog>
#endregion

namespace MaxFactry.Base.DataLayer
{
	using System;
    using MaxFactry.Core;

    /// <summary>
    /// Provides static methods to manipulate storage of temporarily cached data
    /// </summary>
    public class MaxCacheRepository : MaxByMethodFactory
	{
        /// <summary>
        /// Internal storage of single object
        /// </summary>
        private static MaxCacheRepository _oInstance = null;

        /// <summary>
        /// Lock object for multi-threaded access.
        /// </summary>
        private static object _oLock = new object();

        /// <summary>
        /// Gets the single instance of this class.
        /// </summary>
        public static MaxCacheRepository Instance
        {
            get
            {
                if (null == _oInstance)
                {
                    lock (_oLock)
                    {
                        if (null == _oInstance)
                        {
                            _oInstance = new MaxCacheRepository();
                            _oInstance.DefaultProviderType = typeof(Provider.MaxCacheRepositoryDefaultProvider);
                        }
                    }
                }

                return _oInstance;
            }
        }

        /// <summary>
        /// Sets the object in the cache.
        /// </summary>
        /// <param name="loType">Type to use to get provider.</param>
        /// <param name="lsKey">Key to use to retrieve the object.</param>
        /// <param name="loValue">Object to save in cache.</param>
        public static void Set(Type loType, string lsKey, object loValue)
        {
            IMaxCacheRepositoryProvider loProvider = Instance.GetCacheRepositoryProvider(loType);
            loProvider.Set(lsKey, loValue);
        }

        /// <summary>
        /// Retrieves an object from the cache.
        /// </summary>
        /// <param name="loType">Type to use to get provider.</param>
        /// <param name="lsKey">Key used when object was set.</param>
        /// <param name="loTypeExpected">Type expected to be returned.</param>
        /// <returns>Object that was saved in cache, or null.</returns>
        public static object Get(Type loType, string lsKey, Type loTypeExpected)
        {
            IMaxCacheRepositoryProvider loProvider = Instance.GetCacheRepositoryProvider(loType);
            object loR = loProvider.Get(lsKey, loTypeExpected);
            return loR;
        }

        /// <summary>
        /// Gets a list of all internal keys.
        /// </summary>
        /// <param name="loType">Type of object that was stored with the key.</param>
        /// <returns>List of current keys used to set items.</returns>
        public static string[] GetKeyList(Type loType)
        {
            IMaxCacheRepositoryProvider loProvider = Instance.GetCacheRepositoryProvider(loType);
            string[] laR = loProvider.GetKeyList();
            return laR;
        }

        /// <summary>
        /// Removes an item from the cache.
        /// </summary>
        /// <param name="loType">Type of item stored.</param>
        /// <param name="lsKey">Key used to store item.</param>
        public static void Remove(Type loType, string lsKey)
        {
            IMaxCacheRepositoryProvider loProvider = Instance.GetCacheRepositoryProvider(loType);
            loProvider.Remove(lsKey);
        }

        /// <summary>
        /// Gets the provider based on the type of object being stored
        /// </summary>
        /// <param name="loType">Type used to determine provider to use.</param>
        /// <returns>A provider for storing an object of that specified type</returns>
        public virtual IMaxCacheRepositoryProvider GetCacheRepositoryProvider(Type loType)
        {
            IMaxProvider loProvider = this.GetProvider(loType);
            IMaxCacheRepositoryProvider loRepositoryProvider = loProvider as IMaxCacheRepositoryProvider;
            if (null == loRepositoryProvider)
            {
                MaxCacheRepository.HandleInterfaceNotImplemented(loType, loProvider, "MaxCacheRepository", "IMaxCacheRepositoryProvider");
            }

            return loRepositoryProvider;
        }
	}
}