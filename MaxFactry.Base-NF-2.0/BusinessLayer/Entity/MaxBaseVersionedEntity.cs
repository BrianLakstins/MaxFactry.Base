// <copyright file="MaxBaseVersionedEntity.cs" company="Lakstins Family, LLC">
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
// <change date="6/18/2025" author="Brian A. Lakstins" description="Initial creation">
// </changelog>
#endregion

namespace MaxFactry.Base.BusinessLayer
{
    using System;
    using MaxFactry.Base.DataLayer;
    using MaxFactry.Base.DataLayer.Library;

    /// <summary>
    /// Base entity for interacting with items with a name that need to be versioned
    /// </summary>
    public abstract class MaxBaseVersionedEntity : MaxBaseEntity
    {
        private static object _oLock = new object();

        /// <summary>
        /// Initializes a new instance of the MaxBaseIdVersionedEntity class
        /// </summary>
        /// <param name="loData">object to hold data</param>
        public MaxBaseVersionedEntity(MaxFactry.Base.DataLayer.MaxData loData)
            : base(loData)
        {
        }

        /// <summary>
        /// Initializes a new instance of the MaxBaseIdVersionedEntity class.
        /// </summary>
        /// <param name="loDataModelType">Type of data model.</param>
        public MaxBaseVersionedEntity(Type loDataModelType)
            : base(loDataModelType)
        {
        }

        /// <summary>
        /// Gets or sets the name of the versioned information.
        /// </summary>
        public string Name
        {
            get
            {
                return this.GetString(this.MaxBaseVersionedDataModel.Name);
            }

            set
            {
                this.Set(this.MaxBaseVersionedDataModel.Name, value);
            }
        }

        /// <summary>
        /// Gets the version
        /// </summary>
        public int Version
        {
            get
            {
                return this.GetInt(this.MaxBaseVersionedDataModel.Version);
            }
        }

        public Guid Id
        {
            get
            {
                return this.GetGuid(this.MaxBaseVersionedDataModel.Id);
            }
        }

        /// <summary>
        /// Gets the Data Model for this entity
        /// </summary>
        protected MaxBaseVersionedDataModel MaxBaseVersionedDataModel
        {
            get
            {
                return (MaxBaseVersionedDataModel)MaxDataLibrary.GetDataModel(this.DataModelType);
            }
        }

        /// <summary>
        /// Gets the current entity for the virtual path.
        /// </summary>
        /// <param name="lsName">Key used for the versioned information.</param>
        /// <returns>Current entity.</returns>
        public virtual MaxBaseVersionedEntity GetCurrent(string lsName)
        {
            MaxBaseVersionedEntity loR = null;
            MaxDataQuery loDataQuery = this.GetDataQuery();
            loDataQuery.StartGroup();
            loDataQuery.AddFilter(new MaxDataFilter(this.MaxBaseVersionedDataModel.IsActive, "=", true));
            loDataQuery.EndGroup();

            MaxData loData = new MaxData(this.Data.DataModel);
            loData.Set(this.MaxBaseVersionedDataModel.Name, lsName);
            MaxEntityList loList = this.LoadAllByPageCache(loData, 0, 0, string.Empty, loDataQuery);
            if (loList.Count == 0)
            {
                loDataQuery = this.GetDataQuery();
                loList = this.LoadAllByPageCache(loData, 0, 0, string.Empty, loDataQuery);
                if (loList.Count == 0)
                {
                    MaxEntityList loAllList = this.LoadAllCache();
                    for (int lnE = 0; lnE < loAllList.Count; lnE++)
                    {
                        MaxBaseVersionedEntity loEntity = loAllList[lnE] as MaxBaseVersionedEntity;
                        if (loEntity.Name.Equals(lsName, StringComparison.InvariantCultureIgnoreCase))
                        {
                            loList.Add(loEntity);
                        }
                    }
                }
            }

            if (loList.Count == 1)
            {
                loR = loList[0] as MaxBaseVersionedEntity;
            }
            else if (loList.Count > 1)
            {
                for (int lnE = 0; lnE < loList.Count; lnE++)
                {
                    if (null == loR)
                    {
                        loR = loList[lnE] as MaxBaseVersionedEntity;
                    }
                    else if (loR.Version < (loList[lnE] as MaxBaseVersionedEntity).Version)
                    {
                        if (loR.IsActive)
                        {
                            loR.IsActive = false;
                            loR.Update();
                        }

                        loR = loList[lnE] as MaxBaseVersionedEntity;
                    }
                }
            }

            return loR;
        }

        /// <summary>
        /// Gets the next version based on however the data is stored.
        /// </summary>
        /// <returns>integer to use for next version</returns>
        public virtual int GetNextVersion()
        {
            int lnR = 1;
            lock (_oLock)
            {
                MaxBaseVersionedEntity loEntity = this.GetCurrent(this.Name);
                if (null != loEntity)
                {
                    lnR = loEntity.Version + 1;
                }
            }

            return lnR;
        }

        /// <summary>
        /// Gets a string that can be used to sort a list of this entity.
        /// </summary>
        /// <returns>Lowercase version of Name passed to 100 characters.</returns>
        public override string GetDefaultSortString()
        {
            return this.Name.ToLowerInvariant().PadRight(500, ' ') + base.GetDefaultSortString();
        }

        protected override void SetInitial()
        {
            base.SetInitial();
            this.Set(this.MaxBaseVersionedDataModel.Id, Guid.NewGuid());
            this.IsActive = true;
            this.Set(this.MaxBaseVersionedDataModel.Version, this.GetNextVersion());
        }

        public override bool Delete()
        {
            //// Can't delete a versioned entity, just mark it as inactive
            throw new NotImplementedException();
        }
    }
}
