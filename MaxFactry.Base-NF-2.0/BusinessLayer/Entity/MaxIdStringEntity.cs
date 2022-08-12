// <copyright file="MaxIdStringEntity.cs" company="Lakstins Family, LLC">
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
// </changelog>
#endregion

namespace MaxFactry.Base.BusinessLayer
{
	using System;
    using MaxFactry.Core;
    using MaxFactry.Base.DataLayer;

	/// <summary>
	/// Base Business Layer Entity
	/// </summary>
    public abstract class MaxIdStringEntity : MaxEntity
	{
		/// <summary>
        /// Initializes a new instance of the MaxIdStringEntity class
		/// </summary>
		/// <param name="loData">object to hold data</param>
		public MaxIdStringEntity(MaxData loData)
			: base(loData)
		{
		}

        /// <summary>
        /// Initializes a new instance of the MaxIdStringEntity class.
        /// </summary>
        /// <param name="loDataModelType">Type of data model.</param>
        public MaxIdStringEntity(Type loDataModelType) 
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
        protected MaxIdStringDataModel MaxIdDataModel
        {
            get
            {
                return (MaxIdStringDataModel)MaxDataLibrary.GetDataModel(this.DataModelType);
            }
        }

		/// <summary>
		/// Loads an entity based on the Id
		/// </summary>
        /// <param name="lsId">The Id of the entity to load</param>
		/// <returns>True if data was found, loaded, and not marked as deleted.  False could be not found, or deleted.</returns>
		public virtual bool LoadById(string lsId)
		{
            MaxData loData = MaxIdStringRepository.SelectById(this.Data, lsId);
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
            int lnLimit = 10;
            int lnTry = 0;
            bool lbR = false;
            while (!lbR && lnTry < lnLimit)
            {
                lbR = this.Insert(Guid.NewGuid().ToString());
                lnTry++;
            }

            return lbR;
        }

        /// <summary>
        /// Inserts a new record
        /// </summary>
        /// <param name="lsId">Id for the new record</param>
        /// <returns>true if inserted.  False if cannot be inserted.</returns>
        public virtual bool Insert(string lsId)
        {
            this.Set(this.MaxIdDataModel.Id, lsId);
            this.Set(this.MaxIdDataModel.CreatedDate, DateTime.UtcNow);
            return base.Insert();
        }
	}
}
