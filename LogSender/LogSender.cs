using LogSender.Utilities;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

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

            //if (!_configFile.ReadConfigFile())
            //{
            //    throw new Exception("Problem with config files");
            //}

            _directory = new List<KeyValuePair<string, DirectoryInfo>>
            {
               // new KeyValuePair<string , DirectoryInfo>( "cyb" , new DirectoryInfo( ConfigFile.Instance._configData._cybFolderPath ) ) ,
                new KeyValuePair<string , DirectoryInfo>( "fsa" , new DirectoryInfo( ConfigFile.Instance._configData._fsaFolderPath) ) ,
                //new KeyValuePair<string , DirectoryInfo>( "cimg" , new DirectoryInfo( ConfigFile.Instance._configData._cimgFolderPath ) ) ,
                //new KeyValuePair<string , DirectoryInfo>( "mog" , new DirectoryInfo( ConfigFile.Instance._configData._mogFolderPath) )
            };
            log.Debug("log sender class created");
        }

        /// <summary>
        /// This function run main service. create threads
        /// </summary>
        public async void RunService()
        {
            log.Debug("Main Service thread started");

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
                List<Task> taskList = new List<Task>();

                foreach (KeyValuePair<string, DirectoryInfo> dir in _directory)//run on all log directories
                {
                    FileMaintenance.ZeroSizeFileCleanup(dir.Value);
                    /* PREF: Task created for each folder */
                    //check folder status
                    if (FolderWatcher.IsFolderReadyToSendWatcher(dir))
                    {
                        log.Debug("Sending process can begin, creating sending process Task for " + dir.Value.Name + " log folder");

                        //create task for the current folder
                        taskList.Add(Task.Run(() => SendFolderLogsAsync(dir)));

                        log.Debug("Task created for " + dir.Value.Name + " folder and started his operation");
                    }
                }
                /*PREF: the program is not really parallel because main thread is waiting - waiting for the folders to be updated - if not updated th log sender can send the same log to the server.*/
                log.Debug("Main thread is waiting for all task to finish there operation");
                Task.WaitAll(taskList.ToArray());

                taskList.Clear();
                log.Debug("Task list deleted, main loop has finished the current iteration, going to sleep");
            }
            catch (Exception ex)
            {
                log.Fatal("Problem in thread creation", ex);
                Thread.CurrentThread.Abort();
            }
        }

        /// <summary>
        /// This function is processing the log folders when server is offline
        /// </summary>
        private void OfflineProcessLogFolders()
        {
            try
            {
                foreach (KeyValuePair<string, DirectoryInfo> dir in _directory)
                {
                    FileMaintenance.ZeroSizeFileCleanup(dir.Value);
                    if (FolderWatcher.FolderSizeWatcher(dir))
                    {
                        //folder size exceeded delete old files
                        FileMaintenance.DeleteOldFiles(dir);
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
        private async Task SendFolderLogsAsync(KeyValuePair<string, DirectoryInfo> dir)
        {
            try
            {
                log.Debug(dir.Value.Name + " Folder starting his sending process task");

                //Multifile string - hold data from few file
                StringBuilder dataAsString = new StringBuilder();

                //Start the parsing process on the folder
                List<FileInfo> listOfFileToDelete = ParsingBinaryFile.ParseFolder(dataAsString, dir);

                log.Debug("Json serialization for " + dir.Value.Name + " files started");

                //get json data from multiple log files as one string
                string multipleLogFileAsjsonString = JsonDataConvertion.JsonSerialization(dataAsString);

                //for testing the string
                File.WriteAllText(@"C:\Users\idanm\Desktop\test.txt", multipleLogFileAsjsonString);

                //gzip data
                MemoryStream compressedData = GZipCompresstion.CompressString(multipleLogFileAsjsonString);

                ServerConnection serverConnection = new ServerConnection();

                if (serverConnection.
                    ServerManagerAsync(compressedData)
                    .Result)
                {
                    log.Info("Log sender sent " + listOfFileToDelete.Count + " files to the server and the server received them");

                    log.Debug("Begin deleting the files that was sent to the server");
                    FileMaintenance.FileDelete(listOfFileToDelete);
                }
                else
                {
                    log.Error("Task failed. problem occurred while trying to send data to server.");
                }
            }
            catch (Exception ex)
            {
                log.Error("Sending process had stopped", ex);
            }
        }

        #endregion Functions section
    }
}