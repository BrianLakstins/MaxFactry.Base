// <copyright file="MaxDataLibrary.cs" company="Lakstins Family, LLC">
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
// <change date="1/22/2014" author="Brian A. Lakstins" description="Initial Release">
// <change date="2/25/2014" author="Brian A. Lakstins" description="Made mappings able to be overwritten.">
// <change date="3/2/2014" author="Brian A. Lakstins" description="Updates to reduce amount of configuration needed.">
// <change date="3/22/2014" author="Brian A. Lakstins" description="Updated to work with more base data.">
// <change date="5/27/2014" author="Brian A. Lakstins" description="Added ability to register provider by a key.">
// <change date="6/26/2014" author="Brian A. Lakstins" description="Updated to use a provider.">
// <change date="7/3/2014" author="Brian A. Lakstins" description="Added support for getting stream repository provider.">
// <change date="7/4/2014" author="Brian A. Lakstins" description="Added support for getting message repository provider.">
// <change date="8/13/2014" author="Brian A. Lakstins" description="Add diagnostic logging.">
// <change date="8/21/2014" author="Brian A. Lakstins" description="Remove stream repository.">
// <change date="8/24/2014" author="Brian A. Lakstins" description="Update to inherit from MaxBaseSingle.  Change to library.">
// <change date="9/16/2014" author="Brian A. Lakstins" description="Add RespositoryCache support.">
// <change date="10/22/2014" author="Brian A. Lakstins" description="Add ability to specify DataContext through MaxData instance.">
// <change date="5/19/2020" author="Brian A. Lakstins" description="Updating timing logging to use StopWatch instead of DateTime (faster and more accurate to use StopWatch to time things).">
// <change date="5/20/2020" author="Brian A. Lakstins" description="Update logging.">
// <change date="5/22/2020" author="Brian A. Lakstins" description="Update logging.">
// <change date="8/3/2020" author="Brian A. Lakstins" description="Add conditional method for netstandard1_2 for not having StopWatch">
// <change date="3/19/2024" author="Brian A. Lakstins" description="Remove SerializeData method.  Remove need for Stopwatch.  Add methods that were in repository providers.">
// <change date="3/20/2024" author="Brian A. Lakstins" description="Happy birthday to my mom.  Sara Jean Lakstins (Cartwright) - 3/20/1944 to 3/14/2019.">
// <change date="3/24/2024" author="Brian A. Lakstins" description="Update to namespace.">
// <change date="3/25/2024" author="Brian A. Lakstins" description="Remove handling of DataContext">
// <change date="6/11/2025" author="Brian A. Lakstins" description="Rename StorageKey to ApplicationKey and don't use MaxData for value">
// </changelog>
#endregion

namespace MaxFactry.Base.DataLayer.Library
{
	using System;
    using System.Diagnostics;
    using MaxFactry.Core;

    /// <summary>
    /// Provides static methods to manipulate storage of data
    /// </summary>
    public class MaxDataLibrary : MaxSingleFactory
	{
        /// <summary>
        /// Internal storage of single object
        /// </summary>
        private static MaxDataLibrary _oInstance = null;

        /// <summary>
        /// Lock object for multi-threaded access.
        /// </summary>
        private static object _oLock = new object();

        /// <summary>
        /// Gets the Provider used for most MaxFactory methods
        /// </summary>
        public static IMaxDataLibraryProvider Provider
        {
            get
            {
                if (null == Instance.BaseProvider)
                {
                    Instance.SetProvider(typeof(Provider.MaxDataLibraryDefaultProvider));
                }

                return (IMaxDataLibraryProvider)Instance.BaseProvider;
            }
        }

        /// <summary>
        /// Gets the single instance of this class.
        /// </summary>
        public static MaxDataLibrary Instance
        {
            get
            {
                if (null == _oInstance)
                {
                    lock (_oLock)
                    {
                        if (null == _oInstance)
                        {
                            _oInstance = new MaxDataLibrary();
                        }
                    }
                }

                return _oInstance;
            }
        }

		/// <summary>
		/// Gets the context provider using the Repository provider for this type
		/// </summary>
		/// <param name="loType">Type of context provider to get</param>
		/// <returns>Context provider</returns>
		public static MaxDataModel GetDataModel(Type loType)
		{
            return Provider.GetDataModel(loType);
		}

		/// <summary>
		/// Maps a class that uses a specific provider to that provider
		/// </summary>
		/// <param name="loType">Class that uses a provider</param>
		/// <param name="loDataModelType">The provider to use</param>
		public static void RegisterDataModelProvider(Type loType, Type loDataModelType)
		{
            Provider.RegisterDataModelProvider(loType, loDataModelType);
		}

        /// <summary>
        /// Gets the application key used to separate applications
        /// </summary>
        /// <returns>string used for the application key</returns>
        public static string GetApplicationKey()
        {
            DateTime ldStart = DateTime.UtcNow;
            string lsR = Provider.GetApplicationKey();
            TimeSpan loDuration = DateTime.UtcNow - ldStart;
            if (loDuration.TotalMilliseconds > 500)
            {
                MaxFactry.Core.MaxLogLibrary.Log(new MaxLogEntryStructure(Provider.GetType(), "GetApplicationKey", MaxEnumGroup.LogWarning, "GetApplicationKey() took {Duration} ms for key {lsR}.", loDuration.TotalMilliseconds, lsR));
            }

            return lsR;
        }

        /// <summary>
        /// Gets the extension of a file name.
        /// </summary>
        /// <param name="lsName">Name of a file.</param>
        /// <returns>Extension of the file.</returns>
        public static string GetFileNameExtension(string lsName)
        {
            string lsR = Provider.GetFileNameExtension(lsName);
            return lsR;
        }

        public static bool SaveAsFile(string lsDirectory, MaxData loData)
        {
            bool lbR = Provider.SaveAsFile(lsDirectory, loData);
            return lbR;
        }

        public static MaxDataList LoadFromFile(string lsDirectory, MaxData loData)
        {
            MaxDataList loR = Provider.LoadFromFile(lsDirectory, loData);
            return loR;
        }

        /// <summary>
        /// Gets the value of a field used in a DataQuery
        /// </summary>
        /// <param name="loDataQuery">DataQuery to use</param>
        /// <param name="lsDataName">Field name to get value</param>
        /// <returns>The current value in the query that matches the field.  Null if no match.</returns>
        public static object GetValue(MaxDataQuery loDataQuery, string lsDataName)
        {
            object loR = Provider.GetValue(loDataQuery, lsDataName);
            return loR;
        }

        /// <summary>
        /// Sets the internal provider based on the settings
        /// </summary>
        /// <param name="loType">Type to use to determine the provider.</param>
        protected override void SetProvider(Type loType)
        {
            base.SetProvider(loType);
            if (!(Instance.BaseProvider is IMaxDataLibraryProvider))
            {
                throw new MaxException("Provider for MaxDataLibrary does not implement IMaxDataLibraryProvider.");
            }
        }
    }
}
