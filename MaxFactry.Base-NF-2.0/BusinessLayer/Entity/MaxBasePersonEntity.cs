// <copyright file="MaxBasePersonEntity.cs" company="Lakstins Family, LLC">
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
// <change date="11/10/2014" author="Brian A. Lakstins" description="Update to moving Get and Set methods from MaxData.">
// </changelog>
#endregion

namespace MaxFactry.Base.BusinessLayer
{
    using System;
    using MaxFactry.Base.BusinessLayer;
    using MaxFactry.Base.DataLayer;

    /// <summary>
    /// Entity to represent content in a web site.
    /// </summary>
    public abstract class MaxBasePersonEntity : MaxBaseIdEntity
    {
        /// <summary>
        /// Initializes a new instance of the MaxBasePersonEntity class
        /// </summary>
        /// <param name="loData">object to hold data</param>
        public MaxBasePersonEntity(MaxData loData)
            : base(loData)
        {
        }

        /// <summary>
        /// Initializes a new instance of the MaxBasePersonEntity class.
        /// </summary>
        /// <param name="loDataModelType">Type of data model.</param>
        public MaxBasePersonEntity(Type loDataModelType)
            : base(loDataModelType)
        {
        }

        /// <summary>
        /// Gets or sets Full name of the person.
        /// </summary>
        public virtual string Name
        {
            get
            {
                string lsR = this.GetString(this.MaxPersonDataModel.Name);
                if (lsR.Length == 0)
                {
                    lsR = this.CurrentLastName.Trim() + ", " + this.CurrentFirstName.Trim();
                    if (this.CurrentMiddleName.Trim().Length > 0)
                    {
                        lsR += " " + this.CurrentMiddleName.Trim();
                    }
                }

                return lsR;
            }

            set
            {
                this.Set(this.MaxPersonDataModel.Name, value);
            }
        }

        /// <summary>
        /// Gets or sets the First Name of the person when they were born.
        /// </summary>
        public virtual string BirthFirstName
        {
            get
            {
                return this.GetString(this.MaxPersonDataModel.BirthFirstName);
            }

            set
            {
                this.Set(this.MaxPersonDataModel.BirthFirstName, value);
            }
        }

        /// <summary>
        /// Gets or sets the Middle Name of the person when they were born.
        /// </summary>
        public virtual string BirthMiddleName
        {
            get
            {
                return this.GetString(this.MaxPersonDataModel.BirthMiddleName);
            }

            set
            {
                this.Set(this.MaxPersonDataModel.BirthMiddleName, value);
            }
        }

        /// <summary>
        /// Gets or sets the Last Name of the person when they were born.
        /// </summary>
        public virtual string BirthLastName
        {
            get
            {
                return this.GetString(this.MaxPersonDataModel.BirthLastName);
            }

            set
            {
                this.Set(this.MaxPersonDataModel.BirthLastName, value);
            }
        }

        /// <summary>
        /// Gets or sets the First Name of the person.
        /// </summary>
        public virtual string CurrentFirstName
        {
            get
            {
                return this.GetString(this.MaxPersonDataModel.CurrentFirstName);
            }

            set
            {
                this.Set(this.MaxPersonDataModel.CurrentFirstName, value);
            }
        }

        /// <summary>
        /// Gets or sets the Middle Name of the person.
        /// </summary>
        public virtual string CurrentMiddleName
        {
            get
            {
                return this.GetString(this.MaxPersonDataModel.CurrentMiddleName);
            }

            set
            {
                this.Set(this.MaxPersonDataModel.CurrentMiddleName, value);
            }
        }

        /// <summary>
        /// Gets or sets the Last Name of the person.
        /// </summary>
        public virtual string CurrentLastName
        {
            get
            {
                return this.GetString(this.MaxPersonDataModel.CurrentLastName);
            }

            set
            {
                this.Set(this.MaxPersonDataModel.CurrentLastName, value);
            }
        }

        /// <summary>
        /// Gets or sets the Date of birth of the person.
        /// </summary>
        public virtual DateTime BirthDate
        {
            get
            {
                return this.GetDateTime(this.MaxPersonDataModel.BirthDate);
            }

            set
            {
                this.Set(this.MaxPersonDataModel.BirthDate, value);
            }
        }

