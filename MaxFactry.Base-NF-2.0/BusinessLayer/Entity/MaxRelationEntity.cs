// <copyright file="MaxRelationEntity.cs" company="Lakstins Family, LLC">
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
// <change date="1/17/2016" author="Brian A. Lakstins" description="Initial creation.">
// <change date="7/21/2020" author="Brian A. Lakstins" description="Add load cache methods">
// <change date="8/5/2020" author="Brian A. Lakstins" description="Fix load cache methods">
// <change date="1/16/2021" author="Brian A. Lakstins" description="Update definition of cache keys.">
// <change date="2/19/2021" author="Brian A. Lakstins" description="Use standard methods to load relationships.">
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
    public abstract class MaxRelationEntity : MaxEntity
	{
		/// <summary>
        /// Initializes a new instance of the MaxRelationEntity class
		/// </summary>
		/// <param name="loData">object to hold data</param>
		public MaxRelationEntity(MaxData loData)
			: base(loData)
		{
		}

        /// <summary>
        /// Initializes a new instance of the MaxRelationEntity class.
        /// </summary>
        /// <param name="loDataModelType">Type of data model.</param>
        public MaxRelationEntity(Type loDataModelType) 
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
                return this.GetString(this.MaxRelationDataModel.Name);
            }

            set
            {
                this.Set(this.MaxRelationDataModel.Name, value);
            }
        }

        /// <summary>
        /// Gets or sets the Relation Type
        /// </summary>
        public string RelationType
        {
            get
            {
                return this.GetString(this.MaxRelationDataModel.RelationType);
            }

            set
            {
                this.Set(this.MaxRelationDataModel.RelationType, value);
            }
        }

        /// <summary>
        /// Gets or sets the Relative order
        /// </summary>
        public int RelativeOrder
        {
            get
            {
                return this.GetInt(this.MaxRelationDataModel.RelativeOrder);
            }

            set
            {
                this.Set(this.MaxRelationDataModel.RelativeOrder, value);
            }
        }

        /// <summary>
        /// Gets or sets the Parent Id
        /// </summary>
        protected Guid ParentId
        {
            get
            {
                return this.GetGuid(this.MaxRelationDataModel.ParentId);
            }

            set
            {
                this.Set(this.MaxRelationDataModel.ParentId, value);
            }
        }

        /// <summary>
        /// Gets or sets the Child Id
        /// </summary>
        protected Guid ChildId
        {
            get
            {
                return this.GetGuid(this.MaxRelationDataModel.ChildId);
            }

            set
            {
                this.Set(this.MaxRelationDataModel.ChildId, value);
            }
        }

        /// <summary>
        /// Gets the Data Model for this entity
        /// </summary>
        protected MaxRelationDataModel MaxRelationDataModel
        {
            get
            {
                return (MaxRelationDataModel)MaxDataLibrary.GetDataModel(this.DataModelType);
            }
        }

        /// <summary>
        /// Loads all entities that match the given Parent Id
        /// </summary>
        /// <param name="loParentId">The Id of the parent.</param>
        /// <returns>List of relations.</returns>
        protected MaxEntityList LoadAllByParentId(Guid loParentId)
        {
            MaxEntityList loR = this.LoadAllByProperty(this.MaxRelationDataModel.ParentId, loParentId);
            return loR;
        }

        /// <summary>
        /// Loads all entities that match the given Child Id
        /// </summary>
        /// <param name="loChildId">The Id of the child.</param>
        /// <returns>List of relations.</returns>
        protected MaxEntityList LoadAllByChildId(Guid loChildId)
        {
            MaxEntityList loR = this.LoadAllByProperty(this.MaxRelationDataModel.ChildId, loChildId);
            return loR;
        }

        /// <summary>
        /// Loads all entities that match the given Parent Id from cache
        /// </summary>
        /// <param name="loParentId">The Id of the parent.</param>
        /// <returns>List of relations.</returns>
        protected MaxEntityList LoadAllByParentIdCache(Guid loParentId)
        {
            MaxEntityList loR = this.LoadAllByPropertyCache(this.MaxRelationDataModel.ParentId, loParentId);
            return loR;
        }

        /// <summary>
        /// Loads all entities that match the given Child Id
        /// </summary>
        /// <param name="loChildId">The Id of the child.</param>
        /// <returns>List of relations.</returns>
        protected MaxEntityList LoadAllByChildIdCache(Guid loChildId)
        {
            MaxEntityList loR = this.LoadAllByPropertyCache(this.MaxRelationDataModel.ChildId, loChildId);
            return loR;
        }

        protected MaxEntityList LoadAllByParentIdChildIdCache(Guid loParentId, Guid loChildId)
        {
            MaxEntityList loR = MaxEntityList.Create(this.GetType());
            string lsCacheAllDataKey = this.GetCacheKey() + "LoadAll";
            MaxDataList loDataAllList = MaxCacheRepository.Get(this.GetType(), lsCacheAllDataKey, typeof(MaxDataList)) as MaxDataList;
            if (null != loDataAllList && loDataAllList.Count > 0)
            {
                for (int lnD = 0; lnD < loDataAllList.Count; lnD++)
                {
                    object loParentIdCheck = loDataAllList[lnD].Get(this.MaxRelationDataModel.ParentId);
                    if (null != loParentIdCheck && loParentIdCheck.Equals(loParentId))
                    {
                        object loChildIdCheck = loDataAllList[lnD].Get(this.MaxRelationDataModel.ChildId);
                        if (null != loChildIdCheck && loChildIdCheck.Equals(loChildId))
                        {
                            MaxEntity loEntity = MaxBusinessLibrary.GetEntity(this.GetType(), loDataAllList[lnD]);
                            loR.Add(loEntity);
                        }
                    }
                }
            }
            else
            {
                //// Check Parent Id Cache
                string lsCacheDataKey = this.GetCacheKey() + "LoadAllByPropertyCache/" + this.MaxRelationDataModel.ParentId + "/" + MaxConvertLibrary.ConvertToString(typeof(object), loParentId);
                MaxDataList loDataList = MaxCacheRepository.Get(this.GetType(), lsCacheDataKey, typeof(MaxDataList)) as MaxDataList;
                if (null != loDataList && loDataList.Count > 0)
                {
                    for (int lnD = 0; lnD < loDataList.Count; lnD++)
                    {
                        object loChildIdCheck = loDataList[lnD].Get(this.MaxRelationDataModel.ChildId);
                        if (null != loChildIdCheck && loChildIdCheck.Equals(loChildId))
                        {
                            MaxEntity loEntity = MaxBusinessLibrary.GetEntity(this.GetType(), loDataList[lnD]);
                            loR.Add(loEntity);
                        }
                    }
                }
                else
                {
                    //// Check Child Id Cache
                    lsCacheDataKey = this.GetCacheKey() + "LoadAllByPropertyCache/" + this.MaxRelationDataModel.ChildId + "/" + MaxConvertLibrary.ConvertToString(typeof(object), loChildId);
                    loDataList = MaxCacheRepository.Get(this.GetType(), lsCacheDataKey, typeof(MaxDataList)) as MaxDataList;
                    if (null != loDataList && loDataList.Count > 0)
                    {
                        for (int lnD = 0; lnD < loDataList.Count; lnD++)
                        {
                            object loParentIdCheck = loDataList[lnD].Get(this.MaxRelationDataModel.ParentId);
                            if (null != loParentIdCheck && loParentIdCheck.Equals(loParentId))
                            {
                                MaxEntity loEntity = MaxBusinessLibrary.GetEntity(this.GetType(), loDataList[lnD]);
                                loR.Add(loEntity);
                            }
                        }
                    }
                    else
                    {
                        //// Check Specific cache
                        lsCacheDataKey = this.GetCacheKey() + "LoadAllByPropertyCache/" + this.MaxRelationDataModel.ParentId + "/" + MaxConvertLibrary.ConvertToString(typeof(object), loParentId) + "/" + this.MaxRelationDataModel.ChildId + "/" + MaxConvertLibrary.ConvertToString(typeof(object), loChildId);
                        loDataList = MaxCacheRepository.Get(this.GetType(), lsCacheDataKey, typeof(MaxDataList)) as MaxDataList;
                        if (null != loDataList && loDataList.Count > 0)
                        {
                            for (int lnD = 0; lnD < loDataList.Count; lnD++)
                            {
                                MaxEntity loEntity = MaxBusinessLibrary.GetEntity(this.GetType(), loDataList[lnD]);
                                loR.Add(loEntity);
                            }
                        }
                        else
                        {
                            //// Load from database
                            MaxDataQuery loDataQuery = new MaxDataQuery();
                            loDataQuery.StartGroup();
                            loDataQuery.AddFilter(this.MaxRelationDataModel.ParentId, "=", loParentId);
                            loDataQuery.AddCondition("AND");
                            loDataQuery.AddFilter(this.MaxRelationDataModel.ChildId, "=", loChildId);
                            loDataQuery.EndGroup();
                            int lnTotal = 0;
                            loDataList = MaxBaseIdRepository.Select(this.GetData(), loDataQuery, 0, 0, string.Empty, out lnTotal);
                            MaxCacheRepository.Set(this.GetType(), lsCacheDataKey, loDataList);
                            loR = MaxEntityList.Create(this.GetType(), loDataList);
                        }
                    }
                }
            }

            return loR;

        }
    }
}
