﻿// <copyright file="MaxStreamLibraryDefaultProvider.cs" company="Lakstins Family, LLC">
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
// <change date="9/20/2023" author="Brian A. Lakstins" description="Initial creation">
// <change date="3/20/2024" author="Brian A. Lakstins" description="Happy birthday to my mom.  Sara Jean Lakstins (Cartwright) - 3/20/1944 to 3/14/2019.">
// <change date="3/24/2024" author="Brian A. Lakstins" description="Rename to MaxSteamLibrary to not indicate some dependency on MaxDataContextProvider">
// <change date="4/1/2024" author="Brian A. Lakstins" description="Make sure a stream can be read before being processed.">
// <change date="4/14/2025" author="Brian A. Lakstins" description="Open the file stream for reading in a way that other processes can open the same strem.">
// <change date="5/21/2025" author="Brian A. Lakstins" description="Update to handle one field of one element at a time and send flag based return codes">
// <change date="6/21/2025" author="Brian A. Lakstins" description="Update stream path handling">
// <change date="6/23/2025" author="Brian A. Lakstins" description="Fix issues with Streampath using DataName multiple times in the path">
// </changelog>
#endregion

namespace MaxFactry.Base.DataLayer.Library.Provider
{
    using System.IO;
    using MaxFactry.Core;
    using MaxFactry.Base.DataLayer.Library;
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// Provides methods to manipulate storage of streams
    /// </summary>
    public class MaxStreamLibraryDefaultProvider : MaxProvider, IMaxStreamLibraryProvider
    {
        /// <summary>
        /// Folder to use for stream storage
        /// </summary>
        private string _sDataFolder = string.Empty;

        /// <summary>
        /// Initializes the provider
        /// </summary>
        /// <param name="lsName">Name of the provider</param>
        /// <param name="loConfig">Configuration information</param>
        public override void Initialize(string lsName, MaxIndex loConfig)
        {
            base.Initialize(lsName, loConfig);
            string lsDataDirectory = MaxConfigurationLibrary.GetValue(MaxEnumGroup.ScopeApplication, "MaxDataDirectory") as string;
            if (!string.IsNullOrEmpty(lsDataDirectory))
            {
                this._sDataFolder = Path.Combine(lsDataDirectory, "data");
            }
        }

        protected string DataFolder
        {
            get
            {
                return this._sDataFolder;
            }
        }

        protected virtual string GetStreamPath(string[] laStreamPath)
        {
            string lsR = laStreamPath[0];
            for (int lnP = 1; lnP < laStreamPath.Length; lnP++)
            {
                lsR = Path.Combine(lsR, laStreamPath[lnP]);
            }

            return lsR;
        }

        /// <summary>
        /// Make sure the file exists and update the path if needed
        /// </summary>
        /// <param name="loData"></param>
        /// <param name="lsDataName"></param>
        /// <returns></returns>
        protected virtual string[] GetStreamPath(MaxData loData, string lsDataName)
        {
            string[] laStreamDirectory = loData.GetStreamPath();
            List<string> loR = new List<string>(laStreamDirectory);
            if (null != lsDataName && lsDataName.Length > 0)
            {
                loR.Add(lsDataName);
                string lsStreamDirectory = Path.Combine(this.DataFolder, this.GetStreamPath(laStreamDirectory));
                string lsStreamFile = Path.Combine(lsStreamDirectory, lsDataName);
                if (!File.Exists(lsStreamFile))
                {
                    string lsStreamFileLatest = lsStreamFile;
                    bool lbIsStreamFound = false;
                    for (int lnS = MaxDataModel.StreamPathTypeList.Length - 2; lnS >= 0 && !lbIsStreamFound; lnS--)
                    {
                        string lsStreamPathType = MaxDataModel.StreamPathTypeList[lnS];
                        loData.Set("_StreamPathType", lsStreamPathType);
                        laStreamDirectory = loData.GetStreamPath();
                        lsStreamDirectory = Path.Combine(this.DataFolder, this.GetStreamPath(laStreamDirectory));
                        lsStreamFile = Path.Combine(lsStreamDirectory, lsDataName);
                        lbIsStreamFound = File.Exists(lsStreamFile);
                        if (!lbIsStreamFound)
                        {
                            lsStreamFile = Path.Combine(this.DataFolder, this.GetStreamPath(laStreamDirectory) + "_" + lsDataName);
                            lbIsStreamFound = File.Exists(lsStreamFile);
                        }

                        if (lbIsStreamFound)
                        {
                            //// copy the stream to the latest convention
                            File.Copy(lsStreamFile, lsStreamFileLatest, true);
                            //// Delete it from the previous convention
                            File.Delete(lsStreamFile);                            
                        }
                    }
                }
            }

            return loR.ToArray();
        }

