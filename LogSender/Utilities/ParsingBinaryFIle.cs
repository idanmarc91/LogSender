using BinaryFileToTextFile;
using BinaryFileToTextFile.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace LogSender.Utilities
{
    abstract class ParsingBinaryFile
    {
        ///**********************************************
        ///             Functions Section
        ///**********************************************
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger( "ParsingBinaryFile.cs" );

        /// <summary>
        /// This function is parsing the binary file.
        /// There are 4 different kind of parsing methods
        /// </summary>
        /// <param name="FilePath"></param>
        /// <param name="logType"></param>
        public static Table Parse(FileInfo file , string logType)
        {
            try
            {
                BinaryFileHandler bFile = new BinaryFileHandler( file );
                Table logTable = null;

                if( !bFile.IsFileNull() )
                {
                    log.Debug( file.FullName + " started his parsing process" );
                    //preprocessing file
                    BinaryFileData data = bFile.SeparateHeaderAndFile();

                    //expend log section
                    byte[] expandedFileByteArray = data.ExpendLogSection();

                    //get header parameters
                    HeaderParameters headerParameters = new HeaderParameters();
                    headerParameters.ExtractData( data._headerArray );


                    //Build data table from binary file
                    switch( logType )
                    {
                        case "cyb":
                            logTable = new CybTable( expandedFileByteArray , headerParameters._hostName.GetData() , headerParameters._serverClientDelta.GetData() , headerParameters._version.GetData() );
                            break;

                        case "fsa":
                            logTable = new FsaTable( expandedFileByteArray , headerParameters._hostName.GetData() , headerParameters._serverClientDelta.GetData() , headerParameters._version.GetData() );
                            break;

                        case "mog":
                            logTable = new MogTable( expandedFileByteArray , headerParameters._hostName.GetData() , headerParameters._serverClientDelta.GetData() , headerParameters._version.GetData() );
                            break;

                        case "cimg":
                            logTable = new DLLTable( expandedFileByteArray , headerParameters._hostName.GetData() , headerParameters._serverClientDelta.GetData() );
                            break;
                    }
                }
                else
                {
                    throw new Exception( "File Is Null" );
                }

                //check if log table was created in the switch case above
                if( logTable != null )
                {
                    log.Debug( file.FullName + " binary file has finished his parsing process" );
                    return logTable;
                }
                else
                {
                    throw new Exception( "log type string error. log table was not created" );
                }
            }
            catch( Exception ex )
            {
                log.Error( "Problem in Parsing process with  " + file.FullName , ex );
                return null;
            }
        }

        /// <summary>
        /// This funcion add data header for output string
        /// </summary>
        /// <param name="dataAsString"></param>
        public static void AddOutputHeader(StringBuilder dataAsString)
        {
            dataAsString.Append( "OS," +
                "HostName" +
                "," +
                "ClientTime" +
                "," +
                "FullServerTime" +
                "," +
                "ProcessID" +
                "," +
                "ProcessName" +
                "," +
                "ProcessPath" +
                "," +
                "ApplicationName" +
                "," +
                "Protocol" +
                "," +
                "Status" +
                "," +
                "SourcePort" +
                "," +
                "DestinationPort" +
                "," +
                "Direction" +
                "," +
                "FilePath" +
                "," +
                "XCast" +
                "," +
                "State" +
                "," +
                "SourceIP" +
                "," +
                "DestinationIP" +
                "," +
                "SequanceNumber" +
                "," +
                "SubSequanceNumber" +
                "," +
                "UserName" +
                "," +
                "MogCounter" +
                "," +
                "DestinationPath" +
                "," +
                "Reason" +
                "," +
                "ImagePath" +
                "," +
                "ImageName" +
                "," +
                "ParentPath" +
                "," +
                "ParentName" +
                ",ChainArray" );
            dataAsString.Append( "\n " );
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="dataAsString"></param>
        /// <param name="files"></param>
        /// <returns></returns>
        public static List<FileInfo> ParseFolder(StringBuilder dataAsString , KeyValuePair<string , DirectoryInfo> dir , int configJsonMaxDataSize )
        {
            log.Debug( dir.Value.Name + "folder started his parsing process" );

            AddOutputHeader( dataAsString );
            //list of file name - hold file for further action on the files like delete
            List<FileInfo> listOfFileToDelete = new List<FileInfo>();

            //loop on files in folder
            foreach( FileInfo file in dir.Value.GetFiles() )
            {
                try
                {
                    //check if file is locked Write mode
                    if( FileMaintenance.IsFileLocked( file ) )
                    {
                        continue;
                    }

                    //parsing oparation
                    Table logTable = ParsingBinaryFile.Parse( file , dir.Key );

                    if( logTable == null )
                    {
                        throw new Exception();
                    }

                    logTable.ConvertRowsToCsvFormat();

                    //Check if the current table will not cause size limit (configurable)
                    if( dataAsString.Length + logTable.GetDataSize() < configJsonMaxDataSize )
                    {
                        dataAsString.Append( logTable.GetDataAsString() );
                    }
                    else
                    {
                        // save current table in some kind of stack and handle it next time we start sending
                        break;//break from file loop
                    }

                    //add file path to the list.for tracking which file should be deleted 
                    listOfFileToDelete.Add( file );


                }
                catch( Exception ex )
                {
                    log.Error( "Problem While trying to read file -" + file.FullName , ex );
                }
            }

            return listOfFileToDelete;

        }
    }
}
