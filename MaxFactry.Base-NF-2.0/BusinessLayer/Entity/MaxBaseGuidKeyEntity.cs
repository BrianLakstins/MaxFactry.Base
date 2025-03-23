// <copyright file="MaxBaseGuidKeyEntity.cs" company="Lakstins Family, LLC">
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
// <change date="3/20/2024" author="Brian A. Lakstins" description="Happy birthday to my mom.  Sara Jean Lakstins (Cartwright) - 3/20/1944 to 3/14/2019.">
// <change date="3/23/2024" author="Brian A. Lakstins" description="Initial creation">
// <change date="3/24/2024" author="Brian A. Lakstins" description="Updated for changes namespaces">
// <change date="3/30/2024" author="Brian A. Lakstins" description="Add method to load by Id.">
// <change date="1/21/2025" author="Brian A. Lakstins" description="Add SetId method.">
// <change date="3/22/2025" author="Brian A. Lakstins" description="Integrate with changes to base insert.">
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
    public abstract class MaxBaseGuidKeyEntity : MaxBaseEntity
    {
        /// <summary>
        /// Initializes a new instance of the MaxBaseIdEntity class
        /// </summary>
        /// <param name="loData">object to hold data</param>
        public MaxBaseGuidKeyEntity(MaxData loData) : base(loData)
        {
        }

        /// <summary>
        /// Initializes a new instance of the MaxBaseIdEntity class.
        /// </summary>
        /// <param name="loDataModelType">Type of data model.</param>
        public MaxBaseGuidKeyEntity(Type loDataModelType)
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
                return this.GetGuid(this.MaxBaseGuidKeyDataModel.Id);
            }
        }

        /// <summary>
        /// Gets the Data Model for this entity
        /// </summary>
        protected MaxBaseGuidKeyDataModel MaxBaseGuidKeyDataModel
        {
            get
            {
                return (MaxBaseGuidKeyDataModel)MaxDataLibrary.GetDataModel(this.DataModelType);
            }
        }

        /// <summary>
        /// Sets the Id.
        /// </summary>
        /// <param name="loId">Id to use for this entity.</param>
        public virtual void SetId(Guid loId)
        {
            this.Set(this.MaxBaseGuidKeyDataModel.Id, loId);
        }

        /// <summary>
        /// Inserts a new record
        /// </summary>
        /// <param name="loId">Id for the new record</param>
        /// <returns>true if inserted.  False if cannot be inserted.</returns>
        public virtual bool Insert(int lnRetry, Guid loId)
        {
            if (!Guid.Empty.Equals(loId))
            {
                this.Set(this.MaxBaseGuidKeyDataModel.Id, loId);
            }

            bool lbR = this.Insert(lnRetry);
            return lbR;
        }

        /// <summary>
        /// Inserts a new record
        /// </summary>
        /// <param name="loId">Id for the new record</param>
        /// <returns>true if inserted.  False if cannot be inserted.</returns>
        public virtual bool Insert(Guid loId)
        {
            return this.Insert(5, loId);
        }

        protected bool InsertNewGuid(int lnRetry)
        {
            bool lbR = false;
            int lnTry = 0;
            Guid loId = Guid.NewGuid();
            lbR = this.Insert(0, loId);
            while (!lbR && lnTry <= lnRetry)
            {                
                MaxLogLibrary.Log(new MaxLogEntryStructure(this.GetType(), "InsertNewGuid", MaxEnumGroup.LogError, "Insert attempt {0} failed.", lnTry + 1));
                System.Threading.Thread.Sleep(100);
                loId = Guid.NewGuid();
                lbR = this.Insert(0, loId);
                lnTry++;
            }

            return lbR;
        }

        public override bool Insert()
        {
            bool lbR = false;
            if (Guid.Empty.Equals(this.Id))
            {
                lbR = this.InsertNewGuid(5);
            }
            else
            {
                lbR = base.Insert();
            }

            return lbR;
        }

        public virtual bool LoadByIdCache(Guid loId)
        {
            MaxData loData = new MaxData(this.Data.DataModel);
            loData.Set(this.MaxBaseGuidKeyDataModel.Id, loId);
            return this.LoadByDataKeyCache(loData.DataModel.GetDataKey(loData));
        }
    }
}
