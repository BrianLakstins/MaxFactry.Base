// <copyright file="MaxBusinessLibraryDefaultProvider.cs" company="Lakstins Family, LLC">
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

namespace MaxFactry.Base.BusinessLayer.Provider
{
	using System;
    using MaxFactry.Core;
    using MaxFactry.Base.DataLayer;

	/// <summary>
	/// Provides static methods to manipulate storage of data
	/// </summary>
    public class MaxBusinessLibraryDefaultProvider : MaxProvider, IMaxBusinessLibraryProvider
	{
		/// <summary>
		/// Maps classes that use a provider to the class that is the provider
		/// </summary>
		private MaxIndex _oEntityMapIndex = new MaxIndex();

        /// <summary>
        /// Gets an entity of the specified type that is based on the data model type.
        /// </summary>
        /// <param name="loEntityType">Type of entity to get.</param>
        /// <param name="loDataModelType">Data model type to use with the entity.</param>
        /// <returns>Context provider</returns>
        public MaxEntity GetEntity(Type loEntityType, Type loDataModelType)
        {
            Type loCreateType = loEntityType;
            if (this._oEntityMapIndex.Contains(loEntityType.ToString()))
            {
                loCreateType = (Type)this._oEntityMapIndex[loEntityType.ToString()];
            }

            MaxEntity loEntity = MaxFactryLibrary.Create(loCreateType, loDataModelType) as MaxEntity;
            return loEntity;
        }

		/// <summary>
		/// Gets the context provider using the Repository provider for this type
		/// </summary>
		/// <param name="loType">Type of context provider to get</param>
		/// <param name="loData">Data to use to initialize the entity</param>
		/// <returns>Entity specified by the type</returns>
		public MaxEntity GetEntity(Type loType, MaxData loData)
		{
			Type loEntityType = loType;
			if (this._oEntityMapIndex.Contains(loType.ToString()))
			{
                loEntityType = (Type)this._oEntityMapIndex[loType.ToString()];
			}

            MaxEntity loEntity = MaxFactryLibrary.Create(loEntityType, loData) as MaxEntity;
            return loEntity;
		}

		/// <summary>
		/// Maps a class that uses a specific provider to that provider
		/// </summary>
		/// <param name="loType">Class that uses a provider</param>
		/// <param name="loProviderType">The provider to use</param>
		public void RegisterEntityProvider(Type loType, Type loProviderType)
		{
            if (!this._oEntityMapIndex.Contains(loType.ToString()))
			{
                this._oEntityMapIndex.Add(loType.ToString(), loProviderType);
			}
		}
	}
}
