// <copyright file="MaxBaseRelationGuidKeyEntity.cs" company="Lakstins Family, LLC">
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
// <change date="3/20/2024" author="Brian A. Lakstins" description="Happy birthday to my mom.  Sara Jean Lakstins (Cartwright) - 3/20/1944 to 3/14/2019.">
// <change date="3/24/2024" author="Brian A. Lakstins" description="Initial creation.">
// <change date="6/4/2025" author="Brian A. Lakstins" description="Get DataQuery from entity">
// </changelog>
#endregion

namespace MaxFactry.Base.BusinessLayer
{
	using System;
    using MaxFactry.Base.DataLayer;
    using MaxFactry.Base.DataLayer.Library;

    /// <summary>
    /// Base Business Layer Entity
    /// </summary>
    public abstract class MaxBaseRelationGuidKeyEntity : MaxBaseGuidKeyEntity
	{
		/// <summary>
        /// Initializes a new instance of the MaxBaseRelationEntity class
		/// </summary>
		/// <param name="loData">object to hold data</param>
		public MaxBaseRelationGuidKeyEntity(MaxData loData)
			: base(loData)
		{
		}

        /// <summary>
        /// Initializes a new instance of the MaxBaseRelationEntity class.
        /// </summary>
        /// <param name="loDataModelType">Type of data model.</param>
        public MaxBaseRelationGuidKeyEntity(Type loDataModelType) 
            : base(loDataModelType)
        {
        }

        /// <summary>
        /// Gets or sets the Name
        /// </summary>
        public string Name
        {
            get
            {
                return this.GetString(this.MaxBaseRelationGuidKeyDataModel.Name);
            }

            set
            {
                this.Set(this.MaxBaseRelationGuidKeyDataModel.Name, value);
            }
        }

        /// <summary>
        /// Gets or sets the Relation Type
        /// </summary>
        public string RelationType
        {
            get
            {
                return this.GetString(this.MaxBaseRelationGuidKeyDataModel.RelationType);
            }

            set
            {
                this.Set(this.MaxBaseRelationGuidKeyDataModel.RelationType, value);
            }
        }

        /// <summary>
        /// Gets or sets the Relative order
        /// </summary>
        public int RelativeOrder
        {
            get
            {
                return this.GetInt(this.MaxBaseRelationGuidKeyDataModel.RelativeOrder);
            }

            set
            {
                this.Set(this.MaxBaseRelationGuidKeyDataModel.RelativeOrder, value);
            }
        }

        /// <summary>
        /// Gets or sets the Parent Id
        /// </summary>
        protected Guid ParentId
        {
            get
            {
                return this.GetGuid(this.MaxBaseRelationGuidKeyDataModel.ParentId);
            }

            set
            {
                this.Set(this.MaxBaseRelationGuidKeyDataModel.ParentId, value);
            }
        }

        /// <summary>
        /// Gets or sets the Child Id
        /// </summary>
        protected Guid ChildId
        {
            get
            {
                return this.GetGuid(this.MaxBaseRelationGuidKeyDataModel.ChildId);
            }

            set
            {
                this.Set(this.MaxBaseRelationGuidKeyDataModel.ChildId, value);
            }
        }

        /// <summary>
        /// Gets the Data Model for this entity
        /// </summary>
        protected MaxBaseRelationGuidKeyDataModel MaxBaseRelationGuidKeyDataModel
        {
            get
            {
                return (MaxBaseRelationGuidKeyDataModel)MaxDataLibrary.GetDataModel(this.DataModelType);
            }
        }

        /// <summary>
        /// Loads all entities that match the given Parent Id
        /// </summary>
        /// <param name="loParentId">The Id of the parent.</param>
        /// <returns>List of relations.</returns>
        protected MaxEntityList LoadAllByParentId(Guid loParentId)
        {
            MaxEntityList loR = this.LoadAllByProperty(this.MaxBaseRelationGuidKeyDataModel.ParentId, loParentId);
            return loR;
        }

        /// <summary>
        /// Loads all entities that match the given Child Id
        /// </summary>
        /// <param name="loChildId">The Id of the child.</param>
        /// <returns>List of relations.</returns>
        protected MaxEntityList LoadAllByChildId(Guid loChildId)
        {
            MaxEntityList loR = this.LoadAllByProperty(this.MaxBaseRelationGuidKeyDataModel.ChildId, loChildId);
            return loR;
        }

        /// <summary>
        /// Loads all entities that match the given Parent Id from cache
        /// </summary>
        /// <param name="loParentId">The Id of the parent.</param>
        /// <returns>List of relations.</returns>
        protected MaxEntityList LoadAllByParentIdCache(Guid loParentId)
        {
            MaxEntityList loR = this.LoadAllByPropertyCache(this.MaxBaseRelationGuidKeyDataModel.ParentId, loParentId);
            return loR;
        }

        /// <summary>
        /// Loads all entities that match the given Child Id
        /// </summary>
        /// <param name="loChildId">The Id of the child.</param>
        /// <returns>List of relations.</returns>
        protected MaxEntityList LoadAllByChildIdCache(Guid loChildId)
        {
            MaxEntityList loR = this.LoadAllByPropertyCache(this.MaxBaseRelationGuidKeyDataModel.ChildId, loChildId);
            return loR;
        }

        protected MaxEntityList LoadAllByParentIdChildIdCache(Guid loParentId, Guid loChildId)
        {
            MaxDataQuery loDataQuery = this.GetDataQuery();
            loDataQuery.AddFilter(this.MaxBaseRelationGuidKeyDataModel.ParentId, "=", loParentId);
            loDataQuery.AddAnd();
            loDataQuery.AddFilter(this.MaxBaseRelationGuidKeyDataModel.ChildId, "=", loChildId);
            MaxData loData = new MaxData(this.Data);
            return this.LoadAllByPageCache(loData, 0, 0, string.Empty, loDataQuery);
        }
    }
}
