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
// <change date="6/10/2025" author="Brian A. Lakstins" description="Handle null and blank keys.  Lock threads.  ">
// </changelog>
#endregion

namespace MaxFactry.Base.DataLayer.Provider
{
	using System;
    using MaxFactry.Core;

    /// <summary>
    /// Provides static methods to manipulate storage of data
    /// Simple in-memory provider to cache objects for MaxCacheRepository.
    /// </summary>
    public class MaxCacheRepositoryDefaultProvider : MaxProvider, IMaxCacheRepositoryProvider
	{
        /// <summary>
        /// Index to store cached objects.
        /// </summary>
        private static MaxIndex _oIndex = new MaxIndex();

        /// <summary>
        /// Lock to prevent multiple thread access to Index
        /// </summary>
        private static object _oLock = new object();

        /// <summary>
        /// Sets the object in the cache.
        /// </summary>
        /// <param name="lsKey">Key to use to retrieve the object.</param>
        /// <param name="loValue">Object to save in cache.</param>
        public virtual void Set(string lsKey, object loValue)
        {
            lock (_oLock)
            {
                if (null != loValue && lsKey.Length > 0)
                {
                    if (loValue is MaxData)
                    {
                        _oIndex.Add(lsKey, ((MaxData)loValue).ToString());
                    }
                    else if (loValue is MaxDataList && lsKey.Contains("/"))
                    {
                        MaxIndex loIdIndex = null;
                        MaxDataList loDataList = loValue as MaxDataList;
                        if (loDataList.Count > 0)
                        {
                            loIdIndex = new MaxIndex();
                            string[] laKey = lsKey.Split(new char[] { '/' });
                            if (laKey.Length >= 2)
                            {
                                string lsKeyBase = laKey[0] + "/" + laKey[1];
                                if (laKey[laKey.Length - 1].StartsWith("FieldList="))
                                {
                                    lsKeyBase += "/" + laKey[laKey.Length - 1];
                                }

                                for (int lnD = 0; lnD < loDataList.Count && null != loIdIndex; lnD++)
                                {
                                    MaxData loData = loDataList[lnD];
                                    if (loData.DataModel is MaxBaseIdDataModel)
                                    {
                                        MaxBaseIdDataModel loDataModel = loData.DataModel as MaxBaseIdDataModel;
                                        string lsId = MaxConvertLibrary.ConvertToString(typeof(object), loData.Get(loDataModel.Id));
                                        if (!string.IsNullOrEmpty(lsId))
                                        {
                                            this.Set(lsKeyBase + "/LoadById/" + lsId, loData);
                                            if (null != loIdIndex)
                                            {
                                                loIdIndex.Add(lsId);
                                            }
                                        }
                                        else
                                        {
                                            loIdIndex = null;
                                        }
                                    }
                                    else
                                    {
                                        loIdIndex = null;
                                    }
                                }
                            }
                        }

                        if (null != loIdIndex)
                        {
                            string lsValue = MaxConvertLibrary.SerializeObjectToString(loIdIndex);
                            _oIndex.Add(lsKey, lsValue);
                        }
                        else
                        {
                            _oIndex.Add(lsKey, loValue);
                        }
                    }
                    else
                    {
                        _oIndex.Add(lsKey, loValue);
                    }
                }
                else
                {
                    _oIndex.Add(lsKey, null);
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
                if (lsKey.Length > 0 && null != _oIndex[lsKey])
                {
                    if (loType == typeof(MaxData))
                    {
                        MaxData loData = new MaxData(_oIndex[lsKey] as string);
                        loR = loData;
                    }
                    else if (loType == typeof(MaxDataList) && lsKey.Contains("/"))
                    {
                        string lsIdIndex = _oIndex[lsKey] as string;
                        if (!string.IsNullOrEmpty(lsIdIndex))
                        {
                            MaxIndex loIdIndex = MaxConvertLibrary.DeserializeObject(lsIdIndex, typeof(MaxIndex)) as MaxIndex;
                            if (null != loIdIndex)
                            {
                                string[] laKey = lsKey.Split(new char[] { '/' });
                                if (laKey.Length >= 2)
                                {
                                    string lsKeyBase = laKey[0] + "/" + laKey[1];
                                    if (laKey[laKey.Length - 1].StartsWith("FieldList="))
                                    {
                                        lsKeyBase += "/" + laKey[laKey.Length - 1];
                                    }

                                    string[] laIdKey = loIdIndex.GetSortedKeyList();
                                    MaxDataList loDataList = null;
                                    if (laIdKey.Length > 0)
                                    {
                                        loDataList = new MaxDataList();
                                        for (int lnK = 0; lnK < laIdKey.Length && null != loDataList; lnK++)
                                        {
                                            string lsId = loIdIndex[laIdKey[lnK]] as string;
                                            if (!string.IsNullOrEmpty(lsId))
                                            {
                                                MaxData loData = this.Get(lsKeyBase + "/LoadById/" + lsId, typeof(MaxData)) as MaxData;
                                                if (null != loData)
                                                {
                                                    if (null == loDataList.DataModel)
                                                    {
                                                        loDataList = new MaxDataList(loData.DataModel);
                                                    }

                                                    loDataList.Add(loData);
                                                }
                                                else
                                                {
                                                    loDataList = null;
                                                }
                                            }
                                        }
                                    }

                                    loR = loDataList;
                                }
                            }
                        }

                        if (null == loR)
                        {
                            loR = _oIndex[lsKey];
                        }
                    }
                    else
                    {
                        loR = _oIndex[lsKey];
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
            string[] laR = _oIndex.GetSortedKeyList();
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
                                    _oIndex.Remove(lsKeyFromList);
                                }
                            }
                        }
                    }
                    else
                    {
                        _oIndex.Remove(lsKey);
                    }
                }
            }
        }
	}
}