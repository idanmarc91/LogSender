using LogSender.Utilities;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading;

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
        private readonly ConfigFile _config = new ConfigFile();

        ///**********************************************
        ///             Functions Section
        ///**********************************************

        /// <summary>
        /// Ctor of LogSender Class
        /// </summary>
        public LogSender()
        {

            log.Debug("Start creating log sender class");

            _config.CfgFile();

            _directory = new List<KeyValuePair<string, DirectoryInfo>>
            {
                new KeyValuePair<string , DirectoryInfo>( "cyb" , new DirectoryInfo( _config.configData._cybFolderPath ) ) ,
                new KeyValuePair<string , DirectoryInfo>( "fsa" , new DirectoryInfo( _config.configData._fsaFolderPath) ) ,
                new KeyValuePair<string , DirectoryInfo>( "cimg" , new DirectoryInfo( _config.configData._cimgFolderPath ) ) ,
                new KeyValuePair<string , DirectoryInfo>( "mog" , new DirectoryInfo( _config.configData._mogFolderPath) )
            };

            log.Debug("log sender class created");
        }

        /// <summary>
        /// This function run main service. create threads
        /// </summary>
        public async void RunService()
        {
            log.Debug("Main Service thread started");

            List<Thread> threadList = new List<Thread>();
            try
            {
                while (true) //main service loop
                {
                    if (await ServerConnection.IsServerAlive(_config.configData._hostIp)) //check if server is online
                    {
                        foreach (KeyValuePair<string, DirectoryInfo> dir in _directory)//run on all log directories
                        {
                          /* PREF - thread created for each folder */
                            //check folder status
                            if (FolderWatcher.IsFolderReadyToSendWatcher(dir, _config.configData._binaryFileMaxSize, _config.configData._minNumOfFilesToSend, _config.configData._maxBinaryFolderSize))
                            {
                                log.Debug("Sending process can begin, creating sending process thread for " + dir.Value.Name + " log folder");
                                Thread thread = new Thread(() => SendLogs(dir));
                                threadList.Add(thread);
                                thread.Name = dir.Value.Name;
                                thread.Start();

                                log.Debug("thread created for " + dir.Value.Name + " folder and started his operation");
                            }
                        }
                        //wait for threads to end
                        foreach (Thread t in threadList)
                        {
                            t.Join();
                        }
                        threadList.Clear();
                        log.Debug("Thread list deleted, main loop has finished the current iteration going to sleep");
                    }
                    else //server is offline
                    {
                        log.Debug("Server is offline");

                        foreach (KeyValuePair<string, DirectoryInfo> dir in _directory)
                        {
                            if (FolderWatcher.FolderSizeWatcher(dir, _config.configData._maxBinaryFolderSize))
                            {
                                //folder size exceeded delete old files
                                FileMaintenance.DeleteOldFiles(dir.Value, _config.configData._maxBinaryFolderSize);
                            }
                        }
                    }

                    log.Debug("main thread going to sleep for " + _config.configData._threadSleepTime / 1000 + " seconds");
                    Thread.Sleep(_config.configData._threadSleepTime);
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
        private async void SendLogs(KeyValuePair<string, DirectoryInfo> dir)
        {
            try
            {
                log.Debug(dir.Key + " Thread strating his sending process");

                //multifile string - hold data from few file
                StringBuilder dataAsString = new StringBuilder();

                //start the parsing process on the folder
                List<FileInfo> listOfFileToDelete = ParsingBinaryFile.ParseFolder(dataAsString, dir, _config.configData._jsonDataMaxSize);

                log.Debug("serialazation for " + dir.Key + " files started");

                //get json data from multiple log files as one string
                string multipleLogFileAsjsonString = JsonDataConvertion.JsonSerialization(dataAsString);

                //gzip data
                MemoryStream compressedData = GZipCompresstion.CompressString(multipleLogFileAsjsonString);

                int retry = _config.configData._numberOfTimesToRetry;

                while (retry-- != 0)//retry loop
                {
                    if (await ServerConnection.SendDataToServer(_config.configData._hostIp, compressedData))
                    {
                        log.Info("log sender sent " + listOfFileToDelete.Count + " files to the server and the server recived them");
                        log.Debug("begin deleteing the files that was sent to the server");

                        FileMaintenance.FileDelete(listOfFileToDelete);

                        break;//when file sent sucessfuly exit while loop
                    }
                    Thread.Sleep(_config.configData._waitTimeBeforeRetry);
                }
            }
            catch (Exception ex)
            {
                log.Error("Sending process had stoped", ex);
            }
        }
    }
}
