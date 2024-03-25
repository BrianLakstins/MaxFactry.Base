// <copyright file="MaxSerialPortLibraryDefaultProvider.cs" company="Lakstins Family, LLC">
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
// </changelog>
#endregion

namespace MaxFactry.Base.DataLayer.Library.Provider
{
    using System.IO;
    using MaxFactry.Core;
    using System;
    using System.Collections.Generic;
    using System.IO.Ports;
    using System.Threading;

    /// <summary>
    /// Provides methods to manipulate interaction with serial ports
    /// </summary>
    public class MaxSerialPortLibraryDefaultProvider : MaxProvider, IMaxSerialPortLibraryProvider
    {
        /// <summary>
        /// Lock object for multi-threaded access.
        /// </summary>
        private static object _oLock = new object();

        /// <summary>
        /// Index of names and ports currently managed by this library
        /// </summary>
        private MaxIndex _oSerialPortIndex = new MaxIndex();

        private MaxIndex _oSerialPortLockIndex = new MaxIndex();

        private MaxIndex _oSerialPortDataQueueIndex = new MaxIndex();

        private MaxIndex _oSerialPortErrorQueueIndex = new MaxIndex();

        private MaxIndex _oBufferIndex = new MaxIndex();

        /// <summary>
        /// Initializes the provider
        /// </summary>
        /// <param name="lsName">Name of the provider</param>
        /// <param name="loConfig">Configuration information</param>
        public override void Initialize(string lsName, MaxIndex loConfig)
        {
            base.Initialize(lsName, loConfig);
        }

        public string[] GetPortNames()
        {
            return SerialPort.GetPortNames();
        }

        /// <summary>
        /// Gets a serial port this library is managing
        /// </summary>
        /// <param name="lsName"></param>
        /// <returns></returns>
        protected SerialPort GetPort(string lsName)
        {
            return _oSerialPortIndex[lsName] as SerialPort;
        }

        /// <summary>
        /// Gets a serial port this library is managing
        /// </summary>
        /// <param name="lsName"></param>
        /// <returns></returns>
        protected MaxIndex GetPortConfig(string lsName)
        {
            MaxIndex loR = new MaxIndex();
            if (_oSerialPortIndex.Contains(lsName + "-Config") && _oSerialPortIndex[lsName + "-Config"] is MaxIndex)
            {
                loR = (MaxIndex)_oSerialPortIndex[lsName + "-Config"];
            }

            return loR;
        }

        /// <summary>
        /// Adds a lock for a serial port for sending data or received data from the port
        /// Waits up to 2 seconds for any existing locks to free
        /// </summary>
        /// <param name="lsName"></param>
        /// <returns></returns>
        protected string AddPortLock(string lsName)
        {
            string lsR = null;
            lock (_oLock)
            {
                object loPortLockValue = null;
                double lnWaitTime = 10;
                DateTime ldNowUtc = DateTime.UtcNow;
                DateTime ldWaitUntilUtc = ldNowUtc.AddSeconds(lnWaitTime);
                if (_oSerialPortIndex.Contains(lsName) && null != _oSerialPortIndex[lsName])
                {
                    lsR = "Waiting up to " + lnWaitTime + " seconds";
                    //// Wait up some seconds for the lock to free
                    loPortLockValue = _oSerialPortLockIndex[lsName];
                    while (null != loPortLockValue && DateTime.UtcNow < ldWaitUntilUtc)
                    {
                        System.Threading.Thread.Sleep(500);
                        loPortLockValue = _oSerialPortLockIndex[lsName];
                    }
                }

                //// Add a lock only if there is not one already
                if (null == loPortLockValue)
                {
                    lsR = null;
                    //// Lock the port and allow any process to use it that gets the lock
                    _oSerialPortLockIndex.Add(lsName, DateTime.UtcNow);
                    MaxLogLibrary.Log(new MaxLogEntryStructure(typeof(MaxSerialPortLibraryDefaultProvider), "AddPortLock[" + lsName + "]", MaxEnumGroup.LogInfo, "Added lock to port {Name}", lsName));
                }
                else if (loPortLockValue is DateTime)
                {
                    lsR = ((DateTime)loPortLockValue).ToString();
                    MaxLogLibrary.Log(new MaxLogEntryStructure(typeof(MaxSerialPortLibraryDefaultProvider), "AddPortLock[" + lsName + "]", MaxEnumGroup.LogError, "Started to wait at {StartTime} and waited for lock to clear for {WaitTime} seconds until {WaitUntil}, but it was set at {LockTime} and has not been removed yet.", ldNowUtc, lnWaitTime, ldWaitUntilUtc, lsR));
                }
            }

            return lsR;
        }

