// <copyright file="MaxBaseRelationDateEntity.cs" company="Lakstins Family, LLC">
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
// <change date="10/28/2022" author="Brian A. Lakstins" description="Initial creation.">
// <change date="3/20/2024" author="Brian A. Lakstins" description="Happy birthday to my mom.  Sara Jean Lakstins (Cartwright) - 3/20/1944 to 3/14/2019.">
// <change date="3/23/2024" author="Brian A. Lakstins" description="Incorporate parent class method.">
// <change date="3/24/2024" author="Brian A. Lakstins" description="Updated for changes namespaces">
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
    public abstract class MaxBaseRelationDateEntity : MaxBaseRelationEntity
	{
		/// <summary>
        /// Initializes a new instance of the MaxBaseRelationDateEntity class
		/// </summary>
		/// <param name="loData">object to hold data</param>
		public MaxBaseRelationDateEntity(MaxData loData)
			: base(loData)
		{
		}

        /// <summary>
        /// Initializes a new instance of the MaxBaseRelationDateEntity class.
        /// </summary>
        /// <param name="loDataModelType">Type of data model.</param>
        public MaxBaseRelationDateEntity(Type loDataModelType) 
            : base(loDataModelType)
        {
        }

        /// <summary>
        /// Gets or sets the Start Date
        /// </summary>
        public DateTime StartDate
        {
            get
            {
                return this.GetDateTime(this.MaxBaseRelationDateDataModel.StartDate);
            }

            set
            {
                this.Set(this.MaxBaseRelationDateDataModel.StartDate, value);
            }
        }

        /// <summary>
        /// Gets or sets the End Date
        /// </summary>
        public DateTime EndDate
        {
            get
            {
                DateTime loR = this.GetDateTime(this.MaxBaseRelationDateDataModel.EndDate);
                if (loR == DateTime.MinValue)
                {
                    loR = DateTime.MaxValue;
                }

                return loR;
            }

            set
            {
                this.Set(this.MaxBaseRelationDateDataModel.EndDate, value);
            }
        }

        /// <summary>
        /// Gets the Data Model for this entity
        /// </summary>
        protected MaxBaseRelationDateDataModel MaxBaseRelationDateDataModel
        {
            get
            {
                return (MaxBaseRelationDateDataModel)MaxDataLibrary.GetDataModel(this.DataModelType);
            }
        }

        /// <summary>
        /// Load all entities based on the parent Id and including start dates and end dates surrounding the date
        /// </summary>
        /// <param name="loParentId">The Id of the parent entity</param>
        /// <param name="ldDate">The date that is betweent the start and end dates</param>
        /// <returns></returns>
        public MaxEntityList LoadAllByParentIdDateCache(Guid loParentId, DateTime ldDate)
        {
            MaxDataQuery loDataQuery = this.GetDataQuery();
            loDataQuery.AddFilter(this.MaxBaseRelationDateDataModel.ParentId, "=", loParentId);
            loDataQuery.AddCondition("AND");
            loDataQuery.AddFilter(this.MaxBaseRelationDateDataModel.StartDate, "<", ldDate.ToUniversalTime());
            loDataQuery.AddCondition("AND");
            loDataQuery.StartGroup();
            loDataQuery.AddFilter(this.MaxBaseRelationDateDataModel.EndDate, "is null", null);
            loDataQuery.AddCondition("OR");
            loDataQuery.AddFilter(this.MaxBaseRelationDateDataModel.EndDate, ">", ldDate.ToUniversalTime());
            loDataQuery.EndGroup();
            MaxData loData = this.Data.Clone();
            return this.LoadAllByPageCache(loData, 0, 0, string.Empty, loDataQuery);
        }

        /// <summary>
        /// Load all entities based on the parent Id and and child Id and including start dates and end dates surrounding the date
        /// </summary>
        /// <param name="loParentId">The Id of the parent entity</param>
        /// <param name="loChildId">The Id of the child entity</param>
        /// <param name="ldDate">The date that is betweent the start and end dates</param>
        /// <returns></returns>
        public MaxEntityList LoadAllByParentIdChildIdDateCache(Guid loParentId, Guid loChildId, DateTime ldDate)
        {
            MaxDataQuery loDataQuery =  this.GetDataQuery();
            loDataQuery.AddFilter(this.MaxBaseRelationDateDataModel.ParentId, "=", loParentId);
            loDataQuery.AddCondition("AND");
            loDataQuery.AddFilter(this.MaxBaseRelationDateDataModel.ChildId, "=", loChildId);
            loDataQuery.AddCondition("AND");
            loDataQuery.AddFilter(this.MaxBaseRelationDateDataModel.StartDate, "<", ldDate.ToUniversalTime());
            loDataQuery.AddCondition("AND");
            loDataQuery.StartGroup();
            loDataQuery.AddFilter(this.MaxBaseRelationDateDataModel.EndDate, "is null", null);
            loDataQuery.AddCondition("OR");
            loDataQuery.AddFilter(this.MaxBaseRelationDateDataModel.EndDate, ">", ldDate.ToUniversalTime());
            loDataQuery.EndGroup();
            MaxData loData = this.Data.Clone();
            return this.LoadAllByPageCache(loData, 0, 0, string.Empty, loDataQuery);
        }
    }
}
