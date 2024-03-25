// <copyright file="IMaxHttpDataModel.cs" company="Lakstins Family, LLC">
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
    /// Interface for MaxHttpLibrary Data Model
	/// </summary>
	public interface IMaxHttpDataModel
	{
        /// <summary>
        /// Gets the name of the field holding the uri to request through http
        /// </summary>
        string RequestUri { get; }

        /// <summary>
        /// Gets the name of the field holding the client_id
        /// </summary>
        string ClientId { get; }

        /// <summary>
        /// Gets the name of the field holding the client_secret
        /// </summary>
        string ClientSecret { get; }

        /// <summary>
        /// Gets the name of the field holding token
        /// </summary>
        string Token { get; }

        /// <summary>
        /// The scope when requesting a token
        /// </summary>
        string Scope { get; }

        /// <summary>
        /// The type of grant when requesting a token
        /// </summary>
        string GrantType { get; }

        /// <summary>
        /// Gets the name of the field holding the content to send through http
        /// </summary>
        string RequestContent { get; }

        /// <summary>
        /// Time the request started
        /// </summary>
        string RequestTime { get; }

        /// <summary>
        /// Time the response came back
        /// </summary>
        string ResponseTime { get; }

        /// <summary>
        /// Content of the response
        /// </summary>
        string ResponseContent { get; }
    }
}
