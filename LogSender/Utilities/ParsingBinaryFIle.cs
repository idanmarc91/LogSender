using LogSender.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace LogSender.Utilities
{
    public abstract class ParsingBinaryFile
    {
        ///**********************************************
        ///             Members Section
        ///**********************************************
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger("ParsingBinaryFile.cs");
        private static readonly List<string> _csvHeader = new List<string>
        {
            "os",
            "reporting_computer",
            "client_time" ,
            "full_server_time" ,
            "process_id" ,
            "process_name" ,
            "process_path" ,
            "protocol" ,
            "status" ,
            "source_port" ,
            "destination_port" ,
            "direction" ,
            "cast_type",
            "scramble_state" ,
            "source_ip" ,
            "destination_ip" ,
            "sequance_number" ,
            "sub_sequance_number" ,
            "user_name" ,
            "mog_counter" ,
            "destination_path" ,
            "reason" ,
            "dll_path" ,
            "dll_name" ,
            "parent_path" ,
            "parent_name" ,
            "chain_array"
        };

        ///**********************************************
        ///             Functions Section
        ///**********************************************
        /// <summary>
        /// This function is parsing the binary file.
        /// There are 4 different kind of parsing methods
        /// </summary>
        /// <param name="FilePath"></param>
        /// <param name="logType"></param>
        public static Table Parse(FileInfo file, string logType)
        {
            try
            {
                BinaryFileHandler bFile = new BinaryFileHandler(file);
                Table logTable = null;

                if (!bFile.IsFileNull())
                {
                    log.Debug(file.Name + " log file started his parsing process");
                    //preprocessing file
                    BinaryFileData data = bFile.SeparateHeaderAndFile();

                    //expend log section
                    byte[] expandedFileByteArray = data.ExpendLogSection();

                    //get header parameters
                    HeaderParameters headerParameters = new HeaderParameters();
                    headerParameters.ExtractData(data._headerArray);

                    //Build data table from binary file
                    switch (logType)
                    {
                        case "cyb":
                            logTable = new CybTable(expandedFileByteArray, headerParameters._reportingComputer.GetData(), headerParameters._serverClientDelta.GetData(), headerParameters._version.GetData());
                            break;

                        case "fsa":
                            string sourceIP = ServerConnection.GetLocalIPAddress();
                            logTable = new FsaTable(expandedFileByteArray, headerParameters._reportingComputer.GetData(), headerParameters._serverClientDelta.GetData(), headerParameters._version.GetData());
                            break;

                        case "mog":
                            logTable = new MogTable(expandedFileByteArray, headerParameters._reportingComputer.GetData(), headerParameters._serverClientDelta.GetData(), headerParameters._version.GetData());
                            break;

                        case "cimg":
                            logTable = new DLLTable(expandedFileByteArray, headerParameters._reportingComputer.GetData(), headerParameters._serverClientDelta.GetData());
                            break;
                    }
                }
                else
                {
                    throw new Exception("File Is Null");
                }

                //check if log table was created in the switch case above
                if (logTable != null)
                {
                    log.Debug(file.Name + " log file has successfuly finished his parsing process");
                    return logTable;
                }
                else
                {
                    throw new Exception("Problem with table creation. log table was not created");
                }
            }
            catch (Exception ex)
            {
                log.Error("Problem in Parsing process with  " + file.Name, ex);
                return null;
            }
        }

        /// <summary>
        /// This funcion add data header for output string
        /// </summary>
        /// <param name="dataAsString"></param>
        public static void AddOutputHeader(StringBuilder dataAsString)
        {
            dataAsString.Append(ServiceStack.Text.CsvSerializer.SerializeToCsv(_csvHeader));
        }


        /// <summary>
        /// This function parse the binary logs inside current folder
        /// </summary>
        /// <param name="dataAsString"></param>
        /// <param name="files"></param>
        /// <returns>list<fileifno> - files that was finish parsing process and should be deleted</returns>
        public static List<FileInfo> ParseFolder(StringBuilder dataAsString,
                                                 KeyValuePair<string, DirectoryInfo> directory)
        {
            log.Debug(directory.Value.Name + "folder started his parsing process");

            //add csv format data headers line
            AddOutputHeader(dataAsString);

            //list of file name - hold file for further action on the files like delete
            List<FileInfo> listOfFileToDelete = new List<FileInfo>();

            //loop on files in folder
            foreach (FileInfo file in FileSorting.GetAllFilesByConfigSettings(directory))
            {
                try
                {
                    //check if file is locked Write mode or empty
                    if (FileMaintenance.IsFileLocked(file) || file.Length == 0)
                    {
                        continue;
                    }

                    //parsing oparation
                    Table logTable = ParsingBinaryFile.Parse(file, directory.Key);

                    if (logTable == null)
                    {
                        log.Error("parsing process failed, log table is null. file \"" + file.Name + "\" skiped");
                        throw new Exception();
                    }

                    logTable.ConvertRowsToCsvFormat();

                    //Check if the current table will not cause size limit (configurable)
                    //if dataAsString reached size limit stop parsing the folder and send the data
                    if (dataAsString.Length + logTable.GetDataSize() < ConfigFile.Instance._configData._jsonDataMaxSize || listOfFileToDelete.Count == 0)
                    {
                        dataAsString.Append(logTable.GetDataAsString());
                    }
                    else
                    {
                        break;//break from file loop
                    }

                    //add file to the list.for tracking which file should be deleted 
                    listOfFileToDelete.Add(file);
                }
                catch (Exception ex)
                {
                    log.Error("Problem While trying to read file -" + file.Name, ex);
                }
            }
            log.Debug(directory.Value.Name + "folder finished his parsing process");

            return listOfFileToDelete;
        }
    }
}