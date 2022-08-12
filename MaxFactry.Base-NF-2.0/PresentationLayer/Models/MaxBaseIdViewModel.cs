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
// </changelog>
#endregion

namespace MaxFactry.Base.PresentationLayer
{
    using System;
    using MaxFactry.Core;
    using MaxFactry.Base.BusinessLayer;
    using MaxFactry.Base.PresentationLayer;

    /// <summary>
    /// View model base
    /// </summary>
    public abstract class MaxBaseIdViewModel : MaxIdGuidViewModel
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

        public MaxBaseIdViewModel(string lsId)
        {
            this.CreateEntity();
            this.Load(lsId);
        }

        public virtual bool Load(string lsId)
        {
            bool lbR = false;
            this.Id = lsId;
            if (this.EntityLoad())
            {
                lbR = this.Load();
            }

            return lbR;
        }

        /// <summary>
        /// Gets the date last updated.  Not changed through view model.
        /// </summary>
        public virtual DateTime LastUpdateDate
        {
            get
            {
                MaxBaseIdEntity loEntity = this.Entity as MaxBaseIdEntity;
                if (null != loEntity)
                {
                    return MaxConvertLibrary.ConvertToDateTimeFromUtc(typeof(object), loEntity.LastUpdateDate);
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
                MaxBaseIdEntity loEntity = this.Entity as MaxBaseIdEntity;
                if (null != loEntity)
                {
                    return loEntity.AttributeIndex;
                }

                return new MaxIndex();
            }
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
                MaxBaseIdEntity loEntity = this.Entity as MaxBaseIdEntity;
                if (null != loEntity)
                {
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
                MaxBaseIdEntity loEntity = this.Entity as MaxBaseIdEntity;
                if (null != loEntity)
                {
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
            }

            return false;
        }
    }
}
