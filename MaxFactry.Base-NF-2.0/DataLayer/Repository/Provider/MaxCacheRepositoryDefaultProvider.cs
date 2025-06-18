// <copyright file="MaxCacheRepositoryDefaultProvider.cs" company="Lakstins Family, LLC">
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
// <change date="12/18/2014" author="Brian A. Lakstins" description="Removed unused Type parameter.">
// <change date="6/8/2015" author="Brian A. Lakstins" description="Updated to no longer separate data by storage key.  That will need to be done in the key provided.">
// <change date="4/20/2016" author="Brian A. Lakstins" description="Clean up code.  Fix problem removing with wildcard.">
// <change date="1/16/2021" author="Brian A. Lakstins" description="Update storage and retrieval of MaxData and MaxDataList so data is serialized so cannot be changed when edited">
// <change date="6/10/2025" author="Brian A. Lakstins" description="Handle null and blank keys.  Lock threads. Add expire date. Remove specific MaxData and MaxDataList handling">
// <change date="6/18/2025" author="Brian A. Lakstins" description="Clear expired content in a background thread">
// </changelog>
#endregion

namespace MaxFactry.Base.DataLayer.Provider
{
    using MaxFactry.Core;
	using System;
    using System.Runtime.CompilerServices;
    using System.Security.Policy;
    using System.Threading;

    /// <summary>
    /// Provides static methods to manipulate storage of data
    /// Simple in-memory provider to cache objects for MaxCacheRepository.
    /// </summary>
    public class MaxCacheRepositoryDefaultProvider : MaxProvider, IMaxCacheRepositoryProvider
	{
        /// <summary>
        /// Index to store cached objects.
        /// </summary>
        private static MaxIndex _oCacheIndex = new MaxIndex();

        /// <summary>
        /// Index to store expiration times for cached objects.
        /// </summary>
        private static MaxIndex _oExpireIndex = new MaxIndex();

        /// <summary>
        /// Lock to prevent multiple thread access to Index
        /// </summary>
        private static object _oLock = new object();

        private int _nClearExpiredInterval = 5; // Minutes

        public override void Initialize(string lsName, MaxIndex loConfig)
        {
            base.Initialize(lsName, loConfig);
            string lsClearExpiredInterval = this.GetConfigValue(loConfig, "ClearExpiredInterval") as string;
            if (null != lsClearExpiredInterval && lsClearExpiredInterval.Length > 0)
            {
                this._nClearExpiredInterval = MaxConvertLibrary.ConvertToInt(typeof(object), lsClearExpiredInterval);
            }

            // Start thread to clear expired items
            Thread loThread = new Thread(new ThreadStart(this.ClearExpired));
            loThread.IsBackground = true;
            loThread.Start();
        }

        private void ClearExpired()
        {
            lock (_oLock)
            {
                DateTime ldNow = DateTime.UtcNow;
                {                   
                    string[] laKeys = _oExpireIndex.GetSortedKeyList();
                    foreach (string lsKey in laKeys)
                    {
                        if (_oExpireIndex.Contains(lsKey) && (DateTime)_oExpireIndex[lsKey] < ldNow)
                        {
                            _oCacheIndex.Remove(lsKey);
                            _oExpireIndex.Remove(lsKey);
                        }
                    }
                }
            }

            System.Threading.Thread.Sleep(this._nClearExpiredInterval * 60 * 1000); // Sleep for 5 minutes to allow next thread to run
            this.ClearExpired(); // Call again
        }

        /// <summary>
        /// Sets the object in the cache.
        /// </summary>
        /// <param name="lsKey">Key to use to retrieve the object.</param>
        /// <param name="loValue">Object to save in cache.</param>
        /// <param name="ldExpire">Date and time when the object should expire.</param>
        public virtual void Set(string lsKey, object loValue, DateTime ldExpire)
        {
            lock (_oLock)
            {
                if (lsKey.Length > 0)
                {
                    _oExpireIndex.Add(lsKey, ldExpire);
                    _oCacheIndex.Add(lsKey, loValue);
                }
            }
        }

        /// <summary>
        /// Retrieves an object from the cache.
        /// </summary>
        /// <param name="lsKey">Key used when object was set.</param>
        /// <param name="loType">Type of object expected.</param>
        /// <returns>Object that was saved in cache, or null.</returns>
        public virtual object Get(string lsKey, Type loType)
        {
            object loR = null;
            lock (_oLock)
            {
                if (null != lsKey && lsKey.Length > 0)
                {
                    if (_oExpireIndex.Contains(lsKey) && (DateTime)_oExpireIndex[lsKey] < DateTime.UtcNow)
                    {
                        this.Remove(lsKey);
                    }
                    else if (null != _oCacheIndex[lsKey])
                    {
                        loR = _oCacheIndex[lsKey];
                    }
                }
            }

            return loR;
        }

        /// <summary>
        /// Gets a list of all internal keys.
        /// </summary>
        /// <returns>List of current keys used to set items.</returns>
        public virtual string[] GetKeyList()
        {
            string[] laR = _oCacheIndex.GetSortedKeyList();
            return laR;
        }

        /// <summary>
        /// Removes an item from the cache.
        /// </summary>
        /// <param name="lsKey">Key used to store item.</param>
        public virtual void Remove(string lsKey)
        {
            lock (_oLock)
            {
                if (lsKey.Length > 0)
                {
                    if (lsKey.Substring(lsKey.Length - 1, 1) == "*")
                    {
                        string[] laKeyList = this.GetKeyList();
                        string lsKeyStart = lsKey.Substring(0, lsKey.Length - 1);
                        foreach (string lsKeyFromList in laKeyList)
                        {
                            if (lsKeyFromList.Length >= lsKeyStart.Length)
                            {
                                if (lsKeyFromList.Substring(0, lsKeyStart.Length) == lsKeyStart)
                                {
                                    _oCacheIndex.Remove(lsKeyFromList);
                                    _oExpireIndex.Remove(lsKeyFromList);
                                }
                            }
                        }
                    }
                    else
                    {
                        _oCacheIndex.Remove(lsKey);
                        _oExpireIndex.Remove(lsKey);
                    }
                }
            }
        }
    }
}