        /// <summary>
        /// Saves a single field in a data element to storage.
        /// </summary>
        /// <param name="loData">The data element</param>
        /// <param name="lsDataName">Name of data element to save</param>
        /// <returns>Flag based status code indicating level of success.</returns>
        public virtual int StreamSave(MaxData loData, string lsDataName)
        {
            int lnR = 0;
            object loValueType = loData.DataModel.GetValueType(lsDataName);
            if (loData.GetIsChanged(lsDataName))
            {
                try
                {
                    //// Check defined storage type
                    if (typeof(MaxLongString).Equals(loValueType) || typeof(byte[]).Equals(loValueType) || typeof(Stream).Equals(loValueType))
                    {
                        object loValue = loData.Get(lsDataName);
                        if (null != loValue && (loValue is Stream || loValue is string || loValue is byte[]))
                        {
                            Stream loStream = null;
                            if (loValue is string && ((string)loValue).Length > 2000)
                            {
                                //// Store as stream if over 2K in length
                                loData.Set(lsDataName, MaxDataModel.StreamStringIndicator);
                                loStream = new MemoryStream(System.Text.UTF8Encoding.UTF8.GetBytes(((string)loValue)));
                            }
                            else if (loValue is byte[] && ((byte[])loValue).Length > 2000)
                            {
                                //// Store as stream if over 2K in length
                                loData.Set(lsDataName, MaxDataModel.StreamByteIndicator);
                                loStream = new MemoryStream((byte[])loValue);
                            }
                            else if (loValue is Stream)
                            {
                                loStream = (Stream)loValue;
                            }

                            if (null != loStream && loStream.CanRead)
                            {
                                string[] laStreamDirectory = this.GetStreamPath(loData, string.Empty);
                                string lsStreamDirectory = Path.Combine(this.DataFolder, this.GetStreamPath(laStreamDirectory));
                                if (!Directory.Exists(lsStreamDirectory))
                                {
                                    Directory.CreateDirectory(lsStreamDirectory);
                                }

                                string[] laStreamPath = this.GetStreamPath(loData, lsDataName);
                                string lsStreamFile = Path.Combine(this.DataFolder, this.GetStreamPath(laStreamPath));
                                bool lbIsChanged = true;
                                if (File.Exists(lsStreamFile))
                                {
                                    lbIsChanged = false;
                                    if (loStream.CanSeek)
                                    {
                                        loStream.Seek(0, SeekOrigin.Begin);
                                    }

                                    using (FileStream loFileStream = File.OpenRead(lsStreamFile))
                                    {
                                        //// 50K chunks.
                                        int lnBufferSize = 50 * 1024;
                                        byte[] laBufferCurrent = new byte[lnBufferSize];
                                        byte[] laBufferNew = new byte[lnBufferSize];
                                        int lnReadNew = loStream.Read(laBufferNew, 0, lnBufferSize);
                                        int lnReadCurrent = loFileStream.Read(laBufferCurrent, 0, lnBufferSize);
                                        if (lnReadNew != lnReadCurrent)
                                        {
                                            lbIsChanged = true;
                                        }

                                        while (lnReadNew > 0 && lnReadCurrent > 0 && !lbIsChanged)
                                        {
                                            for (int lnC = 0; lnC < lnReadCurrent; lnC++)
                                            {
                                                if (laBufferCurrent[lnC] != laBufferNew[lnC])
                                                {
                                                    lbIsChanged = true;
                                                }
                                            }

                                            if (!lbIsChanged)
                                            {
                                                lnReadNew = loStream.Read(laBufferNew, 0, lnBufferSize);
                                                lnReadCurrent = loFileStream.Read(laBufferCurrent, 0, lnBufferSize);
                                                if (lnReadNew != lnReadCurrent)
                                                {
                                                    lbIsChanged = true;
                                                }
                                            }
                                        }
                                    }

                                    if (lbIsChanged)
                                    {
                                        File.Move(lsStreamFile, lsStreamFile + "-" + DateTime.UtcNow.Ticks.ToString());
                                    }
                                }

                                if (lbIsChanged)
                                {
                                    if (loStream.CanSeek)
                                    {
                                        loStream.Seek(0, SeekOrigin.Begin);
                                    }

                                    //// 50K chunks.
                                    int lnBufferSize = 50 * 1024;
                                    byte[] laBuffer = new byte[lnBufferSize];
                                    using (FileStream loFile = File.Create(lsStreamFile, lnBufferSize))
                                    {
                                        int lnRead = loStream.Read(laBuffer, 0, lnBufferSize);
                                        while (lnRead > 0)
                                        {
                                            loFile.Write(laBuffer, 0, lnRead);
                                            lnRead = loStream.Read(laBuffer, 0, lnBufferSize);
                                        }

                                        loFile.Close();
                                    }
                                }
                                else
                                {
                                    loData.ClearChanged(lsDataName);
                                    lnR = lnR + 2; //// Data is not changed
                                }
                            }
                        }
                        else
                        {
                            lnR = lnR + 8; //// Value is null or not a stream type
                        }
                    }
                    else
                    {
                        lnR = lnR + 4; //// Defined type of value is not a stream type
                    }
                }
                catch (Exception loE)
                {
                    MaxLogLibrary.Log(new MaxLogEntryStructure(this.GetType(), "StreamSave", MaxEnumGroup.LogError, "Exception saving stream for {DataName}", loE, lsDataName));
                    lnR += 1; //// Exception saving stream
                }
            }
            else if (typeof(MaxLongString).Equals(loValueType) || typeof(byte[]).Equals(loValueType) || typeof(Stream).Equals(loValueType))
            {
                lnR = lnR + 2; //// Data is not changed
                object loValue = loData.Get(lsDataName);
                if (null != loValue && (loValue is Stream || loValue is string || loValue is byte[]))
                {
                    if (loValue is string && ((string)loValue).Length > 2000)
                    {
                        //// Store as stream if over 2K in length
                        loData.Set(lsDataName, MaxDataModel.StreamStringIndicator);
                    }
                    else if (loValue is byte[] && ((byte[])loValue).Length > 2000)
                    {
                        //// Store as stream if over 2K in length
                        loData.Set(lsDataName, MaxDataModel.StreamByteIndicator);
                    }
                }
            }            

            return lnR;
        }

