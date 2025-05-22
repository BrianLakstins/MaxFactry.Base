// <copyright file="MaxDataContextLibrarySerialPortDefaultProvider.cs" company="Lakstins Family, LLC">
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
// <change date="10/19/2016" author="Brian A. Lakstins" description="Initial creation">
// <change date="5/11/2017" author="Brian A. Lakstins" description="Update to be more asynchcronous.">
// <change date="10/18/2019" author="Brian A. Lakstins" description="Update to get raw output data and convert to text as needed.">
// <change date="6/5/2020" author="Brian A. Lakstins" description="Updated for change to base.">
// <change date="7/26/2021" author="Brian A. Lakstins" description="Provide connection errors">
// <change date="3/31/2024" author="Brian A. Lakstins" description="Moved from MaxFactry.CoreProvider">
// <change date="5/22/2025" author="Brian A. Lakstins" description="Remove stream handling methods. Review and update for consistency.">
// </changelog>
#endregion

namespace MaxFactry.Base.DataLayer.Library.Provider
{
    using System;
    using System.IO;
    using System.IO.Ports;
    using MaxFactry.Core;
    using MaxFactry.Base.DataLayer;
    using MaxFactry.Base.DataLayer.Library;

    /// <summary>
    /// Data Context used to work with Serial Ports
    /// </summary>
    public class MaxDataContextLibrarySerialPortDefaultProvider : MaxProvider, IMaxDataContextLibraryProvider
    {
        private string _sPortName = string.Empty;

        private int _nBaudRate = 9600;

        private Parity _oParity = Parity.None;

        private int _nDataBits = 8;

        private StopBits _oStopBits = StopBits.One;

        private int _nReadTimeout = 5000;

        private byte[] _aClear = null;

        private string _sEOL = string.Empty;

        private string _sSOL = string.Empty;

        private Handshake _oHandshake = Handshake.None;

        protected MaxIndex _oLog = new MaxIndex();

        /// <summary>
        /// Initializes a new instance of the MaxDataContextSerialPortProvider class
        /// </summary>
        public MaxDataContextLibrarySerialPortDefaultProvider()
            : base()
        {
        }

        /// <summary>
        /// Initializes the provider
        /// </summary>
        /// <param name="lsName">Name of the provider</param>
        /// <param name="loConfig">Configuration information</param>
        public override void Initialize(string lsName, MaxIndex loConfig)
        {
            base.Initialize(lsName, loConfig);
            string lsPortName = this.GetConfigValue(loConfig, "PortName") as string;
            if (!string.IsNullOrEmpty(lsPortName))
            {
                this._sPortName = lsPortName;
            }
            else if (GetPortNames().Length > 0)
            {
                this._sPortName = GetPortNames()[0];
            }

            int lnBaudRate = MaxConvertLibrary.ConvertToInt(typeof(object), this.GetConfigValue(loConfig, "BaudRate"));
            if (lnBaudRate > 0)
            {
                this._nBaudRate = lnBaudRate;
            }

            int lnDataBits = MaxConvertLibrary.ConvertToInt(typeof(object), this.GetConfigValue(loConfig, "DataBits"));
            if (lnDataBits > 0)
            {
                this._nDataBits = lnDataBits;
            }

            object loClear = this.GetConfigValue(loConfig, "Clear");
            if (null != loClear && loClear is byte[])
            {
                this._aClear = (byte[])loClear;
            }

            string lsEOL = MaxConvertLibrary.ConvertToString(typeof(object), this.GetConfigValue(loConfig, "EOL"));
            if (!string.IsNullOrEmpty(lsEOL))
            {
                this._sEOL = lsEOL;
            }

            string lsSOL = MaxConvertLibrary.ConvertToString(typeof(object), this.GetConfigValue(loConfig, "SOL"));
            if (!string.IsNullOrEmpty(lsEOL))
            {
                this._sSOL = lsSOL;
            }

            string lsHandshake = MaxConvertLibrary.ConvertToString(typeof(object), this.GetConfigValue(loConfig, "Handshake"));
            if (!string.IsNullOrEmpty(lsHandshake))
            {
                if (lsHandshake == "RequestToSend")
                {
                    this._oHandshake = Handshake.RequestToSend;
                }
                else if (lsHandshake == "RequestToSendXOnXOff")
                {
                    this._oHandshake = Handshake.RequestToSendXOnXOff;
                }
                else if (lsHandshake == "XOnXOff")
                {
                    this._oHandshake = Handshake.XOnXOff;
                }
            }

            string lsParity = MaxConvertLibrary.ConvertToString(typeof(object), this.GetConfigValue(loConfig, "Parity"));
            if (!string.IsNullOrEmpty(lsParity))
            {
                if (lsParity == "Even")
                {
                    this._oParity = Parity.Even;
                }
                else if (lsParity == "Mark")
                {
                    this._oParity = Parity.Mark;
                }
                else if (lsParity == "Odd")
                {
                    this._oParity = Parity.Odd;
                }
                else if (lsParity == "Space")
                {
                    this._oParity = Parity.Space;
                }
                else if (lsParity == "None")
                {
                    this._oParity = Parity.None;
                }
            }

            string lsStop = MaxConvertLibrary.ConvertToString(typeof(object), this.GetConfigValue(loConfig, "Stop"));
            if (!string.IsNullOrEmpty(lsStop))
            {
                if (lsStop == "One")
                {
                    this._oStopBits = StopBits.One;
                }
                else if (lsStop == "OnePointFive")
                {
                    this._oStopBits = StopBits.OnePointFive;
                }
                else if (lsStop == "Two")
                {
                    this._oStopBits = StopBits.Two;
                }
                else if (lsStop == "None")
                {
                    this._oStopBits = StopBits.None;
                }
            }
        }

