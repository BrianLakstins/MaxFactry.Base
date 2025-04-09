// <copyright file="MaxBaseKeyGuidEntity.cs" company="Lakstins Family, LLC">
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
// <change date="4/9/2025" author="Brian A. Lakstins" description="Initial creation">
// </changelog>
#endregion

namespace MaxFactry.Base.BusinessLayer
{
    using System;
    using MaxFactry.Base.DataLayer;
    using MaxFactry.Base.DataLayer.Library;
    using MaxFactry.Core;

    /// <summary>
    /// Base Business Layer Entity.  Designed to replace MaxBaseIdEntity to use Key in a generic fashion instead of Id.
    /// </summary>
    public abstract class MaxBaseKeyGuidEntity : MaxBaseEntity
    {
        /// <summary>
        /// Initializes a new instance of the MaxBaseIdEntity class
        /// </summary>
        /// <param name="loData">object to hold data</param>
        public MaxBaseKeyGuidEntity(MaxData loData) : base(loData)
        {
        }

        /// <summary>
        /// Initializes a new instance of the MaxBaseIdEntity class.
        /// </summary>
        /// <param name="loDataModelType">Type of data model.</param>
        public MaxBaseKeyGuidEntity(Type loDataModelType)
            : base(loDataModelType)
        {
        }

        /// <summary>
        /// Gets the unique Identifier for this entity
        /// </summary>
        public virtual Guid Id
        {
            get
            {
                return this.GetGuid(this.MaxBaseKeyGuidDataModel.Id);
            }
        }

        /// <summary>
        /// Gets the Data Model for this entity
        /// </summary>
        protected MaxBaseKeyGuidDataModel MaxBaseKeyGuidDataModel
        {
            get
            {
                return (MaxBaseKeyGuidDataModel)MaxDataLibrary.GetDataModel(this.DataModelType);
            }
        }

        /// <summary>
        /// Sets the Id.
        /// </summary>
        /// <param name="loId">Id to use for this entity.</param>
        public virtual void SetId(Guid loId)
        {
            this.Set(this.MaxBaseKeyGuidDataModel.Id, loId);
        }

        protected override void SetInitial()
        {
            base.SetInitial();
            if (Guid.Empty == this.Id)
            {
                this.SetId(Guid.NewGuid());
            }
        }

        public virtual bool LoadByIdCache(Guid loId)
        {
            MaxData loData = new MaxData(this.Data.DataModel);
            loData.Set(this.MaxBaseKeyGuidDataModel.Id, loId);
            return this.LoadByDataKeyCache(loData.DataModel.GetDataKey(loData));
        }
    }
}
