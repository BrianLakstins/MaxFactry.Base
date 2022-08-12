// <copyright file="MaxBaseEmailDataModel.cs" company="Lakstins Family, LLC">
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
#endregion License

#region Change Log
// <changelog>
// <change date="4/9/2015" author="Brian A. Lakstins" description="Initial creation">
// <change date="4/13/2016" author="Brian A. Lakstins" description="Fix for fields that can be null.">
// <change date="12/10/2020" author="Brian A. Lakstins" description="Add SentCount">
// </changelog>
#endregion Change Log

namespace MaxFactry.Base.DataLayer
{
    using System;
    using MaxFactry.Base.DataLayer;

    /// <summary>
    /// Data model to use for emails.
    /// </summary>
    public abstract class MaxBaseEmailDataModel : MaxBaseIdDataModel, IMaxMessageDataModel
    {
        /// <summary>
        /// Initializes a new instance of the MaxBaseEmailDataModel class.
        /// </summary>
        public MaxBaseEmailDataModel()
            : base()
        {
            this.AddType(this.RelationId, typeof(Guid));
            this.AddType(this.RelationType, typeof(MaxShortString));
            this.AddType(this.FromAddress, typeof(MaxShortString));
            this.AddNullable(this.FromName, typeof(MaxShortString));
            this.AddType(this.Subject, typeof(MaxShortString));
            this.AddType(this.ToAddressList, typeof(string));
            this.AddNullable(this.ToNameList, typeof(string));
            this.AddType(this.Content, typeof(MaxLongString));
            this.AddNullable(this.AttachmentCount, typeof(int));
            this.AddNullable(this.SendLog, typeof(MaxLongString));
            this.AddNullable(this.GroupName, typeof(MaxShortString));
            this.AddNullable(this.ReplyAddress, typeof(MaxShortString));
            this.AddNullable(this.SentCount, typeof(int));
        }

        /// <summary>
        /// Gets the name of the field holding the number of attachments the message has
        /// </summary>
        public string AttachmentCount 
        { 
            get { return "AttachmentCount"; } 
        }

        /// <summary>
        /// Gets the name of the field holding the content of the message
        /// </summary>
        public string Content 
        { 
            get { return "Content"; } 
        }

        /// <summary>
        /// Gets the name of the field holding the from address of the message
        /// </summary>
        public string FromAddress 
        { 
            get { return "FromAddress"; } 
        }

        /// <summary>
        /// Gets the name of the field holding the from name of the message
        /// </summary>
        public string FromName 
        { 
            get { return "FromName"; } 
        }

        /// <summary>
        /// Gets the type used as the Message Repository provider
        /// </summary>
        public Type MessageRepositoryProviderType 
        {
            get { return null; } 
        }

        /// <summary>
        /// Gets the Id of the object this email is related to.
        /// </summary>
        public string RelationId 
        { 
            get { return "RelationId"; } 
        }

        /// <summary>
        /// Gets the name of the field holding the Type of the object this email is related to.
        /// </summary>
        public string RelationType 
        { 
            get { return "RelationType"; } 
        }

        /// <summary>
        /// Gets the name of the field holding the Send Log for the message
        /// </summary>
        public string SendLog 
        { 
            get { return "SendLog"; } 
        }

        /// <summary>
        /// Gets the name of the field holding the subject of the message
        /// </summary>
        public string Subject 
        { 
            get { return "Subject"; } 
        }

        /// <summary>
        /// Gets the name of the field holding the list of addresses the message is being st to
        /// </summary>
        public string ToAddressList 
        { 
            get { return "ToAddressList"; } 
        }

        /// <summary>
        /// Gets the name of the field holding the list of names the message is being sent to
        /// </summary>
        public string ToNameList 
        { 
            get { return "ToNameList"; } 
        }

        /// <summary>
        /// Gets the name of the field that describes group that this email belongs to.
        /// </summary>
        public string GroupName
        {
            get { return "GroupName"; }
        }

        /// <summary>
        /// Gets the name of the field holding the reply address of the message
        /// </summary>
        public string ReplyAddress
        {
            get { return "ReplyAddress"; }
        }

        /// <summary>
        /// Gets the name of the field holding the reply address of the message
        /// </summary>
        public string SentCount
        {
            get { return "SentCount"; }
        }
    }
}