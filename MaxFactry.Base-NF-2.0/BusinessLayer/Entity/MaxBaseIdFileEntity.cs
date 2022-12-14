// <copyright file="MaxBaseIdFileEntity.cs" company="Lakstins Family, LLC">
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
// <change date="8/19/2014" author="Brian A. Lakstins" description="Initial Release">
// <change date="9/16/2014" author="Brian A. Lakstins" description="Fix ContentLength to be a long.">
// <change date="9/16/2014" author="Brian A. Lakstins" description="Remove while loop for adding a space to the sort string.">
// <change date="11/10/2014" author="Brian A. Lakstins" description="Update to moving Get and Set methods from MaxData.">
// <change date="11/11/2014" author="Brian A. Lakstins" description="Updated to use as a base for other file enties only.">
// <change date="3/26/2015" author="Brian A. Lakstins" description="Moved to MaxFactry.Base">
// <change date="7/12/2016" author="Brian A. Lakstins" description="Add ability to specify file name to be used for mime type.">
// <change date="9/2/2016" author="Brian A. Lakstins" description="Fix instance where stream is been disposed.">
// <change date="12/21/2016" author="Brian A. Lakstins" description="Removed code that was moved into another method.">
// <change date="11/8/2018" author="Brian A. Lakstins" description="Fix getting mime type from resource.">
// <change date="4/30/2021" author="Brian A. Lakstins" description="Add content date for original file date and time.">
// </changelog>
#endregion

namespace MaxFactry.Base.BusinessLayer
{
    using System;
    using System.IO;
    using System.Reflection;
    using MaxFactry.Core;
    using MaxFactry.Base.BusinessLayer;
    using MaxFactry.Base.DataLayer;

    /// <summary>
    /// Base entity for interacting with files.
    /// </summary>
    public abstract class MaxBaseIdFileEntity : MaxBaseIdVersionedEntity
    {
        /// <summary>
        /// Long string of spaces to help with creating sorted strings.
        /// </summary>
        private static string _sSpace = "                                                                                                         ";

        /// <summary>
        /// An index to hold extensions and mime types.
        /// </summary>
        private static MaxIndex _oMimeTypeIndex = null;

        /// <summary>
        /// Lock to protect threads.
        /// </summary>
        private static object _oLock = new object();

        /// <summary>
        /// Initializes a new instance of the MaxBaseIdFileEntity class
        /// </summary>
        /// <param name="loData">object to hold data</param>
        public MaxBaseIdFileEntity(MaxFactry.Base.DataLayer.MaxData loData)
            : base(loData)
        {
        }

        /// <summary>
        /// Initializes a new instance of the MaxBaseIdFileEntity class.
        /// </summary>
        /// <param name="loDataModelType">Type of data model.</param>
        public MaxBaseIdFileEntity(Type loDataModelType)
            : base(loDataModelType)
        {
        }

        /// <summary>
        /// Gets or sets the name of the file.
        /// </summary>
        public string ContentName
        {
            get
            {
                return this.GetString(this.MaxBaseIdFileDataModel.ContentName);
            }

            set
            {
                this.Set(this.MaxBaseIdFileDataModel.ContentName, value);
            }
        }

        /// <summary>
        /// Gets or sets the content type of the file.
        /// </summary>
        public string ContentType
        {
            get
            {
                return this.GetString(this.MaxBaseIdFileDataModel.ContentType);
            }

            set
            {
                this.Set(this.MaxBaseIdFileDataModel.ContentType, value);
            }
        }

        /// <summary>
        /// Gets or sets the length of the file content.
        /// </summary>
        public long ContentLength
        {
            get
            {
                return this.GetLong(this.MaxBaseIdFileDataModel.ContentLength);
            }

            set
            {
                this.Set(this.MaxBaseIdFileDataModel.ContentLength, value);
            }
        }

        /// <summary>
        /// Gets or sets the date of the file content.
        /// </summary>
        public DateTime ContentDate
        {
            get
            {
                return this.GetDateTime(this.MaxBaseIdFileDataModel.ContentDate);
            }

            set
            {
                this.Set(this.MaxBaseIdFileDataModel.ContentDate, value);
            }
        }

        /// <summary>
        /// Gets or sets the mime type
        /// </summary>
        public string MimeType
        {
            get
            {
                return this.GetString(this.MaxBaseIdFileDataModel.MimeType);
            }

            set
            {
                this.Set(this.MaxBaseIdFileDataModel.MimeType, value);
            }
        }

        /// <summary>
        /// Gets or sets a Stream to access the content of the file.
        /// </summary>
        public Stream Content
        {
            get
            {
                Stream loR = this.GetStream(this.MaxBaseIdFileDataModel.Content);
                return loR;
            }

            set
            {
                this.Set(this.MaxBaseIdFileDataModel.Content, value);
            }
        }

        /// <summary>
        /// Gets or sets the name of the file
        /// </summary>
        public string FileName
        {
            get
            {
                return this.GetString(this.MaxBaseIdFileDataModel.FileName);
            }

            set
            {
                this.Set(this.MaxBaseIdFileDataModel.FileName, value);
            }
        }

