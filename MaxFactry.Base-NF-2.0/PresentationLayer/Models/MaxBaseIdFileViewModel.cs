// <copyright file="MaxBaseIdFileViewModel.cs" company="Lakstins Family, LLC">
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
// <change date="4/4/2015" author="Brian A. Lakstins" description="Initial Release">
// <change date="6/2/2015" author="Brian A. Lakstins" description="Update to only map to entity if entity is loaded.">
// <change date="4/30/2021" author="Brian A. Lakstins" description="Add content date for original file date and time.">
// </changelog>
#endregion

namespace MaxFactry.Base.PresentationLayer
{
    using System;
    using System.IO;
    using MaxFactry.Core;
    using MaxFactry.Base.BusinessLayer;

    /// <summary>
    /// View model base
    /// </summary>
    public abstract class MaxBaseIdFileViewModel : MaxBaseGuidKeyViewModel
    {
        /// <summary>
        /// Internal storage of the content stream
        /// </summary>
        private Stream _oContent = null;

        /// <summary>
        /// Initializes a new instance of the MaxBaseIdFileViewModel class
        /// </summary>
        public MaxBaseIdFileViewModel()
            : base()
        {
        }

        /// <summary>
        /// Initializes a new instance of the MaxBaseIdFileViewModel class
        /// </summary>
        /// <param name="loEntity">Entity to use as data.</param>
        public MaxBaseIdFileViewModel(MaxEntity loEntity)
            : base(loEntity)
        {
        }

        public MaxBaseIdFileViewModel(string lsId) : base(lsId)
        {
        }

        /// <summary>
        /// Gets or sets the name of the file.
        /// </summary>
        public virtual string Name
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the name of the file.
        /// </summary>
        public virtual string ContentName
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the stream of content for the file.
        /// </summary>
        public virtual Stream Content
        {
            get
            {
                if (null == this._oContent)
                {
                    MaxBaseIdFileEntity loEntity = this.Entity as MaxBaseIdFileEntity;
                    this._oContent = loEntity.Content;
                }

                return this._oContent;
            }

            set
            {
                this._oContent = value;
            }
        }

        /// <summary>
        /// Gets or sets the content type for the file
        /// </summary>
        public virtual string ContentType
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the content length for the file
        /// </summary>
        public virtual string ContentLength
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the content length for the file
        /// </summary>
        public virtual string ContentDate
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the Mime Type for the file.
        /// </summary>
        public virtual string MimeType
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the name of the file.
        /// </summary>
        public virtual string FileName
        {
            get;
            set;
        }

        public string ContentUrl
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
                MaxBaseIdFileEntity loEntity = this.Entity as MaxBaseIdFileEntity;
                if (null != loEntity)
                {
                    loEntity.Name = this.Name;
                    loEntity.ContentName = this.ContentName;
                    loEntity.FileName = this.FileName;
                    loEntity.MimeType = this.MimeType;
                    if (null != this._oContent)
                    {
                        loEntity.Content = this.Content;
                        loEntity.ContentType = this.ContentType;
                        loEntity.ContentLength = MaxConvertLibrary.ConvertToLong(typeof(object), this.ContentLength);
                        if (!string.IsNullOrEmpty(this.ContentDate))
                        {
                            loEntity.ContentDate = MaxConvertLibrary.ConvertToDateTimeUtc(typeof(object), this.ContentDate);
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
                MaxBaseIdFileEntity loEntity = this.Entity as MaxBaseIdFileEntity;
                if (null != loEntity && !Guid.Empty.Equals(loEntity.Id))
                {
                    this.Name = loEntity.Name;
                    this.ContentName = loEntity.ContentName;
                    this.FileName = loEntity.FileName;
                    this.ContentLength = loEntity.ContentLength.ToString();
                    this.ContentType = loEntity.ContentType;
                    this.ContentUrl = loEntity.GetContentUrl();
                    if (loEntity.ContentDate > DateTime.MinValue)
                    {
                        this.ContentDate = MaxConvertLibrary.ConvertToDateTimeFromUtc(typeof(object), loEntity.ContentDate).ToString();
                    }

                    if (null != loEntity.MimeType && loEntity.MimeType.Length > 0)
                    {
                        this.MimeType = loEntity.MimeType;
                    }
                    else
                    {
                        this.MimeType = loEntity.GetMimeType();
                    }

                    return true;
                }
            }

            return false;
        }
    }
}
