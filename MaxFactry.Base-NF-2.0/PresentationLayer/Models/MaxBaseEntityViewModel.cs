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
// <change date="1/21/2025" author="Brian A. Lakstins" description="Updated to match MaxBaseEntity.">
// <change date="6/9/2025" author="Brian A. Lakstins" description="Replace StorageKey with DataKey.  Load entity by DataKey">
// <change date="6/11/2025" author="Brian A. Lakstins" description="Use method to get DataKey">
// <change date="6/18/2025" author="Brian A. Lakstins" description="Changed base to MaxViewModel.  This class is being replaced with MaxBaseViewModel.">
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
    public abstract class MaxBaseEntityViewModel : MaxViewModel
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
            this.IsEntityLoaded = true;
            this.MapFromEntity();
        }

        /// <summary>
        /// Gets the date last updated.  Not changed through view model.
        /// </summary>
        public virtual string DataKey
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the date created. Set at record creation.
        /// </summary>
        public virtual string CreatedDate
        {
            get;
            set;
        }

        /// <summary>
        /// Gets the date last updated.  Not changed through view model.
        /// </summary>
        public virtual DateTime LastUpdateDate
        {
            get
            {
                if (this.Entity is MaxBaseEntity)
                {
                    return MaxConvertLibrary.ConvertToDateTimeFromUtc(typeof(object), ((MaxBaseEntity)this.Entity).LastUpdateDate);
                }

                return DateTime.MinValue;
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether the current entity is active.
        /// </summary>
        public virtual string Active { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the current entity is active.
        /// </summary>
        public virtual bool IsActive
        {
            get
            {
                object loR = this.Get("IsActive");
                if (loR is bool)
                {
                    return (bool)loR;
                }
                else if (null != this.Active && string.Empty != this.Active)
                {
                    return MaxConvertLibrary.ConvertToBoolean(typeof(object), this.Active);
                }

                return false;
            }

            set
            {
                if (value)
                {
                    this.Active = "Yes";
                    this.Set("IsActive", true);
                }
                else
                {
                    this.Active = "No";
                    this.Set("IsActive", false);
                }
            }
        }

        /// <summary>
        /// Gets or sets the Attribute property to be stored.
        /// </summary>
        [MaxMeta(Name = "Attributes")]
        public virtual string AttributeIndexText
        {
            get;
            set;
        }

        /// <summary>
        /// Gets the Attribute key value pair information for this entity
        /// </summary>
        public MaxIndex AttributeIndex
        {
            get
            {
                if (this.Entity is MaxBaseEntity)
                {
                    return ((MaxBaseEntity)this.Entity).AttributeIndex;
                }

                return new MaxIndex();
            }
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
        /// Loads the content of the Entity associated with this ViewModel
        /// </summary>
        /// <returns>true if successful</returns>
        public virtual bool EntityLoad()
        {
            bool lbR = this.IsEntityLoaded && null != this.Entity;
            if (!lbR)
            {
                if (null != this.DataKey && string.Empty != this.DataKey && this.Entity is MaxBaseEntity)
                {
                    if (((MaxBaseEntity)this.Entity).LoadByDataKeyCache(this.DataKey))
                    {
                        this.IsEntityLoaded = true;
                        lbR = true;
                    }
                }
            }

            return lbR;
        }

        /// <summary>
        /// Maps the fields in the view model from the entity.
        /// </summary>
        /// <returns>True if the load was successful.</returns>
        public virtual bool Load()
        {
            bool lbR = false;
            if (this.EntityLoad())
            {
                lbR = this.MapFromEntity();
            }

            return lbR;
        }

        /// <summary>
        /// Maps the fields in the view model from the entity.
        /// </summary>
        /// <returns>True if the load was successful.</returns>
        public virtual bool Load(MaxEntity loEntity)
        {
            bool lbR = false;
            if (null != loEntity)
            {
                this.Entity = loEntity;
                this.IsEntityLoaded = true;
                lbR = this.MapFromEntity();
            }

            return lbR;
        }

        /// <summary>
        /// Loads the entity based on the Id property.
        /// Maps the current values of properties in the ViewModel to the Entity.
        /// </summary>
        /// <returns>True if successful. False if it cannot be mapped.</returns>
        protected virtual bool MapToEntity()
        {
            if (this.Entity is MaxBaseEntity)
            {
                MaxBaseEntity loEntity = (MaxBaseEntity)this.Entity;
                if (null != this.Active && this.Active.Length > 0)
                {
                    loEntity.IsActive = MaxConvertLibrary.ConvertToBoolean(typeof(object), this.Active);
                }

                string lsAttributeIndexText = this.AttributeIndexText;
                if (string.IsNullOrEmpty(lsAttributeIndexText))
                {
                    lsAttributeIndexText = string.Empty;
                }

                loEntity.AttributeIndex.Clear();
                if (lsAttributeIndexText.Contains("="))
                {
                    string[] laAttributeIndexText = lsAttributeIndexText.Split('\n');
                    foreach (string lsAttributeKeyValue in laAttributeIndexText)
                    {
                        if (lsAttributeKeyValue.IndexOf('=') > 0)
                        {
                            string lsKey = lsAttributeKeyValue.Substring(0, lsAttributeKeyValue.IndexOf('='));
                            string lsValue = lsAttributeKeyValue.Substring(lsAttributeKeyValue.IndexOf('=') + 1);
                            loEntity.AttributeIndex.Add(lsKey.Trim(), lsValue.Trim());
                        }
                    }
                }
                else if (lsAttributeIndexText.Length > 0)
                {
                    loEntity.AttributeIndex.Add("all", lsAttributeIndexText);
                }

                return true;
            }

            return false;
        }

        /// <summary>
        /// Maps the properties of the Entity to the properties of the ViewModel.
        /// </summary>
        /// <returns>True if the entity exists.</returns>
        protected virtual bool MapFromEntity()
        {            
            if (this.Entity is MaxBaseEntity)
            {
                MaxBaseEntity loEntity = (MaxBaseEntity)this.Entity;
                this.DataKey = loEntity.DataKey;
                if (loEntity.CreatedDate > DateTime.MinValue)
                {
                    this.CreatedDate = MaxConvertLibrary.ConvertToDateTimeFromUtc(typeof(object), loEntity.CreatedDate).ToString("G");
                    this.OriginalValues.Add("CreatedDate", this.CreatedDate);
                }
                
                if (loEntity.LastUpdateDate > DateTime.MinValue)
                {
                    this.OriginalValues.Add("LastUpdateDate", this.LastUpdateDate);
                }

                this.OriginalValues.Add("IsActive", this.IsActive);
                this.IsActive = loEntity.IsActive;
                this.OriginalValues.Add("Active", this.Active);
                string[] laKey = loEntity.AttributeIndex.GetSortedKeyList();
                foreach (string lsKey in laKey)
                {
                    this.AttributeIndexText += lsKey + "=" + loEntity.AttributeIndex[lsKey] + "\r\n";
                }

                return true;
            }

            return false;
        }
    }
}