        /// <summary>
        /// Gets or sets the name of the file where this file came from
        /// </summary>
        public string FromFileName
        {
            get
            {
                return this.GetString(this.MaxBaseIdFileDataModel.FromFileName);
            }

            set
            {
                this.Set(this.MaxBaseIdFileDataModel.FromFileName, value);
            }
        }

        public string GetContentUrl()
        {
            return MaxBaseIdRepository.GetStreamUrl(this.Data, this.MaxBaseIdFileDataModel.Content);
        }

        /// <summary>
        /// Gets an index to hold extensions and mime types.
        /// </summary>
        protected static MaxIndex MimeTypeIndex
        {
            get
            {
                if (null == _oMimeTypeIndex)
                {
                    lock (_oLock)
                    {
                        if (null == _oMimeTypeIndex)
                        {
                            _oMimeTypeIndex = new MaxIndex();
                            string lsContent = MaxFactryLibrary.GetStringResource(typeof(MaxBaseIdFileEntity), "mimetypes");
                            if (lsContent.Contains("\n") && lsContent.Contains("\r"))
                            {
                                lsContent = lsContent.Replace('\r', ' ');
                            }
                            else if (lsContent.Contains("\r"))
                            {
                                lsContent = lsContent.Replace('\r', '\n');
                            }

                            string[] laContent = lsContent.Split('\n');
                            foreach (string lsLine in laContent)
                            {
                                if (lsLine.Length > 1 && !lsLine.Substring(0, 1).Equals("#") && lsLine.Contains("\t"))
                                {
                                    string[] laLine = lsLine.Split('\t');
                                    string lsMimeType = laLine[0];
                                    if (lsMimeType.Length > 0)
                                    {
                                        for (int lnM = 1; lnM < laLine.Length; lnM++)
                                        {
                                            string lsExtensionList = laLine[lnM].Trim();
                                            if (null != lsExtensionList && lsExtensionList.Length > 0)
                                            {
                                                string[] laExtensionList = lsExtensionList.Split(' ');
                                                for (int lnE = 0; lnE < laExtensionList.Length; lnE++)
                                                {
                                                    string lsExtensionPossible = laExtensionList[lnE].Trim().ToLower();
                                                    if (!_oMimeTypeIndex.Contains(lsExtensionPossible))
                                                    {
                                                        _oMimeTypeIndex.Add(lsExtensionPossible, lsMimeType);
                                                    }
                                                    else if (!(MaxConvertLibrary.ConvertToString(typeof(object), _oMimeTypeIndex[lsExtensionPossible]).ToLower().IndexOf(lsExtensionPossible) >= 0) &&
                                                        lsMimeType.ToLower().IndexOf(lsExtensionPossible) >= 0)
                                                    {
                                                        _oMimeTypeIndex[lsExtensionPossible] = lsMimeType;
                                                    }
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }

                return _oMimeTypeIndex;
            }
        }

        /// <summary>
        /// Gets the Data Model for this entity
        /// </summary>
        protected MaxBaseIdFileDataModel MaxBaseIdFileDataModel
        {
            get
            {
                return (MaxBaseIdFileDataModel)MaxDataLibrary.GetDataModel(this.DataModelType);
            }
        }

        /// <summary>
        /// Gets the extension of a file name.
        /// </summary>
        /// <param name="lsName">Name of a file.</param>
        /// <returns>Extension of the file.</returns>
        public static string GetFileNameExtension(string lsName)
        {
            string lsR = string.Empty;
            if (lsName.IndexOf(".") >= 0 && lsName.LastIndexOf('.') != lsName.Length - 1)
            {
                lsR = lsName.Substring(lsName.LastIndexOf('.') + 1);
            }

            return lsR;
        }

        /// <summary>
        /// Gets the mime-type of the file.
        /// </summary>
        /// <param name="lsName">File name</param>
        /// <returns>Data updated based on sending of message.</returns>
        public static string GetMimeType(string lsName)
        {
            string lsR = "application/octet-stream";
            string lsExtension = GetFileNameExtension(lsName);
            if (null != lsExtension && lsExtension.Length > 0)
            {
                if (MimeTypeIndex.Contains(lsExtension.ToLower()))
                {
                    lsR = MimeTypeIndex[lsExtension.ToLower()].ToString();
                }
            }

            return lsR;
        }

        /// <summary>
        /// Gets a string that can be used to sort a list of this entity.
        /// </summary>
        /// <returns>Lowercase version of Name padded to 100 characters.</returns>
        public override string GetDefaultSortString()
        {
            string lsSort = this.Name.ToLower();
            int lnSpaceLength = 100 - lsSort.Length;
            if (lnSpaceLength > 0)
            {
                lsSort = _sSpace.Substring(0, lnSpaceLength) + lsSort;
            }

            return lsSort + base.GetDefaultSortString();
        }

        /// <summary>
        /// Gets the mime-type of the file.
        /// </summary>
        /// <returns>Data updated based on sending of message.</returns>
        public string GetMimeType()
        {
            return GetMimeType(this.FileName);
        }
    }
}
