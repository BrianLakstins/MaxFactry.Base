// <copyright file="MaxDataQuery.cs" company="Lakstins Family, LLC">
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
// <change date="3/20/2024" author="Brian A. Lakstins" description="Happy birthday to my mom.  Sara Jean Lakstins (Cartwright) - 3/20/1944 to 3/14/2019.">
// <change date="3/22/2024" author="Brian A. Lakstins" description="Add some methods so no need to remember strings.  Add a ToString method for caching data based on this query.">
// <change date="9/24/2024" author="Brian A. Lakstins" description="Centralize code for adding an element and let generic elements be added.">
// <change date="6/3/2025" author="Brian A. Lakstins" description="Add method to convert to string for caching hash and visualization in debugger.">
// </changelog>
#endregion

namespace MaxFactry.Base.DataLayer
{
    using System.IO;
    using MaxFactry.Core;

    /// <summary>
    /// Keeps track of a list of parts of a query.
    /// </summary>
    public class MaxDataQuery 
	{
        /// <summary>
        /// Internal list of information
        /// </summary>
        private MaxIndex _oIndex = new MaxIndex();

		/// <summary>
        /// Initializes a new instance of the MaxDataQuery class.
		/// </summary>
        public MaxDataQuery()
		{
		}

		/// <summary>
		/// Starts a group.
		/// </summary>
		public void StartGroup()
		{
            //// Make sure that the last statement is a condition before a new group starts.
            //// If not, then group all previous statements and use an "AND" condition
            if (this._oIndex.Count > 0 && !(this._oIndex[this._oIndex.Count - 1] is string))
            {
                MaxIndex loIndex = new MaxIndex();
                loIndex.Add('(');
                for (int lnI = 0; lnI < this._oIndex.Count; lnI++)
                {
                    loIndex.Add(this._oIndex[lnI]);
                }

                loIndex.Add(')');
                loIndex.Add("AND");
                loIndex.Add('(');
                this._oIndex = loIndex;
            }
            else
            {
                this.Add('(');
            }
		}

        /// <summary>
        /// Ends a group.
        /// </summary>
        public void EndGroup()
        {
            this.Add(')');
        }

        /// <summary>
        /// Adds a filter to the query.
        /// </summary>
        /// <param name="loFilter">Filter to add to the list.</param>
        public void AddFilter(MaxDataFilter loFilter)
        {
            this.Add(loFilter);
        }

        /// <summary>
        /// Adds a filter to the query.
        /// </summary>
        /// <param name="lsName">Name used in the filter.</param>
        /// <param name="lsOperator">Operator used for comparison.</param>
        /// <param name="loValue">Value used for the filter.</param>
        public void AddFilter(string lsName, string lsOperator, object loValue)
        {
            MaxDataFilter loFilter = new MaxDataFilter(lsName, lsOperator, loValue);
            this.Add(loFilter);
        }

        /// <summary>
        /// Adds a condition between groups or filters.
        /// </summary>
        /// <param name="lsCondition">The condition to add.  Normally "AND" or "OR".</param>
        public void AddCondition(string lsCondition)
        {
            this.Add(lsCondition);
        }

        /// <summary>
        /// Adds and AND condition
        /// </summary>
        public void AddAnd()
        {
            this.AddCondition("AND");
        }

        /// <summary>
        /// Adds an OR condition
        /// </summary>
        public void AddOr()
        {
            this.AddCondition("OR");
        }

        /// <summary>
        /// Adds any object
        /// </summary>
        public void Add(object loObject)
        {
            this._oIndex.Add(loObject);
        }

        /// <summary>
        /// Gets the objects that were added to the query.
        /// </summary>
        /// <returns>Array of objects representing the query.</returns>
        public object[] GetQuery()
        {
            object[] laR = new object[this._oIndex.Count];
            for (int lnI = 0; lnI < this._oIndex.Count; lnI++)
            {
                laR[lnI] = this._oIndex[lnI];
            }

            return laR;
        }

        public override string ToString()
        {
            string lsR = string.Empty;
            object[] laDataQuery = this.GetQuery();
            if (laDataQuery.Length > 0)
            {
                for (int lnDQ = 0; lnDQ < laDataQuery.Length; lnDQ++)
                {
                    object loStatement = laDataQuery[lnDQ];
                    if (loStatement is char)
                    {
                        // Group Start or end characters
                        lsR += (char)loStatement;
                    }
                    else if (loStatement is string)
                    {
                        // Comparison operators
                        lsR += " " + (string)loStatement + " ";
                    }
                    else if (loStatement is MaxDataFilter)
                    {
                        MaxDataFilter loDataFilter = (MaxDataFilter)loStatement;
                        lsR += "[" + loDataFilter.Name + "] " + loDataFilter.Operator + " '" + loDataFilter.Value + "'";
                    }
                }
            }

            return lsR;
        }
    }
}
