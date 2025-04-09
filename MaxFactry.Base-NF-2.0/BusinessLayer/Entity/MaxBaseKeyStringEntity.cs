// <copyright file="MaxBaseKeyStringEntity.cs" company="Lakstins Family, LLC">
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

    /// <summary>
    /// Base Business Layer Entity using a string as the primary key
    /// </summary>
    public abstract class MaxBaseKeyStringEntity : MaxBaseEntity
    {
        /// <summary>
        /// Initializes a new instance of the MaxBaseIdEntity class
        /// </summary>
        /// <param name="loData">object to hold data</param>
        public MaxBaseKeyStringEntity(MaxData loData) : base(loData)
        {
        }

        /// <summary>
        /// Initializes a new instance of the MaxBaseIdEntity class.
        /// </summary>
        /// <param name="loDataModelType">Type of data model.</param>
        public MaxBaseKeyStringEntity(Type loDataModelType)
            : base(loDataModelType)
        {
        }

        /// <summary>
        /// Gets the unique Identifier for this entity
        /// </summary>
        public virtual string Id
        {
            get
            {
                return this.GetString(this.MaxBaseKeyStringDataModel.Id);
            }
        }

        /// <summary>
        /// Gets the Data Model for this entity
        /// </summary>
        protected MaxBaseKeyStringDataModel MaxBaseKeyStringDataModel
        {
            get
            {
                return (MaxBaseKeyStringDataModel)MaxDataLibrary.GetDataModel(this.DataModelType);
            }
        }

        /// <summary>
        /// Sets the Id.
        /// </summary>
        /// <param name="loId">Id to use for this entity.</param>
        public virtual void SetId(string lsId)
        {
            this.Set(this.MaxBaseKeyStringDataModel.Id, lsId);
        }

        protected override void SetInitial()
        {
            base.SetInitial();
            if (string.Empty == this.Id)
            {
                this.SetId(Guid.NewGuid().ToString());
            }
        }

        public virtual bool LoadByIdCache(string lsId)
        {
            MaxData loData = new MaxData(this.Data.DataModel);
            loData.Set(this.MaxBaseKeyStringDataModel.Id, lsId);
            return this.LoadByDataKeyCache(loData.DataModel.GetDataKey(loData));
        }
    }
}
