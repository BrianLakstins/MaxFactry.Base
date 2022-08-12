// <copyright file="MaxBasePersonViewModel.cs" company="Lakstins Family, LLC">
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
    public abstract class MaxBasePersonViewModel : MaxBaseIdViewModel
    {
        /// <summary>
        /// Initializes a new instance of the MaxBasePersonViewModel class
        /// </summary>
        public MaxBasePersonViewModel()
            : base()
        {
        }

        /// <summary>
        /// Initializes a new instance of the MaxBasePersonViewModel class
        /// </summary>
        /// <param name="loEntity">Entity to use as data.</param>
        public MaxBasePersonViewModel(MaxEntity loEntity)
            : base(loEntity)
        {
        }

        public MaxBasePersonViewModel(string lsId) : base(lsId)
        {
        }

        /// <summary>
        /// Gets or sets the Country where the person was born.
        /// </summary>
        public virtual string BirthCountry
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the Date of birth of the person.
        /// </summary>
        public virtual string BirthDate
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the First Name of the person when they were born.
        /// </summary>
        public virtual string BirthFirstName
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the Last Name of the person when they were born.
        /// </summary>
        public virtual string BirthLastName
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the Middle Name of the person when they were born.
        /// </summary>
        public virtual string BirthMiddleName
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the Place where the person was born.
        /// </summary>
        public virtual string BirthPlace
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the First Name of the person.
        /// </summary>
        public virtual string CurrentFirstName
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the Last Name of the person.
        /// </summary>
        public virtual string CurrentLastName
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the Middle Name of the person.
        /// </summary>
        public virtual string CurrentMiddleName
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the Date of death of the person.
        /// </summary>
        public virtual string DeathDate
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the Full name of the person.
        /// </summary>
        public virtual string Name
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the Sex of the person.
        /// </summary>
        public virtual string Sex
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
                MaxBasePersonEntity loEntity = this.Entity as MaxBasePersonEntity;
                if (null != loEntity)
                {
                    loEntity.BirthCountry = this.BirthCountry;
                    if (null != this.BirthDate)
                    {
                        loEntity.BirthDate = MaxConvertLibrary.ConvertToDateTimeUtc(this.GetType(), this.BirthDate);
                    }

                    loEntity.BirthFirstName = this.BirthFirstName;
                    loEntity.BirthLastName = this.BirthLastName;
                    loEntity.BirthMiddleName = this.BirthMiddleName;
                    loEntity.BirthPlace = this.BirthPlace;
                    loEntity.CurrentFirstName = this.CurrentFirstName;
                    loEntity.CurrentLastName = this.CurrentLastName;
                    loEntity.CurrentMiddleName = this.CurrentMiddleName;
                    if (null != this.DeathDate)
                    {
                        loEntity.DeathDate = MaxConvertLibrary.ConvertToDateTimeUtc(this.GetType(), this.DeathDate);
                    }

                    loEntity.Name = this.Name;
                    loEntity.Sex = this.Sex;
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
                MaxBasePersonEntity loEntity = this.Entity as MaxBasePersonEntity;
                if (null != loEntity)
                {
                    this.BirthCountry = loEntity.BirthCountry;
                    if (loEntity.BirthDate > DateTime.MinValue)
                    {
                        this.BirthDate = MaxConvertLibrary.ConvertToString(this.GetType(), loEntity.BirthDate);
                    }

                    this.BirthFirstName = loEntity.BirthFirstName;
                    this.BirthLastName = loEntity.BirthLastName;
                    this.BirthMiddleName = loEntity.BirthMiddleName;
                    this.BirthPlace = loEntity.BirthPlace;
                    this.CurrentFirstName = loEntity.CurrentFirstName;
                    this.CurrentLastName = loEntity.CurrentLastName;
                    this.CurrentMiddleName = loEntity.CurrentMiddleName;
                    if (loEntity.DeathDate > DateTime.MinValue)
                    {
                        this.DeathDate = MaxConvertLibrary.ConvertToString(this.GetType(), loEntity.DeathDate);
                    }

                    this.Name = loEntity.Name;
                    this.Sex = loEntity.Sex;
                    return true;
                }
            }

            return false;
        }
    }
}