        /// <summary>
        /// Opens stream data in storage
        /// </summary>
        /// <param name="loData">The data element</param>
        /// <param name="lsDataName">Name of data element to save</param>
        /// <returns>Stream that was opened.</returns>
        public virtual Stream StreamOpen(MaxData loData, string lsDataName)
        {
            string[] laStreamPath = this.GetStreamPath(loData, lsDataName);
            string lsStreamFile = Path.Combine(this.DataFolder, this.GetStreamPath(laStreamPath));
            if (File.Exists(lsStreamFile))
            {
                FileStream loStream = File.Open(lsStreamFile, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
                return loStream;
            }

            return null;
        }

        /// <summary>
        /// Deletes a single field in a data element from storage
        /// </summary>
        /// <param name="loData">The data element</param>
        /// <param name="lsDataName">Name of data element to delete</param>
        /// <returns>Flag based status code indicating level of success.</returns>
        public virtual int StreamDelete(MaxData loData, string lsDataName)
        {
            int lnR = 0;
            string[] laStreamPath = this.GetStreamPath(loData, lsDataName);
            try
            {
                string lsStreamFile = Path.Combine(this.DataFolder, this.GetStreamPath(laStreamPath));
                if (File.Exists(lsStreamFile))
                {
                    File.Delete(lsStreamFile);
                }
                else
                {
                    lnR = lnR + 2; //// File does not exist
                }
            }
            catch (Exception loE)
            {
                MaxLogLibrary.Log(new MaxLogEntryStructure(this.GetType(), "StreamDelete", MaxEnumGroup.LogError, "Exception deleting stream for {DataName}", loE, lsDataName));
                lnR += 1; //// Exception deleting stream
            }

            return lnR;
        }

        /// <summary>
        /// Gets the Url of a saved stream.
        /// </summary>
        /// <param name="loData">The data element</param>
        /// <param name="lsDataName">Name of data element get stream for</param>
        /// <returns>Url of stream if one can be provided.</returns>
        public virtual string GetStreamUrl(MaxData loData, string lsDataName)
        {
            return string.Empty;
        }
    }
}