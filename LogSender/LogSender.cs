using LogSender.Utilities;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading;

namespace LogSender
{
    internal class LogSender
    {
        ///**********************************************
        ///             Members Section
        ///**********************************************

        #region Members section

        private static readonly log4net.ILog log = log4net.LogManager.GetLogger("LogSender.cs");

        private readonly List<KeyValuePair<string, DirectoryInfo>> _directory;

        //Data from config file
        private readonly ConfigFile _configFile = ConfigFile.Instance;

        #endregion Members section

        ///**********************************************
        ///             Functions Section
        ///**********************************************

        #region Functions section

        /// <summary>
        /// Ctor of LogSender Class
        /// </summary>
        public LogSender()
        {
            log.Debug("Start creating log sender class");

            _directory = new List<KeyValuePair<string, DirectoryInfo>>
            {
                new KeyValuePair<string , DirectoryInfo>( "cyb" , new DirectoryInfo( ConfigFile.Instance._configData._cybFolderPath ) ) ,
                new KeyValuePair<string , DirectoryInfo>( "fsa" , new DirectoryInfo( ConfigFile.Instance._configData._fsaFolderPath) ) ,
                new KeyValuePair<string , DirectoryInfo>( "cimg" , new DirectoryInfo( ConfigFile.Instance._configData._cimgFolderPath ) ) ,
                new KeyValuePair<string , DirectoryInfo>( "mog" , new DirectoryInfo( ConfigFile.Instance._configData._mogFolderPath) )
            };

            log.Debug("Log sender class created");
        }

        /// <summary>
        /// This function run main service. create threads
        /// </summary>
        public async void RunService()
        {
            log.Info("Main Service thread started");
            while (true) //main service loop
            {
                //check if server is online
                if (await ServerConnection.IsServerAliveAsync())
                {
                    OnlineProcessLogFolders();
                }
                //server is offline
                else
                {
                    OfflineProcessLogFolders();
                }

                RecSender.SendRecFiles();

                log.Debug("Main thread going to sleep for " + ConfigFile.Instance._configData._threadSleepTime / 1000 + " seconds");
                Thread.Sleep(ConfigFile.Instance._configData._threadSleepTime);
                log.Debug("Main thread wake's up, starting new iteration");
            }
        }

        /// <summary>
        /// This function is processing the log folders when server is online
        /// </summary>
        private void OnlineProcessLogFolders()
        {
            try
            {
                for (int index = 0; index < _directory.Count; index++)
                {
                    FileMaintenance.ZeroSizeFileCleanup(_directory[index].Value);

                    //check folder status
                    if (FolderWatcher.IsFolderReadyToSendWatcher(_directory[index]))
                    {
                        log.Info("Sending process can begin, creating sending process Task for " + _directory[index].Value.Name + " log folder");

                        SendLogFolder(_directory[index]);

                        log.Debug("Task created for " + _directory[index].Value.Name + " folder and started his operation");
                    }
                }
            }
            catch (Exception ex)
            {
                log.Error("Problem occurred while sending the logs data to the server.", ex);
            }
        }


        /// <summary>
        /// This function is processing the log folders when server is offline
        /// </summary>
        private void OfflineProcessLogFolders()
        {
            try
            {
                log.Debug("Starting offline procedure for log folders");
                foreach (KeyValuePair<string, DirectoryInfo> dir in _directory)
                {
                    FileMaintenance.ZeroSizeFileCleanup(dir.Value);
                    long dirSize = FileMaintenance.DirSize(dir.Value.GetFiles());// get directory size

                    if (FolderWatcher.FolderSizeWatcher(dir, dirSize))
                    {
                        //folder size exceeded delete old files
                        FileMaintenance.DeleteOldFiles(dir, dirSize);
                    }
                }
                log.Debug("Folder management finished, main loop has finished the current iteration, going to sleep");
            }
            catch (Exception ex)
            {
                log.Error("Error occurred in log folders offline process", ex);
            }
        }

        /// <summary>
        /// Main thread function
        /// </summary>
        /// <param name="dir"></param>
        private void SendLogFolder(KeyValuePair<string, DirectoryInfo> dir)
        {
            try
            {
                log.Debug("*** Starting sending process with " + dir.Value.Name + " Folder ***");

                //Multi file string - hold data from few file
                StringBuilder dataAsString = new StringBuilder();

                //Start the parsing process on the folder
                List<FileInfo> listOfFileToDelete = ParsingBinaryFile.ParseFolder(dataAsString, dir);

                log.Debug("Json serialization for " + dir.Value.Name + " files started");

                //get json data from multiple log files as one string
                string multipleLogFileAsjsonString = JsonDataConvertion.JsonSerialization(dataAsString);

                //for testing the string
                //File.WriteAllText(@"C:\Users\idanm\Desktop\test.txt", multipleLogFileAsjsonString);

                //gzip data
                MemoryStream compressedData = GZipCompresstion.CompressString(multipleLogFileAsjsonString);

                ServerConnection serverConnection = new ServerConnection();

                if (serverConnection.
                    ServerManagerAsync(compressedData).
                    Result)
                {
                    log.Info("Log sender sent " + listOfFileToDelete.Count + " files to the server and the server received them");

                    log.Debug("Begin deleting the files that was sent to the server");
                    FileMaintenance.FileDelete(listOfFileToDelete);
                }
                else
                {
                    log.Error("Server sending process failed. problem occurred while trying to send data to server.");
                }

                log.Debug("*** Finished sending process with " + dir.Value.Name + " Folder. ***");

            }
            catch (Exception ex)
            {
                log.Error("Sending process had stopped", ex);
            }
        }

        #endregion Functions section
    }
}