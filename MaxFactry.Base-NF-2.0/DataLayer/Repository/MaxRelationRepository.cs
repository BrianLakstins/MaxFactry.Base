// <copyright file="MaxRelationRepository.cs" company="Lakstins Family, LLC">
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
// <change date="6/25/2014" author="Brian A. Lakstins" description="Based on MaxAppRelationRepository">
// <change date="12/18/2014" author="Brian A. Lakstins" description="Updated Provider and DataModel access pattern.">
// <change date="3/26/2015" author="Brian A. Lakstins" description="Moved to MaxFactry.Base">
// <change date="6/2/2015" author="Brian A. Lakstins" description="Updated to use base methods instead of interface and provider.">
// <change date="2/19/2021" author="Brian A. Lakstins" description="Remove methods because they could be done with other methods.">
// </changelog>
#endregion

namespace MaxFactry.Base.DataLayer
{
	using System;
    using MaxFactry.Core;

	/// <summary>
	/// Provides static methods to manipulate storage of data
	/// </summary>
	public class MaxRelationRepository : MaxStorageWriteRepository
	{
	}
}
