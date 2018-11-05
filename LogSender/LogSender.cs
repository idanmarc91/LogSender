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

        private readonly List<KeyValuePair<string , DirectoryInfo>> _directory;

        //private readonly DirectoryInfo _dirCybLogs;
        //private readonly DirectoryInfo _dirFSA;
        //private readonly DirectoryInfo _dirImages;
        //private readonly DirectoryInfo _dirMOG;
        //private DirectoryInfo directoryRepository;
        //private DirectoryInfo directorySnapFreeze;

        //private Thread _threadCyb;
        //private Thread _threadFSA;
        //private Thread _threadImages;
        //private Thread _threadMOG;


        //Data from config file
        private readonly ConfigFile _config = new ConfigFile();


        ///**********************************************
        ///             Functions Section
        ///**********************************************

        /// <summary>
        /// Ctor of LogSender Class
        /// </summary>
        public LogSender(string path)
        {
            log.Debug( "Start creating log sender class" );

            _config.CfgFile( path );

            _directory = new List<KeyValuePair<string , DirectoryInfo>>
            {
                new KeyValuePair<string , DirectoryInfo>( "cyb" , new DirectoryInfo( path + "\\Packets" ) ) ,
                new KeyValuePair<string , DirectoryInfo>( "fsa" , new DirectoryInfo( path + "\\FSAccess" ) ) ,
                new KeyValuePair<string , DirectoryInfo>( "cimg" , new DirectoryInfo( path + "\\Images" ) ) ,
                new KeyValuePair<string , DirectoryInfo>( "mog" , new DirectoryInfo( path + "\\Multievent" ) )
            };

            //_dirCybLogs = new DirectoryInfo( path + "\\Packets" );
            //_dirFSA = new DirectoryInfo( path + "\\FSAccess" );
            //_dirImages = new DirectoryInfo( path + "\\Images" );
            //_dirMOG = new DirectoryInfo( path + "\\Multievent" );

            log.Debug( "log sender class created" );
        }

        /// <summary>
        /// This function run main service. create threads
        /// </summary>
        public async void RunService()
        {
            log.Debug( "Main Service thread started" );
            List<Thread> threadList = new List<Thread>();
            try
            {
                while( true )
                {
                    if( await ServerConnection.IsServerAlive( _config.configData._hostIp ) ) //server is online
                    {
                        log.Debug( "Server is online" );

                        foreach( KeyValuePair<string , DirectoryInfo> dir in _directory )
                        {
                            if( FolderWatcher.FolderOnlineWatcher( dir, _config.configData._binaryFileMaxSize, _config.configData._minNumOfFilesToSend ) )
                            {
                                log.Debug( "Sending process begin" );
                                Thread thread = new Thread( () => SendLogs( dir ) );
                                thread.Start();

                                log.Debug( "thread created for " + dir.Key + " folder and started his operation" );
                                threadList.Add( thread );
                            }
                        }
                        foreach( Thread t in threadList )
                        {
                            t.Join();
                        }
                    }
                    else //server is offline
                    {
                        log.Debug( "Server is offline" );

                        foreach( KeyValuePair<string , DirectoryInfo> dir in _directory )
                        {
                            FolderWatcher.FolderOfflineWatcher( dir ,_config.configData._maxBinaryFolderSize );
                        }
                    }

                    log.Debug( "main thread going to sleep for " + _config.configData._threadSleepTime );
                    Thread.Sleep( _config.configData._threadSleepTime );
                }
            }

            catch( Exception ex )
            {
                log.Fatal( "Problem in thread creation" , ex );
                Thread.CurrentThread.Abort();
            }
        }

 

        /// <summary>
        /// 
        /// </summary>
        /// <param name="dir"></param>
        private async void SendLogs(KeyValuePair<string , DirectoryInfo> dir)
        {
            try
            {
                log.Debug( dir.Key + " Thread strating his sending process" );

                //multifile string - hold data from few file
                StringBuilder dataAsString = new StringBuilder();

                //start the parsing process on the folder
                List<FileInfo> listOfFileToDelete = ParsingBinaryFile.ParseFolder( dataAsString , dir , _config.configData._jsonDataMaxSize );

                log.Debug( "serialazation for " + dir.Key + " files started" );

                //get json data from multiple log files as one string
                string multipleLogFileAsjsonString = JsonDataConvertion.JsonSerialization( dataAsString );

                //gzip data
                byte[] compressedData = GZipCompresstion.CompressString( multipleLogFileAsjsonString );

                int retry = _config.configData._numberOfTimesToRetry;
                while( retry-- != 0 )
                {
                    if( await ServerConnection.SendDataToServer( _config.configData._hostIp , compressedData ) )
                    {
                        log.Info( "log sender sent " + listOfFileToDelete.Count + " files to the server and the server recived them" );
                        log.Debug( "begin deleteing the files that was sent to the server" );

                        FileMaintenance.FileDelete( listOfFileToDelete );

                        break;//when file sent sucessfuly exit while loop
                    }
                    Thread.Sleep( _config.configData._waitTimeBeforeRetry );
                }
            }
            catch( Exception ex )
            {
                log.Error( "Sending process had stoped",ex );
            }
        }


        /// <summary>
        /// This function is the thread operation function
        /// </summary>
        /// <param name="logType"></param>
        /// <param name="currDir"></param>
        //private async void LogSenderController(string logType , DirectoryInfo currDir)
        //{
        //    try
        //    {
        //        log.Debug( logType + " thread started" );
        //        while( true )
        //        {
        //            try
        //            {
        //                //thread sleep for _threadSleepTime miliseconds - configurable
        //                //Thread.Sleep( 2000 );

        //                if( currDir.EnumerateFiles( "*." + logType ).Where( f => ( f.Length <= _config.configData._binaryFileMaxSize ) && ( f.Length > 0 ) ).ToArray().Length > NUM_OF_FILES )
        //                {
        //                    log.Debug( logType + " Thread strating his sending process" );
        //                    //get files from directory
        //                    FileInfo[] files = currDir.GetFiles();

        //                    //multifile string - hold data from few file
        //                    StringBuilder dataAsString = new StringBuilder();

        //                    //add the csv header to output string
        //                    ParsingBinaryFile.AddOutputHeader( dataAsString );

        //                    //list of file name - hold file full name fot further action on the files like delete
        //                    List<string> listOfFileNames = new List<string>();

        //                    long fileSize = 0;

        //                    //loop on files in folder
        //                    foreach( FileInfo file in files )
        //                    {
        //                        try
        //                        {
        //                            //check if file is locked Write mode
        //                            if( FileMaintenance.IsFileLocked( file ) )
        //                            {
        //                                continue;
        //                            }

        //                            //parsing oparation
        //                            Table logTable = ParsingBinaryFile.Parse( file , logType );

        //                            if( logTable == null )
        //                            {
        //                                throw new Exception();
        //                            }

        //                            logTable.ConvertRowsToCsvFormat();

        //                            //Check if the current table will not cause size limit (configurable)
        //                            if( dataAsString.Length + logTable.GetDataSize() < _config.configData._jsonDataMaxSize )
        //                            {
        //                                dataAsString.Append( logTable.GetDataAsString() );
        //                            }
        //                            else
        //                            {
        //                                // save current table in some kind of stack and handle it next time we start sending
        //                                break;//break from file loop
        //                            }

        //                            //add file path to the list.for tracking which file should be deleted 
        //                            listOfFileNames.Add( file.FullName );


        //                        }
        //                        catch( Exception ex )
        //                        {
        //                            log.Error( "Problem While trying to read file -" + file.FullName , ex );
        //                        }
        //                    }


        //                    log.Debug( "serialazation for " + logType + " files started" );

        //                    //get json data from multiple log files as one string
        //                    string multipleLogFileAsjsonString = JsonDataConvertion.JsonSerialization( dataAsString );


        //                    //gzip data
        //                    byte[] compressedData = GZipCompresstion.CompressString( multipleLogFileAsjsonString );



        //                    //check if server is alive
        //                    //if alive
        //                    await ServerConnection.ConnectionController( _config.configData._hostIp , compressedData );

        //                    //if not alive 
        //                    //Check if this part should be before the parsing or after
        //                    //thread go to sleep for X second and check again.
        //                    //each time the thread awake he check the log folder for size. if the size exceded delete files
        //                    //If server is back, continue with previous sending process or start new parsing process with diffrent files.


        //                }

        //                //check with aviad. how to check the size before sending the data.
        //                //fileSize = System.Text.ASCIIEncoding.Unicode.GetByteCount(multipleLogFileAsjsonString);

        //                //compress string with gzip


        //                //send to server
        //                //SendToServer()

        //                //if server response was 202 delete the files
        //            }

        //            catch( Exception ex )
        //            {
        //                log.Error( "Error occored while sending file to server" , ex );
        //            }
        //        }
        //    }
        //    catch( Exception ex )
        //    {
        //        log.Fatal( logType + " Thread has stoped" , ex );
        //        Thread.CurrentThread.Abort();
        //    }
        //}


    }
}
