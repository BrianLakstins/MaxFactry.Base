// <copyright file="MaxBaseHttpDataModel.cs" company="Lakstins Family, LLC">
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
// <change date="3/20/2024" author="Brian A. Lakstins" description="Happy birthday to my mom.  Sara Jean Lakstins (Cartwright) - 3/20/1944 to 3/14/2019.">
// <change date="3/25/2024" author="Brian A. Lakstins" description="Initial creation">
// </changelog>
#endregion Change Log

namespace MaxFactry.Base.DataLayer
{
    using System;
    
    /// <summary>
    /// Defines data model to use to retrieve remote data through Http(s)
    /// </summary>
    public class MaxBaseHttpDataModel : MaxDataModel, IMaxHttpDataModel
    {
        /// <summary>
        /// Initializes a new instance of the MaxBaseHttpDataModel class
        /// </summary>
        public MaxBaseHttpDataModel()
        {
            this.AddNullable(this.Token, typeof(string));
            this.AddAttribute(this.Token, "IsEncrypted", "true");
            this.AddNullable(this.ClientId, typeof(string));
            this.AddNullable(this.ClientSecret, typeof(string));
            this.AddAttribute(this.ClientSecret, "IsEncrypted", "true");
            this.AddNullable(this.Scope, typeof(MaxLongString));
            this.AddNullable(this.GrantType, typeof(string));
            this.AddNullable(this.RequestUri, typeof(string));
            this.AddNullable(this.RequestTime, typeof(DateTime));
            this.AddNullable(this.RequestContent, typeof(MaxLongString));
            this.AddNullable(this.ResponseTime, typeof(DateTime));
            this.AddNullable(this.ResponseContent, typeof(MaxLongString));
        }

        public string Token
        {
            get
            {
                return "Token";
            }
        }

        public string ClientId
        {
            get
            {
                return "ClientId";
            }
        }

        public string ClientSecret
        {
            get
            {
                return "ClientSecret";
            }
        }

        public string Scope
        {
            get
            {
                return "Scope";
            }
        }

        public string GrantType
        {
            get
            {
                return "GrantType";
            }
        }

        public string RequestUri
        {
            get
            {
                return "RequestUrl";
            }
        }

        public string RequestContent
        {
            get
            {
                return "RequestContent";
            }
        }

        public string RequestTime
        {
            get
            {
                return "RequestTime";
            }
        }
        public string ResponseTime
        {
            get
            {
                return "ResponseTime";
            }
        }

        public string ResponseContent
        {
            get
            {
                return "ResponseContent";
            }
        }
    }
}