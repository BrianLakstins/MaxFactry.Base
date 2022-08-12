// <copyright file="MaxMessageRepositoryDefaultProvider.cs" company="Lakstins Family, LLC">
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
// <change date="9/15/2014" author="Brian A. Lakstins" description="Based on MaxCRUDRepository">
// <change date="12/18/2014" author="Brian A. Lakstins" description="Updated Provider and DataModel access pattern.">
// <change date="3/26/2015" author="Brian A. Lakstins" description="Restructured.">
// <change date="9/23/2020" author="Brian A. Lakstins" description="Update ToAddress and ToName list parsing.">
// <change date="12/10/2020" author="Brian A. Lakstins" description="Add SentCount">
// </changelog>
#endregion

namespace MaxFactry.Base.DataLayer.Provider
{
	using System;
    using MaxFactry.Core;

    /// <summary>
    /// Provides static methods to manipulate storage of temporarily cached data
    /// </summary>
    public class MaxMessageRepositoryDefaultProvider : MaxProvider, IMaxMessageRepositoryProvider
	{
        private MaxIndex _oHtmlIndex = null;

        public MaxIndex HtmlIndex
        {
            get
            {
                if (null == this._oHtmlIndex)
                {
                    this._oHtmlIndex = new MaxIndex();
                    string[] laTagList = "a|abbr|acronym|address|area|b|base|bdo|big|blockquote|body|br|button|caption|cite|code|col|colgroup|dd|del|dfn|div|dl|DOCTYPE|dt|em|fieldset|form|h1|h2|h3|h4|h5|h6|head|html|hr|i|img|input|ins|kbd|label|legend|li|link|map|meta|noscript|object|ol|optgroup|option|p|param|pre|q|samp|script|select|small|span|strong|style|sub|sup|table|tbody|td|textarea|tfoot|th|thead|title|tr|tt|ul|var".Split('|');
                    foreach (string lsTag in laTagList)
                    {
                        string lsPattern = @"<\s*" + lsTag + @"\s*\/?>";
                        System.Text.RegularExpressions.Regex loRegex = new System.Text.RegularExpressions.Regex(lsPattern.ToLower());
                        this._oHtmlIndex.Add(lsTag, loRegex);
                    }
                }

                return this._oHtmlIndex;
            }
        }

