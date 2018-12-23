using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace LogSender.Utilities
{
    public class FileMaintenance
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger("FileMaintenance.cs");

        ///**********************************************
        ///             Functions Section
        ///**********************************************
        
        /// <summary>
        /// This function check if the current file can be access
        /// </summary>
        /// <param name="file"></param>
        /// <returns>true - if the file is locked and cannot be access at the moment, false if the file can be access</returns>
        public static bool IsFileLocked(FileInfo file)
        {
            FileStream stream = null;

            try
            {
                stream = file.Open(FileMode.Open, FileAccess.Read, FileShare.None);
            }
            catch (IOException ex)
            {
                //the file is unavailable because it is:
                //still being written to
                //or being processed by another thread
                //or does not exist (has already been processed)
                log.Warn(file.Name + " file is in writing mode. cannot be access!", ex);
                return true;
            }
            finally
            {
                if (stream != null)
                {
                    stream.Close();
                }
            }

            //file is not locked
            return false;
        }

        /// <summary>
        /// This fuction delete a file
        /// </summary>
        /// <param name="listOfFileToDelete"></param>
        public static void FileDelete(List<FileInfo> listOfFileToDelete)
        {
            log.Debug("starting delete files process");

            foreach (FileInfo file in listOfFileToDelete)
            {
                //check if file can be access to
                if (IsFileLocked(file))
                {
                    continue;
                }

                file.Delete();
                log.Debug(file.Name + " file has deleted");
            }
        }

        /// <summary>
        /// This function calculate the directory size
        /// </summary>
        /// <param name="fileArray"></param>
        /// <returns>long - directory size in bytes</returns>
        public static long DirSize(FileInfo[] fileArray)
        {
            long size = 0;
            // Add file sizes.
            foreach (FileInfo file in fileArray)
            {
                //cannot access file- cant add his vale to counter
                if (IsFileLocked(file))
                {
                    continue;
                }

                size += file.Length;
            }
            return size;
        }

        /// <summary>
        /// This function cleanup the empty log files from a directory
        /// </summary>
        /// <param name="fileArray"></param>
        public static void ZeroSizeFileCleanup(DirectoryInfo directory)
        {
            try
            {
                log.Debug("Cleaning all zero size files in " + directory.Name + " directory");
                foreach (FileInfo file in directory.GetFiles())
                {
                    ////cannot access file- cant add his vale to counter
                    //if (IsFileLocked(file))
                    //{
                    //    continue;
                    //}

                    //delete file with zero size
                    if (file.Length == 0 && !IsFileLocked(file))
                    {
                        file.Delete();
                        //continue;
                    }
                }
            }
            catch(Exception ex)
            {
                log.Error("Problem occured while trying to delete an empty file",ex);
            }
        }

        /// <summary>
        /// This function oparete when the folder has exceeded limit size and delete the old log 
        /// files from it
        /// </summary>
        /// <param name="dir"></param>
        /// <param name="binaryFileMaxSize"></param>
        public static void DeleteOldFiles(KeyValuePair<string, DirectoryInfo> dir)
        {
            try
            {
                log.Debug(dir.Value.Name + " Folder size exceeded starting delete old file");

                //get all files by date. first one in the array is the oldes
                FileInfo[] files = dir.Value.GetFiles().OrderBy(f => f.CreationTime).ToArray();

                foreach (FileInfo file in files)
                {
                    if (IsFileLocked(file))
                    {
                        continue;
                    }
                    file.Delete();

                    if (!FolderWatcher.FolderSizeWatcher(dir))
                    {
                        break;
                    }
                }
            }
            catch(Exception ex)
            {
                log.Error("Problem occured while trying to delete old log file", ex);
            }
        }
    }
}