        /// <summary>
        /// Gets or sets the Place where the person was born.
        /// </summary>
        public virtual string BirthPlace
        {
            get
            {
                return this.GetString(this.MaxPersonDataModel.BirthPlace);
            }

            set
            {
                this.Set(this.MaxPersonDataModel.BirthPlace, value);
            }
        }

        /// <summary>
        /// Gets or sets the Country where the person was born.
        /// </summary>
        public virtual string BirthCountry
        {
            get
            {
                return this.GetString(this.MaxPersonDataModel.BirthCountry);
            }

            set
            {
                this.Set(this.MaxPersonDataModel.BirthCountry, value);
            }
        }

        /// <summary>
        /// Gets or sets the Date of death of the person.
        /// </summary>
        public virtual DateTime DeathDate
        {
            get
            {
                return this.GetDateTime(this.MaxPersonDataModel.DeathDate);
            }

            set
            {
                this.Set(this.MaxPersonDataModel.DeathDate, value);
            }
        }

        /// <summary>
        /// Gets or sets the Sex of the person.
        /// </summary>
        public virtual string Sex
        {
            get
            {
                return this.GetString(this.MaxPersonDataModel.Sex);
            }

            set
            {
                this.Set(this.MaxPersonDataModel.Sex, value);
            }
        }

        /// <summary>
        /// Gets the Data Model for this entity
        /// </summary>
        protected MaxBasePersonDataModel MaxPersonDataModel
        {
            get
            {
                return (MaxBasePersonDataModel)MaxDataLibrary.GetDataModel(this.DataModelType);
            }
        }

        /// <summary>
        /// Gets the number of years between two dates
        /// </summary>
        /// <param name="ldStart">Start date</param>
        /// <param name="ldEnd">End date</param>
        /// <returns>Number of years</returns>
        public static int GetDateDiffYear(DateTime ldStart, DateTime ldEnd)
        {
            int lnR = 0;
            if (ldEnd > ldStart)
            {
                lnR = ldEnd.Year - ldStart.Year;
                if (ldEnd.DayOfYear < ldStart.DayOfYear)
                {
                    lnR--;
                }
            }

            return lnR;
        }

        /// <summary>
        /// Gets the difference between two dates described as text
        /// </summary>
        /// <param name="ldStart">Start date</param>
        /// <param name="ldEnd">End date</param>
        /// <returns>Number of years, months, or days</returns>
        public static string GetDateDiffText(DateTime ldStart, DateTime ldEnd)
        {
            int lnTime = GetDateDiffYear(ldStart, ldEnd);
            string lsDuration = "Years";
            //// less then  1 year
            if (lnTime == 0) 
            {
                lsDuration = "Months";
                lnTime = ldEnd.Month - ldStart.Month;
                if (lnTime < 0)
                {
                    lnTime += 12;
                }

                if (ldEnd.Day < ldStart.Day)
                {
                    lnTime--;
                }

                //// less than 1 month
                if (lnTime == 0) 
                {
                    lsDuration = "Days";
                    lnTime = (ldEnd - ldStart).Days;
                    if (lnTime == 1)
                    {
                        lsDuration = "Day";
                    }
                }
                else if (lnTime == 1)
                {
                    lsDuration = "Month";
                }
            }
            else if (lnTime == 1)
            {
                lsDuration = "Year";
            }

            string lsAge = lnTime.ToString() + " " + lsDuration;
            return lsAge;
        }

        /// <summary>
        /// Gets the age in years
        /// </summary>
        /// <param name="loAgeDate">Date to check for age</param>
        /// <returns>Age in years</returns>
        public int GetAgeYear(DateTime loAgeDate)
        {
            return GetDateDiffYear(this.BirthDate, loAgeDate);
        }

        /// <summary>
        /// Gets text describing the age
        /// </summary>
        /// <param name="loAgeDate">Date to check for age</param>
        /// <returns>Number of years, months, or days</returns>
        public string GetAgeText(DateTime loAgeDate)
        {
            return GetDateDiffText(this.BirthDate, loAgeDate);
        }

        /// <summary>
        /// Default is to sort by created date.
        /// </summary>
        /// <returns>Sortable created date.</returns>
        public override string GetDefaultSortString()
        {
            return this.CurrentLastName + "," + this.CurrentFirstName + " " + this.CurrentMiddleName + base.GetDefaultSortString();
        }
    }
}
