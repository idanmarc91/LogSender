using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace LogSender.Utilities
{
    public class FolderWatcher
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger("FolderWatcher.cs");

        ///**********************************************
        ///             Functions Section
        ///**********************************************

        /// <summary>
        /// This function watch the binary folder when server is online
        /// </summary>
        /// <param name="dir"></param>
        /// <returns>true if sending process should begin else false</returns>
        public static bool IsFolderReadyToSendWatcher(KeyValuePair<string, DirectoryInfo> dir, long binaryFileMaxSize, int minNumOfFilesToSend, long maxBinaryFolderSize)
        {
            try
            {
                log.Debug("Watching \'" + dir.Value.Name + "\' folder");

                if (FolderSizeWatcher(dir, maxBinaryFolderSize))
                {
                    //folder size exceeded delete old files
                    FileMaintenance.DeleteOldFiles(dir.Value, maxBinaryFolderSize);
                }

                if (dir.Value.EnumerateFiles("*." + dir.Key)
                             .Where(file => (file.Length <= binaryFileMaxSize) && (file.Length > 0))
                             .ToArray()
                             .Length >= minNumOfFilesToSend)
                {
                    log.Debug("There are files to send in " + dir.Value.Name + " folder");
                    return true;
                }
                else
                {
                    log.Debug("There are not enough files to send in " + dir.Value.Name + " folder");
                    return false;
                }
            }
            catch (Exception ex)
            {
                log.Debug("Problem with " + dir.Value.Name + " folder", ex);
                return false;
            }
        }

        /// <summary>
        /// This function maintaine folder when server is offline 
        /// </summary>
        /// <param name="dir"></param>
        /// <param name="maxBinaryFolderSize"></param>
        /// <returns>true if folder need maintenance false if not</returns>
        public static bool FolderSizeWatcher(KeyValuePair<string, DirectoryInfo> dir, long maxBinaryFolderSize)
        {
            try
            {
                log.Debug("Watching size of \'" + dir.Value.Name + "\' folder");

                long lenght = FileMaintenance.DirSize(dir.Value.GetFiles());

                if (lenght > maxBinaryFolderSize)
                {
                    log.Error("The binary folder " + dir.Value.Name + " has reached size limit");
                    return true;
                }
                log.Debug("Folder size is " + lenght + " within the limit (" + maxBinaryFolderSize + " bytes)");

                return false;
            }
            catch (Exception ex)
            {
                log.Error("Error occurred while checking " + dir.Value.Name + " folder size", ex);
                return true;
            }
        }
    }
}