        /// <summary>
        /// Sends a message
        /// </summary>
        /// <param name="loData">The data for the message</param>
        /// <returns>The data that was sent.</returns>
        public virtual MaxData Send(MaxData loData)
        {
            IMaxMessageDataModel loDataModel = loData.DataModel as IMaxMessageDataModel;
            if (null == loDataModel)
            {
                throw new MaxException("DataModel for [" + this.GetType().ToString() + "] does not implement required interface.");
            }

            string lsLog = MaxConvertLibrary.ConvertToString(this.GetType(), loData.Get(loDataModel.SendLog));
            try
            {

                string lsFromAddress = MaxConvertLibrary.ConvertToString(this.GetType(), loData.Get(loDataModel.FromAddress));
                string lsFromName = MaxConvertLibrary.ConvertToString(this.GetType(), loData.Get(loDataModel.FromName));
                string lsSubject = MaxConvertLibrary.ConvertToString(this.GetType(), loData.Get(loDataModel.Subject));
                string lsContent = MaxConvertLibrary.ConvertToString(this.GetType(), loData.Get(loDataModel.Content));
                string lsToAddressList = MaxConvertLibrary.ConvertToString(this.GetType(), loData.Get(loDataModel.ToAddressList));
                string lsReplyAddress = MaxConvertLibrary.ConvertToString(this.GetType(), loData.Get(loDataModel.ReplyAddress));
                int lnSentCount = MaxConvertLibrary.ConvertToInt(this.GetType(), loData.Get(loDataModel.SentCount));
                if (lnSentCount < 0)
                {
                    lnSentCount = 0;
                }

                object loType = loData.Get(loDataModel.RelationType);
                string lsType = "MaxDefault";
                if (null != loType)
                {
                    lsType = MaxConvertLibrary.ConvertToString(this.GetType(), loType);
                }

                lsLog += DateTime.UtcNow.ToString() + "\tInfo\tStart sending email\n";
                if (lsToAddressList.Length > 0 && lsSubject.Length > 0)
                {
                    string[] laToAddressList = lsToAddressList.Split('\t');
                    if (laToAddressList.Length == 1)
                    {
                        string lsAddressListParsed = lsToAddressList.Replace("\";\"", "\t");
                        if (lsAddressListParsed.IndexOf(';') != -1)
                        {
                            laToAddressList = lsAddressListParsed.Split(';');
                            for (int lnN = 0; lnN < laToAddressList.Length; lnN++)
                            {
                                laToAddressList[lnN] = laToAddressList[lnN].Replace("\t", "\";\"");
                            }
                        }

                        if (laToAddressList.Length == 1)
                        {
                            lsAddressListParsed = lsToAddressList.Replace("\",\"", "\t");
                            if (lsAddressListParsed.IndexOf(',') != -1)
                            {
                                laToAddressList = lsAddressListParsed.Split(',');
                                for (int lnN = 0; lnN < laToAddressList.Length; lnN++)
                                {
                                    laToAddressList[lnN] = laToAddressList[lnN].Replace("\t", "\",\"");
                                }
                            }
                        }
                    }

                    string[] laToNameList = new string[] { string.Empty };
                    string lsToNameList = MaxConvertLibrary.ConvertToString(this.GetType(), loData.Get(loDataModel.ToNameList));
                    if (lsToNameList.Length > 0)
                    {
                        laToNameList = lsToNameList.Split('\t');
                    }

                    //// Attachment requires name, stream, and mime type
                    MaxIndex loAttachmentList = new MaxIndex();
                    int lnAttachmentCount = MaxConvertLibrary.ConvertToInt(typeof(object), loData.Get(loDataModel.AttachmentCount));
                    if (lnAttachmentCount > 0)
                    {
                        for (int lnA = 0; lnA < lnAttachmentCount; lnA++)
                        {
                            MaxIndex loAttachmentData = loData.Get(loDataModel.AttachmentCount + "_" + lnA.ToString()) as MaxIndex;
                            if (null != loAttachmentData)
                            {
                                loAttachmentList.Add(lnA.ToString(), loAttachmentData);
                            }
                        }
                    }

                    string lsSendResult = this.SendConditional(lsType, lsFromAddress, lsFromName, lsSubject, lsContent, laToAddressList, laToNameList, loAttachmentList, lsReplyAddress);
                    if (!lsSendResult.Contains("\tError\t"))
                    {
                        lnSentCount++;
                        loData.Set(loDataModel.SentCount, lnSentCount);
                    }

                    lsLog += lsSendResult;
                }
                else
                {
                    lsLog += DateTime.UtcNow.ToString() + "\tError\tInsufficient information to send email\n";
                    if (lsToAddressList.Length == 0 && lsSubject.Length > 0)
                    {
                        MaxLogLibrary.Log(new MaxLogEntryStructure(MaxEnumGroup.LogError, "Address missing for email with subject: {Subject}", lsSubject));
                    }
                    else if (lsToAddressList.Length > 0 && lsSubject.Length == 0)
                    {
                        MaxLogLibrary.Log(new MaxLogEntryStructure(MaxEnumGroup.LogError, "Subject missing for email to: {Address}", lsToAddressList));
                    }
                    else
                    {
                        MaxLogLibrary.Log(new MaxLogEntryStructure(MaxEnumGroup.LogError, "Missing required info for sending email."));
                    }
                }
            }
            catch (Exception loE)
            {
                lsLog += "\tError\tException message sending: " + loE.Message + "\n";
                MaxLogLibrary.Log(new MaxLogEntryStructure(MaxEnumGroup.LogError, "Error sending email", loE));
            }

            loData.Set(loDataModel.SendLog, lsLog);
            return loData;
        }

        /// <summary>
        /// Checks for Html tags
        /// </summary>
        /// <param name="lsPossibleHtml">Text that may contain HTML</param>
        /// <returns>True if an HTML tag is found.</returns>
        public bool HasHtml(string lsPossibleHtml)
        {
            string[] laKey = this.HtmlIndex.GetSortedKeyList();
            foreach (string lsKey in laKey)
            {
                System.Text.RegularExpressions.Regex loRegex = this.HtmlIndex[lsKey] as System.Text.RegularExpressions.Regex;
                if (null != loRegex)
                {
                    if (loRegex.IsMatch(lsPossibleHtml.ToLower()))
                    {
                        return true;
                    }
                }
            }

            return false;
        }

