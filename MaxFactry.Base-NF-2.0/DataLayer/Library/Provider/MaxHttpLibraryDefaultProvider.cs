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
#endif

    /// <summary>
    /// Provides static methods to manipulate storage of data
    /// </summary>
    public class MaxHttpLibraryDefaultProvider : MaxProvider, IMaxHttpLibraryProvider
    {
        private static object _oLock = new object();

        protected virtual object GetClient(MaxData loData, MaxDataQuery loDataQuery)
        {
            return this.GetClientConditional(loData, loDataQuery);
        }

        protected virtual Uri GetUri(MaxData loData, MaxDataQuery loDataQuery, string lsDataName)
        {
            Uri loR = MaxDataLibrary.GetValue(loDataQuery, lsDataName) as Uri;
            if (null == loR)
            {
                string lsRequestUrl = MaxDataLibrary.GetValue(loDataQuery, lsDataName) as string;
                if (null != lsRequestUrl)
                {
                    loR = new Uri(lsRequestUrl);
                }

                if (null == loR)
                {
                    loR = loData.Get(lsDataName) as Uri;
                    if (null != loR)
                    {
                        lsRequestUrl = loData.Get(lsDataName) as string;
                        if (null != lsRequestUrl)
                        {
                            loR = new Uri(lsRequestUrl);
                        }
                    }
                }
            }

            return loR;
        }

        protected virtual string GetString(MaxData loData, MaxDataQuery loDataQuery, string lsDataName)
        {
            string lsR = MaxDataLibrary.GetValue(loDataQuery, lsDataName) as string;
            if (null == lsR)
            {
                lsR = loData.Get(lsDataName) as string;
            }

            return lsR;
        }

        protected virtual object GetContent(MaxData loData, MaxDataQuery loDataQuery, string lsDataName)
        {
            return this.GetContentConditional(loData, loDataQuery, lsDataName);
        }

        public virtual MaxIndex GetResponse(MaxData loData, MaxDataQuery loDataQuery, int lnPageIndex, int lnPageSize, string lsOrderBy, params string[] laDataNameList)
        {
            return this.GetResponseConditional(loData, loDataQuery, lnPageSize, lnPageSize, lsOrderBy, laDataNameList);
        }

        public virtual object GetResponse(string lsRequestUrl, string lsToken)
        {
            return this.GetResponseConditional(lsRequestUrl, lsToken);
        }

        public virtual string GetAccessToken(Uri loTokenUrl, string lsClientId, string lsClientSecret, string lsScope)
        {
            MaxBaseHttpDataModel loDataModel = new MaxBaseHttpDataModel();
            MaxData loData = new MaxData(loDataModel);
            loData.Set(loDataModel.RequestUri, loTokenUrl);
            loData.Set(loDataModel.ClientId, lsClientId);
            loData.Set(loDataModel.ClientSecret, lsClientSecret);
            loData.Set(loDataModel.GrantType, "client_credentials");
            loData.Set(loDataModel.Scope, lsScope);
            MaxIndex loContent = new MaxIndex();
            loContent.Add("grant_type", "client_credentials");
            loContent.Add("scope", lsScope);
            loData.Set(loDataModel.RequestContent, loContent);

            MaxIndex loResponse = this.GetResponse(loData, null, 0, 0, string.Empty);
            string lsR = loResponse.GetValueString(loDataModel.ResponseContent);
            return lsR;
        }

#if net4_52 || netcore2 || netstandard1_2
        private System.Net.Http.HttpClient _oHttpClient = null;

        protected HttpClient GetMaxClient()
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
            loR.Timeout = new TimeSpan(0, 0, 10);
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
                            _oHttpClient = GetMaxClient();
                        }
                    }
                }

                return _oHttpClient;
            }
        }

        protected virtual object GetClientConditional(MaxData loData, MaxDataQuery loDataQuery)
        {
            HttpClient loR = HttpClient;
            IMaxHttpDataModel loDataModel = loData.DataModel as IMaxHttpDataModel;
            string lsClientId = this.GetString(loData, loDataQuery, loDataModel.ClientId);
            string lsClientSecret = this.GetString(loData, loDataQuery, loDataModel.ClientSecret);
            string lsToken = this.GetString(loData, loDataQuery, loDataModel.Token);
            if ((!string.IsNullOrEmpty(lsClientId) && !string.IsNullOrEmpty(lsClientSecret)) || !string.IsNullOrEmpty(lsToken))
            {
                loR = GetMaxClient();
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

        protected virtual object GetContentConditional(MaxData loData, MaxDataQuery loDataQuery, string lsDataName)
        {
            object loR = MaxDataLibrary.GetValue(loDataQuery, lsDataName);
            if (null == loR)
            {
                loR = loData.Get(lsDataName);
            }

            if (null != loR)
            {
                if (loR is string)
                {
                    loR = new System.Net.Http.StringContent((string)loR);
                }
                else if (loR is MaxIndex)
                {
                    System.Collections.Generic.Dictionary<string, string> loContentDictionary = new System.Collections.Generic.Dictionary<string, string>();
                    string[] laKey = ((MaxIndex)loR).GetSortedKeyList();
                    foreach (string lsKey in laKey)
                    {
                        loContentDictionary.Add(lsKey, ((MaxIndex)loR).GetValueString(lsKey));
                    }

                    loR = new System.Net.Http.FormUrlEncodedContent(loContentDictionary);
                }
                else
                {
                    loR = new System.Net.Http.StringContent(MaxConvertLibrary.SerializeObjectToString(loR));
                }
            }

            return loR;
        }

        protected virtual MaxIndex GetResponseConditional(MaxData loData, MaxDataQuery loDataQuery, int lnPageIndex, int lnPageSize, string lsOrderBy, params string[] laDataNameList)
        {
            MaxIndex loR = new MaxIndex();
            IMaxHttpDataModel loDataModel = loData.DataModel as IMaxHttpDataModel;
            Uri loRequestUri = this.GetUri(loData, loDataQuery, loDataModel.RequestUri);
            if (null != loRequestUri)
            {
                loR.Add(loDataModel.RequestUri, loRequestUri);
                System.Net.Http.HttpClient loClient = this.GetClient(loData, loDataQuery) as System.Net.Http.HttpClient;
                if (null != loClient)
                {
                    System.Net.Http.HttpContent loContent = this.GetContent(loData, loDataQuery, loDataModel.RequestContent) as System.Net.Http.HttpContent;
                    System.Net.Http.HttpResponseMessage loHttpClientResponse = null;
                    object loResponseContent = null;
                    System.Threading.Tasks.Task<System.Net.Http.HttpResponseMessage> loTask = null;
                    loR.Add(loDataModel.RequestTime, DateTime.UtcNow);
                    if (null != loContent)
                    {
                        loR.Add(loDataModel.RequestContent, loContent);
                        loTask = loClient.PostAsync(loRequestUri, loContent);
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
                        loR.Add(loDataModel.ResponseTime, DateTime.UtcNow);
                        loHttpClientResponse = loTask.Result;
                        if (loHttpClientResponse.Content != null)
                        {
                            System.Threading.Tasks.Task loContentTask = null;
                            if (loHttpClientResponse.Content.GetType() == typeof(System.Net.Http.StreamContent))
                            {
                                loContentTask = loHttpClientResponse.Content.ReadAsStreamAsync();
                            }
                            else if (loHttpClientResponse.Content.GetType() == typeof(string))
                            {
                                loContentTask = loHttpClientResponse.Content.ReadAsStringAsync();
                            }
                            else if (loHttpClientResponse.Content.GetType() == typeof(byte[]))
                            {
                                loContentTask = loHttpClientResponse.Content.ReadAsByteArrayAsync();
                            }

                            while (!loContentTask.IsCompleted)
                            {
                                System.Threading.Thread.Sleep(10);
                            }

                            if (loContentTask.Status == System.Threading.Tasks.TaskStatus.RanToCompletion)
                            {
                                if (loContentTask is System.Threading.Tasks.Task<Stream>)
                                {
                                    loResponseContent = ((System.Threading.Tasks.Task<Stream>)loContentTask).Result;
                                }
                                else if (loContentTask is System.Threading.Tasks.Task<string>)
                                {
                                    loResponseContent = ((System.Threading.Tasks.Task<string>)loContentTask).Result;
                                }
                                else if (loContentTask is System.Threading.Tasks.Task<byte[]>)
                                {
                                    loResponseContent = ((System.Threading.Tasks.Task<byte[]>)loContentTask).Result;
                                }
                            }
                            else
                            {
                                throw new MaxException("Read content task to " + loRequestUri + " completed with status " + loTask.Status.ToString());
                            }

                            loR.Add(loDataModel.ResponseContent, loResponseContent);
                            loR.Add("Response.ReasonPhrase", loHttpClientResponse.ReasonPhrase);
                            loR.Add("Response.IsSuccessStatusCode", loHttpClientResponse.IsSuccessStatusCode);
                            loR.Add("Response.StatusCode", MaxConvertLibrary.ConvertToString(typeof(object), loHttpClientResponse.StatusCode));
                            loR.Add("Response.Version", MaxConvertLibrary.ConvertToString(typeof(object), loHttpClientResponse.Version));
                            loR.Add("Response.RequestMessage.RequestUri", MaxConvertLibrary.ConvertToString(typeof(object), loHttpClientResponse.RequestMessage.RequestUri));
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

        protected virtual object GetResponseConditional(string lsRequestUrl, string lsToken)
        {
            object loR = null;
            Uri loRequestUri = new Uri(lsRequestUrl);
            if (null != loRequestUri)
            {
                HttpClient loClient = HttpClient;
                if (!string.IsNullOrEmpty(lsToken))
                {
                    loClient = GetMaxClient();
                    loClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", lsToken);
                    System.Net.Http.Headers.CacheControlHeaderValue loCache = new System.Net.Http.Headers.CacheControlHeaderValue();
                    loCache.NoCache = true;
                    loClient.DefaultRequestHeaders.CacheControl = loCache;
                }

                if (null != loClient)
                {
                    System.Net.Http.HttpResponseMessage loHttpClientResponse = null;
                    System.Threading.Tasks.Task<System.Net.Http.HttpResponseMessage> loTask = loClient.GetAsync(loRequestUri);
                    while (!loTask.IsCompleted)
                    {
                        System.Threading.Thread.Sleep(10);
                    }

                    if (loTask.Status == System.Threading.Tasks.TaskStatus.RanToCompletion)
                    {
                        loHttpClientResponse = loTask.Result;
                        if (loHttpClientResponse.Content != null)
                        {
                            System.Threading.Tasks.Task loContentTask = null;
                            if (loHttpClientResponse.Content.GetType() == typeof(System.Net.Http.StreamContent))
                            {
                                loContentTask = loHttpClientResponse.Content.ReadAsStreamAsync();
                            }
                            else if (loHttpClientResponse.Content.GetType() == typeof(string))
                            {
                                loContentTask = loHttpClientResponse.Content.ReadAsStringAsync();
                            }
                            else if (loHttpClientResponse.Content.GetType() == typeof(byte[]))
                            {
                                loContentTask = loHttpClientResponse.Content.ReadAsByteArrayAsync();
                            }

                            while (!loContentTask.IsCompleted)
                            {
                                System.Threading.Thread.Sleep(10);
                            }

                            if (loContentTask.Status == System.Threading.Tasks.TaskStatus.RanToCompletion)
                            {
                                if (loContentTask is System.Threading.Tasks.Task<Stream>)
                                {
                                    loR = ((System.Threading.Tasks.Task<Stream>)loContentTask).Result;
                                }
                                else if (loContentTask is System.Threading.Tasks.Task<string>)
                                {
                                    loR = ((System.Threading.Tasks.Task<string>)loContentTask).Result;
                                }
                                else if (loContentTask is System.Threading.Tasks.Task<byte[]>)
                                {
                                    loR = ((System.Threading.Tasks.Task<byte[]>)loContentTask).Result;
                                }
                            }
                            else
                            {
                                throw new MaxException("Read content task to " + loRequestUri + " completed with status " + loTask.Status.ToString());
                            }
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
#else
        protected virtual object GetContentConditional(MaxData loData, MaxDataQuery loDataQuery, string lsDataName)
        {
            throw new NotImplementedException();
        }

        public virtual MaxIndex GetResponseConditional(MaxData loData, MaxDataQuery loDataQuery, int lnPageIndex, int lnPageSize, string lsOrderBy, params string[] laDataNameList)
        {
            throw new NotImplementedException();
        }

        protected virtual object GetClientConditional(MaxData loData, MaxDataQuery loDataQuery)
        {
            throw new NotImplementedException();
        }

        protected virtual object GetResponseConditional(string lsRequestUrl, string lsToken)
        {
            throw new NotImplementedException();
        }
#endif
    }
}