/*
 * Copyright 2012-2013 inBloom, Inc. and its affiliates.
 *
 * Licensed under the Apache License, Version 2.0 (the "License");
 * you may not use this file except in compliance with the License.
 * You may obtain a copy of the License at
 *
 * http://www.apache.org/licenses/LICENSE-2.0
 *
 * Unless required by applicable law or agreed to in writing, software
 * distributed under the License is distributed on an "AS IS" BASIS,
 * WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 * See the License for the specific language governing permissions and
 * limitations under the License.
 */

using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using System.Security.Policy;
using System.Web;

using System.Diagnostics;

namespace TeamMnMGroupingWebApp.Helper
{
    public class FileHelper
    {
        // Application data path. Retrieved from Web.Config
        internal static String pathRoot = ConfigurationManager.AppSettings["AppRoot"];

        /// <summary>
        /// Uploads a file from the system.
        /// </summary>
        /// <param name="directory"></param>
        /// <param name="fileName"></param>
        /// <param name="stream"></param>
        /// <returns></returns>
        public static void uploadFileFromStream(string directory, string fileName, Stream stream)
        {
            String fullPath = pathRoot + directory + "\\";
            // Use a buffer to temporarily store the bytes of the file.
            byte[] buffer = new byte[stream.Length];
            try
            {
                // If the directory does not exist, create it.
                if (!Directory.Exists(fullPath))
                {
                    createDir(fullPath);
                }
                // Read the bytes from the file into the buffer
                stream.Read(buffer, 0, buffer.Length);
                // Close the stream to free up resources.
                stream.Close();

                // Create a new file stream to save the file
                FileStream dataOut = new FileStream(fullPath + fileName, FileMode.Create, FileAccess.Write);
                // Write the file from the buffer
                dataOut.Write(buffer, 0, buffer.Length);
                // Close the file
                dataOut.Close();
            }
            catch (Exception ex)
            {
                Elmah.ErrorSignal.FromCurrentContext().Raise(ex);
            }
        }

        /// <summary>
        /// Create a new directory. Used when there is no group directory.
        /// </summary>
        /// <param name="directoryName"></param>
        /// <returns>boolean</returns>
        public static bool createDir(string directoryName)
        {
            try
            {
                Directory.CreateDirectory(directoryName);
                return true;
            }
            catch (Exception ex)
            {
                Elmah.ErrorSignal.FromCurrentContext().Raise(ex);
                return false;
            }
        }

        /// <summary>
        /// Removes a directory from the file system. Used when
        ///     a lesson is removed or group is deleted.
        /// </summary>
        /// <param name="dirPath"></param>
        /// <returns>boolean</returns>
        public static bool removeDir(string dirPath)
        {
            try
            {
                // Removes the directory. The true parameter
                //   signifies a recursive delete
                Directory.Delete(pathRoot + dirPath, true);
                return true;
            }
            catch (Exception ex)
            {
                Elmah.ErrorSignal.FromCurrentContext().Raise(ex);
                return false;
            }
        }

        /// <summary>
        /// Downloads a file from the system.
        /// </summary>
        /// <param name="directory"></param>
        /// <param name="fileName"></param>
        /// <returns>A filestream containing the file</returns>
        public static FileStream downloadFile(string directory, string fileName)
        {
            String fullPath = pathRoot + directory + "\\" + fileName;
            // Create a filestream using the path to the file. Open it in read mode
            try
            {
                FileStream fileStream = new FileStream(fullPath, FileMode.Open, FileAccess.Read);
                return fileStream;
            }
            catch (Exception ex)
            {
                Elmah.ErrorSignal.FromCurrentContext().Raise(ex);
                return null;
            }
        }
    }
}