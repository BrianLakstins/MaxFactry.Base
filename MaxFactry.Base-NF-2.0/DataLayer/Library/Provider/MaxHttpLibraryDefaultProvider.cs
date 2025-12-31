// <copyright file="MaxHttpLibraryDefaultProvider.cs" company="Lakstins Family, LLC">
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
// <change date="3/20/2024" author="Brian A. Lakstins" description="Happy birthday to my mom.  Sara Jean Lakstins (Cartwright) - 3/20/1944 to 3/14/2019.">
// <change date="3/24/2024" author="Brian A. Lakstins" description="Initial creation">
// <change date="3/25/2024" author="Brian A. Lakstins" description="Update method to match arguments used to get data from repositories. Add method to get token.  Add method to get response with just token.">
// <change date="3/30/2024" author="Brian A. Lakstins" description="Add fields for Request and Response variable names. Streamlined methods.">
// <change date="5/3/2024" author="Brian A. Lakstins" description="Add a way to specify a time out.">
// <change date="1/20/2025" author="Brian A. Lakstins" description="Check request url and use it if it's a string.">
// <change date="12/31/2025" author="Brian A. Lakstins" description="Handle any response content">
// </changelog>
#endregion

namespace MaxFactry.Base.DataLayer.Library.Provider
{
    using System.IO;
    using MaxFactry.Core;
    using MaxFactry.Base.DataLayer.Library;
    using System;

#if net4_52 || netcore2 || netstandard1_2
    using System.Net.Http;
    using System.Net.Http.Headers;
    using System.Threading;
    using System.Threading.Tasks;
#endif

    /// <summary>
    /// Provides static methods to manipulate storage of data
    /// </summary>
    public class MaxHttpLibraryDefaultProvider : MaxProvider, IMaxHttpLibraryProvider
    {
        private static object _oLock = new object();

        public static class RequestContentName
        {
            public const string Token = "Token";

            public const string ClientId = "client_id";

            public const string ClientSecret = "client_secret";

            public const string Scope = "scope";

            public const string RequestUrl = "RequestUrl";

            public const string StringContent = "StringContent";

            public const string FormUrlEncodedContent = "FormUrlEncodedContent";

            public const string HttpContent = "HttpContent";

            public const string Timeout = "Timeout";
        }

        public static class ResponseName
        {
            public const string RequestTime = "RequestTime";

            public const string ResponseTime = "ResponseTime";

            public const string Response = "Response";

            public const string Content = "Content";

            public const string IsSuccessStatusCode = "IsSuccessStatusCode";

            public const string ReasonPhrase = "ReasonPhrase";

            public const string StatusCodeText = "StatusCodeText";

            public const string Version = "Version";

            public const string Headers = "Headers";

            public const string ContentCreationDate = "ContentCreationDate";

            public const string ContentDispositionType = "ContentDispositionType";

            public const string ContentFileName = "ContentFileName";

            public const string ContentFileNameStar = "ContentFileNameStar";

            public const string ContentModificationDate = "ContentModificationDate";

            public const string ContentName = "ContentName";

            public const string ContentReadDate = "ContentReadDate";

            public const string ContentSize = "ContentSize";

            public const string ContentEncoding = "ContentEncoding";

            public const string ContentLanguage = "ContentLanguage";

            public const string ContentLength = "ContentLength";

            public const string ContentLocation = "ContentLocation";

            public const string ContentMD5 = "ContentMD5";

            public const string ContentType = "ContentType";

            public const string ContentExpires = "ContentExpires";

            public const string ContentLastModified = "ContentLastModified";
        }

        protected virtual object GetClient(string lsClientId, string lsClientSecret, string lsToken, TimeSpan loTimeout)
        {
            return this.GetClientConditional(lsClientId, lsClientSecret, lsToken, loTimeout);
        }

        protected virtual object GetResponseContent(object loContent)
        {
            return this.GetResponseContentConditional(loContent);
        }

