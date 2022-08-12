// <copyright file="IMaxRepositoryProvider.cs" company="Lakstins Family, LLC">
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
// <change date="6/25/2014" author="Brian A. Lakstins" description="Based on IMaxAppRepositoryProvider">
// <change date="8/21/2014" author="Brian A. Lakstins" description="Add stream management.">
// <change date="11/11/2014" author="Brian A. Lakstins" description="Added general Selecting by a single property that does not include records marked as deleted.">
// <change date="12/2/2014" author="Brian A. Lakstins" description="Add laFields.">
// <change date="1/28/2015" author="Brian A. Lakstins" description="Add a context provider name.">
// </changelog>
#endregion

namespace MaxFactry.Base.DataLayer
{
	using System;
    using System.IO;
    using MaxFactry.Core;

    /// <summary>
    /// Provides methods to manipulate storage of data
    /// </summary>
    public interface IMaxRepositoryProvider : IMaxProvider
	{
        /// <summary>
        /// Gets the Default provider Type for the Data Context.
        /// </summary>
        Type DefaultContextProviderType { get; }

        /// <summary>
        /// Gets the name of the provider for the Data Context.
        /// </summary>
        string ContextProviderName { get; }
    }
}
