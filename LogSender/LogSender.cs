using BinaryFileToTextFile;
using BinaryFileToTextFile.Models;
using LogSender.Utilities;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace LogSender
{
    class LogSender
    {
        ///**********************************************
        ///             Members Section
        ///**********************************************
        ///
        private const int NUM_OF_FILES = 1;

        private DirectoryInfo _dirFlowLogs;
        private DirectoryInfo _dirFSA;
        private DirectoryInfo _dirImages;
        private DirectoryInfo _dirMOG;
        //private DirectoryInfo directoryRepository;
        //private DirectoryInfo directorySnapFreeze;

        private Thread _threadFlow;
        private Thread _threadFSA;
        private Thread _threadImages;
        private Thread _threadMOG;


        //Data from config file
        private int _jsonDataMaxSize;
        private int _binaryFileMaxSize;
        private int _threadSleepTime; //in miliseconds



        ///**********************************************
        ///             Functions Section
        ///**********************************************
        public LogSender(string path)
        {
            CfgFile(path);
            _dirFlowLogs = new DirectoryInfo(path + "\\Packets");
            _dirFSA = new DirectoryInfo(path + "\\FSAccess");
            _dirImages = new DirectoryInfo(path + "\\Images");
            _dirMOG = new DirectoryInfo(path+ "\\Multievent");
        }

        public void RunService()
        {
            _threadFlow = new Thread(() => SendLogs("cyb",_dirFlowLogs));
            _threadFlow.Start();
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

                    if (currDir.EnumerateFiles("*." + logType).Where(f => (f.Length <= _binaryFileMaxSize) && (f.Length > 0)).ToArray().Length > NUM_OF_FILES)
                    {
                        FileInfo[] files = currDir.GetFiles();

                        //json array - multifile json log - hold data from few file
                        List<JsonLog> jsonArray = new List<JsonLog>();

                        //list of file name - hold file full name fot further action on the files like delete
                        List<string> listOfFileNames = new List<string>();

                        //loop on files in folder
                        foreach (FileInfo file in files)
                        {
                            //check if file is locked Write mode
                            if (FileMaintenance.IsFileLocked(file))
                                continue;

                            //add file path to the list.for tracking which file should be deleted 
                            listOfFileNames.Add(file.FullName);

                            //parsing oparation
                            ParseBinaryFile(file.FullName, jsonArray, logType);

                        }
                        string jsonString = JsonDataConvertion.JsonSerialization(jsonArray);
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

        /// <summary>
        /// This function is parsing the binary file.
        /// There are 4 different kind of parsing methods
        /// </summary>
        /// <param name="FilePath"></param>
        /// <param name="jsonArray"></param>
        /// <param name="logType"></param>
        private void ParseBinaryFile(string FilePath, List<JsonLog> jsonArray, string logType)
        {
            try
            {
                BinaryFileHandler bFile = new BinaryFileHandler(FilePath);

                if (!bFile.IsFileNull())
                {
                    //preprocessing file
                    BinaryFileData data = bFile.SeparateHeaderAndFile();

                    //expend log section
                    byte[] expandedFileByteArray = data.ExpendLogSection();

                    //get header parameters
                    HeaderParameters headerParameters = new HeaderParameters();
                    headerParameters.ExtractData(data._headerArray);

                    Table logTable = null;

                    //Build data table from binary file
                    switch (logType)
                    {
                        case "cyb":
                            logTable = new CybTable(expandedFileByteArray, headerParameters._hostName.GetData(), headerParameters._serverClientDelta.GetData(), headerParameters._version.GetData());
                            break;

                        case "fsa":
                            logTable = new FSATable(expandedFileByteArray, headerParameters._hostName.GetData(), headerParameters._serverClientDelta.GetData(), headerParameters._version.GetData());
                            break;

                        case "mog":
                            logTable = new MogTable(expandedFileByteArray, headerParameters._hostName.GetData(), headerParameters._serverClientDelta.GetData(), headerParameters._version.GetData());
                            break;

                        case "cimg":
                            logTable = new DLLTable(expandedFileByteArray, headerParameters._hostName.GetData(), headerParameters._serverClientDelta.GetData());
                            break;
                    }

                    //check if log table was created in the switch case above
                    if (logTable != null)
                        logTable.GetAsJson(jsonArray);
                    else
                        throw new Exception("log type string error. log table was not created");
                }
                else
                    throw new Exception("File Is Null");
            }
            catch (Exception ex)
            {
                System.IO.StreamWriter logFile = new System.IO.StreamWriter(AppDomain.CurrentDomain.BaseDirectory + "log.txt", true);
                logFile.WriteLine("problem with " + FilePath + " in parsing function" + ex.Message);
                logFile.Close();
            }
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="path"></param>
        public void CfgFile(string path)
        {
            try
            {
                //create config file if not exist
                if (!(System.IO.File.Exists(path + "\\Log Sender Configuration.cfg")))
                    System.IO.File.WriteAllText(path + "\\Log Sender Configuration.cfg", CreateCfgFile());

                //Read Log Sender Configuration file
                string[] lineArr = System.IO.File.ReadAllLines(path + "\\Log Sender Configuration.cfg");

                int startOffset;

                //Check lines in cfg file
                foreach (string line in lineArr)
                {
                    if (line.Contains("json_data_max_size="))
                    {
                        try
                        {
                            startOffset = 19;
                            if (line.Contains("#"))
                            {
                                _jsonDataMaxSize = int.Parse(line.Substring(startOffset, line.IndexOf('#') - startOffset).Trim());
                            }
                            else
                                _jsonDataMaxSize = int.Parse(line.Substring(startOffset, line.Length - startOffset));
                        }
                        catch
                        {
                            _jsonDataMaxSize = 10000000;
                        }
                    }

                    if (line.Contains("binary_file_max_size="))
                    {
                        try
                        {
                            startOffset = 21;
                            if (line.Contains("#"))
                            {
                                _binaryFileMaxSize = int.Parse(line.Substring(startOffset, line.IndexOf('#') - startOffset).Trim());
                            }
                            else
                                _binaryFileMaxSize = int.Parse(line.Substring(startOffset, line.Length - startOffset));
                        }
                        catch
                        {
                            _binaryFileMaxSize = 6291456; // 6 MB
                        }
                    }

                    if (line.Contains("send_file_time="))
                    {
                        try
                        {
                            startOffset = 15;
                            if (line.Contains("#"))
                            {
                                _threadSleepTime = int.Parse(line.Substring(startOffset, line.IndexOf('#') - startOffset).Trim());
                            }
                            else
                                _threadSleepTime = int.Parse(line.Substring(startOffset, line.Length - startOffset));
                        }
                        catch
                        {
                            _threadSleepTime = 60000; // 60 Seconds by Default
                        }
                    }
                }

            }
            catch (Exception ex)
            {
                //TODO:Create logger
                System.IO.StreamWriter logFile = new System.IO.StreamWriter(AppDomain.CurrentDomain.BaseDirectory + "log.txt", true);
                logFile.WriteLine("Configuration File Error \n" + ex.Message);
                logFile.Close();
            }



        }

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
#		represents one value: 'A B'." + Environment.NewLine;


            strConfig += "[Options]" + Environment.NewLine;

            strConfig += "json_data_max_size=10000000 #max size of json data string before sending to server" + Environment.NewLine;

            strConfig += "binary_file_max_size=6291456 #max size of binary file - if file exceeded this value the log sender will not send it to the server" + Environment.NewLine;

            strConfig += "send_file_time=60000 #every 'value' miliseconds the log sender will check the log folders for new files" + Environment.NewLine;

            return strConfig;
        }
    }
}
