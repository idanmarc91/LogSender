﻿using LogSender.Data;
using System;

namespace LogSender.Utilities
{
    public class ConfigFile
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger("ConfigFile.cs");

        public ConfigData _configData = new ConfigData();


        /// <summary>
        /// This function read from configuration data
        /// </summary>
        /// <returns>bool - true if config file reading process succeded </returns>
        public bool ReadConfigFile()
        {
            try
            {
                string path = AppDomain.CurrentDomain.BaseDirectory;

                //create config file if not exist
                if (!(System.IO.File.Exists(path + "\\Log Sender Configuration.cfg")))
                {
                    System.IO.File.WriteAllText(path + "\\Log Sender Configuration.cfg", CreateCfgFile());
                    log.Debug("Log Sender Configuration file is created");
                }
                else
                {
                    log.Debug("Log Sender Configuration file exist");
                }

                int startOffset;

<<<<<<< HEAD
                #region Read from LogSender config file
=======
                //Read Log Sender Configuration file
                string[] lineArr = System.IO.File.ReadAllLines(path + "\\Log Sender Configuration.cfg");
>>>>>>> parent of 80e4aa2... stable version for x86 and x64

                //Read Log Sender Configuration file
                string[] lineArr = System.IO.File.ReadAllLines(path + "\\Log Sender Configuration.cfg");

                //Check lines in cfg file
                foreach (string line in lineArr)
                {
                    if (line.Contains("cyb Folder Path="))
                    {
                        try
                        {
                            startOffset = 16;
                            if (line.Contains("#"))
                            {
                                _configData._cybFolderPath = line.Substring(startOffset, line.IndexOf('#') - startOffset).Trim();
                            }
                            else
                            {
                                _configData._cybFolderPath = line.Substring(startOffset, line.Length - startOffset);
                            }
                        }
                        catch (Exception)
                        {
                            _configData._cybFolderPath = "C:\\Program Files\\Cyber 2.0\\Cyber 2.0 Agent\\Packets";
                        }
                    }
                    if (line.Contains("fsa Folder Path="))
                    {
                        try
                        {
                            startOffset = 16;
                            if (line.Contains("#"))
                            {
                                _configData._fsaFolderPath = line.Substring(startOffset, line.IndexOf('#') - startOffset).Trim();
                            }
                            else
                            {
                                _configData._fsaFolderPath = line.Substring(startOffset, line.Length - startOffset);
                            }
                        }
                        catch (Exception)
                        {
                            _configData._fsaFolderPath = "C:\\Program Files\\Cyber 2.0\\Cyber 2.0 Agent\\FSAccess";
                        }
                    }
                    if (line.Contains("cimg Folder Path="))
                    {
                        try
                        {
                            startOffset = 17;
                            if (line.Contains("#"))
                            {
                                _configData._cimgFolderPath = line.Substring(startOffset, line.IndexOf('#') - startOffset).Trim();
                            }
                            else
                            {
                                _configData._cimgFolderPath = line.Substring(startOffset, line.Length - startOffset);
                            }
                        }
                        catch (Exception)
                        {
                            _configData._cimgFolderPath = "C:\\Program Files\\Cyber 2.0\\Cyber 2.0 Agent\\Images";
                        }
                    }
                    if (line.Contains("mog Folder Path="))
                    {
                        try
                        {
                            startOffset = 16;
                            if (line.Contains("#"))
                            {
                                _configData._mogFolderPath = line.Substring(startOffset, line.IndexOf('#') - startOffset).Trim();
                            }
                            else
                            {
                                _configData._mogFolderPath = line.Substring(startOffset, line.Length - startOffset);
                            }
                        }
                        catch (Exception)
                        {
                            _configData._mogFolderPath = "C:\\Program Files\\Cyber 2.0\\Cyber 2.0 Agent\\Multievent";
                        }
                    }
                    if (line.Contains("json_data_max_size="))
                    {
                        try
                        {
                            startOffset = 19;
                            if (line.Contains("#"))
                            {
                                _configData._jsonDataMaxSize = long.Parse(line.Substring(startOffset, line.IndexOf('#') - startOffset).Trim());
                            }
                            else
                            {
                                _configData._jsonDataMaxSize = long.Parse(line.Substring(startOffset, line.Length - startOffset));
                            }
                        }
                        catch (Exception)
                        {
                            _configData._jsonDataMaxSize = 10000000;
                        }
                    }
                    if (line.Contains("binary_file_max_size="))
                    {
                        try
                        {
                            startOffset = 21;
                            if (line.Contains("#"))
                            {
                                _configData._binaryFileMaxSize = long.Parse(line.Substring(startOffset, line.IndexOf('#') - startOffset).Trim());
                            }
                            else
                            {
                                _configData._binaryFileMaxSize = long.Parse(line.Substring(startOffset, line.Length - startOffset));
                            }
                        }
                        catch (Exception)
                        {
                            _configData._binaryFileMaxSize = 6291456; // 6 MB
                        }
                    }
                    if (line.Contains("sleep_time_cycle="))
                    {
                        try
                        {
                            startOffset = 17;
                            if (line.Contains("#"))
                            {
                                _configData._threadSleepTime = int.Parse(line.Substring(startOffset, line.IndexOf('#') - startOffset).Trim());
                            }
                            else
                            {
                                _configData._threadSleepTime = int.Parse(line.Substring(startOffset, line.Length - startOffset));
                            }
                        }
                        catch (Exception)
                        {
                            _configData._threadSleepTime = 60000; // 60 Seconds by Default
                        }
                    }
                    if (line.Contains("host_port="))
                    {
                        try
                        {
                            startOffset = 10;
                            if (line.Contains("#"))
                            {
                                _configData._hostPort = line.Substring(startOffset, line.IndexOf('#') - startOffset).Trim();
                            }
                            else
                            {
                                _configData._hostPort = line.Substring(startOffset, line.Length - startOffset);
                            }
                        }
                        catch (Exception)
                        {
                            _configData._hostPort = ""; // default host port
                        }
                    }
                    if (line.Contains("max_binary_folder_size="))
                    {
                        try
                        {
                            startOffset = 23;
                            if (line.Contains("#"))
                            {
                                _configData._maxBinaryFolderSize = long.Parse(line.Substring(startOffset, line.IndexOf('#') - startOffset).Trim());
                            }
                            else
                            {
                                _configData._maxBinaryFolderSize = long.Parse(line.Substring(startOffset, line.Length - startOffset));
                            }
                        }
                        catch (Exception)
                        {
                            _configData._maxBinaryFolderSize = 104857600;
                        }
                    }
                    if (line.Contains("minimum_number_of_file_to_send="))
                    {
                        try
                        {
                            startOffset = 31;
                            if (line.Contains("#"))
                            {
                                _configData._minNumOfFilesToSend = int.Parse(line.Substring(startOffset, line.IndexOf('#') - startOffset).Trim());
                            }
                            else
                            {
                                _configData._minNumOfFilesToSend = int.Parse(line.Substring(startOffset, line.Length - startOffset));
                            }
                        }
                        catch (Exception)
                        {
                            _configData._minNumOfFilesToSend = 2;
                        }
                    }
                    if (line.Contains("wait_time_before_retry="))
                    {
                        try
                        {
                            startOffset = 23;
                            if (line.Contains("#"))
                            {
                                _configData._waitTimeBeforeRetry = int.Parse(line.Substring(startOffset, line.IndexOf('#') - startOffset).Trim());
                            }
                            else
                            {
                                _configData._waitTimeBeforeRetry = int.Parse(line.Substring(startOffset, line.Length - startOffset));
                            }
                        }
                        catch (Exception)
                        {
                            _configData._waitTimeBeforeRetry = 2000;
                        }
                    }
                    if (line.Contains("number_of_times_to_retry_sending="))
                    {
                        try
                        {
                            startOffset = 33;
                            if (line.Contains("#"))
                            {
                                _configData._numberOfTimesToRetry = int.Parse(line.Substring(startOffset, line.IndexOf('#') - startOffset).Trim());
                            }
                            else
                            {
                                _configData._numberOfTimesToRetry = int.Parse(line.Substring(startOffset, line.Length - startOffset));
                            }
                        }
                        catch (Exception)
                        {
                            _configData._numberOfTimesToRetry = 5;
                        }
                    }
                }
                #endregion


#if DEBUG
                _configData._hostIp = "http://10.0.0.40:8080";
#else
                #region Read from Agent config file

                log.Info("reading host ip from Agent config file");

                string[] AgentCfgFile = System.IO.File.ReadAllLines(path + @"\..\Config.cfg");
                string tempIp;
                foreach (string line in AgentCfgFile)
                {
                    if (line.Contains("host="))
                    {
                        startOffset = 5;

                        if (line.Contains("#"))
                        {
                            tempIp = line.Substring(startOffset, line.IndexOf('#') - startOffset).Trim();
                        }
                        else
                        {
                            tempIp = line.Substring(startOffset, line.Length - startOffset);
                        }
                        _configData._hostIp = "http://" + tempIp +":" + _configData._hostPort;
                    }
                }
                #endregion
                if (_configData._hostIp == string.Empty)
                    log.Error("No host ip");
#endif


                return true;
            }
            catch (Exception ex)
            {
                log.Fatal("Fatal error config file in creation or reading process", ex);
                return false;
            }
        }


