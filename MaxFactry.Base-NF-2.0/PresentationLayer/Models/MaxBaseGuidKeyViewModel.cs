// <copyright file="MaxBaseGuidKeyViewModel.cs" company="Lakstins Family, LLC">
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
// <change date="1/21/2025" author="Brian A. Lakstins" description="Initial creation">
// <change date="6/9/2025" author="Brian A. Lakstins" description="Update method used to load when Id is available">
// <change date="6/21/2025" author="Brian A. Lakstins" description="Update base class">
// <change date="7/25/2025" author="Brian A. Lakstins" description="Fix issue loading after insert">
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
    public abstract class MaxBaseGuidKeyViewModel : MaxBaseViewModel
    {
        /// <summary>
        /// Initializes a new instance of the MaxBaseIdViewModel class.
        /// </summary>
        public MaxBaseGuidKeyViewModel()
            : base()
        {
        }

        /// <summary>
        /// Initializes a new instance of the MaxBaseIdViewModel class.
        /// </summary>
        /// <param name="loEntity">Entity to use as data.</param>
        public MaxBaseGuidKeyViewModel(MaxEntity loEntity)
            : base(loEntity)
        {
        }

        public MaxBaseGuidKeyViewModel(string lsId)
        {
            this.CreateEntity();
            this.Load(lsId);
        }

        /// <summary>
        /// Gets or sets the unique Identifier for this entity. Set at record creation.
        /// </summary>
        public virtual string Id
        {
            get;
            set;
        }

        /// <summary>
        /// Loads the content of the Entity associated with this ViewModel
        /// </summary>
        /// <returns>true if successful</returns>
        public override bool EntityLoad()
        {
            bool lbR = base.EntityLoad();
            if (!lbR)
            {
                if (null != this.Id && string.Empty != this.Id && this.Entity is MaxBaseGuidKeyEntity)
                {
                    if (((MaxBaseGuidKeyEntity)this.Entity).LoadByDataKeyCache(this.Id))
                    {
                        this.IsEntityLoaded = true;
                        lbR = true;
                    }
                }
            }

            return lbR;
        }

        public virtual bool Load(string lsId)
        {
            bool lbR = false;
            if (null != lsId && string.Empty != lsId)
            {
                this.Id = lsId;
                lbR = this.Load();
            }
                
            return lbR;
        }

        /// <summary>
        /// Reloads the current entity
        /// </summary>
        /// <returns></returns>
        public virtual bool Reload()
        {
            this.IsEntityLoaded = false;
            this.Entity.Clear();
            if (this.EntityLoad())
            {
                return this.Load();
            }

            return false;
        }

        /// <summary>
        /// Saves the model
        /// </summary>
        /// <returns>true if successful</returns>
        public override bool Save()
        {
            if (this.MapToEntity())
            {
                if (this.Entity is MaxBaseGuidKeyEntity)
                {
                    MaxBaseGuidKeyEntity loEntity = (MaxBaseGuidKeyEntity)this.Entity;
                    bool lbR = false;
                    if (loEntity.Id.Equals(Guid.Empty))
                    {
                        if (loEntity.Insert())
                        {
                            this.DataKey = loEntity.DataKey;
                            lbR = this.Load();
                        }
                    }
                    else
                    {
                        if (loEntity.Update())
                        {
                            lbR = true;
                            if (this.IsEntityLoaded)
                            {
                                this.Load();
                            }
                        }
                    }

                    return lbR;
                }
            }

            return false;
        }

        /// <summary>
        /// Deletes the model
        /// </summary>
        /// <returns>true if successful</returns>
        public virtual bool Delete()
        {
            if (this.Entity is MaxBaseGuidKeyEntity && this.Load(this.Id))
            {
                MaxBaseGuidKeyEntity loEntity = (MaxBaseGuidKeyEntity)this.Entity;
                if (!Guid.Empty.Equals(loEntity.Id))
                {
                    return loEntity.Delete();
                }
            }

            return false;
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
                if (this.Entity is MaxBaseGuidKeyEntity)
                {
                    MaxBaseGuidKeyEntity loEntity = (MaxBaseGuidKeyEntity)this.Entity;
                    if (null != this.Id && string.Empty != this.Id)
                    {
                        Guid loId = MaxConvertLibrary.ConvertToGuid(typeof(object), this.Id);
                        if (!Guid.Empty.Equals(loId))
                        {
                            loEntity.SetId(loId);
                        }
                    }

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
                if (this.Entity is MaxBaseGuidKeyEntity)
                {
                    MaxBaseGuidKeyEntity loEntity = (MaxBaseGuidKeyEntity)this.Entity;
                    if (!loEntity.Id.Equals(Guid.Empty))
                    {
                        this.Id = loEntity.Id.ToString();
                        this.OriginalValues.Add("Id", this.Id);
                    }

                    return true;
                }
            }

            return false;
        }
    }
}
