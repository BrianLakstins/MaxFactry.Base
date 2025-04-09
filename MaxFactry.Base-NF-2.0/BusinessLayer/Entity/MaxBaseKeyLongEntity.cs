// <copyright file="MaxBaseKeyLongEntity.cs" company="Lakstins Family, LLC">
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
// <change date="4/9/2025" author="Brian A. Lakstins" description="Initial creation">
// </changelog>
#endregion

namespace MaxFactry.Base.BusinessLayer
{
    using System;
    using MaxFactry.Base.DataLayer;
    using MaxFactry.Base.DataLayer.Library;

    /// <summary>
    /// Base Business Layer Entity using a long as the primary key
    /// </summary>
    public abstract class MaxBaseKeyLongEntity : MaxBaseEntity
    {
        private static object _oLock = new object();

        private static long _nLastId = long.MinValue;

        /// <summary>
        /// Number of Id per day
        /// </summary>
        private int _nIdPerDay = 0;

        /// <summary>
        /// Number of Id per day
        /// </summary>
        private DateTime _nIdDate = DateTime.Now;

        /// <summary>
        /// Initializes a new instance of the MaxBaseKeyLongEntity class
        /// </summary>
        /// <param name="loData">object to hold data</param>
        public MaxBaseKeyLongEntity(MaxData loData) : base(loData)
        {
        }

        /// <summary>
        /// Initializes a new instance of the MaxBaseKeyLongEntity class.
        /// </summary>
        /// <param name="loDataModelType">Type of data model.</param>
        public MaxBaseKeyLongEntity(Type loDataModelType)
            : base(loDataModelType)
        {
        }

        /// <summary>
        /// Gets the unique Identifier for this entity
        /// </summary>
        public virtual long Id
        {
            get
            {
                return this.GetLong(this.MaxBaseKeyLongDataModel.Id);
            }
        }

        /// <summary>
        /// Gets or sets the length of the Id.
        /// </summary>
        protected int IdPerDay
        {
            get
            {
                return this._nIdPerDay;
            }
            set
            {
                this._nIdPerDay = value;
            }
        }

        /// <summary>
        /// Gets or sets the length of the Id.
        /// </summary>
        protected DateTime IdDate
        {
            get
            {
                return this._nIdDate;
            }
            set
            {
                this._nIdDate = value;
            }
        }

        /// <summary>
        /// Gets the Data Model for this entity
        /// </summary>
        protected MaxBaseKeyLongDataModel MaxBaseKeyLongDataModel
        {
            get
            {
                return (MaxBaseKeyLongDataModel)MaxDataLibrary.GetDataModel(this.DataModelType);
            }
        }

        /// <summary>
        /// Sets the Id.
        /// </summary>
        /// <param name="loId">Id to use for this entity.</param>
        public virtual void SetId(long lnId)
        {
            this.Set(this.MaxBaseKeyLongDataModel.Id, lnId);
        }

        protected override void SetInitial()
        {
            base.SetInitial();
            if (long.MinValue == this.Id)
            {
                lock (_oLock)
                {
                    //// Defaults to generate a 16 digit number base on the created date
                    long lnId = this.IdDate.Ticks;
                    if (this.IdPerDay > 0)
                    {
                        //// Format of Id is YYMMDD#####.  
                        lnId = (this.IdDate.Year - 2000) * 10000
                             + this.IdDate.Month * 100
                             + this.IdDate.Day;
                        if (this.IdPerDay > 1000000)
                        {
                            //// Format of start Id Integer is YYDDD#####.
                            lnId = (this.IdDate.Year - 2000) * 100000
                            + this.IdDate.DayOfYear * 100;
                        }

                        lnId = lnId * this.IdPerDay;
                    }

                    MaxDataQuery loDataQuery = new MaxDataQuery();
                    loDataQuery.StartGroup();
                    loDataQuery.AddFilter(this.MaxBaseKeyLongDataModel.Id, ">=", lnId);
                    loDataQuery.EndGroup();
                    MaxDataList loList = MaxBaseReadRepository.Select(this.Data, loDataQuery, 1, 1, this.MaxBaseKeyLongDataModel.Id + " desc", this.MaxBaseKeyLongDataModel.Id);
                    if (loList.Count > 0)
                    {
                        lnId = MaxFactry.Core.MaxConvertLibrary.ConvertToLong(typeof(object), loList[0].Get(this.MaxBaseKeyLongDataModel.Id)) + 1;
                    }

                    if (lnId <= _nLastId)
                    {
                        lnId = _nLastId + 1;
                    }

                    this.SetId(lnId);
                    _nLastId = lnId;
                }
            }
        }

        public virtual bool LoadByIdCache(Guid lnId)
        {
            MaxData loData = new MaxData(this.Data.DataModel);
            loData.Set(this.MaxBaseKeyLongDataModel.Id, lnId);
            return this.LoadByDataKeyCache(loData.DataModel.GetDataKey(loData));
        }
    }
}
