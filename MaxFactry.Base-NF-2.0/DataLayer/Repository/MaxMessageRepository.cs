// <copyright file="MaxMessageRepository.cs" company="Lakstins Family, LLC">
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
// <change date="12/18/2014" author="Brian A. Lakstins" description="Updated Provider and DataModel access pattern.">
// <change date="3/26/2015" author="Brian A. Lakstins" description="Restructured.">
// </changelog>
#endregion

namespace MaxFactry.Base.DataLayer
{
	using System;
    using MaxFactry.Core;

    /// <summary>
    /// Provides static methods to manipulate storage of temporarily cached data
    /// </summary>
    public class MaxMessageRepository : MaxByMethodFactory
	{
        /// <summary>
        /// Internal storage of single object
        /// </summary>
        private static MaxMessageRepository _oInstance = null;

        /// <summary>
        /// Lock object for multi-threaded access.
        /// </summary>
        private static object _oLock = new object();

        /// <summary>
        /// Gets the single instance of this class.
        /// </summary>
        public static MaxMessageRepository Instance
        {
            get
            {
                if (null == _oInstance)
                {
                    lock (_oLock)
                    {
                        if (null == _oInstance)
                        {
                            _oInstance = new MaxMessageRepository();
                        }
                    }
                }

                return _oInstance;
            }
        }

        /// <summary>
        /// Sends a message
        /// </summary>
        /// <param name="loData">The data for the message</param>
        /// <returns>The data that was sent.</returns>
        public static MaxData Send(MaxData loData)
        {
            IMaxMessageRepositoryProvider loProvider = Instance.GetMessageRepositoryProvider(loData);
            MaxData loDataOut = loProvider.Send(loData);
            return loDataOut;
        }

        /// <summary>
        /// Gets the configuration value for a certain from address.
        /// </summary>
        /// <param name="lsConfigName">Name of the configuration value</param>
        /// <param name="lsFromAddress">The address the email is being sent from.</param>
        /// <param name="lsRelationType">The relation type of the email being sent.</param>
        /// <returns>A configuration value based on the config name.</returns>
        public static string GetConfig(string lsConfigName, string lsFromAddress, string lsRelationType)
        {
            string lsAddressPrefix = lsFromAddress.ToLower();
            string lsDomainPrefix = lsFromAddress.ToLower().Substring(lsFromAddress.IndexOf('@') + 1);
            string lsR = MaxConfigurationLibrary.GetValue(MaxEnumGroup.ScopeAny, lsAddressPrefix + lsRelationType + lsConfigName) as string;
            if (lsR == null || lsR.Length == 0)
            {
                lsR = MaxConfigurationLibrary.GetValue(MaxEnumGroup.ScopeAny, lsDomainPrefix + lsRelationType + lsConfigName) as string;
                if (lsR == null || lsR.Length == 0)
                {
                    lsR = MaxConfigurationLibrary.GetValue(MaxEnumGroup.ScopeAny, lsAddressPrefix + lsConfigName) as string;
                    if (lsR == null || lsR.Length == 0)
                    {
                        lsR = MaxConfigurationLibrary.GetValue(MaxEnumGroup.ScopeAny, lsDomainPrefix + lsConfigName) as string;
                        if (lsR == null || lsR.Length == 0)
                        {
                            lsR = MaxConfigurationLibrary.GetValue(MaxEnumGroup.ScopeAny, lsRelationType + lsConfigName) as string;
                            if (lsR == null || lsR.Length == 0)
                            {
                                lsR = MaxConfigurationLibrary.GetValue(MaxEnumGroup.ScopeAny, "MaxDefault" + lsConfigName) as string;
                            }
                        }
                    }
                }
            }

            return lsR;
        }

        /// <summary>
        /// Gets the provider based on the type of object being stored
        /// </summary>
        /// <param name="loDataModel">Data model used to determine provider to use.</param>
        /// <returns>A provider for storing an object of that specified type</returns>
        public virtual IMaxProvider GetProvider(MaxDataModel loDataModel)
        {
            IMaxMessageDataModel loMessageDataModel = loDataModel as IMaxMessageDataModel;
            if (null == loMessageDataModel)
            {
                throw new MaxException("DataModel for [" + this.GetType().ToString() + "] does not implement required interface.");
            }

            Type loDefaultProviderType = loMessageDataModel.MessageRepositoryProviderType;
            //// Try to create the provider specific to the Data Model, but using the default if there is not a specific one.
            IMaxProvider loR = this.GetProvider(loDataModel.GetType().ToString(), loDefaultProviderType);
            return loR;
        }

        /// <summary>
        /// Gets the provider based on the type of object being stored
        /// </summary>
        /// <param name="loData">Data used to determine provider to use.</param>
        /// <returns>A provider for storing an object of that specified type</returns>
        public virtual IMaxMessageRepositoryProvider GetMessageRepositoryProvider(MaxData loData)
        {
            IMaxProvider loProvider = null;
            Type loDataProvider = loData.Get("IMaxMessageRepositoryProvider") as Type;
            if (null != loDataProvider)
            {
                loProvider = this.GetProvider(loDataProvider);
            }

            if (null == loProvider)
            {
                loProvider = this.GetProvider(loData.DataModel);
            }

            IMaxMessageRepositoryProvider loR = loProvider as IMaxMessageRepositoryProvider;
            if (null == loR)
            {
                if (null != loProvider)
                {
                    throw new MaxException("Error casting message repository provider [" + loProvider.GetType() + "]");
                }

                throw new MaxException("Error creating message repository provider for [" + loData.DataModel.GetType() + "]");
            }

            return loR;
        }
	}
}