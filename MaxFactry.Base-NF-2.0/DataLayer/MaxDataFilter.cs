// <copyright file="MaxDataFilter.cs" company="Lakstins Family, LLC">
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
// <change date="2/25/2014" author="Brian A. Lakstins" description="Initial Release">
// </changelog>
#endregion

namespace MaxFactry.Base.DataLayer
{
	using System;

	/// <summary>
	/// Keeps track of information needed to filter data.
	/// </summary>
	public class MaxDataFilter
	{
		/// <summary>
		/// Name used in the filter statement.
		/// </summary>
		private string _sName = string.Empty;

		/// <summary>
		/// Operator used in the filter statement.
		/// </summary>
		private string _sOperator = string.Empty;

		/// <summary>
		/// Value used in the filter statement.
		/// </summary>
		private object _oValue = string.Empty;

		/// <summary>
        /// Initializes a new instance of the MaxDataFilter class.
		/// </summary>
        /// <param name="lsName">Name used in the filter.</param>
        /// <param name="lsOperator">Operator used for comparison.</param>
        /// <param name="loValue">Value used for the filter.</param>
        public MaxDataFilter(string lsName, string lsOperator, object loValue)
		{
            this._sName = lsName;
            this._sOperator = lsOperator;
            this._oValue = loValue;
		}

		/// <summary>
		/// Gets the name
		/// </summary>
		public string Name
		{
			get
			{
				return this._sName;
			}
		}

        /// <summary>
        /// Gets the operator
        /// </summary>
        public string Operator
        {
            get
            {
                return this._sOperator;
            }
        }

        /// <summary>
        /// Gets the value
        /// </summary>
        public object Value
        {
            get
            {
                return this._oValue;
            }
        }
	}
}
