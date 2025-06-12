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
// <change date="3/20/2024" author="Brian A. Lakstins" description="Happy birthday to my mom.  Sara Jean Lakstins (Cartwright) - 3/20/1944 to 3/14/2019.">
// <change date="3/22/2024" author="Brian A. Lakstins" description="Add some methods that were in Repository proviers.  Remove some unused methods.">
// <change date="3/24/2024" author="Brian A. Lakstins" description="Update to namespace.">
// <change date="3/25/2024" author="Brian A. Lakstins" description="Remove handling of DataContext">
// <change date="6/11/2025" author="Brian A. Lakstins" description="Rename StorageKey to ApplicationKey">
// </changelog>
#endregion

namespace MaxFactry.Base.DataLayer.Library
{
	using System;
    using MaxFactry.Core;

    /// <summary>
    /// Provides static methods to manipulate storage of data
    /// </summary>
    public interface IMaxDataLibraryProvider : IMaxProvider
	{
        /// <summary>
        /// 
        /// </summary>
        /// <param name="loType">Type of context provider to get</param>
        /// <returns>Context provider</returns>
        MaxDataModel GetDataModel(Type loType);

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
        string GetApplicationKey();

        /// <summary>
        /// Gets the extension of a file name.
        /// </summary>
        /// <param name="lsName">Name of a file.</param>
        /// <returns>Extension of the file.</returns>
        string GetFileNameExtension(string lsName);

        /// <summary>
        /// Saves a data to a file
        /// </summary>
        /// <param name="lsDirectory"></param>
        /// <param name="loData"></param>
        /// <returns></returns>
        bool SaveAsFile(string lsDirectory, MaxData loData);

        /// <summary>
        /// Loads data from a file
        /// </summary>
        /// <param name="lsDirectory"></param>
        /// <param name="loData"></param>
        /// <returns></returns>
        MaxDataList LoadFromFile(string lsDirectory, MaxData loData);

        /// <summary>
        /// Gets the value of a field used in a DataQuery
        /// </summary>
        /// <param name="loDataQuery">DataQuery to use</param>
        /// <param name="lsDataName">Field name to get value</param>
        /// <returns>The current value in the query that matches the field.  Null if no match.</returns>
        object GetValue(MaxDataQuery loDataQuery, string lsDataName);
    }
}
