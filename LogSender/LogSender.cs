using LogSender.Utilities;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;

namespace LogSender
{
    class LogSender
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger( "LogSender.cs" );

        ///**********************************************
        ///             Members Section
        ///**********************************************

        private const int NUM_OF_FILES = 1;

        private DirectoryInfo _dirCybLogs;
        private DirectoryInfo _dirFSA;
        private DirectoryInfo _dirImages;
        private DirectoryInfo _dirMOG;
        //private DirectoryInfo directoryRepository;
        //private DirectoryInfo directorySnapFreeze;

        private Thread _threadCyb;
        private Thread _threadFSA;
        private Thread _threadImages;
        private Thread _threadMOG;


        //Data from config file
        ConfigFile _config = new ConfigFile();


        ///**********************************************
        ///             Functions Section
        ///**********************************************

        /// <summary>
        /// Ctor of LogSender Class
        /// </summary>
        public LogSender(string path)
        {
            _config.CfgFile(path);

            _dirCybLogs = new DirectoryInfo(path + "\\Packets");
            _dirFSA = new DirectoryInfo(path + "\\FSAccess");
            _dirImages = new DirectoryInfo(path + "\\Images");
            _dirMOG = new DirectoryInfo(path+ "\\Multievent");
        }

        /// <summary>
        /// 
        /// </summary>
        public void RunService()
        {
            log.Error("error");
            log.Error("error");
            log.Error("error");
            log.Error("error");
            log.Error("error");
            log.Error("error");
            log.Error("error");
            log.Error("error");
            log.Error("error");
            log.Error("error");
            log.Error("error");
            log.Error("error");
            log.Error("error");
            log.Error("error");
            log.Error("error");
            log.Error("error");
            log.Error("error");
            log.Error("error");
            log.Error("error");
            _threadCyb = new Thread(() => SendLogs("cyb", _dirCybLogs));
            _threadCyb.Start();

            //_threadFSA = new Thread(() => SendLogs("fsa", _dirFSA));
            //_threadFSA.Start();

            //_threadImages = new Thread(() => SendLogs("cimg", _dirImages));
            //_threadImages.Start();

            //_threadMOG = new Thread(() => SendLogs("mog", _dirMOG));
            //_threadMOG.Start();

        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="logType"></param>
        /// <param name="currDir"></param>
        private void SendLogs(string logType, DirectoryInfo currDir)
        {
            try
            {
                while(true)
                {
                    //thread sleep for _threadSleepTime miliseconds - configurable
                    //Thread.Sleep(_threadSleepTime);

                    if (currDir.EnumerateFiles("*." + logType).Where(f => (f.Length <= _config.configData._binaryFileMaxSize) && (f.Length > 0)).ToArray().Length > NUM_OF_FILES)
                    {
                        FileInfo[] files = currDir.GetFiles();

                        //multifile string - hold data from few file
                        StringBuilder dataAsString = new StringBuilder();

                        ParsingBinaryFile.AddOutputHeader(dataAsString);

                        //list of file name - hold file full name fot further action on the files like delete
                        List<string> listOfFileNames = new List<string>();

                        long fileSize = 0;

                        //loop on files in folder
                        foreach (FileInfo file in files)
                        {
                            //check if file is locked Write mode
                            if (FileMaintenance.IsFileLocked(file))
                                continue;

                            //add file path to the list.for tracking which file should be deleted 
                            listOfFileNames.Add(file.FullName);

                            //parsing oparation
                            ParsingBinaryFile.Parse(file.FullName, dataAsString, logType);

                        }

                        //get json data from multiple log files as one string
                        string multipleLogFileAsjsonString = JsonDataConvertion.JsonSerialization(dataAsString);


                        //check with aviad. how to check the size before sending the data.
                        //fileSize = System.Text.ASCIIEncoding.Unicode.GetByteCount(multipleLogFileAsjsonString);

                        //compress string with gzip


                        //send to server
                        //SendToServer()
                    }
                }
            }
            catch (Exception ex)
            {
                System.IO.StreamWriter logFile = new System.IO.StreamWriter(AppDomain.CurrentDomain.BaseDirectory + "log.txt", true);
                logFile.WriteLine("\n" + ex.Message);
                logFile.Close();
            }
        }
    }
}
