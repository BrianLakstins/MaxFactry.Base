// <copyright file="MaxIdIntegerEntity.cs" company="Lakstins Family, LLC">
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
// <change date="6/26/2014" author="Brian A. Lakstins" description="Copied from MaxIdEntity, rebased on MaxBaseEntity, removed AppId functionality.">
// <change date="11/10/2014" author="Brian A. Lakstins" description="Update to moving Get and Set methods from MaxData.">
// <change date="3/26/2015" author="Brian A. Lakstins" description="Moved to MaxFactry.Base">
// <change date="3/20/2024" author="Brian A. Lakstins" description="Happy birthday to my mom.  Sara Jean Lakstins (Cartwright) - 3/20/1944 to 3/14/2019.">
// <change date="3/24/2024" author="Brian A. Lakstins" description="Updated for changes namespaces">
// </changelog>
#endregion

namespace MaxFactry.Base.BusinessLayer
{
	using System;
    using MaxFactry.Base.DataLayer;
    using MaxFactry.Base.DataLayer.Library;

    /// <summary>
    /// Base Business Layer Entity
    /// </summary>
    public abstract class MaxIdIntegerEntity : MaxEntity
	{
        /// <summary>
        /// Length of the Id.
        /// </summary>
        private int _nIdLength = 16; 

		/// <summary>
        /// Initializes a new instance of the MaxIdIntegerEntity class
		/// </summary>
		/// <param name="loData">object to hold data</param>
		public MaxIdIntegerEntity(MaxData loData)
			: base(loData)
		{
		}

        /// <summary>
        /// Initializes a new instance of the MaxIdIntegerEntity class.
        /// </summary>
        /// <param name="loDataModelType">Type of data model.</param>
        public MaxIdIntegerEntity(Type loDataModelType) 
            : base(loDataModelType)
        {
        }

		/// <summary>
		/// Gets the unique Identifier for this entity
		/// </summary>
		public long Id
		{
			get
			{
                return this.GetLong(this.MaxIdDataModel.Id);
			}
		}

        /// <summary>
        /// Gets or sets the unique Identifier for this entity
        /// </summary>
        public string AlternateId
        {
            get
            {
                return this.GetString(this.MaxIdDataModel.AlternateId);
            }

            set
            {
                this.Set(this.MaxIdDataModel.AlternateId, value);
            }
        }

        /// <summary>
        /// Gets the date the entity was created
        /// </summary>
        public DateTime CreatedDate
        {
            get
            {
                return this.GetDateTime(this.MaxIdDataModel.CreatedDate);
            }
        }

        /// <summary>
        /// Gets the Data Model for this entity
        /// </summary>
        protected MaxIdIntegerDataModel MaxIdDataModel
        {
            get
            {
                return (MaxIdIntegerDataModel)MaxDataLibrary.GetDataModel(this.DataModelType);
            }
        }

        /// <summary>
        /// Gets or sets the length of the Id.
        /// </summary>
        protected int IdLength
        {
            get
            {
                return this._nIdLength;
            }

            set
            {
                this._nIdLength = value;
            }
        }

		/// <summary>
		/// Loads an entity based on the Id
		/// </summary>
        /// <param name="lnId">The Id of the entity to load</param>
		/// <returns>True if data was found, loaded, and not marked as deleted.  False could be not found, or deleted.</returns>
		public virtual bool LoadById(int lnId)
		{
            MaxData loData = MaxIdIntegerRepository.SelectById(this.Data, lnId);
			if (this.Load(loData))
			{
                return true;
			}

			return false;
		}

        /// <summary>
        /// Inserts a new record
        /// </summary>
        /// <returns>true if inserted.  False if cannot be inserted.</returns>
        public override bool Insert()
        {
            int lnLimit = 100;
            int lnTry = 0;
            bool lbR = false;
            //// Defaults to generate a 16 digit number until at least 2040 with a maximum of 10000000 records per second. 
            long lnStart = new DateTime(2012, 1, 1).Ticks;
            if (this.IdLength == 9)
            {
                //// Format of start Id Integer is YYMMDD000.  Provides for up to 1000 unique Ids per day and always the same length.
                DateTime ldIdDate = DateTime.UtcNow;
                lnStart = (ldIdDate.Year - 2000) * 10000000;
                lnStart += ldIdDate.Month * 100000;
                lnStart += ldIdDate.Day * 1000;

                MaxDataList loList = MaxIdIntegerRepository.SelectAllByCreatedDateRange(this.Data, ldIdDate.AddDays(-1), ldIdDate.AddHours(1));
                if (loList.Count > 0)
                {
                    for (int lnD = 0; lnD < loList.Count; lnD++)
                    {
                        long lnIdTest = MaxFactry.Core.MaxConvertLibrary.ConvertToLong(typeof(object), loList[lnD].Get(this.MaxIdDataModel.Id));
                        if (lnIdTest >= lnStart)
                        {
                            lnStart = lnIdTest + 1;
                        }
                    }
                }
            }
            else if (this.IdLength == 7)
            {
                //// Format of start Id Integer is YYDDD00.  Provides for up to 100 unique Ids per day and always the same length.
                DateTime ldIdDate = DateTime.UtcNow;
                lnStart = (ldIdDate.Year - 2000) * 100000;
                lnStart += ldIdDate.DayOfYear * 100;

                MaxDataList loList = MaxIdIntegerRepository.SelectAllByCreatedDateRange(this.Data, ldIdDate.AddDays(-1), ldIdDate.AddHours(1));
                if (loList.Count > 0)
                {
                    for (int lnD = 0; lnD < loList.Count; lnD++)
                    {
                        long lnIdTest = MaxFactry.Core.MaxConvertLibrary.ConvertToLong(typeof(object), loList[lnD].Get(this.MaxIdDataModel.Id));
                        if (lnIdTest >= lnStart)
                        {
                            lnStart = lnIdTest + 1;
                        }
                    }
                }
            }

            while (!lbR && lnTry < lnLimit)
            {
                try
                {
                    lbR = this.Insert(lnStart + lnTry);
                }
                catch
                {
                    lbR = false;
                }

                lnTry++;
            }

            return lbR;
        }

        /// <summary>
        /// Inserts a new record
        /// </summary>
        /// <param name="lnId">Id for the new record</param>
        /// <returns>true if inserted.  False if cannot be inserted.</returns>
        public virtual bool Insert(long lnId)
        {
            this.Set(this.MaxIdDataModel.Id, lnId);
            this.Set(this.MaxIdDataModel.CreatedDate, DateTime.UtcNow);
            return base.Insert();
        }
	}
}
