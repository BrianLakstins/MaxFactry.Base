// <copyright file="MaxBaseUSPostalAddressDataModel.cs" company="Lakstins Family, LLC">
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
    /// Base Data Model for address related information.
    /// <see href="http://pe.usps.gov/text/pub28/28c3_012.htm"/>
    /// </summary>
    public abstract class MaxBaseUSPostalAddressDataModel : MaxBaseIdDataModel
    {
        /// <summary>
        /// Individual Title (Line 6)
        /// </summary>
        public readonly string Title = "Title";

        /// <summary>
        /// Functional Title (Line 7)
        /// </summary>
        public readonly string Function = "Function";

        /// <summary>
        /// Group, Department, Division Name (Line 8)
        /// </summary>
        public readonly string Group = "Group";

        /// <summary>
        /// Name of person at the address (Line 5)
        /// </summary>
        public readonly string Attention = "Attention";

        /// <summary>
        /// Name of the company at the address (Line #9)
        /// </summary>
        public readonly string Company = "Company";

        /// <summary>
        /// US Post office delivery address (Line #10)
        /// </summary>
        public readonly string DeliveryAddress = "DeliveryAddress";

        /// <summary>
        /// Spell city names in their entirety when possible. When it is not possible, use the 13 character abbreviations from the USPS City State File.
        /// </summary>
        public readonly string City = "City";

        /// <summary>
        /// Use 2 letter USPS State Abbreviations.
        /// </summary>
        public readonly string StateCode = "StateCode";

        /// <summary>
        /// Zip Code or Zip+4 Number
        /// </summary>
        public readonly string PostalCode = "PostalCode";

        /// <summary>
        /// Other address information
        /// </summary>
        public readonly string OtherAddressInfo = "OtherAddressInfo";

        /// <summary>
        /// Initializes a new instance of the MaxBaseUSPostalAddressDataModel class
        /// </summary>
        public MaxBaseUSPostalAddressDataModel()
        {
            this.AddType(this.Attention, typeof(MaxShortString));
            this.AddNullable(this.Company, typeof(MaxShortString));
            this.AddNullable(this.Group, typeof(MaxShortString));
            this.AddNullable(this.Function, typeof(MaxShortString));
            this.AddNullable(this.Title, typeof(MaxShortString));
            this.AddType(this.DeliveryAddress, typeof(MaxShortString));
            this.AddType(this.City, typeof(MaxShortString));
            this.AddType(this.StateCode, typeof(MaxShortString));
            this.AddType(this.PostalCode, typeof(MaxShortString));
            this.AddType(this.OtherAddressInfo, typeof(MaxShortString));
        }
    }
}
