using BinaryFileToTextFile.Data;
using System;
using System.Collections.Generic;

namespace BinaryFileToTextFile.Models
{
    class MogRow : LogRow
    {        
        ///**********************************************
        ///             Members Section
        ///**********************************************
        enum _fileExtractDataIndexs
        {
            PROTOCOL, STATUS, SOURCE_PORT, DESTINATION_PORT, DIRECTION, ADDRESS_FAMILY, APPLICATION_NAME, FILE_PATH, FLOW_HANDLE, FLOW_STATE, X_CAST,
            STATE, SOURCE_IP, DESTINATION_IP, SEQ_NUM, SUB_SEQ_NUM, USER_NAME, MOG_COUNTER
        };

        ///**********************************************
        ///             Functions Section
        ///**********************************************

        /// <summary>
        /// ctor of LogRow class
        /// </summary>
        /// <param name="serverClientDelta"></param>
        /// <param name="hostName"></param>
        /// <param name="headerVersion"></param>
        /// <param name="rowNumber"></param>
        public MogRow(Int64 serverClientDelta, string hostName, ushort headerVersion)
        {
            _expandSVCHost = new List<ExpandSVCHostRow>();
            //insert host name to row
            _hostName = hostName;
            _headerVersion = headerVersion;
            _timeStamp = new TimeStamp(serverClientDelta);

            _fileExtractData = new List<FileData>
            {
                new Protocol(),
                new StatusLog(),
                new Port(), //source port
                new Port(), //destination port
                new Direction(),
                new AddressFamily(),
                new ApplicationName(),
                new FilePath(),
                new FlowHandle(),
                new FlowState(),
                new XCast(),
                new State(),
                new IP(), //source ip
                new IP(), //destination ip
                new SquenceNumber(),
                new SquenceNumber(),
                new UserName(_headerVersion),
                new MogCounter()
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

            //extract time stamp from binary file
            _timeStamp.ExtractData(loopIndex, expandedFileByteArray, ref fileIndex);

            //extract Data
            for (int index = 0; index < _fileExtractData.Count; index++)
            {
                //source ip need address family for extraction
                if (index == (int)_fileExtractDataIndexs.SOURCE_IP)
                    _fileExtractData[index] = new IP(_fileExtractData[(int)_fileExtractDataIndexs.ADDRESS_FAMILY].GetData());
                
                //destination ip need address family for extraction
                if (index == (int)_fileExtractDataIndexs.DESTINATION_IP)
                    _fileExtractData[index] = new IP(_fileExtractData[(int)_fileExtractDataIndexs.ADDRESS_FAMILY].GetData());

                _fileExtractData[index].ExtractData(loopIndex, expandedFileByteArray, ref fileIndex);
            }

            if (_fileExtractData[(int)_fileExtractDataIndexs.FLOW_STATE].GetData() == "END")
                SetEmptyString();
        }

        /// <summary>
        /// check if sub sequence number == 0 
        /// </summary>
        /// <returns>boolean value true if == , false if !=</returns>
        public bool CheckSubSeqNum()
        {
            return _fileExtractData[(int)_fileExtractDataIndexs.SUB_SEQ_NUM].GetData() == "0" ? true : false;
        }

        /// <summary>
        /// when flow state parameter == END resert some parameters to empty string
        /// </summary>
        private void SetEmptyString()
        {
            _fileExtractData[(int)_fileExtractDataIndexs.APPLICATION_NAME].SetData("");
            _fileExtractData[(int)_fileExtractDataIndexs.DESTINATION_IP].SetData("");
            _fileExtractData[(int)_fileExtractDataIndexs.DESTINATION_PORT].SetData("");
            _fileExtractData[(int)_fileExtractDataIndexs.DIRECTION].SetData("");
            _fileExtractData[(int)_fileExtractDataIndexs.PROTOCOL].SetData("");
            _fileExtractData[(int)_fileExtractDataIndexs.SOURCE_IP].SetData("");
            _fileExtractData[(int)_fileExtractDataIndexs.SOURCE_PORT].SetData("");
            _fileExtractData[(int)_fileExtractDataIndexs.STATE].SetData("");
            _fileExtractData[(int)_fileExtractDataIndexs.STATUS].SetData("");
            _fileExtractData[(int)_fileExtractDataIndexs.X_CAST].SetData("");
            _fileExtractData[(int)_fileExtractDataIndexs.FILE_PATH].SetData("");
        }

        /// <summary>
        /// This function return the parameters as json
        /// </summary>
        /// <returns>csv format line</returns>
        public JsonLog GetRowAsJson()
        {
            JsonLog row = new JsonLog
            {
                HostName = _hostName,
                ClientTime = _timeStamp.GetClientTime(),
                FullServerTime = _timeStamp.GetFullServerTime(),
                Protocol = _fileExtractData[(int)_fileExtractDataIndexs.PROTOCOL].GetData(),
                Status = _fileExtractData[(int)_fileExtractDataIndexs.STATUS].GetData(),
                Direction = _fileExtractData[(int)_fileExtractDataIndexs.DIRECTION].GetData(),
                ApplicationName = _fileExtractData[(int)_fileExtractDataIndexs.APPLICATION_NAME].GetData(),
                FilePath = _fileExtractData[(int)_fileExtractDataIndexs.FILE_PATH].GetData(),
                XCast = _fileExtractData[(int)_fileExtractDataIndexs.X_CAST].GetData(),
                State = _fileExtractData[(int)_fileExtractDataIndexs.STATE].GetData(),
                SourceIP = _fileExtractData[(int)_fileExtractDataIndexs.SOURCE_IP].GetData(),
                DestinationIP = _fileExtractData[(int)_fileExtractDataIndexs.DESTINATION_IP].GetData(),
                OS = "win"
        };


            if (_fileExtractData[(int)_fileExtractDataIndexs.SOURCE_PORT].GetData() != "")
                row.SourcePort = Int32.Parse(_fileExtractData[(int)_fileExtractDataIndexs.SOURCE_PORT].GetData());
            if (_fileExtractData[(int)_fileExtractDataIndexs.DESTINATION_PORT].GetData() != "")
                row.DestinationPort = Int32.Parse(_fileExtractData[(int)_fileExtractDataIndexs.DESTINATION_PORT].GetData());
            if (_fileExtractData[(int)_fileExtractDataIndexs.SEQ_NUM].GetData() != "")
                row.SequanceNumber = Int32.Parse(_fileExtractData[(int)_fileExtractDataIndexs.SEQ_NUM].GetData());
            if (_fileExtractData[(int)_fileExtractDataIndexs.SUB_SEQ_NUM].GetData() != "")
                row.SubSequanceNumber = Int32.Parse(_fileExtractData[(int)_fileExtractDataIndexs.SUB_SEQ_NUM].GetData());
            if (_fileExtractData[(int)_fileExtractDataIndexs.MOG_COUNTER].GetData() != "")
                row.MogCounter = Int32.Parse(_fileExtractData[(int)_fileExtractDataIndexs.MOG_COUNTER].GetData());

            if (_expandSVCHost.Count > 0)
            {
                row.ChainArray = new ExpandSVCHostRow[_expandSVCHost.Count];

                for (int index = 0; index < _expandSVCHost.Count; index++)
                {
                    row.ChainArray[index] = _expandSVCHost[index];
                }
            }

            return row;
        }

        /// <summary>
        /// Get application name
        /// </summary>
        /// <returns>string - app name</returns>
        public string GetAppName()
        {
            return _fileExtractData[(int)_fileExtractDataIndexs.APPLICATION_NAME].GetData();
        }

        /// <summary>
        /// Change application name
        /// </summary>
        public void ChangeAppName()
        {
            _fileExtractData[(int)_fileExtractDataIndexs.APPLICATION_NAME].SetData(_fileExtractData[(int)_fileExtractDataIndexs.APPLICATION_NAME].GetData() + " (" + _expandSVCHost.Count + ")");
        }


        /// <summary>
        /// get sub seq number parameter
        /// </summary>
        /// <returns>string - sequence number</returns>
        public string GetSeqNum()
        {
            return _fileExtractData[(int)_fileExtractDataIndexs.SEQ_NUM].GetData();
        }

        /// <summary>
        /// get sub seq number parameter
        /// </summary>
        /// <returns>string - sub sequence number</returns>
        public string GetSubSeqNum()
        {
            return _fileExtractData[(int)_fileExtractDataIndexs.SUB_SEQ_NUM].GetData();
        }

        /// <summary>
        /// get full server time parameter
        /// </summary>
        /// <returns></returns>
        public string GetFullAccTime()
        {
            return _timeStamp.GetFullServerTime();
        }

        /// <summary>
        /// expand svchost data - using the service table
        /// </summary>
        /// <param name="serviceRow"></param>
        public void ExpandSvc(MogRow serviceRow)
        {
            ExpandSVCHostRow newExpandRow = new ExpandSVCHostRow
            {
                _appName = serviceRow._fileExtractData[(int)_fileExtractDataIndexs.APPLICATION_NAME].GetData(),
                _fullPath = serviceRow._fileExtractData[(int)_fileExtractDataIndexs.FILE_PATH].GetData(),
                _status = serviceRow._fileExtractData[(int)_fileExtractDataIndexs.STATUS].GetData()
            };
            _expandSVCHost.Add(newExpandRow);
        }
    }
}