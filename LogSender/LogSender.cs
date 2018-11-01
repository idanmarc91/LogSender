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

        private readonly DirectoryInfo _dirCybLogs;
        private readonly DirectoryInfo _dirFSA;
        private readonly DirectoryInfo _dirImages;
        private readonly DirectoryInfo _dirMOG;
        //private DirectoryInfo directoryRepository;
        //private DirectoryInfo directorySnapFreeze;

        private Thread _threadCyb;
        private Thread _threadFSA;
        private Thread _threadImages;
        private Thread _threadMOG;


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

            _dirCybLogs = new DirectoryInfo( path + "\\Packets" );
            _dirFSA = new DirectoryInfo( path + "\\FSAccess" );
            _dirImages = new DirectoryInfo( path + "\\Images" );
            _dirMOG = new DirectoryInfo( path + "\\Multievent" );

            log.Debug( "log sender class created" );
        }

        /// <summary>
        /// This function run main service. create threads
        /// </summary>
        public void RunService()
        {
            log.Debug( "Service thread for each folder creation started" );

            try
            {
                _threadCyb = new Thread( () => SendLogs( "cyb" , _dirCybLogs ) );
                _threadCyb.Start();

                //_threadFSA = new Thread( () => SendLogs( "fsa" , _dirFSA ) );
                //_threadFSA.Start();

                //_threadImages = new Thread( () => SendLogs( "cimg" , _dirImages ) );
                //_threadImages.Start();

                //_threadMOG = new Thread( () => SendLogs( "mog" , _dirMOG ) );
                //_threadMOG.Start();
            }
            catch( Exception ex )
            {
                log.Fatal( "Problem in thread creation" , ex );
                Thread.CurrentThread.Abort();
            }
        }

        /// <summary>
        /// This function is the thread operation function
        /// </summary>
        /// <param name="logType"></param>
        /// <param name="currDir"></param>
        private void SendLogs(string logType , DirectoryInfo currDir)
        {
            try
            {
                log.Debug( logType + " thread started" );
                while( true )
                {
                    try
                    {
                        //thread sleep for _threadSleepTime miliseconds - configurable
                        //Thread.Sleep( 2000 );

                        if( currDir.EnumerateFiles( "*." + logType ).Where( f => ( f.Length <= _config.configData._binaryFileMaxSize ) && ( f.Length > 0 ) ).ToArray().Length > NUM_OF_FILES )
                        {
                            log.Debug( logType + " Thread strating his sending process" );
                            //get files from directory
                            FileInfo[] files = currDir.GetFiles();

                            //multifile string - hold data from few file
                            StringBuilder dataAsString = new StringBuilder();

                            //add the csv header to output string
                            ParsingBinaryFile.AddOutputHeader( dataAsString );

                            //list of file name - hold file full name fot further action on the files like delete
                            List<string> listOfFileNames = new List<string>();

                            long fileSize = 0;

                            //loop on files in folder
                            foreach( FileInfo file in files )
                            {
                                try
                                {
                                    //check if file is locked Write mode
                                    if( FileMaintenance.IsFileLocked( file ) )
                                    {
                                        continue;
                                    }

                                    //add file path to the list.for tracking which file should be deleted 
                                    listOfFileNames.Add( file.FullName );

                                    //parsing oparation
                                    ParsingBinaryFile.Parse( file , dataAsString , logType );
                                }
                                catch(Exception ex)
                                {
                                    log.Error("Problem While trying to read file -" + file.FullName , ex );
                                }
                            }



                            log.Debug( "serialazation for " + logType + " files started" );
                            //get json data from multiple log files as one string
                            string multipleLogFileAsjsonString = JsonDataConvertion.JsonSerialization( dataAsString );

                        }
                        //check with aviad. how to check the size before sending the data.
                        //fileSize = System.Text.ASCIIEncoding.Unicode.GetByteCount(multipleLogFileAsjsonString);

                        //compress string with gzip


                        //send to server
                        //SendToServer()
                    }
                    
                    catch( Exception ex )
                    {
                        log.Error( "Error occored while sending file to server" , ex );
                    }
                }
            }
            catch( Exception ex )
            {
                log.Fatal( logType + " Thread has stoped" , ex );
                Thread.CurrentThread.Abort();
            }
        }
    }
}
