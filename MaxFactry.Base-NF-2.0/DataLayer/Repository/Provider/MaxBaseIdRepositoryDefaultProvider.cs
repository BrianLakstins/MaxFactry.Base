// <copyright file="MaxBaseIdRepositoryDefaultProvider.cs" company="Lakstins Family, LLC">
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
// <change date="6/25/2014" author="Brian A. Lakstins" description="Based on MaxAppIdRepositoryProvider">
// <change date="12/2/2014" author="Brian A. Lakstins" description="Update to match interface.  Add laFields.">
// <change date="3/26/2015" author="Brian A. Lakstins" description="Moved to MaxFactry.Base">
// <change date="12/21/2016" author="Brian A. Lakstins" description="Added central method to filter by deleted property.">
// <change date="3/20/2024" author="Brian A. Lakstins" description="Happy birthday to my mom.  Sara Jean Lakstins (Cartwright) - 3/20/1944 to 3/14/2019.">
// <change date="3/23/2024" author="Brian A. Lakstins" description="Change parent class.  Update for changes to DataModel.">
// <change date="3/25/2024" author="Brian A. Lakstins" description="Removing passing Total">
// <change date="3/26/2024" author="Brian A. Lakstins" description="Remove method to filter by IsDeleted because it's included in the parent class">
// </changelog>
#endregion

namespace MaxFactry.Base.DataLayer.Provider
{
    using MaxFactry.Base.DataLayer;

	/// <summary>
    /// Provider for MaxBaseIdRepository
	/// </summary>
    public abstract class MaxBaseIdRepositoryDefaultProvider : MaxFactry.Base.DataLayer.Provider.MaxBaseRepositoryDefaultProvider, IMaxBaseIdRepositoryProvider
	{
	}
}
