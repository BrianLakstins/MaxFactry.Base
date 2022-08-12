// <copyright file="MaxBaseIdMessageDataModel.cs" company="Lakstins Family, LLC">
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
// <change date="7/4/2014" author="Brian A. Lakstins" description="Based on MaxAppIdBaseIdDataModel without the AppId functionality.">
// <change date="3/26/2015" author="Brian A. Lakstins" description="Moved to MaxFactry.Base">
// </changelog>
#endregion

namespace MaxFactry.Base.DataLayer
{
	using System;
    using MaxFactry.Base.DataLayer;

	/// <summary>
	/// Data structure to use for creating data that can have properties overridden.
	/// </summary>
	public abstract class MaxBaseIdMessageDataModel : MaxBaseIdDataModel
	{
		/// <summary>
		/// To address for message.
		/// </summary>
		public readonly string To = "To";

        /// <summary>
        /// Message Content
        /// </summary>
        public readonly string Message = "Message";

        /// <summary>
        /// Configuration information stored as bits
        /// </summary>
        public readonly string ConfigurationFlag = "ConfigurationFlag";

        /// <summary>
        /// Log information about handling of the message
        /// </summary>
        public readonly string Log = "Log";

		/// <summary>
        /// Initializes a new instance of the MaxBaseIdMessageDataModel class
		/// </summary>
        public MaxBaseIdMessageDataModel()
			: base()
		{
            this.AddType(this.To, typeof(string));
            this.AddType(this.Message, typeof(MaxLongString));
            this.AddNullable(this.ConfigurationFlag, typeof(long));
            this.AddNullable(this.Log, typeof(MaxLongString));
        }
	}
}
