// <copyright file="MaxBaseIdViewModel.cs" company="Lakstins Family, LLC">
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
// <change date="6/2/2015" author="Brian A. Lakstins" description="Update to only map to entity if entity is loaded.">
// <change date="3/21/2016" author="Brian A. Lakstins" description="Convert last update from UTC to local time.">
// <change date="4/20/2016" author="Brian A. Lakstins" description="Updated to allow setting IsActive.">
// <change date="9/14/2018" author="Brian A. Lakstins" description="Added public method that confirms if data is found and loaded.">
// <change date="11/29/2018" author="Brian A. Lakstins" description="Add support for AttributeIndex entity property.">
// <change date="11/30/2018" author="Brian A. Lakstins" description="Update IsActive so it can be set with a bool and not just text.">
// <change date="9/17/2020" author="Brian A. Lakstins" description="Return current value for IsActive if the text has been updated.">
// <change date="1/21/2025" author="Brian A. Lakstins" description="Update for changed base entity.">
// </changelog>
#endregion

namespace MaxFactry.Base.PresentationLayer
{
    using MaxFactry.Base.BusinessLayer;

    /// <summary>
    /// View model base
    /// </summary>
    public abstract class MaxBaseIdViewModel : MaxBaseGuidKeyViewModel
    {
        /// <summary>
        /// Initializes a new instance of the MaxBaseIdViewModel class.
        /// </summary>
        public MaxBaseIdViewModel()
            : base()
        {
        }

        /// <summary>
        /// Initializes a new instance of the MaxBaseIdViewModel class.
        /// </summary>
        /// <param name="loEntity">Entity to use as data.</param>
        public MaxBaseIdViewModel(MaxEntity loEntity)
            : base(loEntity)
        {
        }

        public MaxBaseIdViewModel(string lsId): base(lsId)
        {
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
        /// Loads the entity based on the Id property.
        /// Maps the current values of properties in the ViewModel to the Entity.
        /// </summary>
        /// <returns>True if successful. False if it cannot be mapped.</returns>
        protected override bool MapToEntity()
        {
            if (base.MapToEntity())
            {
                if (this.Entity is MaxBaseIdEntity)
                {
                    MaxBaseIdEntity loEntity = (MaxBaseIdEntity)this.Entity;
                    loEntity.AlternateId = this.AlternateId;                   
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
                if (this.Entity is MaxBaseIdEntity)
                {
                    MaxBaseIdEntity loEntity = (MaxBaseIdEntity)this.Entity;
                    this.AlternateId = loEntity.AlternateId;
                    this.OriginalValues.Add("AlternateId", this.AlternateId);
                    return true;
                }
            }

            return false;
        }
    }
}
