// <copyright file="MaxIdGuidViewModel.cs" company="Lakstins Family, LLC">
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
// <change date="5/27/2015" author="Brian A. Lakstins" description="Added keeping track of original values.">
// <change date="6/1/2015" author="Brian A. Lakstins" description="Updated to only map from an enitity that has data loaded.">
// <change date="6/7/2015" author="Brian A. Lakstins" description="Updated to insert entity if it's Id is empty.">
// <change date="6/8/2015" author="Brian A. Lakstins" description="Updated to no longer load before saving (set the Id of the entity instead).">
// <change date="7/14/2015" author="Brian A. Lakstins" description="Fixed setting Id to be when Id is available.">
// <change date="7/29/2015" author="Brian A. Lakstins" description="Move setting Id from Save to MapToEntity.">
// <change date="7/14/2016" author="Brian A. Lakstins" description="Add reseting the entity to default when the ID is Guid.Empty.">
// <change date="10/3/2019" author="Brian A. Lakstins" description="Add a reload method.">
// <change date="8/3/2020" author="Brian A. Lakstins" description="Change base class.">
// <change date="3/20/2024" author="Brian A. Lakstins" description="Happy birthday to my mom.  Sara Jean Lakstins (Cartwright) - 3/20/1944 to 3/14/2019.">
// <change date="3/23/2024" author="Brian A. Lakstins" description="Updated for change to dependency">
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
    public abstract class MaxIdGuidViewModel : MaxBaseEntityViewModel
    {
        /// <summary>
        /// Initializes a new instance of the MaxIdGuidViewModel class
        /// </summary>
        public MaxIdGuidViewModel()
            : base()
        {
        }

        /// <summary>
        /// Initializes a new instance of the MaxIdGuidViewModel class
        /// </summary>
        /// <param name="loEntity">Entity to use as data.</param>
        public MaxIdGuidViewModel(MaxEntity loEntity)
            : base(loEntity)
        {
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
        /// Gets or sets the alternate Id
        /// </summary>
        public virtual string AlternateId
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
            if (null != this.Id && this.Id.Length > 0)
            {
                return this.EntityLoad(this.Id);
            }

            return false;
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
        public virtual bool Save()
        {
            if (this.MapToEntity())
            {
                MaxIdGuidEntity loEntity = this.Entity as MaxIdGuidEntity;
                if (null != loEntity)
                {
                    bool lbR = false;
                    if (loEntity.Id.Equals(Guid.Empty))
                    {
                        if (loEntity.Insert())
                        {
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
            if (null != this.Id)
            {
                if (this.EntityLoad(this.Id))
                {
                    MaxIdGuidEntity loEntity = this.Entity as MaxIdGuidEntity;
                    if (null != loEntity && !Guid.Empty.Equals(loEntity.Id))
                    {
                        return loEntity.Delete();
                    }
                }
            }

            return false;
        }

        /// <summary>
        /// Loads the data for the entity matching the Id only if it is not already matching.
        /// </summary>
        /// <param name="loId">Id of MaxIdEntity to load.</param>
        /// <returns>true if loaded or already loaded, false if loading fails or current entity is null.</returns>
        protected virtual bool EntityLoad(Guid loId)
        {
            bool lbR = this.IsEntityLoaded;
            if (!lbR && null != this.Entity && this.Entity is MaxIdGuidEntity)
            {
                if (Guid.Empty.Equals(loId))
                {
                    this.Entity.Clear();
                    this.IsEntityLoaded = false;
                }
                else
                {
                    if (((MaxIdGuidEntity)this.Entity).LoadByIdCache(loId))
                    {
                        this.IsEntityLoaded = true;
                        lbR = true;
                    }
                }
            }

            return lbR;
        }

        /// <summary>
        /// Loads the data for the entity matching the Id only if it is not already matching.
        /// </summary>
        /// <param name="lsId">String version of the Id to load.</param>
        /// <returns>true if loaded or already loaded, false if loading fails, current entity is null, or string version of Id is not a unique id.</returns>
        protected virtual bool EntityLoad(string lsId)
        {
            bool lbR = false;
            if (null != lsId && lsId.Length > 0)
            {
                try
                {
                    Guid loId = MaxConvertLibrary.ConvertToGuid(typeof(object), lsId);
                    lbR = this.EntityLoad(loId);
                }
                catch (Exception loE)
                {
                    MaxLogLibrary.Log(new MaxLogEntryStructure(MaxEnumGroup.LogError, "Error loading {Id} for {Type}", loE, lsId, this.GetType()));
                }
            }

            return lbR;
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
                MaxIdGuidEntity loEntity = this.Entity as MaxIdGuidEntity;
                if (null != loEntity)
                {
                    if (null != this.Id && string.Empty != this.Id)
                    {
                        Guid loId = MaxConvertLibrary.ConvertToGuid(typeof(object), this.Id);
                        if (!Guid.Empty.Equals(loId))
                        {
                            loEntity.SetId(loId);
                        }
                    }

                    if (null != this.AlternateId)
                    {
                        loEntity.AlternateId = this.AlternateId;
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
                MaxIdGuidEntity loEntity = this.Entity as MaxIdGuidEntity;
                if (null != loEntity)
                {
                    this.AlternateId = loEntity.AlternateId;
                    this.OriginalValues.Add("AlternateId", this.AlternateId);
                    if (loEntity.CreatedDate > DateTime.MinValue)
                    {
                        this.CreatedDate = MaxConvertLibrary.ConvertToDateTimeFromUtc(typeof(object), loEntity.CreatedDate).ToString("G");
                        this.OriginalValues.Add("CreatedDate", this.CreatedDate);
                    } 
                    
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
