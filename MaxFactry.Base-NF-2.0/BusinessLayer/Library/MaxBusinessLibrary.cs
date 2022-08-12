// <copyright file="MaxBusinessLibrary.cs" company="Lakstins Family, LLC">
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
// <change date="2/3/2014" author="Brian A. Lakstins" description="Initial Release">
// <change date="2/23/2014" author="Brian A. Lakstins" description="Updates to use type of DataModel to determine the entity.">
// <change date="3/2/2014" author="Brian A. Lakstins" description="Updates to reduce amount of configuration needed.">
// <change date="7/21/2014" author="Brian A. Lakstins" description="Removed MaxAppId dependency stuff.">
// <change date="8/25/2014" author="Brian A. Lakstins" description="Changed to library.">
// </changelog>
#endregion

namespace MaxFactry.Base.BusinessLayer
{
	using System;
    using MaxFactry.Core;
    using MaxFactry.Base.DataLayer;

	/// <summary>
	/// Provides static methods to manipulate storage of data
	/// </summary>
	public class MaxBusinessLibrary : MaxSingleFactory
	{
        /// <summary>
        /// Internal storage of single object
        /// </summary>
        private static MaxBusinessLibrary _oInstance = null;

        /// <summary>
        /// Lock object for multi-threaded access.
        /// </summary>
        private static object _oLock = new object();

        /// <summary>
        /// Gets the Provider used for most MaxFactory methods
        /// </summary>
        public static IMaxBusinessLibraryProvider Provider
        {
            get
            {
                if (null == Instance.BaseProvider)
                {
                    Instance.SetProvider(typeof(MaxFactry.Base.BusinessLayer.Provider.MaxBusinessLibraryDefaultProvider));
                }

                return (IMaxBusinessLibraryProvider)Instance.BaseProvider;
            }
        }

        /// <summary>
        /// Gets the single instance of this class.
        /// </summary>
        public static MaxBusinessLibrary Instance
        {
            get
            {
                if (null == _oInstance)
                {
                    lock (_oLock)
                    {
                        if (null == _oInstance)
                        {
                            _oInstance = new MaxBusinessLibrary();
                        }
                    }
                }

                return _oInstance;
            }
        }

        /// <summary>
        /// Gets an entity of the specified type that is based on the data model type.
        /// </summary>
        /// <param name="loEntityType">Type of entity to get.</param>
        /// <param name="loDataModelType">Data model type to use with the entity.</param>
        /// <returns>Context provider</returns>
        public static MaxEntity GetEntity(Type loEntityType, Type loDataModelType)
        {
            return Provider.GetEntity(loEntityType, loDataModelType);
        }

		/// <summary>
		/// Gets the context provider using the Repository provider for this type
		/// </summary>
		/// <param name="loType">Type of context provider to get</param>
		/// <param name="loData">Data to use to initialize the entity</param>
		/// <returns>Entity specified by the type</returns>
        public static MaxEntity GetEntity(Type loType, MaxData loData)
		{
            return Provider.GetEntity(loType, loData);
		}

		/// <summary>
		/// Maps a class that uses a specific provider to that provider
		/// </summary>
		/// <param name="loType">Class that uses a provider</param>
		/// <param name="loProviderType">The provider to use</param>
		public static void RegisterEntityProvider(Type loType, Type loProviderType)
		{
            Provider.RegisterEntityProvider(loType, loProviderType);
		}

        /// <summary>
        /// Sets the internal provider based on the settings
        /// </summary>
        /// <param name="loType">Type to use to determine the provider.</param>
        protected override void SetProvider(Type loType)
        {
            base.SetProvider(loType);
            if (!(Instance.BaseProvider is IMaxBusinessLibraryProvider))
            {
                throw new MaxException("Provider for MaxDataLibrary does not implement IMaxDataLibraryProvider.");
            }
        }
	}
}