        public virtual string Send(string lsType, string lsFromAddress, string lsFromName, string lsSubject, string lsContent, string[] laToAddressList, string[] laToNameList, MaxIndex loAttachmentList, string lsReplyAddress)
        {
            return this.SendConditional(lsType, lsFromAddress, lsFromName, lsSubject, lsContent, laToAddressList, laToNameList, loAttachmentList, lsReplyAddress);
        }


#if net2 || netcore2
        public virtual string SendConditional(string lsType, string lsFromAddress, string lsFromName, string lsSubject, string lsContent, string[] laToAddressList, string[] laToNameList, MaxIndex loAttachmentList, string lsReplyAddress)
        {
            string lsLog = string.Empty;
            System.Net.Mail.MailMessage loMessage = new System.Net.Mail.MailMessage();
            for (int lnT = 0; lnT < laToAddressList.Length; lnT++)
            {
                if (!string.IsNullOrEmpty(laToAddressList[lnT]))
                {
                    System.Net.Mail.MailAddress loToAddress = new System.Net.Mail.MailAddress(laToAddressList[lnT]);
                    if (lnT < laToNameList.Length)
                    {
                        loToAddress = new System.Net.Mail.MailAddress(laToAddressList[lnT], laToNameList[lnT]);
                    }
                    else if (laToNameList.Length > 0)
                    {
                        loToAddress = new System.Net.Mail.MailAddress(laToAddressList[lnT], laToNameList[0]);
                    }

                    loMessage.To.Add(loToAddress);
                }
            }

            if (lsFromAddress.Length > 0)
            {
                loMessage.From = new System.Net.Mail.MailAddress(MaxConvertLibrary.ConvertToString(this.GetType(), lsFromAddress));
                if (lsFromName.Length > 0)
                {
                    loMessage.From = new System.Net.Mail.MailAddress(
                        lsFromAddress,
                        lsFromName);
                }
            }

#if net4_52 || netcore1 || netstandard1_2
            if (!string.IsNullOrEmpty(lsReplyAddress))
            {
                System.Net.Mail.MailAddress loReplyAddress = new System.Net.Mail.MailAddress(lsReplyAddress);
                loMessage.ReplyToList.Add(loReplyAddress);
            }
#endif

            loMessage.Subject = lsSubject;
            loMessage.Body = lsContent;
            loMessage.IsBodyHtml = false;

            if (this.HasHtml(loMessage.Body))
            {
                loMessage.IsBodyHtml = true;
            }

            //// Attachment requires name, stream, and mime type
            for (int lnA = 0; lnA < loAttachmentList.Count; lnA++)
            {
                MaxIndex loAttachmentData = loAttachmentList[lnA.ToString()] as MaxIndex;
                if (null != loAttachmentData)
                {
                    string lsName = MaxConvertLibrary.ConvertToString(typeof(object), loAttachmentData["name"]);
                    string lsMime = MaxConvertLibrary.ConvertToString(typeof(object), loAttachmentData["mime"]);
                    System.IO.Stream loStream = loAttachmentData["stream"] as System.IO.Stream;
                    loMessage.Attachments.Add(new System.Net.Mail.Attachment(loStream, lsName, lsMime));
                }
            }

            if (this.MailMessageSend(loMessage, lsType, 3))
            {
                lsLog += DateTime.UtcNow.ToString() + "\tInfo\tSent email\n";
            }
            else
            {
                lsLog += DateTime.UtcNow.ToString() + "\tError\tEmail not sent\n";
                MaxLogLibrary.Log(new MaxLogEntryStructure(MaxEnumGroup.LogError, "Email not sent with subject: {Subject}", lsSubject));
            }

            return lsLog;
        }

        /// <summary>
        /// Sends a .NET mail message using configuration
        /// </summary>
        /// <param name="loMessage">The message to send</param>
        /// <param name="lsConfigPrefix">The configuration information</param>
        /// <param name="lnRetry">The number of times to try again.</param>
        /// <returns>true if successful</returns>
        protected bool MailMessageSend(System.Net.Mail.MailMessage loMessage, string lsConfigPrefix, int lnRetry)
        {
            string lsEmailServer = MaxMessageRepository.GetConfig("EmailServer", loMessage.From.Address, lsConfigPrefix);
            string lsEmailPort = MaxMessageRepository.GetConfig("EmailPort", loMessage.From.Address, lsConfigPrefix);
            string lsEmailUser = MaxMessageRepository.GetConfig("EmailUser", loMessage.From.Address, lsConfigPrefix);
            string lsEmailPassword = MaxMessageRepository.GetConfig("EmailPassword", loMessage.From.Address, lsConfigPrefix);
            int lnPort = MaxConvertLibrary.ConvertToInt(typeof(object), lsEmailPort);
            if (lnPort <= 0)
            {
                lnPort = 25;
            }

            System.Net.Mail.SmtpClient loClient = new System.Net.Mail.SmtpClient(lsEmailServer, lnPort);
            if (lnPort != 25)
            {
                loClient.EnableSsl = true;
            }

            loClient.Credentials = new System.Net.NetworkCredential(lsEmailUser, lsEmailPassword);
            if (null == loMessage.From)
            {
                loMessage.From = new System.Net.Mail.MailAddress(lsEmailUser);
            }

            try
            {
                loClient.Send(loMessage);
                return true;
            }
            catch (Exception loESend)
            {
                MaxLogLibrary.Log(new MaxLogEntryStructure(MaxEnumGroup.LogError, "Error Sending Email: {Message}", loESend, loMessage));
                if (lnRetry > 0)
                {
                    return this.MailMessageSend(loMessage, lsConfigPrefix, lnRetry - 1);
                }
            }

            return false;
        }
#else
        public virtual string SendConditional(string lsType, string lsFromAddress, string lsFromName, string lsSubject, string lsContent, string[] laToAddressList, string[] laToNameList, MaxIndex loAttachmentList, string lsReplyAddress)
        {
            return string.Empty;
        }
#endif


    }
}