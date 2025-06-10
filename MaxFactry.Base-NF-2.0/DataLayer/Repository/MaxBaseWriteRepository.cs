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
// <change date="5/21/2025" author="Brian A. Lakstins" description="Remove stream handling.  Return flag based status codes. Always handle a list.">
// <change date="6/10/2025" author="Brian A. Lakstins" description="Fix deleting.">
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
        /// Inserts a new list of elements
        /// </summary>
        /// <param name="loDataList">The list of elements</param>
        /// <returns>Flag based status code indicating level of success.</returns>
        public static int Insert(MaxDataList loDataList)
        {
            int lnR = 0;
            if (loDataList.Count > 0)
            {
                IMaxBaseWriteRepositoryProvider loProvider = (IMaxBaseWriteRepositoryProvider)Instance.GetRepositoryProvider(loDataList[0]);
                lnR = loProvider.Insert(loDataList);
            }
            else
            {
                lnR |= 2;
            }

            return lnR;
        }

        /// <summary>
        /// Inserts a new element
        /// </summary>
        /// <param name="loData">The element data</param>
        /// <returns>true if success and false if any issues</returns>
        public static bool Insert(MaxData loData)
        {
            MaxDataList loDataList = new MaxDataList(loData.DataModel);
            loDataList.Add(loData);
            int lnReturn = Insert(loDataList);
            if (lnReturn == 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// Updates a list of elements
        /// </summary>
        /// <param name="loDataList">The list of elements</param>
        /// <returns>Flag based status code indicating level of success.</returns>
        public static int Update(MaxDataList loDataList)
        {
            int lnR = 0;
            if (loDataList.Count > 0)
            {
                IMaxBaseWriteRepositoryProvider loProvider = (IMaxBaseWriteRepositoryProvider)Instance.GetRepositoryProvider(loDataList[0]);
                lnR = loProvider.Update(loDataList);
            }
            else
            {
                lnR |= 2;
            }

            return lnR;
        }

        /// <summary>
        /// Updates an element
        /// </summary>
        /// <param name="loData">The element data</param>
        /// <returns>true if success and false if any issues</returns>
        public static bool Update(MaxData loData)
        {
            MaxDataList loDataList = new MaxDataList(loData.DataModel);
            loDataList.Add(loData);
            int lnReturn = Update(loDataList);
            if (lnReturn == 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// Deletes a list of elements
        /// </summary>
        /// <param name="loDataList">The list of elements</param>
        /// <returns>Flag based status code indicating level of success.</returns>
        public static int Delete(MaxDataList loDataList)
        {
            int lnR = 0;
            if (loDataList.Count > 0)
            {
                IMaxBaseWriteRepositoryProvider loProvider = (IMaxBaseWriteRepositoryProvider)Instance.GetRepositoryProvider(loDataList[0]);
                lnR = loProvider.Delete(loDataList);
            }
            else
            {
                lnR |= 2;
            }

            return lnR;
        }

        /// <summary>
        /// Deletes an element
        /// </summary>
        /// <param name="loData">The element data</param>
        /// <returns>true if success and false if any issues</returns>
        public static bool Delete(MaxData loData)
        {
            MaxDataList loDataList = new MaxDataList(loData.DataModel);
            loDataList.Add(loData);
            int lnReturn = Delete(loDataList);
            if (lnReturn == 0)
            {
                return true;
            }
            else
            {
                return false;
            }
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