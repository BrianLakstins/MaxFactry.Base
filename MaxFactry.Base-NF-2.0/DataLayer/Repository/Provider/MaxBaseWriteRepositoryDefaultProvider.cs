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
// <change date="5/21/2025" author="Brian A. Lakstins" description="Remove stream handling.  Return flag based status codes. Always handle a list.">
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
        /// <summary>
        /// Inserts a new list of elements
        /// </summary>
        /// <param name="loDataList">The list of elements</param>
        /// <returns>Flag based status code indicating level of success.</returns>
        public virtual int Insert(MaxDataList loDataList)
        {
            int lnR = MaxDataContextLibrary.Insert(this, loDataList);
            return lnR;
        }

        /// <summary>
        /// Updates a list of elements
        /// </summary>
        /// <param name="loDataList">The list of elements</param>
        /// <returns>Flag based status code indicating level of success.</returns>
		public virtual int Update(MaxDataList loDataList)
        {
            int lnR = MaxDataContextLibrary.Update(this, loDataList);
            return lnR;
        }

        /// <summary>
        /// Deletes a list of elements
        /// </summary>
        /// <param name="loDataList">The list of elements</param>
        /// <returns>Flag based status code indicating level of success.</returns>
		public virtual int Delete(MaxDataList loDataList)
        {
            int lnR = MaxDataContextLibrary.Delete(this, loDataList);
            return lnR;
        }
    }
}
