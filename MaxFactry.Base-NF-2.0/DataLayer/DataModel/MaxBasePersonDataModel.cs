// <copyright file="MaxBasePersonDataModel.cs" company="Lakstins Family, LLC">
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
// <change date="11/6/2014" author="Brian A. Lakstins" description="Initial Release">
// </changelog>
#endregion

namespace MaxFactry.Base.DataLayer
{
	using System;
    using MaxFactry.Base.DataLayer;

	/// <summary>
    /// Base Data model for the information associated with a person.
	/// </summary>
	public abstract class MaxBasePersonDataModel : MaxBaseIdDataModel
	{
		/// <summary>
		/// Full name of the person.
		/// </summary>
		public readonly string Name = "Name";

        /// <summary>
        /// First Name of the person when they were born.
        /// </summary>
        public readonly string BirthFirstName = "BirthFirstName";

        /// <summary>
        /// Middle Name of the person when they were born.
        /// </summary>
        public readonly string BirthMiddleName = "BirthMiddleName";

        /// <summary>
        /// Last Name of the person when they were born.
        /// </summary>
        public readonly string BirthLastName = "BirthLastName";

        /// <summary>
        /// First Name of the person.
        /// </summary>
        public readonly string CurrentFirstName = "CurrentFirstName";

        /// <summary>
        /// Middle Name of the person.
        /// </summary>
        public readonly string CurrentMiddleName = "CurrentMiddleName";

        /// <summary>
        /// Last Name of the person.
        /// </summary>
        public readonly string CurrentLastName = "CurrentLastName";

        /// <summary>
        /// Date of birth of the person.
        /// </summary>
        public readonly string BirthDate = "BirthDate";

        /// <summary>
        /// Place where the person was born.
        /// </summary>
        public readonly string BirthPlace = "BirthPlace";

        /// <summary>
        /// Country where the person was born.
        /// </summary>
        public readonly string BirthCountry = "BirthCountry";

        /// <summary>
        /// Date of death of the person.
        /// </summary>
        public readonly string DeathDate = "DeathDate";

        /// <summary>
        /// Sex of the person.
        /// </summary>
        public readonly string Sex = "Sex";
        
        /// <summary>
        /// Initializes a new instance of the MaxBasePersonDataModel class.
		/// </summary>
        public MaxBasePersonDataModel()
            : base()
		{
            this.AddNullable(this.BirthCountry, typeof(string));
            this.AddNullable(this.BirthDate, typeof(DateTime));
            this.AddNullable(this.BirthFirstName, typeof(MaxShortString));
            this.AddNullable(this.BirthLastName, typeof(MaxShortString));
            this.AddNullable(this.BirthMiddleName, typeof(MaxShortString));
            this.AddNullable(this.BirthPlace, typeof(string));
            this.AddNullable(this.CurrentFirstName, typeof(MaxShortString));
            this.AddNullable(this.CurrentLastName, typeof(MaxShortString));
            this.AddNullable(this.CurrentMiddleName, typeof(MaxShortString));
            this.AddNullable(this.DeathDate, typeof(DateTime));
            this.AddNullable(this.Name, typeof(string));
            this.AddNullable(this.Sex, typeof(MaxShortString));
        }
	}
}