        /// <summary>
        /// Removes a lock on a port so it can be used by another thread
        /// </summary>
        /// <param name="lsName"></param>
        protected void RemovePortLock(string lsName)
        {
            MaxLogLibrary.Log(new MaxLogEntryStructure("MaxSerialPortLibraryDefaultProvider.PortLock." + lsName, MaxEnumGroup.LogStatic, "Removing lock to port {Name} during RemovePortLock", lsName));
            _oSerialPortLockIndex.Remove(lsName);
        }

        /// <summary>
        /// Checks to see if a port exists and is in an open state
        /// </summary>
        /// <param name="lsPortName"></param>
        /// <returns></returns>
        public bool IsConnected(string lsPortName)
        {
            if (lsPortName.Contains("Test"))
            {
                return true;
            }

            SerialPort loPort = GetPort(lsPortName);
            if (null != loPort)
            {
                if (loPort.IsOpen)
                {
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// Creates a SerialPort object for the port, configures it, opens it, and starts reading from it
        /// </summary>
        /// <param name="lsPortName"></param>
        /// <param name="loPortConfigIndex"></param>
        public void Connect(string lsPortName, MaxIndex loPortConfigIndex)
        {
            if (!IsConnected(lsPortName))
            {
                MaxLogLibrary.Log(new MaxLogEntryStructure("MaxSerialPortLibraryDefaultProvider.Connect." + lsPortName, MaxEnumGroup.LogStatic, "Connecting to serial port {Name} during Connect", lsPortName));
                MaxException loException = null;
                SerialPort loPort = GetPort(lsPortName);
                if (null == loPort)
                {
                    lock (_oLock)
                    {
                        loPort = GetPort(lsPortName);
                        if (null == loPort)
                        {
                            _oSerialPortIndex.Add(lsPortName, null);
                            _oSerialPortIndex.Add(lsPortName + "-Config", loPortConfigIndex);
                            if (!string.IsNullOrEmpty(lsPortName) && lsPortName.ToLower().StartsWith("com"))
                            {
                                SerialPort loPortNew = new SerialPort(lsPortName);
                                _oSerialPortDataQueueIndex.Add(loPortNew.PortName, new Queue<byte[]>());
                                _oSerialPortErrorQueueIndex.Add(loPortNew.PortName, new Queue<IOException>());

                                int lnBaudRate = MaxConvertLibrary.ConvertToInt(typeof(object), loPortConfigIndex["BaudRate"]);
                                if (lnBaudRate <= 0)
                                {
                                    lnBaudRate = 9600;
                                }

                                loPortNew.BaudRate = lnBaudRate;

                                if (loPortConfigIndex["Parity"] is Parity)
                                {
                                    loPortNew.Parity = (Parity)loPortConfigIndex["Parity"];
                                }
                                else
                                {
                                    loPortNew.Parity = Parity.None;
                                }

                                int lnDataBits = MaxConvertLibrary.ConvertToInt(typeof(object), loPortConfigIndex["DataBits"]);
                                if (lnDataBits <= 0)
                                {
                                    lnDataBits = 8;
                                }

                                loPortNew.DataBits = lnDataBits;

                                if (loPortConfigIndex["StopBits"] is StopBits)
                                {
                                    loPortNew.StopBits = (StopBits)loPortConfigIndex["StopBits"];
                                }
                                else
                                {
                                    loPortNew.StopBits = StopBits.One;
                                }

                                if (loPortConfigIndex["Handshake"] is Handshake)
                                {
                                    loPortNew.Handshake = (Handshake)loPortConfigIndex["Handshake"];
                                }
                                else
                                {
                                    loPortNew.Handshake = Handshake.None;
                                }

                                int lnReadTimeout = MaxConvertLibrary.ConvertToInt(typeof(object), loPortConfigIndex["ReadTimeout"]);
                                if (lnReadTimeout <= 0)
                                {
                                    lnReadTimeout = 5000;
                                }

                                loPortNew.ReadTimeout = lnReadTimeout;
                                string lsOpenError = OpenPort(loPortNew);
                                if (string.IsNullOrEmpty(lsOpenError))
                                {
                                    //// Only add the configured port if it can be opened without an error
                                    _oSerialPortIndex.Add(lsPortName, loPortNew);
                                    //// Start reading anything from the port that is being sent
                                    StartReadAsync(loPortNew);
                                }
                                else
                                {
                                    loException = new MaxException("Exception opening serial port [" + lsPortName + "] during Connect [" + lsOpenError + "]");
                                }
                            }
                            else
                            {
                                loException = new MaxException("Serial port [" + lsPortName + "] is not a valid port name during Connect");
                            }
                        }
                        else
                        {
                            loException = new MaxException("Serial port [" + lsPortName + "] is null after lock during Connect");
                        }
                    }
                }
                else
                {

                    loException = new MaxException("Serial port [" + lsPortName + "] is null during Connect");
                }

                if (null != loException)
                {
                    MaxLogLibrary.Log(new MaxLogEntryStructure("MaxSerialPortLibraryDefaultProvider.Connect." + lsPortName, MaxEnumGroup.LogStatic, "Exception connecting to serial port {Name} during Connect", loException, lsPortName));
                    throw loException;
                }
            }
        }

        /// <summary>
        /// Opens and initializes a port that is closed
        /// </summary>
        /// <param name="loPort"></param>
        /// <returns></returns>
        public string OpenPort(SerialPort loPort)
        {
            string lsR = string.Empty;
            if (!loPort.IsOpen)
            {
                try
                {
                    MaxIndex loPortConfiguration = GetPortConfig(loPort.PortName);
                    MaxLogLibrary.Log(new MaxLogEntryStructure("MaxSerialPortLibraryDefaultProvider.Connect." + loPort.PortName, MaxEnumGroup.LogStatic, "Opening serial port {Name} with configuration {Config}", loPort.PortName, loPortConfiguration));
                    loPort.Open();
                    loPort.DiscardInBuffer();
                    loPort.DiscardOutBuffer();
                    byte[] loClear = loPortConfiguration["Clear"] as byte[];
                    if (null != loClear)
                    {
                        //' Send Acknowledge to clear port
                        loPort.Write(loClear, 0, loClear.Length);
                    }
                }
                catch (Exception loE)
                {
                    MaxLogLibrary.Log(new MaxLogEntryStructure("MaxSerialPortLibraryDefaultProvider.Connect." + loPort.PortName, MaxEnumGroup.LogStatic, "Error opening serial port {Name}", loE, loPort.PortName));
                    lsR = loE.Message;
                }
            }

            return lsR;
        }

        /// <summary>
        /// Sends a request to a port
        /// </summary>
        /// <param name="lsPortName"></param>
        /// <param name="lsRequest"></param>
        public void SendRequest(string lsPortName, string lsRequest)
        {
            MaxLogLibrary.Log(new MaxLogEntryStructure("MaxSerialPortLibraryDefaultProvider.RequestResponse." + lsPortName, MaxEnumGroup.LogStatic, "Start sending request {lsRequest} to serial port {Name} during SendRequest", lsRequest, lsPortName));
            MaxException loException = null;
            if (!string.IsNullOrEmpty(lsPortName) && !string.IsNullOrEmpty(lsRequest))
            {
                if (lsPortName.ToLower().StartsWith("com"))
                {
                    if (IsConnected(lsPortName))
                    {
                        string lsPortLockResult = AddPortLock(lsPortName);
                        if (null == lsPortLockResult)
                        {
                            SerialPort loPort = GetPort(lsPortName);
                            if (null != loPort)
                            {
                                byte[] laRequest = loPort.Encoding.GetBytes(lsRequest);
                                loPort.BaseStream.BeginWrite(laRequest, 0, laRequest.Length, HandleEndWriteEvent, loPort);
                                MaxLogLibrary.Log(new MaxLogEntryStructure("MaxSerialPortLibraryDefaultProvider.RequestResponse." + lsPortName, MaxEnumGroup.LogStatic, "End sending request {lsRequest} to serial port {Name} during SendRequest", lsRequest, lsPortName));
                            }
                            else
                            {
                                loException = new MaxException("Serial port [" + lsPortName + "] is null during SendRequest");
                            }
                        }
                        else
                        {
                            loException = new MaxException("Timed out getting lock on port [" + lsPortName + "] during SendRequest with result [" + lsPortLockResult + "]");
                        }
                    }
                    else
                    {
                        loException = new MaxException("Port has not been connected [" + lsPortName + "] during SendRequest");
                    }
                }
                else if (!lsPortName.Contains("Test"))
                {
                    loException = new MaxException("Connection name does not start with COM [" + lsPortName + "] during SendRequest");
                }
            }
            else
            {
                loException = new MaxException("Port Name or Request is empty or null during SendRequest");
            }

            if (null != loException)
            {
                MaxLogLibrary.Log(new MaxLogEntryStructure(typeof(MaxSerialPortLibraryDefaultProvider), "SendRequest", MaxEnumGroup.LogError, "Exception sending request {Request} to serial port {PortName} during SendRequest", loException, lsRequest, lsPortName));
                throw loException;
            }
        }

        /// <summary>
        /// Gets data from a port that has been added to a queue
        /// </summary>
        /// <param name="lsPortName"></param>
        /// <returns></returns>
        public byte[] GetResponse(string lsPortName)
        {
            byte[] laR = null;
            try
            {
                if (_oSerialPortDataQueueIndex.Contains(lsPortName))
                {
                    Queue<byte[]> loQueue = _oSerialPortDataQueueIndex[lsPortName] as Queue<byte[]>;
                    if (null != loQueue)
                    {
                        byte[] laOut = null;
                        int lnLimit = 0;
                        if (loQueue.Count > 0)
                        {
                            MaxLogLibrary.Log(new MaxLogEntryStructure("MaxSerialPortLibraryDefaultProvider.RequestResponse." + lsPortName, MaxEnumGroup.LogStatic, "Checking {Count} items in queue for response from serial port {Name} during GetResponse", loQueue.Count, lsPortName));
                            while (loQueue.Count > 0 && lnLimit < 100)
                            {
                                laOut = loQueue.Dequeue();
                                if (null != laOut)
                                {
                                    if (null == laR)
                                    {
                                        laR = laOut;
                                    }
                                    else
                                    {
                                        byte[] laBuffer = new byte[laR.Length + laOut.Length];
                                        laR.CopyTo(laBuffer, 0);
                                        laOut.CopyTo(laBuffer, laR.Length);
                                        laR = laBuffer;
                                    }
                                }

                                lnLimit++;
                            }
                        }
                    }
                }
            }
            catch (Exception loE)
            {
                MaxLogLibrary.Log(new MaxLogEntryStructure("MaxSerialPortLibraryDefaultProvider.RequestResponse." + lsPortName, MaxEnumGroup.LogError, "Error getting response from serial port {Name}", loE, lsPortName));
            }

            return laR;
        }

        /// <summary>
        /// Adds received data to a queue related to the port
        /// </summary>
        /// <param name="loPort"></param>
        /// <param name="laData"></param>
        protected void DataReceived(SerialPort loPort, byte[] laData)
        {
            string lsHex = BitConverter.ToString(laData);
            string lsData = loPort.Encoding.GetString(laData);
            if (null != laData && laData.Length > 0)
            {
                MaxLogLibrary.Log(new MaxLogEntryStructure("MaxDataSerialPortLibrary", MaxEnumGroup.LogInfo, "Adding data to queue from serial port {Name}: {lsData} during DataReceived", loPort.PortName, lsData));
                MaxLogLibrary.Log(new MaxLogEntryStructure("MaxDataSerialPortLibrary.DataReceived." + loPort.PortName, MaxEnumGroup.LogStatic, "Data {lsData}", lsData));
                ((Queue<byte[]>)_oSerialPortDataQueueIndex[loPort.PortName]).Enqueue(laData);
            }
        }

        /// <summary>
        /// Adds errors to a queue and logs the error
        /// </summary>
        /// <param name="loPort"></param>
        /// <param name="loException"></param>
        protected void ErrorReceived(SerialPort loPort, IOException loException)
        {
            ((Queue<IOException>)_oSerialPortErrorQueueIndex[loPort.PortName]).Enqueue(loException);
            MaxLogLibrary.Log(new MaxLogEntryStructure("MaxDataSerialPortLibrary.ErrorReceived", MaxEnumGroup.LogCritical, "Error Receiving on Port {Port}", loException, loPort));
            MaxLogLibrary.Log(new MaxLogEntryStructure("MaxDataSerialPortLibrary.ErrorReceived." + loPort.PortName, MaxEnumGroup.LogStatic, "Error Received", loException));
        }

        /// <summary>
        /// Starts reading from a port on a background thread
        /// </summary>
        /// <param name="loPort"></param>
        protected void StartReadAsync(SerialPort loPort)
        {
            MaxLogLibrary.Log(new MaxLogEntryStructure("MaxDataSerialPortLibrary.RequestResponse." + loPort.PortName, MaxEnumGroup.LogStatic, "Start reading from serial port {Name} during StartReadAsync", loPort.PortName));
            //// Create a shared buffer for the port
            _oBufferIndex.Add(loPort.PortName, new byte[1000]);
            //// Start reading from the port in another thread
            Thread thread = new Thread(new ThreadStart(() => { ReadContinuous(loPort.PortName); }));
            thread.IsBackground = true;
            thread.Start();
            _oSerialPortIndex.Add(loPort.PortName + "-Thread", thread);
            //// Add a cleanup event to close all ports
            AppDomain.CurrentDomain.ProcessExit += new EventHandler(OnProcessExit);
            MaxLogLibrary.Log(new MaxLogEntryStructure("MaxDataSerialPortLibrary.RequestResponse." + loPort.PortName, MaxEnumGroup.LogStatic, "Reading process started on serial port {Name} during StartReadAsync", loPort.PortName));
        }

        /// <summary>
        /// Frees all ports when the process exits
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void OnProcessExit(object sender, EventArgs e)
        {
            foreach (string lsPortName in this.GetPortNames())
            {
                MaxLogLibrary.Log(new MaxLogEntryStructure("MaxDataSerialPortLibrary.Connect." + lsPortName, MaxEnumGroup.LogStatic, "Closing serial port {Name} during OnProcessExit", lsPortName));
                Disconnect(lsPortName);
            }
        }

        /// <summary>
        /// Closes an open port and disposes of the port
        /// </summary>
        /// <param name="lsPortName"></param>
        public void Disconnect(string lsPortName)
        {
            lock (_oLock)
            {
                SerialPort loPort = this.GetPort(lsPortName);
                if (null != loPort)
                {
                    MaxLogLibrary.Log(new MaxLogEntryStructure("MaxDataSerialPortLibrary.Connect." + lsPortName, MaxEnumGroup.LogStatic, "Removing serial port {Name} during Disconnect", lsPortName));
                    _oSerialPortIndex.Remove(lsPortName);
                    try
                    {
                        if (loPort.IsOpen)
                        {
                            MaxLogLibrary.Log(new MaxLogEntryStructure("MaxDataSerialPortLibrary.Connect." + lsPortName, MaxEnumGroup.LogStatic, "Closing serial port {Name} during Disconnect", lsPortName));
                            loPort.Close();
                        }

                        loPort.Dispose();
                    }
                    catch (Exception loE)
                    {
                        MaxLogLibrary.Log(new MaxLogEntryStructure("MaxDataSerialPortLibrary.Connect." + lsPortName, MaxEnumGroup.LogError, "Error closing Port {Port} during Disconnect", loE, loPort));
                    }
                }
            }
        }

        /// <summary>
        /// Continuously reads the data from a port.  Adds a lock to the port when reading starts and removes it when reading ends
        /// </summary>
        /// <param name="loPort"></param>
        protected void ReadContinuous(string lsPortName)
        {
            MaxLogLibrary.Log(new MaxLogEntryStructure("MaxDataSerialPortLibrary.RequestResponse." + lsPortName, MaxEnumGroup.LogStatic, "Getting serial port {Name} during ReadContinuous.  Checking for data on port every 1 second.", lsPortName));
            SerialPort loPort = GetPort(lsPortName);
            while (null != loPort)
            {
                MaxLogLibrary.Log(new MaxLogEntryStructure("MaxDataSerialPortLibrary.Read." + lsPortName, MaxEnumGroup.LogStatic, "Adding lock for serial port {Name} during ReadContinuous", loPort.PortName));
                string lsPortLockResult = AddPortLock(loPort.PortName);
                if (null == lsPortLockResult)
                {
                    MaxLogLibrary.Log(new MaxLogEntryStructure("MaxDataSerialPortLibrary.Read." + lsPortName, MaxEnumGroup.LogStatic, "Attempting to start read from serial port {Name} during ReadContinuous", loPort.PortName));
                    if (!StartBeginRead(loPort))
                    {
                        //// Remove the lock because there are no bytes or the port is closed
                        RemovePortLock(loPort.PortName);
                        MaxLogLibrary.Log(new MaxLogEntryStructure("MaxDataSerialPortLibrary.Read." + lsPortName, MaxEnumGroup.LogStatic, "Removed lock from port {Name} because no data or port closed during ReadContinuous", lsPortName));
                    }
                }
                else
                {
                    MaxLogLibrary.Log(new MaxLogEntryStructure("MaxDataSerialPortLibrary.Read.Error." + lsPortName, MaxEnumGroup.LogStatic, "Failed to add lock for serial port {Name} during ReadContinuous with result {PortLockResult}", loPort.PortName, lsPortLockResult));
                }

                //// Pause 1 second between attempted reads
                System.Threading.Thread.Sleep(1000);
                //// Get the port again, so if it's been removed that the reading will stop
                loPort = GetPort(lsPortName);
            }
        }

        /// <summary>
        /// Started the BaseStream "BeginRead" process which was indicated to be the only stable way to access serial ports based on this: https://www.sparxeng.com/blog/software/must-use-net-system-io-ports-serialport
        /// </summary>
        /// <param name="loPort"></param>
        /// <returns></returns>
        protected bool StartBeginRead(SerialPort loPort)
        {
            if (loPort.IsOpen)
            {
                if (loPort.BytesToRead > 0)
                {
                    MaxLogLibrary.Log(new MaxLogEntryStructure("MaxDataSerialPortLibrary.Read." + loPort.PortName, MaxEnumGroup.LogStatic, "Started read {Count} bytes from serial port {Name} during StartBeginRead", loPort.BytesToRead, loPort.PortName));
                    byte[] loBuffer = _oBufferIndex[loPort.PortName] as byte[];
                    loPort.BaseStream.BeginRead(loBuffer, 0, loBuffer.Length, HandleEndReadEvent, loPort);
                    return true;
                }
                else
                {
                    MaxLogLibrary.Log(new MaxLogEntryStructure("MaxDataSerialPortLibrary.Read." + loPort.PortName, MaxEnumGroup.LogStatic, "No bytes to read at this time on port {Name} during StartBeginRead", loPort.PortName));
                }
            }
            else
            {
                MaxLogLibrary.Log(new MaxLogEntryStructure("MaxDataSerialPortLibrary.Read." + loPort.PortName, MaxEnumGroup.LogError, "Cannot read from port because it's not open {Port}", loPort));
            }

            return false;
        }

        /// <summary>
        /// Handle the end of any read process. Make sure the lock is removed
        /// </summary>
        /// <param name="loResult"></param>
        protected void HandleEndReadEvent(IAsyncResult loResult)
        {
            SerialPort loPort = loResult.AsyncState as SerialPort;
            try
            {
                MaxLogLibrary.Log(new MaxLogEntryStructure("MaxDataSerialPortLibrary.Read." + loPort.PortName, MaxEnumGroup.LogStatic, "Ending read from serial port {Name} during HandleEndReadEvent", loPort.PortName));
                int lnLength = loPort.BaseStream.EndRead(loResult);
                byte[] laReceived = new byte[lnLength];
                byte[] laBufferReceived = _oBufferIndex[loPort.PortName] as byte[];
                Buffer.BlockCopy(laBufferReceived, 0, laReceived, 0, lnLength);
                DataReceived(loPort, laReceived);
            }
            catch (IOException loException)
            {
                ErrorReceived(loPort, loException);
            }
            finally
            {
                RemovePortLock(loPort.PortName);
                MaxLogLibrary.Log(new MaxLogEntryStructure("MaxDataSerialPortLibrary.Read." + loPort.PortName, MaxEnumGroup.LogStatic, "Removed lock from port {Name} because read ended during HandleEndReadEvent", loPort.PortName));
            }
        }

        protected void HandleEndWriteEvent(IAsyncResult loResult)
        {
            SerialPort loPort = loResult.AsyncState as SerialPort;
            try
            {
                RemovePortLock(loPort.PortName);
                MaxLogLibrary.Log(new MaxLogEntryStructure("MaxDataSerialPortLibrary.Write." + loPort.PortName, MaxEnumGroup.LogStatic, "Removed lock from port {Name} because write ended during HandleEndWriteEvent", loPort.PortName));
            }
            catch (IOException loException)
            {
                ErrorReceived(loPort, loException);
            }
        }
    }
}