        protected virtual void Connect()
        {
            MaxIndex loPortConfig = new MaxIndex();
            loPortConfig.Add("BaudRate", this._nBaudRate);
            loPortConfig.Add("Parity", this._oParity);
            loPortConfig.Add("DataBits", this._nDataBits);
            loPortConfig.Add("StopBits", this._oStopBits);
            loPortConfig.Add("Handshake", this._oHandshake);
            loPortConfig.Add("ReadTimeout", this._nReadTimeout);
            loPortConfig.Add("Clear", this._aClear);
            MaxSerialPortLibrary.Connect(this._sPortName, loPortConfig);
        }

        protected virtual void Disconnect()
        {
            MaxSerialPortLibrary.Disconnect(this._sPortName);
        }

        protected string PortName
        {
            get
            {
                return this._sPortName;
            }
        }

        /// <summary>
        /// Inserts a new list of elements
        /// </summary>
        /// <param name="loDataList">The list of elements</param>
        /// <returns>Flag based status code indicating level of success.</returns>
        public int Insert(MaxDataList loDataList)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Updates a list of elements
        /// </summary>
        /// <param name="loDataList">The list of elements</param>
        /// <returns>Flag based status code indicating level of success.</returns>
        public int Update(MaxDataList loDataList)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Deletes a list of elements
        /// </summary>
        /// <param name="loDataList">The list of elements</param>
        /// <returns>Flag based status code indicating level of success.</returns>
        public int Delete(MaxDataList loDataList)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Selects all data
        /// </summary>
        /// <param name="loData">Data to use as definition</param>
        /// <param name="laDataNameList">Names of fields to return</param>
        /// <returns>List of data that is stored</returns>
        public virtual MaxDataList SelectAll(MaxData loData, params string[] laDataNameList)
        {
            MaxLogLibrary.Log(MaxEnumGroup.LogDebug, "Select [" + loData.DataModel.DataStorageName + "] start", "MaxDataContextSerialPortProvider");
            MaxDataList loR = new MaxDataList(loData.DataModel);
            return loR;
        }

