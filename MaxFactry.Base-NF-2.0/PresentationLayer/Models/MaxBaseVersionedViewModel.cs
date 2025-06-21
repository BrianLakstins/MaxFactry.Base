// <copyright file="MaxBaseVersionedViewModel.cs" company="Lakstins Family, LLC">
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
// <change date="6/18/2025" author="Brian A. Lakstins" description="Initial creation">
// </changelog>
#endregion

namespace MaxFactry.Base.PresentationLayer
{
    using MaxFactry.Base.BusinessLayer;
    using MaxFactry.Core;
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// View model base
    /// </summary>
    public class MaxBaseVersionedViewModel : MaxBaseViewModel
    {
        private List<MaxBaseVersionedViewModel> _oSortedList = null;

        /// <summary>
        /// Initializes a new instance of the MaxBaseIdViewModel class.
        /// </summary>
        public MaxBaseVersionedViewModel()
            : base()
        {
        }

        /// <summary>
        /// Initializes a new instance of the MaxBaseIdViewModel class.
        /// </summary>
        /// <param name="loEntity">Entity to use as data.</param>
        public MaxBaseVersionedViewModel(MaxEntity loEntity)
            : base(loEntity)
        {
        }

        public MaxBaseVersionedViewModel(string lsName)
        {
            this.CreateEntity();
            this.Load(lsName);
        }

        /// <summary>
        /// Gets or sets the unique Name for this entity.
        /// </summary>
        public virtual string Name
        {
            get;
            set;
        }

        /// <summary>
        /// Gets the version for this entity.
        /// </summary>
        public virtual int Version
        {
            get
            {
                if (this.Entity is MaxBaseVersionedEntity)
                {
                    return ((MaxBaseVersionedEntity)this.Entity).Version;
                }

                return 0;
            }
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
                if (null != this.Name && string.Empty != this.Name && this.Entity is MaxBaseVersionedEntity)
                {
                    MaxBaseVersionedEntity loEntity = ((MaxBaseVersionedEntity)this.Entity).GetCurrent(this.Name);
                    if (null != loEntity)
                    {
                        this.IsEntityLoaded = true;
                        lbR = true;
                    }
                }
            }

            return lbR;
        }

        public virtual bool Load(string lsName)
        {
            bool lbR = false;
            if (null != lsName && string.Empty != lsName)
            {
                this.Name = lsName;
                if (null != this.Entity && this.Entity is MaxBaseVersionedEntity)
                {
                    this.Entity = ((MaxBaseVersionedEntity)this.Entity).GetCurrent(lsName);
                    return this.MapFromEntity();
                }

                lbR = this.Load();
            }

            return lbR;
        }

        public virtual List<MaxBaseVersionedViewModel> GetSortedList()
        {
            if (null == this._oSortedList)
            {
                this._oSortedList = new List<MaxBaseVersionedViewModel>();
                SortedList<string, MaxBaseVersionedViewModel> loSortedList = new SortedList<string, MaxBaseVersionedViewModel>();
                string[] laKey = this.EntityIndex.GetSortedKeyList();
                for (int lnK = 0; lnK < laKey.Length; lnK++)
                {
                    MaxBaseVersionedViewModel loViewModel = new MaxBaseVersionedViewModel(this.EntityIndex[laKey[lnK]] as MaxBaseVersionedEntity);
                    string lsKey = loViewModel.Name.ToLowerInvariant();
                    if (loSortedList.ContainsKey(lsKey))
                    {
                        if (loSortedList[lsKey].Version < loViewModel.Version)
                        {
                            loSortedList[lsKey] = loViewModel;
                        }
                    }
                    else
                    {
                        loSortedList.Add(lsKey, loViewModel);
                    }
                }

                this._oSortedList = new List<MaxBaseVersionedViewModel>(loSortedList.Values);
            }

            return this._oSortedList;
        }

        /// <summary>
        /// Saves the model
        /// </summary>
        /// <returns>true if successful</returns>
        public override bool Save()
        {
            if (this.MapToEntity())
            {
                if (this.Entity is MaxBaseVersionedEntity)
                {
                    MaxBaseVersionedEntity loEntity = (MaxBaseVersionedEntity)this.Entity;
                    bool lbR = false;
                    if (!string.IsNullOrEmpty(loEntity.Name) && loEntity.Insert())
                    {
                        lbR = this.Load();
                    }

                    return lbR;
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
                if (this.Entity is MaxBaseVersionedEntity)
                {
                    MaxBaseVersionedEntity loEntity = (MaxBaseVersionedEntity)this.Entity;
                    string lsName = this.Name.ToLowerInvariant();
                    loEntity.Name = lsName;
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
                if (this.Entity is MaxBaseVersionedEntity)
                {
                    MaxBaseVersionedEntity loEntity = (MaxBaseVersionedEntity)this.Entity;
                    this.Name = loEntity.Name;
                    return true;
                }
            }

            return false;
        }
    }
}