        /// <summary>
        /// This Function create configuration file with default values if not exist
        /// </summary>
        /// <returns></returns>
        private string CreateCfgFile()
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
#		represents one value: 'A B'." + Environment.NewLine + Environment.NewLine;

            strConfig += "[Options]" + Environment.NewLine + Environment.NewLine;

            strConfig += "cyb Folder Path=C:\\Program Files\\Cyber 2.0\\Cyber 2.0 Agent\\Packets #The folder path of cyb logs" + Environment.NewLine + Environment.NewLine;

            strConfig += "fsa Folder Path=C:\\Program Files\\Cyber 2.0\\Cyber 2.0 Agent\\FSAccess #The folder path of fsa logs" + Environment.NewLine + Environment.NewLine;

            strConfig += "cimg Folder Path=C:\\Program Files\\Cyber 2.0\\Cyber 2.0 Agent\\Images #The folder path of cimg logs" + Environment.NewLine + Environment.NewLine;

            strConfig += "mog Folder Path=C:\\Program Files\\Cyber 2.0\\Cyber 2.0 Agent\\Multievent #The folder path of mog logs" + Environment.NewLine + Environment.NewLine;

            strConfig += "json_data_max_size=5000000 #max size of json data string before sending to server" + Environment.NewLine + Environment.NewLine;

            strConfig += "binary_file_max_size=6291456 #max size of binary file - if file exceeded this value the log sender will not send it to the server" + Environment.NewLine + Environment.NewLine;

            strConfig += "sleep_time_cycle=60000 #every 'value' milliseconds the log sender will check the log folders for new files" + Environment.NewLine + Environment.NewLine;

            strConfig += "host_port=8080 #The server port number" + Environment.NewLine + Environment.NewLine;

            strConfig += "minimum_number_of_file_to_send=2 #The minimum number of binary files in a folder to start sending process (sending trigger)" + Environment.NewLine + Environment.NewLine;

            strConfig += "number_of_times_to_retry_sending=10 #The number of time the log sender will try to send the data to the server " + Environment.NewLine + Environment.NewLine;

            strConfig += "wait_time_before_retry=2000 #time in milliseconds that the sending process pause before sending the data to the server(if first sending failed)" + Environment.NewLine + Environment.NewLine;

            strConfig += "max_binary_folder_size=104857600 #Maximum Size of binary folder. when the server is offline the binary files are pileing up in the folder so this limit the number of files." + Environment.NewLine + Environment.NewLine;

            return strConfig;
        }
    }
}
