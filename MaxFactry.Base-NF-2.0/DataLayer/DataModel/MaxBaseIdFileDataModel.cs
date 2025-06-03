// <copyright file="MaxBaseIdFileDataModel.cs" company="Lakstins Family, LLC">
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
// <change date="8/19/2014" author="Brian A. Lakstins" description="Based on MaxAppIdDataModel without the AppId functionality.">
// <change date="3/26/2015" author="Brian A. Lakstins" description="Moved to MaxFactry.Base">
// <change date="7/12/2016" author="Brian A. Lakstins" description="Add ContentName to be used for name of file related to the Content.">
// <change date="4/30/2021" author="Brian A. Lakstins" description="Add content date for original file date and time.">
// </changelog>
#endregion

namespace MaxFactry.Base.DataLayer
{
	using System;
    using System.IO;
    using MaxFactry.Base.DataLayer;

	/// <summary>
    /// Defines base data model for data with a unique identifier
	/// </summary>
    public abstract class MaxBaseIdFileDataModel : MaxBaseIdVersionedDataModel
	{
        /// <summary>
        /// Name for the content in the file
        /// </summary>
        public readonly string ContentName = "ContentName";

        /// <summary>
        /// Content type in the file
        /// </summary>
        public readonly string ContentType = "ContentType";

        /// <summary>
        /// Length of the file
        /// </summary>
        public readonly string ContentLength = "ContentLength";

        /// <summary>
        /// Date of the file
        /// </summary>
        public readonly string ContentDate = "ContentDate";

        /// <summary>
        /// Mime Type of the file
        /// </summary>
        public readonly string MimeType = "MimeType";
        
        /// <summary>
        /// Content of the file.
        /// </summary>
        public readonly string Content = "Content";

        /// <summary>
        /// Name to use for the file.
        /// </summary>
        public readonly string FileName = "FileName";

        /// <summary>
        /// Name to use for the file.
        /// </summary>
        public readonly string FromFileName = "FromFileName";

        /// <summary>
        /// Initializes a new instance of the MaxBaseIdFileDataModel class
        /// </summary>
        public MaxBaseIdFileDataModel()
			: base()
		{
            this.AddNullable(this.ContentName, typeof(MaxShortString));
            this.AddType(this.ContentType, typeof(MaxShortString));
            this.AddType(this.ContentLength, typeof(long));
            this.AddType(this.ContentDate, typeof(DateTime));
            this.AddType(this.MimeType, typeof(MaxShortString));
            this.AddType(this.Content, typeof(Stream));
            this.AddType(this.FileName, typeof(string));
            this.AddNullable(this.FromFileName, typeof(string));
        }
    }
}
