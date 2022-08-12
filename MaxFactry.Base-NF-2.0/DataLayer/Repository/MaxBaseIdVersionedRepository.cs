// <copyright file="MaxBaseIdVersionedRepository.cs" company="Lakstins Family, LLC">
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
// <change date="11/10/2014" author="Brian A. Lakstins" description="Based on MaxAppIdIndexRepository">
// <change date="12/2/2014" author="Brian A. Lakstins" description="Add laFields.">
// <change date="12/18/2014" author="Brian A. Lakstins" description="Updated Provider and DataModel access pattern.">
// <change date="3/26/2015" author="Brian A. Lakstins" description="Moved to MaxFactry.Base">
// </changelog>
#endregion

namespace MaxFactry.Base.DataLayer
{
	using System;
    using MaxFactry.Core;
    using MaxFactry.Base.DataLayer;

	/// <summary>
	/// Repository for basic application information
	/// </summary>
    public class MaxBaseIdVersionedRepository : MaxBaseIdRepository
	{
        /// <summary>
        /// Selects all not marked as deleted based on the name
        /// </summary>
        /// <param name="loData">Data information used for select</param>
        /// <param name="lsName">The key used for the versioned data</param>
        /// <param name="laFields">list of fields to return from select</param>
        /// <returns>List of information matching the name</returns>
        public static MaxDataList SelectAllByName(MaxData loData, string lsName, params string[] laFields)
        {
            MaxBaseIdVersionedDataModel loDataModel = loData.DataModel as MaxBaseIdVersionedDataModel;
            if (null == loDataModel)
            {
                throw new MaxException("Error casting [" + loData.DataModel.GetType() + "] for DataModel");
            }

            MaxDataList loDataList = SelectAllByProperty(loData, loDataModel.Name, lsName, laFields);
            return loDataList;
        }

        /// <summary>
        /// Selects all active not marked as deleted based on the name
        /// </summary>
        /// <param name="loData">Data information used for select.</param>
        /// <param name="lsName">The key used for the versioned data</param>
        /// <param name="laFields">list of fields to return from select</param>
        /// <returns>List of information matching the name</returns>
        public static MaxDataList SelectAllActiveByName(MaxData loData, string lsName, params string[] laFields)
        {
            MaxBaseIdVersionedDataModel loDataModel = loData.DataModel as MaxBaseIdVersionedDataModel;
            if (null == loDataModel)
            {
                throw new MaxException("Error casting [" + loData.DataModel.GetType() + "] for DataModel");
            }

            MaxDataList loDataList = SelectAllActiveByProperty(loData, loDataModel.Name, lsName, laFields);
            return loDataList;
        }
	}
}
