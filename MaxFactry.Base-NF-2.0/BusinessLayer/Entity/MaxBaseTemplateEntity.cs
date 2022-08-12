// <copyright file="MaxBaseTemplateEntity.cs" company="Lakstins Family, LLC">
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

namespace MaxFactry.Module.Template.BusinessLayer
{
    using System;
    using MaxFactry.Base.BusinessLayer;
    using MaxFactry.Base.DataLayer;
    using MaxFactry.Module.Template.DataLayer;

    /// <summary>
    /// Entity template based on MaxBaseId
    /// </summary>
    public class MaxBaseTemplateEntity : MaxFactry.Base.BusinessLayer.MaxBaseIdEntity
    {
        /// <summary>
        /// Initializes a new instance of the MaxBaseTemplateEntity class
        /// </summary>
        /// <param name="loData">object to hold data</param>
        public MaxBaseTemplateEntity(MaxData loData)
            : base(loData)
        {
        }

        /// <summary>
        /// Initializes a new instance of the MaxBaseTemplateEntity class.
        /// </summary>
        /// <param name="loDataModelType">Type of data model.</param>
        public MaxBaseTemplateEntity(Type loDataModelType)
            : base(loDataModelType)
        {
        }

        /// <summary>
        /// Gets or sets the property
        /// </summary>
        public string Property
        {
            get
            {
                return this.GetString(this.DataModel.Property);
            }

            set
            {
                this.Set(this.DataModel.Property, value);
            }
        }

        /// <summary>
        /// Gets the Data Model for this entity
        /// </summary>
        protected MaxBaseTemplateDataModel DataModel
        {
            get
            {
                return (MaxBaseTemplateDataModel)MaxDataLibrary.GetDataModel(this.DataModelType);
            }
        }

        /// <summary>
        /// Creates a new instance of the MaxTemplateEntity class.
        /// </summary>
        /// <returns>a new instance of the MaxTemplateEntity class.</returns>
        public static MaxBaseTemplateEntity Create()
        {
            return MaxBusinessLibrary.GetEntity(
                typeof(MaxBaseTemplateEntity),
                typeof(MaxBaseTemplateDataModel)) as MaxBaseTemplateEntity;
        }
    }
}
