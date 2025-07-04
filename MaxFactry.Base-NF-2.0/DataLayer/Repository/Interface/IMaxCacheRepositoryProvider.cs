﻿// <copyright file="IMaxCacheRepositoryProvider.cs" company="Lakstins Family, LLC">
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
// <change date="9/15/2014" author="Brian A. Lakstins" description="Initial Release">
// <change date="12/18/2014" author="Brian A. Lakstins" description="Removed unused Type parameters.">
// <change date="6/10/2025" author="Brian A. Lakstins" description="Add expire date.">
// </changelog>
#endregion

namespace MaxFactry.Base.DataLayer
{
	using System;
    using MaxFactry.Core;

    /// <summary>
    /// Interface for MaxCacheRepository.
    /// </summary>
    public interface IMaxCacheRepositoryProvider : IMaxProvider
	{
        /// <summary>
        /// Sets the object in the cache.
        /// </summary>
        /// <param name="lsKey">Key to use to retrieve the object.</param>
        /// <param name="loValue">Object to save in cache.</param>
        /// <param name="ldExpire">Date and time when the object should expire.</param>
        void Set(string lsKey, object loValue, DateTime ldExpire);

        /// <summary>
        /// Retrieves an object from the cache.
        /// </summary>
        /// <param name="lsKey">Key used when object was set.</param>
        /// <param name="loType">Type of object expected.</param>
        /// <returns>Object that was saved in cache, or null.</returns>
        object Get(string lsKey, Type loType);

        /// <summary>
        /// Gets a list of all internal keys.
        /// </summary>
        /// <returns>List of current keys used to set items.</returns>
        string[] GetKeyList();

        /// <summary>
        /// Removes an item from the cache.
        /// </summary>
        /// <param name="lsKey">Key used to store item.</param>
        void Remove(string lsKey);
	}
}
