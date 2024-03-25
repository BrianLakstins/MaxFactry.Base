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
// </changelog>
#endregion

namespace MaxFactry.Base.DataLayer.Library
{
    using System.IO;
    using MaxFactry.Core;

    /// <summary>
    /// Provides static methods to manipulate storage of data
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
        /// Writes stream data to storage.
        /// </summary>
        /// <param name="loData">The data index for the object</param>
        /// <param name="lsKey">Data element name to write</param>
        /// <returns>Number of bytes written to storage.</returns>
        public static bool StreamSave(MaxData loData, string lsKey)
        {
            return Provider.StreamSave(loData, lsKey);
        }

        /// <summary>
        /// Opens stream data in storage
        /// </summary>
        /// <param name="loData">The data index for the object</param>
        /// <param name="lsKey">Data element name to write</param>
        /// <returns>Stream that was opened.</returns>
        public static Stream StreamOpen(MaxData loData, string lsKey)
        {
            return Provider.StreamOpen(loData, lsKey);
        }

        /// <summary>
        /// Deletes a stream data in storage
        /// </summary>
        /// <param name="loData">The data index for the object</param>
        /// <param name="lsKey">Data element name to write</param>
        /// <returns>Stream that was opened.</returns>
        public static bool StreamDelete(MaxData loData, string lsKey)
        {
            return Provider.StreamDelete(loData, lsKey);
        }

        /// <summary>
        /// Gets the Url to use to access the stream.
        /// </summary>
        /// <param name="loData">Data used to help determine url.</param>
        /// <param name="lsKey">Key used to help determine key.</param>
        /// <returns>Url to access the stream.</returns>
        public static string GetStreamUrl(MaxData loData, string lsKey)
        {
            return Provider.GetStreamUrl(loData, lsKey);
        }
    }
}
