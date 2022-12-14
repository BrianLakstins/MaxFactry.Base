// <copyright file="IMaxDataLibraryProvider.cs" company="Lakstins Family, LLC">
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
// <change date="6/26/2014" author="Brian A. Lakstins" description="Initial Release">
// <change date="7/3/2014" author="Brian A. Lakstins" description="Added support for getting stream repository provider.">
// <change date="7/4/2014" author="Brian A. Lakstins" description="Added support for getting message repository provider.">
// <change date="8/21/2014" author="Brian A. Lakstins" description="Remove stream support.">
// <change date="8/24/2014" author="Brian A. Lakstins" description="Rename.">
// <change date="9/16/2014" author="Brian A. Lakstins" description="Rename.">
// <change date="10/22/2014" author="Brian A. Lakstins" description="Add ability to specify DataContext through MaxData instance.">
// </changelog>
#endregion

namespace MaxFactry.Base.DataLayer
{
	using System;
    using MaxFactry.Core;

    /// <summary>
    /// Provides static methods to manipulate storage of data
    /// </summary>
    public interface IMaxDataLibraryProvider : IMaxProvider
	{
		/// <summary>
		/// Gets the context provider using the Repository provider for this type
		/// </summary>
        /// <param name="loRepositoryProvider">The repository provider calling the context provider.</param>
        /// <param name="loData">The data being updated by the context provider.</param>
        /// <returns>Data context provider</returns>
        IMaxDataContextProvider GetContextProvider(IMaxRepositoryProvider loRepositoryProvider, MaxData loData);

		/// <summary>
		/// Gets the context provider using the Repository provider for this type
		/// </summary>
		/// <param name="loType">Type of context provider to get</param>
		/// <returns>Context provider</returns>
		MaxDataModel GetDataModel(Type loType);

		/// <summary>
		/// Gets a data list for the data model based on the type
		/// </summary>
		/// <param name="loType">Type of data list to get</param>
		/// <returns>DataList for the data model specified by the type</returns>
		MaxDataList GetDataList(Type loType);

		/// <summary>
		/// Gets a data list for the data model based on the type
		/// </summary>
		/// <param name="loDataModel">Data model to be used in the list</param>
		/// <returns>DataList for the data model specified by the type</returns>
		MaxDataList GetDataList(MaxDataModel loDataModel);

		/// <summary>
		/// Maps a class that uses a specific provider to that provider
		/// </summary>
		/// <param name="loType">Class that uses a provider</param>
		/// <param name="loProviderType">The provider to use</param>
		void RegisterContextProvider(Type loType, Type loProviderType);

		/// <summary>
		/// Maps a class that uses a specific provider to that provider
		/// </summary>
		/// <param name="loType">Class that uses a provider</param>
		/// <param name="loDataModelType">The provider to use</param>
		void RegisterDataModelProvider(Type loType, Type loDataModelType);

        /// <summary>
        /// Gets the storage key used to separate the storage of data
        /// </summary>
        /// <param name="loData">The data to be stored using the storage key.</param>
        /// <returns>string used for the storage key</returns>
        string GetStorageKey(MaxData loData);
	}
}
