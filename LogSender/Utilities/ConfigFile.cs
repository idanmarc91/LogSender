using LogSender.Data;
using System;

namespace LogSender.Utilities
{
    public class ConfigFile
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger( "ConfigFile.cs" );

        public ConfigData configData = new ConfigData();

        /// <summary>
        /// This function read from configuration data
        /// </summary>
        /// <param name="path"></param>
        public void CfgFile(string path)
        {
            try
            {
                //create config file if not exist
                if( !( System.IO.File.Exists( path + "\\Log Sender Configuration.cfg" ) ) )
                {
                    System.IO.File.WriteAllText( path + "\\Log Sender Configuration.cfg" , CreateCfgFile() );
                    log.Debug( "Log Sender Configuration file is created" );
                }
                else
                {
                    log.Debug( "Log Sender Configuration file exist" );
                }


                //Read Log Sender Configuration file
                string[] lineArr = System.IO.File.ReadAllLines( path + "\\Log Sender Configuration.cfg" );

                int startOffset;

                //Check lines in cfg file
                foreach( string line in lineArr )
                {
                    if( line.Contains( "json_data_max_size=" ) )
                    {
                        try
                        {
                            startOffset = 19;
                            if( line.Contains( "#" ) )
                            {
                                configData._jsonDataMaxSize = int.Parse( line.Substring( startOffset , line.IndexOf( '#' ) - startOffset ).Trim() );
                            }
                            else
                            {
                                configData._jsonDataMaxSize = int.Parse( line.Substring( startOffset , line.Length - startOffset ) );
                            }
                        }
                        catch( Exception )
                        {
                            configData._jsonDataMaxSize = 10000000;
                        }
                    }
                    if( line.Contains( "binary_file_max_size=" ) )
                    {
                        try
                        {
                            startOffset = 21;
                            if( line.Contains( "#" ) )
                            {
                                configData._binaryFileMaxSize = int.Parse( line.Substring( startOffset , line.IndexOf( '#' ) - startOffset ).Trim() );
                            }
                            else
                            {
                                configData._binaryFileMaxSize = int.Parse( line.Substring( startOffset , line.Length - startOffset ) );
                            }
                        }
                        catch( Exception )
                        {
                            configData._binaryFileMaxSize = 6291456; // 6 MB
                        }
                    }
                    if( line.Contains( "sleep_time_cycle=" ) )
                    {
                        try
                        {
                            startOffset = 17;
                            if( line.Contains( "#" ) )
                            {
                                configData._threadSleepTime = int.Parse( line.Substring( startOffset , line.IndexOf( '#' ) - startOffset ).Trim() );
                            }
                            else
                            {
                                configData._threadSleepTime = int.Parse( line.Substring( startOffset , line.Length - startOffset ) );
                            }
                        }
                        catch( Exception )
                        {
                            configData._threadSleepTime = 60000; // 60 Seconds by Default
                        }
                    }
                    if( line.Contains( "host_ip=" ) )
                    {
                        try
                        {
                            startOffset = 8;
                            if( line.Contains( "#" ) )
                            {
                                configData._hostIp =  line.Substring( startOffset , line.IndexOf( '#' ) - startOffset ).Trim() ;
                            }
                            else
                            {
                                configData._hostIp =  line.Substring( startOffset , line.Length - startOffset ) ;
                            }
                        }
                        catch( Exception )
                        {
                            configData._hostIp = "http://10.0.0.174:8080"; // 60 Seconds by Default
                        }
                    }
                    if( line.Contains( "minimum_number_of_file_to_send=" ) )
                    {
                        try
                        {
                            startOffset = 31;
                            if( line.Contains( "#" ) )
                            {
                                configData._minNumOfFilesToSend = int.Parse( line.Substring( startOffset , line.IndexOf( '#' ) - startOffset ).Trim() );
                            }
                            else
                            {
                                configData._minNumOfFilesToSend = int.Parse( line.Substring( startOffset , line.Length - startOffset ) );
                            }
                        }
                        catch( Exception )
                        {
                            configData._minNumOfFilesToSend = 2; 
                        }
                    }
                    if( line.Contains( "number_of_times_to_retry_sending=" ) )
                    {
                        try
                        {
                            startOffset = 33;
                            if( line.Contains( "#" ) )
                            {
                                configData._numberOfTimesToRetry = int.Parse( line.Substring( startOffset , line.IndexOf( '#' ) - startOffset ).Trim() );
                            }
                            else
                            {
                                configData._numberOfTimesToRetry = int.Parse( line.Substring( startOffset , line.Length - startOffset ) );
                            }
                        }
                        catch( Exception )
                        {
                            configData._numberOfTimesToRetry = 5;
                        }
                    }
                    if( line.Contains( "wait_time_before_retry=" ) )
                    {
                        try
                        {
                            startOffset = 23;
                            if( line.Contains( "#" ) )
                            {
                                configData._waitTimeBeforeRetry = int.Parse( line.Substring( startOffset , line.IndexOf( '#' ) - startOffset ).Trim() );
                            }
                            else
                            {
                                configData._waitTimeBeforeRetry = int.Parse( line.Substring( startOffset , line.Length - startOffset ) );
                            }
                        }
                        catch( Exception )
                        {
                            configData._waitTimeBeforeRetry = 2000;
                        }
                    }
                    if( line.Contains( "max_binary_folder_size=" ) )
                    {
                        try
                        {
                            startOffset = 23;
                            if( line.Contains( "#" ) )
                            {
                                configData._maxBinaryFolderSize = long.Parse( line.Substring( startOffset , line.IndexOf( '#' ) - startOffset ).Trim() );
                            }
                            else
                            {
                                configData._maxBinaryFolderSize = long.Parse( line.Substring( startOffset , line.Length - startOffset ) );
                            }
                        }
                        catch( Exception )
                        {
                            configData._maxBinaryFolderSize = 104857600;
                        }
                    }

                }
            }
            catch( Exception ex )
            {
                log.Fatal( "Fatal error config file in creation or reading process" , ex );
            }
        }