        public virtual object GetContent(string lsRequestUrl, string lsToken)
        {
            MaxIndex loRequestContent = new MaxIndex();
            loRequestContent.Add(RequestContentName.RequestUrl, new Uri(lsRequestUrl));
            loRequestContent.Add(RequestContentName.Token, lsToken);
            MaxIndex loResponse = this.GetResponse(loRequestContent);
            return loResponse[ResponseName.Content];
        }

        public virtual MaxIndex GetResponse(MaxIndex loRequestContent)
        {
            return this.GetResponseConditional(loRequestContent);
        }

        public virtual string GetAccessToken(Uri loTokenUrl, string lsClientId, string lsClientSecret, string lsScope)
        {
            MaxIndex loRequestContent = new MaxIndex();
            loRequestContent.Add(RequestContentName.RequestUrl, loTokenUrl);
            loRequestContent.Add(RequestContentName.ClientId, lsClientId);
            loRequestContent.Add(RequestContentName.ClientSecret, lsClientSecret);

            MaxIndex loContent = new MaxIndex();

            //loContent.Add(RequestContentName.ClientId, lsClientId);
            //loContent.Add(RequestContentName.ClientSecret, lsClientSecret);
            loContent.Add(RequestContentName.Scope, lsScope);
            loContent.Add("grant_type", "client_credentials");

            loRequestContent.Add(RequestContentName.FormUrlEncodedContent, loContent);
            MaxIndex loResponse = this.GetResponse(loRequestContent);
            object loResponseContent = loResponse[ResponseName.Content];
            string lsR = loResponseContent as string;
            if (loResponseContent is Stream)
            {
                lsR = new StreamReader((Stream)loResponseContent).ReadToEnd();
            }

            return lsR;
        }
        

#if net4_52 || netcore2 || netstandard1_2
        private System.Net.Http.HttpClient _oHttpClient = null;

        protected HttpClient GetMaxClient(TimeSpan loTimeout)
        {
            HttpClient loR = null;
            System.Net.ServicePointManager.SecurityProtocol |= System.Net.SecurityProtocolType.Tls11 | System.Net.SecurityProtocolType.Tls12;
            HttpClientHandler loHandler = new System.Net.Http.HttpClientHandler();
            if (loHandler.SupportsAutomaticDecompression)
            {
                loHandler.AutomaticDecompression = System.Net.DecompressionMethods.GZip | System.Net.DecompressionMethods.Deflate;
            }

            loHandler.CookieContainer = new System.Net.CookieContainer();
            loR = new HttpClient(loHandler);
            loR.Timeout = loTimeout;
            loR.DefaultRequestHeaders.Add("User-Agent", "Mozilla /5.0 (MaxFactry .NET Framework)");
            loR.DefaultRequestHeaders.Add("DNT", "1");
            loR.DefaultRequestHeaders.Add("Accept-Encoding", "gzip, deflate");
            loR.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            return loR;
        }

        protected System.Net.Http.HttpClient HttpClient
        {
            get
            {
                if (null == _oHttpClient)
                {
                    lock (_oLock)
                    {
                        if (null == _oHttpClient)
                        {
                            _oHttpClient = GetMaxClient(new TimeSpan(0, 0, 30));
                        }
                    }
                }

                return _oHttpClient;
            }
        }

        protected virtual object GetClientConditional(string lsClientId, string lsClientSecret, string lsToken, TimeSpan loTimeout)
        {
            HttpClient loR = HttpClient;
            if ((!string.IsNullOrEmpty(lsClientId) && !string.IsNullOrEmpty(lsClientSecret)) || !string.IsNullOrEmpty(lsToken) || loR.Timeout != loTimeout)
            {
                loR = GetMaxClient(loTimeout);
                if (!string.IsNullOrEmpty(lsClientId) && !string.IsNullOrEmpty(lsClientSecret))
                {
                    string lsAuth = Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes(string.Format("{0}:{1}", lsClientId, lsClientSecret)));
                    loR.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Basic", lsAuth); ;
                }
                else if (!string.IsNullOrEmpty(lsToken))
                {
                    loR.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", lsToken);
                }

                System.Net.Http.Headers.CacheControlHeaderValue loCache = new System.Net.Http.Headers.CacheControlHeaderValue();
                loCache.NoCache = true;
                loR.DefaultRequestHeaders.CacheControl = loCache;
            }

