// <copyright file="MaxHttpLibrary.cs" company="Lakstins Family, LLC">
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
// <change date="3/24/2024" author="Brian A. Lakstins" description="Initial creation">
// <change date="3/25/2024" author="Brian A. Lakstins" description="Add method for getting remote data.  Add method to get token. Add method to get response with just token.">
// <change date="3/30/2024" author="Brian A. Lakstins" description="Update method signatures to make them easier to use.">
// </changelog>
#endregion

namespace MaxFactry.Base.DataLayer.Library
{
    using System;
    using System.IO;
    using MaxFactry.Core;

    /// <summary>
    /// Provides static methods to manipulate storage of data
    /// </summary>
    public class MaxHttpLibrary : MaxSingleFactory
    {
        /// <summary>
        /// Internal storage of single object
        /// </summary>
        private static MaxHttpLibrary _oInstance = null;

        /// <summary>
        /// Lock object for multi-threaded access.
        /// </summary>
        private static object _oLock = new object();

        /// <summary>
        /// Gets the Provider used for most methods
        /// </summary>
        public static IMaxHttpLibraryProvider Provider
        {
            get
            {
                if (null == Instance.BaseProvider)
                {
                    Instance.SetProvider(typeof(Provider.MaxHttpLibraryDefaultProvider));
                }

                return (IMaxHttpLibraryProvider)Instance.BaseProvider;
            }
        }

        /// <summary>
        /// Gets the single instance of this class.
        /// </summary>
        public static MaxHttpLibrary Instance
        {
            get
            {
                if (null == _oInstance)
                {
                    lock (_oLock)
                    {
                        if (null == _oInstance)
                        {
                            _oInstance = new MaxHttpLibrary();
                        }
                    }
                }

                return _oInstance;
            }
        }

        /// <summary>
        /// Uses client_id and client_secret as basic authentication and includes grant_type and scope in body as formurlencoded data
        /// </summary>
        /// <param name="loTokenUrl"></param>
        /// <param name="lsClientId"></param>
        /// <param name="lsClientSecret"></param>
        /// <param name="lsScope"></param>
        /// <returns></returns>
        public static string GetAccessToken(Uri loTokenUrl, string lsClientId, string lsClientSecret, string lsScope)
        {
            return Provider.GetAccessToken(loTokenUrl, lsClientId, lsClientSecret, lsScope);
        }

        public static object GetContent(string lsRequestUrl, string lsToken)
        {
            return Provider.GetContent(lsRequestUrl, lsToken);
        }

        public static MaxIndex GetResponse(MaxIndex loRequestContent)
        {
            return Provider.GetResponse(loRequestContent);
        }
    }
}
