// <copyright file="MaxStartup.cs" company="Lakstins Family, LLC">
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
// <change date="7/6/2015" author="Brian A. Lakstins" description="Initial creation">
// <change date="6/5/2020" author="Brian A. Lakstins" description="Updated to give a default glboal configuration for all providers so config is not needed to be set for every provider.">
// <change date="7/24/2023" author="Brian A. Lakstins" description="Change order of startup methods.">
// </changelog>
#endregion

namespace MaxFactry.Base
{
    using MaxFactry.Core;
    using MaxFactry.Base.DataLayer;

    /// <summary>
    /// Class used to define initialization a library.
    /// Define the any initialization processes using SetProviderConfiguration, RegisterProviders, and ApplicationStartup
    /// </summary>
    public class MaxStartup
	{
        /// <summary>
        /// Internal storage of single object
        /// </summary>
        private static object _oInstance = null;

        /// <summary>
        /// Lock object for multi-threaded access.
        /// </summary>
        private static object _oLock = new object();

        protected static object CreateInstance(System.Type loType, object loCurrent)
        {
            if (null == loCurrent)
            {
                lock (_oLock)
                {
                    if (null == loCurrent)
                    {
                        loCurrent = MaxFactry.Core.MaxFactryLibrary.CreateSingleton(loType);
                    }
                }
            }

            return loCurrent;
        }

        /// <summary>
        /// Gets the single instance of this class.
        /// </summary>
        public static MaxStartup Instance
        {
            get
            {
                _oInstance = CreateInstance(typeof(MaxStartup), _oInstance);
                return _oInstance as MaxStartup;
            }
        }

        /// <summary>
        /// Sets the basic configuration for all providers
        /// </summary>
        /// <param name="loConfig">The default configuration for all providers used when they are initialized.</param>
        public virtual void SetProviderConfiguration(MaxIndex loConfig)
        {
            //// Anything that is a descendent of MaxProvider will use the specified config.
            //// Variables in the config need to be prefixed with the typeof(object) that they pertain to so that they are not used on the wrong provider
            MaxFactry.Core.MaxFactryLibrary.SetValue(typeof(MaxFactry.Core.MaxProvider) + "-Config", loConfig);
        }

        /// <summary>
        /// To be run after configuration has been set
        /// </summary>
        public virtual void RegisterProviders()
        {
            MaxCompressionLibrary.Instance.ProviderAdd(
                typeof(MaxFactry.Core.Provider.MaxCompressionLibraryDefaultProvider));

            MaxConvertLibrary.Instance.ProviderAdd(
                typeof(MaxFactry.Core.Provider.MaxConvertLibraryDefaultProvider));

            MaxEncryptionLibrary.Instance.ProviderAdd(
                typeof(MaxFactry.Core.Provider.MaxEncryptionLibraryDefaultProvider));

            MaxLogLibrary.Instance.ProviderAdd(
                typeof(MaxFactry.Core.Provider.MaxLogLibraryDefaultProvider));

            MaxMessageRepository.Instance.ProviderAdd(
                    typeof(MaxFactry.Base.DataLayer.Provider.MaxMessageRepositoryDefaultProvider));

            MaxMetaLibrary.Instance.ProviderAdd(
                typeof(MaxFactry.Core.Provider.MaxMetaLibraryDefaultProvider));
        }

        /// <summary>
        /// To be run after providers have been configured.
        /// </summary>
        public virtual void ApplicationStartup()
        {
        }
	}
}