            return loR;
        }

        protected virtual MaxIndex GetResponseConditional(MaxIndex loRequestContent)
        {
            MaxIndex loR = new MaxIndex();
            Uri loRequestUri = loRequestContent[RequestContentName.RequestUrl] as Uri;
            if (null == loRequestUri && loRequestContent.Contains(RequestContentName.RequestUrl))
            {
                string lsRequestUrl = loRequestContent.GetValueString(RequestContentName.RequestUrl);
                if (!string.IsNullOrEmpty(lsRequestUrl))
                {
                    loRequestUri = new Uri(lsRequestUrl);
                }
            }

            if (null != loRequestUri)
            {
                string lsClientId = loRequestContent.GetValueString(RequestContentName.ClientId);
                string lsClientSecret = loRequestContent.GetValueString(RequestContentName.ClientSecret);
                string lsToken = loRequestContent.GetValueString(RequestContentName.Token);
                TimeSpan loTimeout = new TimeSpan(0, 0, 30);
                if (loRequestContent.Contains(RequestContentName.Timeout) && loRequestContent[RequestContentName.Timeout] is TimeSpan)
                {
                    loTimeout = (TimeSpan)loRequestContent[RequestContentName.Timeout];
                }

                System.Net.Http.HttpClient loClient = this.GetClient(lsClientId, lsClientSecret, lsToken, loTimeout) as System.Net.Http.HttpClient;
                if (null != loClient)
                {                   
                    object loContent = loRequestContent[RequestContentName.StringContent] as string;
                    if (null != loContent)
                    {
                        loContent = new System.Net.Http.StringContent((string)loContent);
                    }
                    else
                    {
                        loContent = loRequestContent[RequestContentName.FormUrlEncodedContent];
                        if (loContent is MaxIndex)
                        {
                            System.Collections.Generic.Dictionary<string, string> loContentDictionary = new System.Collections.Generic.Dictionary<string, string>();
                            string[] laKey = ((MaxIndex)loContent).GetSortedKeyList();
                            foreach (string lsKey in laKey)
                            {
                                loContentDictionary.Add(lsKey, ((MaxIndex)loContent).GetValueString(lsKey));
                            }

                            loContent = new System.Net.Http.FormUrlEncodedContent(loContentDictionary);
                        }
                        else if (null == loContent)
                        {
                            loContent = loRequestContent[RequestContentName.HttpContent];
                        }
                    }

                    System.Net.Http.HttpResponseMessage loResponse = null;
                    System.Threading.Tasks.Task<System.Net.Http.HttpResponseMessage> loTask = null;
                    loR.Add(ResponseName.RequestTime, DateTime.UtcNow);
                    if (loContent is System.Net.Http.HttpContent)
                    {
                        loTask = loClient.PostAsync(loRequestUri, (System.Net.Http.HttpContent)loContent);
                    }
                    else
                    {
                        loTask = loClient.GetAsync(loRequestUri);
                    }

                    while (!loTask.IsCompleted)
                    {
                        System.Threading.Thread.Sleep(10);
                    }

                    if (loTask.Status == System.Threading.Tasks.TaskStatus.RanToCompletion)
                    {
                        loR.Add(ResponseName.ResponseTime, DateTime.UtcNow);
                        loResponse = loTask.Result;
                        loR.Add(ResponseName.Response, loResponse);
                        loR.Add(ResponseName.IsSuccessStatusCode, loResponse.IsSuccessStatusCode);
                        loR.Add(ResponseName.ReasonPhrase, loResponse.ReasonPhrase);
                        loR.Add(ResponseName.StatusCodeText, loResponse.StatusCode.ToString());
                        loR.Add(ResponseName.Version, loResponse.Version);
                        loR.Add(ResponseName.Headers, loResponse.Headers);
                        if (loResponse.Content != null)
                        {
                            if (loResponse.Content.Headers != null)
                            {
                                MaxIndex loResponseHeaderIndex = new MaxIndex();
                                if (loResponse.Content.Headers.ContentDisposition != null)
                                {
                                    loR.Add(ResponseName.ContentCreationDate, loResponse.Content.Headers.ContentDisposition.CreationDate);
                                    loR.Add(ResponseName.ContentDispositionType, loResponse.Content.Headers.ContentDisposition.DispositionType);
                                    loR.Add(ResponseName.ContentFileName, loResponse.Content.Headers.ContentDisposition.FileName);
                                    loR.Add(ResponseName.ContentFileNameStar, loResponse.Content.Headers.ContentDisposition.FileNameStar);
                                    loR.Add(ResponseName.ContentModificationDate, loResponse.Content.Headers.ContentDisposition.ModificationDate);
                                    loR.Add(ResponseName.ContentName, loResponse.Content.Headers.ContentDisposition.Name);
                                    loR.Add(ResponseName.ContentReadDate, loResponse.Content.Headers.ContentDisposition.ReadDate);
                                    loR.Add(ResponseName.ContentSize, loResponse.Content.Headers.ContentDisposition.Size);
                                }

                                if (loResponse.Content.Headers.ContentType != null)
                                {
                                    loR.Add(ResponseName.ContentType, loResponse.Content.Headers.ContentType.MediaType);
                                }

                                loR.Add(ResponseName.ContentEncoding, loResponse.Content.Headers.ContentEncoding);
                                loR.Add(ResponseName.ContentLanguage, loResponse.Content.Headers.ContentLanguage);
                                loR.Add(ResponseName.ContentLength, loResponse.Content.Headers.ContentLength);
                                loR.Add(ResponseName.ContentLocation, loResponse.Content.Headers.ContentLocation);
                                loR.Add(ResponseName.ContentMD5, loResponse.Content.Headers.ContentMD5);
                                loR.Add(ResponseName.ContentExpires, loResponse.Content.Headers.Expires);
                                loR.Add(ResponseName.ContentLastModified, loResponse.Content.Headers.LastModified);
                            }

                            object loResponseContent = this.GetResponseContent(loResponse.Content);
                            loR.Add(ResponseName.Content, loResponseContent);
                        }
                    }
                    else
                    {
                        throw new MaxException("Task to " + loRequestUri + " completed with status " + loTask.Status.ToString());
                    }
                }
            }

            return loR;
        }

