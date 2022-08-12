// <copyright file="MaxBaseTemplateViewModel.cs" company="Lakstins Family, LLC">
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
// <change date="4/4/2015" author="Brian A. Lakstins" description="Initial creation.">
// </changelog>
#endregion

namespace MaxFactry.Module.Template.PresentationLayer
{
	using System;
    using MaxFactry.Core;
    using MaxFactry.Base.BusinessLayer;
    using MaxFactry.Module.Template.BusinessLayer;

	/// <summary>
	/// View model for content.
	/// </summary>
    public class MaxBaseTemplateViewModel : MaxFactry.Base.PresentationLayer.MaxBaseIdViewModel
	{
        /// <summary>
        /// Internal storage of a sorted list.
        /// Can use Generic List if supported in the framework.
        /// </summary>
        private MaxIndex _oSortedList = null;

        /// <summary>
        /// Initializes a new instance of the MaxBaseTemplateViewModel class
        /// </summary>
        public MaxBaseTemplateViewModel()
            : base()
        {
        }

        /// <summary>
        /// Initializes a new instance of the MaxBaseTemplateViewModel class
        /// </summary>
        /// <param name="loEntity">Entity to use as data.</param>
        public MaxBaseTemplateViewModel(MaxEntity loEntity)
            : base(loEntity)
        {
        }

        /// <summary>
        ///  Initializes a new instance of the MaxBaseTemplateViewModel class
        /// </summary>
        /// <param name="lsId">Id to load</param>
        public MaxBaseTemplateViewModel(string lsId) : base(lsId)
        {
        }

        protected override void CreateEntity()
        {
            this.Entity = MaxBaseTemplateEntity.Create();
        }

        /// <summary>
        /// Gets or sets the property to be stored.
        /// </summary>
        public string Property
        {
            get;
            set;
        }

        /// <summary>
        /// Gets a sorted list of all 
        /// Can use Generic List if supported in the framework.
        /// </summary>
        /// <returns>List of ViewModels</returns>
        public MaxIndex GetSortedList()
        {
            if (null == this._oSortedList)
            {
                this._oSortedList = new MaxIndex();
                string[] laKey = this.EntityIndex.GetSortedKeyList();
                for (int lnK = 0; lnK < laKey.Length; lnK++)
                {
                    MaxBaseTemplateViewModel loViewModel = new MaxBaseTemplateViewModel(this.EntityIndex[laKey[lnK]] as MaxEntity);
                    loViewModel.Load();
                    this._oSortedList.Add(loViewModel);
                }
            }

            return this._oSortedList;
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
                MaxBaseTemplateEntity loEntity = this.Entity as MaxBaseTemplateEntity;
                if (null != loEntity)
                {
                    loEntity.Property = this.Property;
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
                MaxBaseTemplateEntity loEntity = this.Entity as MaxBaseTemplateEntity;
                if (null != loEntity)
                {
                    this.Property = loEntity.Property;
                    return true;
                }
            }

            return false;
        }
    }
}
