using BinaryFileToTextFile;
using BinaryFileToTextFile.Models;
using System;
using System.IO;
using System.Text;

namespace LogSender.Utilities
{
    abstract class ParsingBinaryFile
    {
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
    }
}
