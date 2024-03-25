// <copyright file="MaxSerialPortLibrary.cs" company="Lakstins Family, LLC">
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

namespace MaxFactry.Base.DataLayer.Library
{
    using System;
    using System.IO.Ports;
    using MaxFactry.Core;

    public class MaxSerialPortLibrary : MaxSingleFactory
    {
        /// <summary>
        /// Internal storage of single object
        /// </summary>
        private static MaxSerialPortLibrary _oInstance = null;

        /// <summary>
        /// Lock object for multi-threaded access.
        /// </summary>
        private static object _oLock = new object();

        /// <summary>
        /// Gets the Provider used for most methods
        /// </summary>
        public static IMaxSerialPortLibraryProvider Provider
        {
            get
            {
                if (null == Instance.BaseProvider)
                {
                    Instance.SetProvider(typeof(Provider.MaxSerialPortLibraryDefaultProvider));
                }

                return (IMaxSerialPortLibraryProvider)Instance.BaseProvider;
            }
        }

        /// <summary>
        /// Gets the single instance of this class.
        /// </summary>
        public static MaxSerialPortLibrary Instance
        {
            get
            {
                if (null == _oInstance)
                {
                    lock (_oLock)
                    {
                        if (null == _oInstance)
                        {
                            _oInstance = new MaxSerialPortLibrary();
                        }
                    }
                }

                return _oInstance;
            }
        }

        public static string[] GetPortNames()
        {
            return Provider.GetPortNames();
        }

        /// <summary>
        /// Checks to see if a port exists and is in an open state
        /// </summary>
        /// <param name="lsPortName"></param>
        /// <returns></returns>
        public static bool IsConnected(string lsPortName)
        {
            return Provider.IsConnected(lsPortName);
        }

        /// <summary>
        /// Creates a SerialPort object for the port, configures it, opens it, and starts reading from it
        /// </summary>
        /// <param name="lsPortName"></param>
        /// <param name="loPortConfigIndex"></param>
        public static void Connect(string lsPortName, MaxIndex loPortConfigIndex)
        {
            Provider.Connect(lsPortName, loPortConfigIndex);
        }

        /// <summary>
        /// Opens and initializes a port that is closed
        /// </summary>
        /// <param name="loPort"></param>
        /// <returns></returns>
        public static string OpenPort(SerialPort loPort)
        {
            return Provider.OpenPort(loPort);
        }

        /// <summary>
        /// Sends a request to a port
        /// </summary>
        /// <param name="lsPortName"></param>
        /// <param name="lsRequest"></param>
        public static void SendRequest(string lsPortName, string lsRequest)
        {
            Provider.SendRequest(lsPortName, lsRequest);
        }

        /// <summary>
        /// Gets data from a port that has been added to a queue
        /// </summary>
        /// <param name="lsPortName"></param>
        /// <returns></returns>
        public static byte[] GetResponse(string lsPortName)
        {
            return Provider.GetResponse(lsPortName);
        }

        /// <summary>
        /// Frees all ports when the process exits
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public static void OnProcessExit(object sender, EventArgs e)
        {
            Provider.OnProcessExit(sender, e);
        }

        /// <summary>
        /// Closes an open port and disposes of the port
        /// </summary>
        /// <param name="lsPortName"></param>
        public static void Disconnect(string lsPortName)
        {
            Provider.Disconnect(lsPortName);
        }
    }
}
