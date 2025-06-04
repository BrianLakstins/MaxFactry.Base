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
// <change date="4/9/2025" author="Brian A. Lakstins" description="Add ways to customize the number used for the Id">
// <change date="6/4/2025" author="Brian A. Lakstins" description="Get DataQuery from entity">
// </changelog>
#endregion

namespace MaxFactry.Base.BusinessLayer
{
    using System;
    using MaxFactry.Base.DataLayer;
    using MaxFactry.Base.DataLayer.Library;
    using MaxFactry.Core;

    /// <summary>
    /// Base Business Layer Entity using a long as the primary key
    /// </summary>
    public abstract class MaxBaseKeyLongEntity : MaxBaseEntity
    {
        private static object _oLock = new object();

        /// <summary>
        /// Number of digits to use for daily Id
        /// </summary>
        private int _nIdDigits = 18;

        /// <summary>
        /// Date to use for Id 
        /// </summary>
        private DateTime _dIdDate = DateTime.MinValue;

        /// <summary>
        /// Field to use for date (default is CreatedDate)
        /// </summary>
        private string _sIdDateField = string.Empty;

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
        protected int IdDigits
        {
            get
            {
                return this._nIdDigits;
            }
            set
            {
                this._nIdDigits = value;
            }
        }

        /// <summary>
        /// Gets or sets the name of the field to use for the Id date.
        /// </summary>
        protected DateTime IdDate
        {
            get
            {
                return this._dIdDate;
            }
            set
            {
                this._dIdDate = value;
            }
        }

        /// <summary>
        /// Gets or sets the name of the field to use for the Id date.
        /// </summary>
        protected string IdDateField
        {
            get
            {
                return this._sIdDateField;
            }
            set
            {
                this._sIdDateField = value;
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

        protected virtual string GetIdStart(DateTime ldIdDate)
        {
            //// Format of Id is YYMMDD#####. 
            string lsR = ((ldIdDate.Year - 2000) * 10000 + ldIdDate.Month * 100 + ldIdDate.Day).ToString();
            return lsR;
        }

        protected override void SetInitial()
        {
            base.SetInitial();
            if (long.MinValue == this.Id)
            {
                lock (_oLock)
                {
                    //// Defaults to generate a 16 digit number base on now
                    long lnId = DateTime.UtcNow.Ticks;
                    DateTime ldIdDate = this.IdDate;

                    if (!string.IsNullOrEmpty(this._sIdDateField))
                    {
                        ldIdDate = this.GetDateTime(this._sIdDateField);
                    }

                    if (ldIdDate > DateTime.MinValue) 
                    {
                        //9,223,372,036,854,775,807 is max long which is 19 digits long
                        string lsStartId = this.GetIdStart(ldIdDate);
                        string lsEndId = this.GetIdStart(ldIdDate.AddDays(1));
                        long lnStartId = MaxConvertLibrary.ConvertToLong(typeof(object), lsStartId.PadRight(this.IdDigits, '0'));
                        long lnEndId = MaxConvertLibrary.ConvertToLong(typeof(object), lsEndId.PadRight(this.IdDigits, '0'));

                        MaxDataQuery loDataQuery = this.GetDataQuery();
                        loDataQuery.StartGroup();
                        loDataQuery.AddFilter(this.MaxBaseKeyLongDataModel.Id, ">=", lnStartId);
                        loDataQuery.AddAnd();
                        loDataQuery.AddFilter(this.MaxBaseKeyLongDataModel.Id, "<", lnEndId);
                        loDataQuery.EndGroup();

                        MaxDataList loList = MaxBaseReadRepository.Select(this.Data, loDataQuery, 1, 1, this.MaxBaseKeyLongDataModel.Id + " desc", this.MaxBaseKeyLongDataModel.Id);
                        lnId = lnStartId;
                        if (loList.Count > 0)
                        {
                            long lnLastIdOnDay = MaxFactry.Core.MaxConvertLibrary.ConvertToLong(typeof(object), loList[0].Get(this.MaxBaseKeyLongDataModel.Id));
                            lnId = lnLastIdOnDay + 1;
                        }
                    }

                    this.SetId(lnId);
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
