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
// </changelog>
#endregion

namespace MaxFactry.Base.BusinessLayer
{
	using System;
    using MaxFactry.Core;
    using MaxFactry.Base.DataLayer;

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


        public MaxEntityList LoadAllByParentIdDateCache(Guid loParentId, DateTime ldDate)
        {
            MaxEntityList loR = MaxEntityList.Create(this.GetType());
            string lsCacheDataKey = this.GetCacheKey() + "LoadAllByPropertyCache/" + this.MaxBaseRelationDateDataModel.ParentId + "/" + MaxConvertLibrary.ConvertToString(typeof(object), loParentId) + "/" + ldDate.ToString();
            MaxDataList loDataList = MaxCacheRepository.Get(this.GetType(), lsCacheDataKey, typeof(MaxDataList)) as MaxDataList;
            if (null == loDataList)
            {
                MaxDataQuery loQuery = new MaxDataQuery();
                loQuery.AddFilter(this.MaxBaseRelationDateDataModel.ParentId, "=", loParentId);
                loQuery.AddCondition("AND");
                loQuery.AddFilter(this.MaxBaseRelationDateDataModel.StartDate, "<", ldDate.ToUniversalTime());
                loQuery.AddCondition("AND");
                loQuery.StartGroup();
                loQuery.AddFilter(this.MaxBaseRelationDateDataModel.EndDate, "is null", null);
                loQuery.AddCondition("OR");
                loQuery.AddFilter(this.MaxBaseRelationDateDataModel.EndDate, ">", ldDate.ToUniversalTime());
                loQuery.EndGroup();
                int lnTotal = 0;
                loDataList = MaxBaseIdRepository.Select(this.Data, loQuery, 0, 0, string.Empty, out lnTotal);
                MaxCacheRepository.Set(this.GetType(), lsCacheDataKey, loDataList);
            }

            loR = MaxEntityList.Create(this.GetType(), loDataList);
            return loR;
        }

        public MaxEntityList LoadAllByParentIdChildIdDateCache(Guid loParentId, Guid loChildId, DateTime ldDate)
        {
            MaxEntityList loR = MaxEntityList.Create(this.GetType());
            string lsCacheDataKey = this.GetCacheKey() + "LoadAllByPropertyCache/" + this.MaxBaseRelationDateDataModel.ParentId + "/" + MaxConvertLibrary.ConvertToString(typeof(object), loParentId) + "/" + MaxConvertLibrary.ConvertToString(typeof(object), loChildId) + "/" + ldDate.ToString();
            MaxDataList loDataList = MaxCacheRepository.Get(this.GetType(), lsCacheDataKey, typeof(MaxDataList)) as MaxDataList;
            if (null == loDataList)
            {
                MaxDataQuery loQuery = new MaxDataQuery();
                loQuery.AddFilter(this.MaxBaseRelationDateDataModel.ParentId, "=", loParentId);
                loQuery.AddCondition("AND");
                loQuery.AddFilter(this.MaxBaseRelationDateDataModel.ChildId, "=", loChildId);
                loQuery.AddCondition("AND");
                loQuery.AddFilter(this.MaxBaseRelationDateDataModel.StartDate, "<", ldDate.ToUniversalTime());
                loQuery.AddCondition("AND");
                loQuery.StartGroup();
                loQuery.AddFilter(this.MaxBaseRelationDateDataModel.EndDate, "is null", null);
                loQuery.AddCondition("OR");
                loQuery.AddFilter(this.MaxBaseRelationDateDataModel.EndDate, ">", ldDate.ToUniversalTime());
                loQuery.EndGroup();
                int lnTotal = 0;
                loDataList = MaxBaseIdRepository.Select(this.Data, loQuery, 0, 0, string.Empty, out lnTotal);
                MaxCacheRepository.Set(this.GetType(), lsCacheDataKey, loDataList);
            }

            loR = MaxEntityList.Create(this.GetType(), loDataList);
            return loR;
        }

        public bool LoadByKeyCache(Guid loParentId, Guid loChildId, DateTime ldStartDate)
        {
            bool lbR = false;
            MaxEntityList loList = this.LoadAllByParentIdChildIdCache(loParentId, loChildId);
            for (int lnE = 0; lnE < loList.Count && !lbR; lnE++)
            {
                MaxBaseRelationDateEntity loEntity = loList[lnE] as MaxBaseRelationDateEntity;
                if (loEntity.StartDate.Date == ldStartDate.Date)
                {
                    MaxData loData = loEntity.GetData();
                    loData.ClearChanged();
                    lbR = this.Load(loData);
                }
            }

            return lbR;
        }
    }
}