        /// <summary>
        /// Selects data
        /// </summary>
        /// <param name="loData">Data to use as definition</param>
        /// <param name="loDataQuery">Filter for the query</param>
        /// <param name="lnPageIndex">Page number of the data</param>
        /// <param name="lnPageSize">Size of the page</param>
        /// <param name="lsOrderBy">Data field used to sort</param>
        /// <param name="laDataNameList">Names of fields to return</param>
        /// <returns>List of data that matches the query parameters</returns>
        public virtual MaxDataList Select(MaxData loData, MaxDataQuery loDataQuery, int lnPageIndex, int lnPageSize, string lsOrderBy, out int lnTotal, params string[] laDataNameList)
        {
            MaxLogLibrary.Log(MaxEnumGroup.LogDebug, "Select [" + loData.DataModel.DataStorageName + "] start", "MaxDataContextSerialPortProvider");
            MaxBaseSerialPortDataModel loDataModel = loData.DataModel as MaxBaseSerialPortDataModel;
            if (null == loDataModel)
            {
                throw new MaxException("MaxBaseSerialPortDataModel is expected by MaxDataContextSerialPortProvider");
            }

            MaxDataList loR = new MaxDataList(loDataModel);
            MaxData loDataReturn = new MaxData(loData.DataModel);
            lnTotal = 0;
            loDataReturn.Set(loDataModel.RequestTime, DateTime.UtcNow);
            string lsRequest = loData.Get(loDataModel.Request) as string;
            MaxIndex loResponse = new MaxIndex();
            try
            {
                this.Connect();
                if (!string.IsNullOrEmpty(lsRequest))
                {
                    this.SendRequest(lsRequest);
                }

                string lsResponse = this.GetResponseText();
                int lnRetry = 0;
                DateTime ldResponseEnd = DateTime.UtcNow;
                string lsEOL = "\n";
                if (!string.IsNullOrEmpty(this._sEOL))
                {
                    lsEOL = this._sEOL;
                }

                string lsSOL = string.Empty;
                if (!string.IsNullOrEmpty(this._sSOL))
                {
                    lsSOL = this._sSOL;
                }

                while (!(lsResponse.Contains(lsSOL) && lsResponse.Contains(lsEOL)) && lnRetry < 10)
                {
                    System.Threading.Thread.Sleep(10);
                    lnRetry++;
                    lsResponse += this.GetResponseText();
                    ldResponseEnd = DateTime.UtcNow;
                }

                loDataReturn.Set(loDataModel.ResponseTime, ldResponseEnd);
                loDataReturn.Set(loDataModel.ResponseRaw, lsResponse);
                string[] laResponse = lsResponse.Split(new string[] { lsEOL }, StringSplitOptions.RemoveEmptyEntries);
                foreach (string lsLine in laResponse)
                {
                    if (lsSOL.Length > 0 && lsLine.Contains(lsSOL))
                    {
                        loResponse.Add(MaxLogLibrary.GetUniqueLogTime().Ticks.ToString(), lsLine.Substring(lsLine.LastIndexOf(lsSOL) + lsSOL.Length));
                    }
                    else if (lsSOL.Length == 0)
                    {
                        loResponse.Add(MaxLogLibrary.GetUniqueLogTime().Ticks.ToString(), lsLine);
                    }
                }
                
                loDataReturn.Set(loDataModel.Log, MaxConvertLibrary.SerializeObjectToString(this._oLog));
            }
            catch (Exception loE)
            {
                loDataReturn.Set(loDataModel.Exception, loE);
                //// Disconnect the port after an exception so a connect will occur again for the next query
                this.Disconnect();
                //// Wait 5 seconds after an exception
                System.Threading.Thread.Sleep(5000);
            }
            finally
            {
                loDataReturn.Set(loDataModel.Response, MaxConvertLibrary.SerializeObjectToString(loResponse));
            }

            loR.Add(loDataReturn);
            lnTotal = loR.Count;
            MaxLogLibrary.Log(MaxEnumGroup.LogDebug, "Select [" + loData.DataModel.DataStorageName + "] end", "MaxDataContextSerialPortProvider");
            return loR;
        }

        /// <summary>
        /// Selects a count of records
        /// </summary>
        /// <param name="loData">Data to use as definition</param>
        /// <param name="loDataQuery">Filter for the query</param>
        /// <returns>Count that matches the query parameters</returns>
        public virtual int SelectCount(MaxData loData, MaxDataQuery loDataQuery)
        {
            int lnR = 0;

            return lnR;
        }

        public string[] GetPortNames()
        {
            return MaxSerialPortLibrary.GetPortNames();
        }

        protected void AddToLog(MaxIndex loLog, string lsLog)
        {
            string[] laLog = lsLog.Split('\n');
            foreach (string lsLine in laLog)
            {
                if (lsLine.Contains("\t"))
                {
                    string[] laLine = lsLine.Split('\t');
                    loLog.Add(laLine[0], laLine[1]);
                }
            }
        }

        protected virtual void SendRequest(string lsRequest)
        {
            MaxSerialPortLibrary.SendRequest(this._sPortName, lsRequest);
        }

        protected virtual byte[] GetResponse()
        {
            byte[] laR = MaxSerialPortLibrary.GetResponse(this._sPortName);
            return laR;
        }

        protected virtual string GetResponseText()
        {
            string lsR = string.Empty;
            byte[] laResponse = this.GetResponse();
            if (null != laResponse && laResponse.Length > 0)
            {
                lsR = System.Text.Encoding.ASCII.GetString(laResponse);
            }

            return lsR;
        }
    }
}
