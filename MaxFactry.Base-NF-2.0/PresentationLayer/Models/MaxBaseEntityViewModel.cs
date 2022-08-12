// <copyright file="MaxBaseEntityViewModel.cs" company="Lakstins Family, LLC">
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
#endregion License

#region Change Log
// <changelog>
// <change date="2/17/2015" author="Brian A. Lakstins" description="Initial Release">
// <change date="5/27/2015" author="Brian A. Lakstins" description="Added way to keep track of original values.">
// <change date="4/20/2016" author="Brian A. Lakstins" description="Added methods to get and set text based on keys.">
// <change date="8/5/2016" author="Brian A. Lakstins" description="Updated change comparison to handle nulls better.">
// <change date="8/29/2016" author="Brian A. Lakstins" description="Add ability to set MaxIndex.">
// <change date="10/3/2019" author="Brian A. Lakstins" description="Add support for using functional expressions for getting and setting properties the frameworks that support functional expressions.">
// <change date="8/3/2020" author="Brian A. Lakstins" description="Change base class and remove duplicated methods.">
// </changelog>
#endregion Change Log

namespace MaxFactry.Base.PresentationLayer
{
    using System;
    using MaxFactry.Core;
    using MaxFactry.Base.BusinessLayer;

    /// <summary>
    /// View model base
    /// </summary>
    public abstract class MaxBaseEntityViewModel : MaxBaseViewModel
    {
        /// <summary>
        /// Internal storage of list of this class
        /// </summary>
        private MaxIndex _oEntityIndex = null;

        /// <summary>
        /// Indicates that the entity has been successfully loaded from external storage.
        /// </summary>
        private bool _bIsEntityLoaded = false;

        /// <summary>
        /// Initializes a new instance of the MaxViewModel class.
        /// </summary>
        public MaxBaseEntityViewModel()
        {
            this.CreateEntity();
        }

        /// <summary>
        /// Initializes a new instance of the MaxViewModel class
        /// </summary>
        /// <param name="loEntity">Entity to use as data.</param>
        public MaxBaseEntityViewModel(MaxEntity loEntity)
        {
            this.Entity = loEntity;
        }

        /// <summary>
        /// Override this to create the entity for the specific ViewModel
        /// </summary>
        protected virtual void CreateEntity()
        {
        }

        /// <summary>
        /// Gets or sets the MaxEntity related to this ViewModel.
        /// </summary>
        protected MaxEntity Entity { get; set; }

        /// <summary>
        /// Gets or sets the list of all entities of this type.
        /// </summary>
        protected virtual MaxIndex EntityIndex
        {
            get
            {
                if (null == this._oEntityIndex)
                {
                    this._oEntityIndex = new MaxIndex();
                    if (null != this.Entity)
                    {
                        MaxEntityList loEntityList = this.Entity.LoadAllCache();
                        this._oEntityIndex = new MaxIndex(loEntityList.Count - 1);
                        for (int lnE = 0; lnE < loEntityList.Count; lnE++)
                        {
                            this._oEntityIndex.AddWithoutKeyCheck(loEntityList[lnE].GetDefaultSortString(), loEntityList[lnE]);
                        }
                    }
                }

                return this._oEntityIndex;
            }

            set
            {
                this._oEntityIndex = value;
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether that the entity has been loaded.
        /// </summary>
        protected bool IsEntityLoaded
        {
            get
            {
                return this._bIsEntityLoaded;
            }

            set
            {
                this._bIsEntityLoaded = value;
            }
        }

        /// <summary>
        /// Maps the fields in the view model from the entity.
        /// </summary>
        /// <returns>True if the load was successful.</returns>
        public virtual bool Load()
        {
            return this.MapFromEntity();
        }

        /// <summary>
        /// Initializes the view model using this entity.  Does not map the entity.
        /// </summary>
        /// <param name="loEntity">Entity to Load.</param>
        protected void Init(MaxEntity loEntity)
        {
            this.Entity = loEntity;
        }

        /// <summary>
        /// Loads the entity based on the Id property.
        /// Maps the current values of properties in the ViewModel to the Entity.
        /// </summary>
        /// <returns>True if successful. False if it cannot be mapped.</returns>
        protected virtual bool MapToEntity()
        {
            bool lbR = false;

            if (null != this.Entity)
            {
                lbR = true;
            }

            return lbR;
        }

        /// <summary>
        /// Maps the properties of the Entity to the properties of the ViewModel.
        /// </summary>
        /// <returns>True if the entity exists.</returns>
        protected virtual bool MapFromEntity()
        {
            if (null != this.Entity)
            {
                return true;
            }

            return false;
        }
    }
}