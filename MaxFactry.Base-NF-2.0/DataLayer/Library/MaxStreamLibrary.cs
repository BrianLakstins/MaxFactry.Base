// <copyright file="MaxDataContextStreamLibrary.cs" company="Lakstins Family, LLC">
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
// <change date="9/20/2023" author="Brian A. Lakstins" description="Initial creation">
// <change date="3/20/2024" author="Brian A. Lakstins" description="Happy birthday to my mom.  Sara Jean Lakstins (Cartwright) - 3/20/1944 to 3/14/2019.">
// <change date="3/24/2024" author="Brian A. Lakstins" description="Rename to MaxSteamLibrary to not indicate some dependency on MaxDataContextProvider">
// <change date="5/21/2025" author="Brian A. Lakstins" description="Update to handle one field of one element at a time and send flag based return codes">
// </changelog>
#endregion

namespace MaxFactry.Base.DataLayer.Library
{
    using System.IO;
    using MaxFactry.Core;

    /// <summary>
    /// Provides static methods to manipulate storage of streams
    /// </summary>
    public class MaxStreamLibrary : MaxSingleFactory
    {
        /// <summary>
        /// Internal storage of single object
        /// </summary>
        private static MaxStreamLibrary _oInstance = null;

        /// <summary>
        /// Lock object for multi-threaded access.
        /// </summary>
        private static object _oLock = new object();

        /// <summary>
        /// Gets the Provider used for most methods
        /// </summary>
        public static IMaxStreamLibraryProvider Provider
        {
            get
            {
                if (null == Instance.BaseProvider)
                {
                    Instance.SetProvider(typeof(Provider.MaxStreamLibraryDefaultProvider));
                }

                return (IMaxStreamLibraryProvider)Instance.BaseProvider;
            }
        }

        /// <summary>
        /// Gets the single instance of this class.
        /// </summary>
        public static MaxStreamLibrary Instance
        {
            get
            {
                if (null == _oInstance)
                {
                    lock (_oLock)
                    {
                        if (null == _oInstance)
                        {
                            _oInstance = new MaxStreamLibrary();
                        }
                    }
                }

                return _oInstance;
            }
        }

        /// <summary>
        /// Saves a single field in a data element to storage.
        /// </summary>
        /// <param name="loData">The data element</param>
        /// <param name="lsDataName">Name of data element to save</param>
        /// <returns>Flag based status code indicating level of success.</returns>
        public static int StreamSave(MaxData loData, string lsDataName)
        {
            return Provider.StreamSave(loData, lsDataName);
        }

        /// <summary>
        /// Opens stream data in storage
        /// </summary>
        /// <param name="loData">The data element</param>
        /// <param name="lsDataName">Name of data element to save</param>
        /// <returns>Stream that was opened.</returns>
        public static Stream StreamOpen(MaxData loData, string lsDataName)
        {
            return Provider.StreamOpen(loData, lsDataName);
        }

        /// <summary>
        /// Deletes a single field in a data element from storage
        /// </summary>
        /// <param name="loData">The data element</param>
        /// <param name="lsDataName">Name of data element to delete</param>
        /// <returns>Flag based status code indicating level of success.</returns>
        public static int StreamDelete(MaxData loData, string lsDataName)
        {
            return Provider.StreamDelete(loData, lsDataName);
        }

        /// <summary>
        /// Gets the Url of a saved stream.
        /// </summary>
        /// <param name="loData">The data element</param>
        /// <param name="lsDataName">Name of data element to delete</param>
        /// <returns>Url of stream if one can be provided.</returns>
        public static string GetStreamUrl(MaxData loData, string lsDataName)
        {
            return Provider.GetStreamUrl(loData, lsDataName);
        }
    }
}

