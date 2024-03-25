// <copyright file="MaxBaseUSPostalAddressEntity.cs" company="Lakstins Family, LLC">
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
// <change date="2/26/2014" author="Brian A. Lakstins" description="Initial Release">
// <change date="2/1/2016" author="Brian A. Lakstins" description="Add a way to load by postal address.">
// <change date="3/20/2024" author="Brian A. Lakstins" description="Happy birthday to my mom.  Sara Jean Lakstins (Cartwright) - 3/20/1944 to 3/14/2019.">
// <change date="3/23/2024" author="Brian A. Lakstins" description="Updated to use methods from parent.">
// <change date="3/24/2024" author="Brian A. Lakstins" description="Updated for changes namespaces">
// </changelog>
#endregion

namespace MaxFactry.Base.BusinessLayer
{
    using System;
    using MaxFactry.Base.DataLayer;
    using MaxFactry.Base.DataLayer.Library;

    /// <summary>
    /// Entity to represent content in a web site.
    /// </summary>
    public abstract class MaxBaseUSPostalAddressEntity : MaxBaseIdEntity
    {
        /// <summary>
        /// Initializes a new instance of the MaxBaseUSPostalAddressEntity class
        /// </summary>
        /// <param name="loData">object to hold data</param>
        public MaxBaseUSPostalAddressEntity(MaxData loData)
            : base(loData)
        {
        }

        /// <summary>
        /// Initializes a new instance of the MaxBaseUSPostalAddressEntity class.
        /// </summary>
        /// <param name="loDataModelType">Type of data model.</param>
        public MaxBaseUSPostalAddressEntity(Type loDataModelType)
            : base(loDataModelType)
        {
        }

        /// <summary>
        /// Gets or sets the Name of person at the address (Line 5)
        /// </summary>
        public string Attention
        {
            get
            {
                return this.GetString(this.MaxUSPostalAddressDataModel.Attention);
            }

            set
            {
                this.Set(this.MaxUSPostalAddressDataModel.Attention, value);
            }
        }

        /// <summary>
        /// Gets or sets the City
        /// Spell city names in their entirety when possible. When it is not possible, use the 13 character abbreviations from the USPS City State File.
        /// </summary>
        public string City
        {
            get
            {
                return this.GetString(this.MaxUSPostalAddressDataModel.City);
            }

            set
            {
                this.Set(this.MaxUSPostalAddressDataModel.City, value);
            }
        }

        /// <summary>
        /// Gets or sets the Name of the company at the address (Line #9)
        /// </summary>
        public string Company
        {
            get
            {
                return this.GetString(this.MaxUSPostalAddressDataModel.Company);
            }

            set
            {
                this.Set(this.MaxUSPostalAddressDataModel.Company, value);
            }
        }

        /// <summary>
        /// Gets or sets US Post office delivery address (Line #10)
        /// </summary>
        public string DeliveryAddress
        {
            get
            {
                return this.GetString(this.MaxUSPostalAddressDataModel.DeliveryAddress);
            }

            set
            {
                this.Set(this.MaxUSPostalAddressDataModel.DeliveryAddress, value);
            }
        }

        /// <summary>
        /// Gets or sets the Functional Title (Line 7)
        /// </summary>
        public string Function
        {
            get
            {
                return this.GetString(this.MaxUSPostalAddressDataModel.Function);
            }

            set
            {
                this.Set(this.MaxUSPostalAddressDataModel.Function, value);
            }
        }

        /// <summary>
        /// Gets or sets the Group, Department, Division Name (Line 8)
        /// </summary>
        public string Group
        {
            get
            {
                return this.GetString(this.MaxUSPostalAddressDataModel.Group);
            }

            set
            {
                this.Set(this.MaxUSPostalAddressDataModel.Group, value);
            }
        }

        /// <summary>
        /// Gets or sets the Other address information
        /// </summary>
        public string OtherAddressInfo
        {
            get
            {
                return this.GetString(this.MaxUSPostalAddressDataModel.OtherAddressInfo);
            }

            set
            {
                this.Set(this.MaxUSPostalAddressDataModel.OtherAddressInfo, value);
            }
        }

        /// <summary>
        /// Gets or sets the Zip Code or Zip+4 Number
        /// </summary>
        public string PostalCode
        {
            get
            {
                return this.GetString(this.MaxUSPostalAddressDataModel.PostalCode);
            }

            set
            {
                this.Set(this.MaxUSPostalAddressDataModel.PostalCode, value);
            }
        }

        /// <summary>
        /// Gets or sets the state
        /// Use 2 letter USPS State Abbreviations.
        /// </summary>
        public string StateCode
        {
            get
            {
                return this.GetString(this.MaxUSPostalAddressDataModel.StateCode);
            }

            set
            {
                this.Set(this.MaxUSPostalAddressDataModel.StateCode, value);
            }
        }

        /// <summary>
        /// Gets or sets the Individual Title (Line 6)
        /// </summary>
        public string Title
        {
            get
            {
                return this.GetString(this.MaxUSPostalAddressDataModel.Title);
            }

            set
            {
                this.Set(this.MaxUSPostalAddressDataModel.Title, value);
            }
        }

        /// <summary>
        /// Gets the Data Model for this entity
        /// </summary>
        protected MaxBaseUSPostalAddressDataModel MaxUSPostalAddressDataModel
        {
            get
            {
                return (MaxBaseUSPostalAddressDataModel)MaxDataLibrary.GetDataModel(this.DataModelType);
            }
        }

        /// <summary>
        /// Loads all records that match the delivery address
        /// </summary>
        /// <param name="lsDeliverAddress">Delivery address to match</param>
        /// <returns>List of addresses</returns>
        public MaxEntityList LoadAllByDeliveryAddress(string lsDeliverAddress)
        {
            return this.LoadAllByProperty(this.MaxUSPostalAddressDataModel.DeliveryAddress, lsDeliverAddress);
        }

        /// <summary>
        /// Loads all records that match the postal code
        /// </summary>
        /// <param name="lsPostalCode">Postal code to match</param>
        /// <returns>List of addresses</returns>
        public MaxEntityList LoadAllByPostalCode(string lsPostalCode)
        {
            return this.LoadAllByProperty(this.MaxUSPostalAddressDataModel.PostalCode, lsPostalCode);
        }
    }
}
