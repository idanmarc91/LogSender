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
        public static bool IsFolderReadyToSendWatcher(KeyValuePair<string, DirectoryInfo> dir)
        {
            try
            {
                log.Info("Watching \'" + dir.Value.Name + "\' folder");

                if (FolderSizeWatcher(dir))
                {
                    //folder size exceeded delete old files
                    FileMaintenance.DeleteOldFiles(dir);
                }

                if (FileSorting.GetAllFilesByConfigSettings(dir)
                               .Length >= ConfigFile.Instance._configData._minNumOfFilesToSend)
                {
                    log.Info("There are files to send in " + dir.Value.Name + " folder");
                    return true;
                }
                else
                {
                    log.Info("There are not enough files that pass configuration settings in " + dir.Value.Name + " folder");
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
        /// the function check if the folder size is bigger or smaller then "_maxBinaryFolderSize" set in config file
        /// </summary>
        /// <param name="dir"></param>
        /// <param name="maxBinaryFolderSize"></param>
        /// <returns>true if folder need maintenance false if not</returns>
        public static bool FolderSizeWatcher(KeyValuePair<string, DirectoryInfo> dir)
        {
            try
            {
                log.Debug("Watching size of \'" + dir.Value.Name + "\' folder");

                long lenght = FileMaintenance.DirSize(dir.Value.GetFiles());

                if (lenght > ConfigFile.Instance._configData._binaryFolderMaxSize)
                {
                    log.Error("The binary folder " + dir.Value.Name + " has reached size limit");
                    return true;
                }
                log.Debug("Folder size is " + lenght + " within the limit (" + ConfigFile.Instance._configData._binaryFolderMaxSize + " bytes)");

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
