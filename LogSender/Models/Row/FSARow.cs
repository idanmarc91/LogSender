using LogSender.Data;
using LogSender.Utilities;
using System;
using System.Collections.Generic;
using System.Text;

namespace LogSender.Models
{
    internal class FSARow : LogRow
    {
        ///**********************************************
        ///             Members Section
        ///**********************************************

        #region Member section

        private enum _fileExtractDataIndexs
        {
            PROCESS_ID, PROCESS_NAME, PROCESS_PATH, DESTINATION_PATH, USER_NAME, STATUS, REASON,
            SEQ_NUM, SUB_SEQ_NUM
        };

        private readonly string _protocol = "SMB";
        private readonly string _direction = "Outbound";
        private readonly string _destinationPort = "445";
        private string _sourceIP;
        private DestinationIpFsa _destIP;
        private MapedStatus _status = new MapedStatus();

        #endregion Member section

        ///**********************************************
        ///             Functions Section
        ///**********************************************

        #region Function section

        /// <summary>
        /// Ctor of FSARow
        /// </summary>
        public FSARow(Int64 serverClientDelta, string reportingComputer, ushort headerVersion, string sourceIP)
        {
            _expandSVCHost = new List<ExpandSVCHostRow>();

            //insert host name to row
            _reportingComputer = reportingComputer;
            TimeStamp = new TimeStamp(serverClientDelta);
            _headerVersion = headerVersion;

            _sourceIP = sourceIP;

            _fileExtractData = new List<FileData>
            {
                new ProcessId(),
                new ProcessName(),
                new ProcessPath(),
                new DestinationPath(),
                new UserName(headerVersion),
                new StatusFsa(),
                new ReasonFsa(),
                new SquenceNumber(),//sequence number
                new SquenceNumber()//sub sequence number
            };
        }

        /// <summary>
        /// This function extract data from binary file
        /// </summary>
        /// <param name="loopIndex"></param>
        /// <param name="expandedFileByteArray"></param>
        public void ExtractData(int loopIndex, byte[] expandedFileByteArray)
        {
            //reset file index
            int fileIndex = 0;

            TimeStamp.ExtractData(loopIndex, expandedFileByteArray, ref fileIndex);

            for (int index = 0; index < _fileExtractData.Count; index++)
            {
                //if we are in process path iteration make new process path object with process name
                if (index == (int)_fileExtractDataIndexs.PROCESS_PATH)
                {
                    _fileExtractData[(int)_fileExtractDataIndexs.PROCESS_PATH] = new ProcessPath(_fileExtractData[(int)_fileExtractDataIndexs.PROCESS_NAME].GetData());
                }
                //extract data from expandedFileByteArray
                _fileExtractData[index].ExtractData(loopIndex, expandedFileByteArray, ref fileIndex);
            }

            _destIP = new DestinationIpFsa(_fileExtractData[(int)_fileExtractDataIndexs.DESTINATION_PATH].GetData(), _sourceIP);
            _status.DefineStatusFromDataLog(_fileExtractData[(int)_fileExtractDataIndexs.REASON].GetData(), _fileExtractData[(int)_fileExtractDataIndexs.SEQ_NUM].GetData());
        }

        /// <summary>
        /// This function add current row to data output
        /// </summary>
        public StringBuilder AddRowToDataOutput()
        {
            List<string> paramList = GetAsList();

            return BuildAsCsv(paramList);
        }

        /// <summary>
        /// expand svchost data - using the service table
        /// </summary>
        /// <param name="serviceRow"></param>
        public void ExpandSvc(FSARow serviceRow)
        {
            ExpandSVCHostRow newExpandRow = new ExpandSVCHostRow
            {
                _appName = serviceRow._fileExtractData[(int)_fileExtractDataIndexs.PROCESS_NAME].GetData(),
                _fullPath = serviceRow._fileExtractData[(int)_fileExtractDataIndexs.DESTINATION_PATH].GetData(),
                _reason = serviceRow._fileExtractData[(int)_fileExtractDataIndexs.REASON].GetData()
            };

            _expandSVCHost.Add(newExpandRow);
        }

        /// <summary>
        /// get sequence number
        /// </summary>
        /// <returns>string value</returns>
        public string GetSeqNum()
        {
            return _fileExtractData[(int)_fileExtractDataIndexs.SEQ_NUM].GetData();
        }

        /// <summary>
        /// get sub sequence number
        /// </summary>
        /// <returns>string value</returns>
        public string GetSubSeqNum()
        {
            return _fileExtractData[(int)_fileExtractDataIndexs.SUB_SEQ_NUM].GetData();
        }

        /// <summary>
        /// Get fsa row parameters as List of string
        /// </summary>
        /// <returns>list<string> - fsa parameters</returns>
        public List<string> GetAsList()
        {
            List<string> list = new List<string>
            {
                Constant.OPERATING_SYSTEM,
                _reportingComputer,
                TimeStamp._clientTimeStamp,
                TimeStamp._fullServerTimeStamp,
                _fileExtractData[(int)_fileExtractDataIndexs.PROCESS_ID].GetData(),
                _fileExtractData[(int)_fileExtractDataIndexs.PROCESS_NAME].GetData(),
                _fileExtractData[(int)_fileExtractDataIndexs.PROCESS_PATH].GetData(),
                _protocol,//protocol
                _status._status,
                //_fileExtractData[(int)_fileExtractDataIndexs.STATUS].GetData(),
                "",//source_port
                _destinationPort,//destination_port
                _direction,//direction
                "",//cast_type
                "",//scramble_state
                _sourceIP,//source_ip
                _destIP._destinationIp,//destination_ip,//destination_ip
                _fileExtractData[(int)_fileExtractDataIndexs.SEQ_NUM].GetData(),
                _fileExtractData[(int)_fileExtractDataIndexs.SUB_SEQ_NUM].GetData(),
                _fileExtractData[(int)_fileExtractDataIndexs.USER_NAME].GetData(),
                "",//mog_counter
                _fileExtractData[(int)_fileExtractDataIndexs.DESTINATION_PATH].GetData(),
                _fileExtractData[(int)_fileExtractDataIndexs.REASON].GetData(),
                "",//dll path
                "",//dll name
                "",//chain_array
            };
            return list;
        }

        #endregion Function section
    }
}