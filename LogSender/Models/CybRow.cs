using BinaryFileToTextFile.Data;
using System;
using System.Collections.Generic;
using System.Text;

namespace BinaryFileToTextFile.Models
{
    class CybRow :LogRow
    {
        ///**********************************************
        ///             Members Section
        ///**********************************************
        enum _fileExtractDataIndexs {
            PROTOCOL, STATUS, SOURCE_PORT ,DESTINATION_PORT, DIRECTION, ADDRESS_FAMILY, APPLICATION_NAME, FILE_PATH, FLOW_HANDLE, FLOW_STATE, X_CAST,
            STATE, SOURCE_IP, DESTINATION_IP, SEQ_NUM, SUB_SEQ_NUM, USER_NAME
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
        public CybRow(Int64 serverClientDelta, string hostName, ushort headerVersion)
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
                new UserName(_headerVersion)
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

            for (int index = 0; index < _fileExtractData.Count; index++)
            {

                if (index == (int)_fileExtractDataIndexs.SOURCE_IP)
                    _fileExtractData[index] = new IP(_fileExtractData[(int)_fileExtractDataIndexs.ADDRESS_FAMILY].GetData());

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
        /// This function add current row to data output
        /// </summary>
        /// <param name="dataAsString"></param>
        public void AddRowToDataOutput(StringBuilder dataAsString)
        {
            List<string> paramList = GetAsList();
            BuildAsCsv(paramList, dataAsString);
        }

        /// <summary>
        /// Return all parameters as list
        /// </summary>
        /// <returns>list of parameters </returns>
        private List<string> GetAsList()
        {
            List<string> list = new List<string>
            {
                "win", //OS field
                _hostName,
                _timeStamp.GetClientTime(),
                _timeStamp.GetFullServerTime(),
                "",
                "",
                "",
                _fileExtractData[(int)_fileExtractDataIndexs.APPLICATION_NAME].GetData(),
                _fileExtractData[(int)_fileExtractDataIndexs.PROTOCOL].GetData(),
                _fileExtractData[(int)_fileExtractDataIndexs.STATUS].GetData(),
                _fileExtractData[(int)_fileExtractDataIndexs.SOURCE_PORT].GetData(),
                _fileExtractData[(int)_fileExtractDataIndexs.DESTINATION_PORT].GetData(),
                _fileExtractData[(int)_fileExtractDataIndexs.DIRECTION].GetData(),
                _fileExtractData[(int)_fileExtractDataIndexs.FILE_PATH].GetData(),
                _fileExtractData[(int)_fileExtractDataIndexs.X_CAST].GetData(),
                _fileExtractData[(int)_fileExtractDataIndexs.STATE].GetData(),
                _fileExtractData[(int)_fileExtractDataIndexs.SOURCE_IP].GetData(),
                _fileExtractData[(int)_fileExtractDataIndexs.DESTINATION_IP].GetData(),
                _fileExtractData[(int)_fileExtractDataIndexs.SEQ_NUM].GetData(),
                _fileExtractData[(int)_fileExtractDataIndexs.SUB_SEQ_NUM].GetData(),
                _fileExtractData[(int)_fileExtractDataIndexs.USER_NAME].GetData(),
                "",
                "",
                "",
                "",
                "",
                "",
                ""
            };
            return list;
        }

        ///// <summary>
        ///// This function return the parametersas csv format line
        ///// </summary>
        ///// <returns>csv format line</returns>
        //public string GetLineAsCsv()
        //{
        //    List<string> paramList = GetAsList();

        //}

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
        /// expand svchost data - using the service table
        /// </summary>
        /// <param name="serviceRow"></param>
        public void ExpandSvc(CybRow serviceRow)
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