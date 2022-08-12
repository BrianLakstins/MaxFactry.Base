// <copyright file="IMaxMessageDataModel.cs" company="Lakstins Family, LLC">
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
// <change date="3/30/2015" author="Brian A. Lakstins" description="Initial Release">
// <change date="12/10/2020" author="Brian A. Lakstins" description="Add SentCount">
// </changelog>
#endregion

namespace MaxFactry.Base.DataLayer
{
	using System;

	/// <summary>
    /// Interface for MaxMessageRepository Data Model
	/// </summary>
	public interface IMaxMessageDataModel
	{
        /// <summary>
        /// Gets the name of the field holding the Id of the object this message is related to.
        /// </summary>
        string RelationId { get; }

        /// <summary>
        /// Gets the name of the field holding the Type of the object this message is related to.
        /// </summary>
        string RelationType { get; }

        /// <summary>
        /// Gets the name of the field holding the from address of the message
        /// </summary>
        string FromAddress { get; }

        /// <summary>
        /// Gets the name of the field holding the from name of the message
        /// </summary>
        string FromName { get; }

        /// <summary>
        /// Gets the name of the field holding the subject of the message
        /// </summary>
        string Subject { get; }

        /// <summary>
        /// Gets the name of the field holding the list of addresses the message is being st to
        /// </summary>
        string ToAddressList { get; }

        /// <summary>
        /// Gets the name of the field holding the list of names the message is being sent to
        /// </summary>
        string ToNameList { get; }

        /// <summary>
        /// Gets the name of the field holding the content of the message
        /// </summary>
        string Content { get; }

        /// <summary>
        /// Gets the name of the field holding the number of attachments the message has
        /// </summary>
        string AttachmentCount { get; }

        /// <summary>
        /// Gets the name of the field holding the Send Log for the message
        /// </summary>
        string SendLog { get; }

        /// <summary>
        /// Gets the name of the field that describes group that this message belongs to.
        /// </summary>
        string GroupName { get; }

        /// <summary>
        /// Gets the name of the field holding the reply address of the message
        /// </summary>
        string ReplyAddress { get; }

        /// <summary>
        /// Gets the number of emails actually sent
        /// </summary>
        string SentCount { get; }

        /// <summary>
        /// Gets the type used as the Message Repository provider
        /// </summary>
        Type MessageRepositoryProviderType { get; }
	}
}