        /// <summary>
        /// This Function create configuration file with default values if not exist
        /// </summary>
        /// <returns></returns>
        private static string CreateCfgFile()
        {
            string strConfig;

            //instruction 
            strConfig = @"# Instructions
# ============
#	1. The '#' character is used for comments.
#	2. You can use comments everywhere in the file. The text after '#' won't be considered as a configuration.
#	3. Lines that are not comments shouldn't start with spaces, tabs and etc...
#	4. In Key=Value lines, don't put spaces or tabs between the key, the '=' sign and the value.
#	5. List items are separated by 'enter' only. Example:
#			[Example]
#			A
#			B
#		represtants two values: 'A' and 'B'.
#			[Example]
#			A B
#		represents one value: 'A B'." + Environment.NewLine;


            strConfig += "[Options]" + Environment.NewLine;

            strConfig += "json_data_max_size=10000000 #max size of json data string before sending to server" + Environment.NewLine;

            strConfig += "binary_file_max_size=6291456 #max size of binary file - if file exceeded this value the log sender will not send it to the server" + Environment.NewLine;

            strConfig += "sleep_time_cycle=60000 #every 'value' milliseconds the log sender will check the log folders for new files" + Environment.NewLine;

            strConfig += "host_ip=http://10.0.0.174:8080 #The server IP address" + Environment.NewLine;

            strConfig += "minimum_number_of_file_to_send=2 #The minimum number of binary files in a folder to start sending process (sending trigger)" + Environment.NewLine;

            strConfig += "number_of_times_to_retry_sending=10 #The number of time the log sender will try to send the data to the server " + Environment.NewLine;

            strConfig += "wait_time_before_retry=2000 #time in milliseconds that the sending process pause before sending the data to the server(if first sending failed)" + Environment.NewLine;

            strConfig += "max_binary_folder_size=10485 #Maximum Size of binary folder. when the server is offline the binary files are pileing up in the folder so this limit the number of files." + Environment.NewLine;

            

            return strConfig;
        }
    }
}