        protected virtual object GetResponseContentConditional(object loContent)
        {
            object loR = null;
            HttpContent loHttpContent = loContent as HttpContent;
            if (null != loHttpContent)
            {
                Task<object> loTask = this.GetResponseContentConditionalAsync(loHttpContent);
                while (!loTask.IsCompleted)
                {
                    System.Threading.Thread.Sleep(10);
                }

                if (loTask.Status == TaskStatus.RanToCompletion)
                {
                    loR = loTask.Result;
                }
                else
                {
                    throw new MaxException("Read content task completed with status " + loTask.Status.ToString());
                }
            }

            return loR;
        }

        protected async virtual Task<object> GetResponseContentConditionalAsync(HttpContent loContent)
        {
            object loR = null;
            if (loContent is System.Net.Http.StreamContent)
            {
                loR = await loContent.ReadAsStreamAsync();
            }
            else if (loContent is System.Net.Http.ByteArrayContent)
            {
                loR = await loContent.ReadAsByteArrayAsync();
            }
            else 
            {
                try
                {
                    loR = await loContent.ReadAsStringAsync();
                }
                catch (Exception loE)
                {
                    MaxLogLibrary.Log(new MaxLogEntryStructure(this.GetType(), "GetResponseContentConditionalAsync", MaxEnumGroup.LogError, "Error reading content as string", loE));
                }
            }

            return loR;
        }
#else
        protected virtual MaxIndex GetResponseConditional(MaxIndex loRequestContent)
        {
            throw new NotImplementedException();
        }

        protected virtual object GetClientConditional(string lsClientId, string lsClientSecret, string lsToken, TimeSpan loTimeout)
        {
            throw new NotImplementedException();
        }

        protected virtual object GetResponseContentConditional(object loContent)
        {
            throw new NotImplementedException();
        }
#endif
    }
}