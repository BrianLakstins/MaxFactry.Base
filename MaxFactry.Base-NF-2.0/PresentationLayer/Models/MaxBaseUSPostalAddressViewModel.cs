// <copyright file="MaxBaseUSPostalAddressViewModel.cs" company="Lakstins Family, LLC">
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
// <change date="2/24/2015" author="Brian A. Lakstins" description="Initial Release">
// <change date="3/26/2015" author="Brian A. Lakstins" description="Moved to MaxFactry.Base">
// </changelog>
#endregion

namespace MaxFactry.Base.PresentationLayer
{
    using System;
    using MaxFactry.Core;
    using MaxFactry.Base.BusinessLayer;

    /// <summary>
    /// View model base
    /// </summary>
    public abstract class MaxBaseUSPostalAddressViewModel : MaxBaseIdViewModel
    {
        /// <summary>
        /// Initializes a new instance of the MaxBaseUSPostalAddressViewModel class
        /// </summary>
        public MaxBaseUSPostalAddressViewModel()
            : base()
        {
        }

        /// <summary>
        /// Initializes a new instance of the MaxBaseUSPostalAddressViewModel class
        /// </summary>
        /// <param name="loEntity">Entity to use as data.</param>
        public MaxBaseUSPostalAddressViewModel(MaxEntity loEntity)
            : base(loEntity)
        {
        }

        public MaxBaseUSPostalAddressViewModel(string lsId) : base(lsId)
        {
        }

        /// <summary>
        /// Gets or sets the Name of person at the address (Line 5)
        /// </summary>
        public virtual string Attention
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the City
        /// Spell city names in their entirety when possible. When it is not possible, use the 13 character abbreviations from the USPS City State File.
        /// </summary>
        public virtual string City
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the Name of the company at the address (Line #9)
        /// </summary>
        public virtual string Company
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the US Post office delivery address (Line #10)
        /// </summary>
        public virtual string DeliveryAddress
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the Functional Title (Line 7)
        /// </summary>
        public virtual string Function
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the Group, Department, Division Name (Line 8)
        /// </summary>
        public virtual string Group
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the Other address information
        /// </summary>
        public virtual string OtherAddressInfo
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the Zip Code or Zip+4 Number
        /// </summary>
        public virtual string PostalCode
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the state
        /// Use 2 letter USPS State Abbreviations.
        /// </summary>
        public virtual string StateCode
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the Individual Title (Line 6)
        /// </summary>
        public virtual string Title
        {
            get;
            set;
        }

        /// <summary>
        /// Loads the entity based on the Id property.
        /// Maps the current values of properties in the ViewModel to the Entity.
        /// </summary>
        /// <returns>True if successful. False if it cannot be mapped.</returns>
        protected override bool MapToEntity()
        {
            if (base.MapToEntity())
            {
                MaxBaseUSPostalAddressEntity loEntity = this.Entity as MaxBaseUSPostalAddressEntity;
                if (null != loEntity)
                {
                    loEntity.Attention = this.Attention;
                    loEntity.City = this.City;
                    loEntity.Company = this.Company;
                    loEntity.DeliveryAddress = this.DeliveryAddress;
                    loEntity.Function = this.Function;
                    loEntity.Group = this.Group;
                    loEntity.OtherAddressInfo = this.OtherAddressInfo;
                    loEntity.PostalCode = this.PostalCode;
                    loEntity.StateCode = this.StateCode;
                    loEntity.Title = this.Title;
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// Maps the properties of the Entity to the properties of the ViewModel.
        /// </summary>
        /// <returns>True if the entity exists.</returns>
        protected override bool MapFromEntity()
        {
            if (base.MapFromEntity())
            {
                MaxBaseUSPostalAddressEntity loEntity = this.Entity as MaxBaseUSPostalAddressEntity;
                if (null != loEntity)
                {
                    this.Attention = loEntity.Attention;
                    this.City = loEntity.City;
                    this.Company = loEntity.Company;
                    this.DeliveryAddress = loEntity.DeliveryAddress;
                    this.Function = loEntity.Function;
                    this.Group = loEntity.Group;
                    this.OtherAddressInfo = loEntity.OtherAddressInfo;
                    this.PostalCode = loEntity.PostalCode;
                    this.StateCode = loEntity.StateCode;
                    this.Title = loEntity.Title; 
                    return true;
                }
            }

            return false;
        }
    }
}
