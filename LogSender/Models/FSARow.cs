using BinaryFileToTextFile.Data;
using System;
using System.Collections.Generic;
using System.Text;

namespace BinaryFileToTextFile.Models
{
    class FSARow :LogRow
    {
        ///**********************************************
        ///             Members Section
        ///**********************************************
        enum _fileExtractDataIndexs
        {
            PROCESS_ID, PROCESS_NAME, PROCESS_PATH, DESTINATION_PATH, USER_NAME, STATUS, REASON, 
            SEQ_NUM, SUB_SEQ_NUM
        };

        ///**********************************************
        ///             Functions Section
        ///**********************************************


        public FSARow(Int64 serverClientDelta, string hostName, ushort headerVersion)
        {
            _expandSVCHost = new List<ExpandSVCHostRow>();

            //insert host name to row
            _hostName = hostName;
            _timeStamp = new TimeStamp(serverClientDelta);
            _headerVersion = headerVersion;
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

            _timeStamp.ExtractData(loopIndex, expandedFileByteArray, ref fileIndex);

            for (int index = 0; index < _fileExtractData.Count; index++ )
            {
                //if we are in process path iteration make new process path object with process name
                if (index == (int)_fileExtractDataIndexs.PROCESS_PATH)
                {
                    _fileExtractData[(int)_fileExtractDataIndexs.PROCESS_PATH] = new ProcessPath(_fileExtractData[(int)_fileExtractDataIndexs.PROCESS_NAME].GetData());
                }
                //extract data from expandedFileByteArray
                _fileExtractData[index].ExtractData(loopIndex, expandedFileByteArray, ref fileIndex);
            }

        }


        /// <summary>
        /// Get fsa row parameters as List of string
        /// </summary>
        /// <returns>list<string> - fsa parameters</returns>
        public List<string> GetAsList()
        {
            List<string> list = new List<string>
            {
                "win",
                _hostName,
                _timeStamp.GetClientTime(),
                _timeStamp.GetFullServerTime(),
                _fileExtractData[(int)_fileExtractDataIndexs.PROCESS_ID].GetData(),
                _fileExtractData[(int)_fileExtractDataIndexs.PROCESS_NAME].GetData(),
                _fileExtractData[(int)_fileExtractDataIndexs.PROCESS_PATH].GetData(),
                "",
                "",
                _fileExtractData[(int)_fileExtractDataIndexs.STATUS].GetData(),
                "",
                "",
                "",
                "",
                "",
                "",
                "",
                "",
                _fileExtractData[(int)_fileExtractDataIndexs.SEQ_NUM].GetData(),
                _fileExtractData[(int)_fileExtractDataIndexs.SUB_SEQ_NUM].GetData(),
                _fileExtractData[(int)_fileExtractDataIndexs.USER_NAME].GetData(),
                "",
                _fileExtractData[(int)_fileExtractDataIndexs.DESTINATION_PATH].GetData(),
                _fileExtractData[(int)_fileExtractDataIndexs.REASON].GetData(),
                "",
                "",
                "",
                "",
            };
            return list;
        }

        /// <summary>
        /// This function add current row to data output
        /// </summary>
        /// <param name="dataAsString"></param>
        public void AddRowToDataOutput(StringBuilder dataAsString)
        {
            List<string> paramList = GetAsList();

            BuildAsCsv(paramList, dataAsString);
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
                _status = serviceRow._fileExtractData[(int)_fileExtractDataIndexs.REASON].GetData()
            };
            _expandSVCHost.Add(newExpandRow);
        }

        /// <summary>
        /// This function return process name parameter
        /// </summary>
        /// <returns>string process name</returns>
        public string GetProcessName()
        {
            return _fileExtractData[(int)_fileExtractDataIndexs.PROCESS_NAME].GetData();
        }

        /// <summary>
        /// Change Process name
        /// </summary>
        public void ChangeProcessName()
        {
            _fileExtractData[(int)_fileExtractDataIndexs.PROCESS_NAME].SetData(_fileExtractData[(int)_fileExtractDataIndexs.PROCESS_NAME].GetData() + " (" + _expandSVCHost.Count + ")");
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
        /// get full access time
        /// </summary>
        /// <returns>string value</returns>
        public string GetFullAccTime()
        {
            return _timeStamp.GetFullServerTime();
        }
    }
}
