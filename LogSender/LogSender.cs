using LogSender.Utilities;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace LogSender
{
    class LogSender
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger("LogSender.cs");

        ///**********************************************
        ///             Members Section
        ///**********************************************

        private readonly List<KeyValuePair<string, DirectoryInfo>> _directory;

        //Data from config file
        private readonly ConfigFile _configFile = new ConfigFile();

        ///**********************************************
        ///             Functions Section
        ///**********************************************

        /// <summary>
        /// Ctor of LogSender Class
        /// </summary>
        public LogSender()
        {

            log.Debug("Start creating log sender class");

            if (!_configFile.ReadConfigFile())
            {
                throw new Exception("Problem with config files");
            }

            _directory = new List<KeyValuePair<string, DirectoryInfo>>
            {
                //new KeyValuePair<string , DirectoryInfo>( "cyb" , new DirectoryInfo( _configFile._configData._cybFolderPath ) ) ,
                //new KeyValuePair<string , DirectoryInfo>( "fsa" , new DirectoryInfo( _configFile._configData._fsaFolderPath) ) ,
                //new KeyValuePair<string , DirectoryInfo>( "cimg" , new DirectoryInfo( _configFile._configData._cimgFolderPath ) ) ,
                new KeyValuePair<string , DirectoryInfo>( "mog" , new DirectoryInfo( _configFile._configData._mogFolderPath) )
            };
            log.Debug("log sender class created");

        }

        /// <summary>
        /// This function run main service. create threads
        /// </summary>
        public async void RunService()
        {

            log.Debug("Main Service thread started");

            List<Task> taskList = new List<Task>();
            try
            {
                while (true) //main service loop
                {
                    if (await ServerConnection.IsServerAliveAsync(_configFile._configData._hostIp)) //check if server is online
                    {
                        foreach (KeyValuePair<string, DirectoryInfo> dir in _directory)//run on all log directories
                        {
                            FileMaintenance.ZeroSizeFileCleanup(dir.Value.GetFiles());
                            /* PREF: Task created for each folder */
                            //check folder status
                            if (FolderWatcher
                                .IsFolderReadyToSendWatcher(dir,
                                                            _configFile._configData._binaryFileMaxSize,
                                                            _configFile._configData._minNumOfFilesToSend,
                                                            _configFile._configData._maxBinaryFolderSize))
                            {
                                log.Debug("Sending process can begin, creating sending process Task for " + dir.Value.Name + " log folder");

                                //create task for the current folder
                                taskList.Add(Task.Run(() => SendLogsAsync(dir)));

                                log.Debug("Task created for " + dir.Value.Name + " folder and started his operation");
                            }
                        }
                        /*PREF: the program is not really parallel because main threasis wating - wating for the folders to be updated - if not updated th log sender can send the same log to the server.*/
                        log.Debug("Main thread is wating for all task to finish there operation");
                        Task.WaitAll(taskList.ToArray());

                        taskList.Clear();
                        log.Debug("Task list deleted, main loop has finished the current iteration, going to sleep");
                    }
                    else //server is offline
                    {
                        foreach (KeyValuePair<string, DirectoryInfo> dir in _directory)
                        {
                            if (FolderWatcher.FolderSizeWatcher(dir, _configFile._configData._maxBinaryFolderSize))
                            {
                                //folder size exceeded delete old files
                                FileMaintenance.DeleteOldFiles(dir.Value, _configFile._configData._maxBinaryFolderSize);
                            }
                        }
                        log.Debug("Folder managment finished, main loop has finished the current iteration, going to sleep");
                    }

                    log.Debug("Main thread going to sleep for " + _configFile._configData._threadSleepTime / 1000 + " seconds");
                    Thread.Sleep(_configFile._configData._threadSleepTime);
                    log.Debug("Main thread wake's up, starting new iteration");
                }
            }

            catch (Exception ex)
            {
                log.Fatal("Problem in thread creation", ex);
                Thread.CurrentThread.Abort();
            }
        }

        /// <summary>
        /// Main thread function
        /// </summary>
        /// <param name="dir"></param>
        private async Task SendLogsAsync(KeyValuePair<string, DirectoryInfo> dir)
        {
            try
            {
                log.Debug(dir.Value.Name + " Folder strating his sending process task");

                //Multifile string - hold data from few file
                StringBuilder dataAsString = new StringBuilder();

                //Start the parsing process on the folder
                List<FileInfo> listOfFileToDelete = ParsingBinaryFile.ParseFolder(dataAsString, dir, _configFile._configData._jsonDataMaxSize);

                log.Debug("Json serialazation for " + dir.Value.Name + " files started");

                //get json data from multiple log files as one string
                string multipleLogFileAsjsonString = JsonDataConvertion.JsonSerialization(dataAsString);

                //for testing the string
                //File.WriteAllText(@"C:\Users\idanm\Desktop\test.txt", multipleLogFileAsjsonString);

                //gzip data
                MemoryStream compressedData = GZipCompresstion.CompressString(multipleLogFileAsjsonString);

                ServerConnection serverConnection = new ServerConnection();

                if (serverConnection.
                    ServerManagerAsync(_configFile._configData._numberOfTimesToRetry,
                                       _configFile._configData._hostIp,
                                       _configFile._configData._waitTimeBeforeRetry,
                                       compressedData)
                    .Result)
                {
                    log.Info("Log sender sent " + listOfFileToDelete.Count + " files to the server and the server recived them");

                    log.Debug("Begin deleteing the files that was sent to the server");
                    //FileMaintenance.FileDelete(listOfFileToDelete);
                }
                else
                {
                    log.Error("Task failed. problem occured while trying to send data to server.");
                }
            }
            catch (Exception ex)
            {
                log.Error("Sending process had stoped", ex);
            }
        }
    }
}
