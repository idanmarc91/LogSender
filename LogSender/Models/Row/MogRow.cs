using LogSender.Data;
using LogSender.Utilities;
using System;
using System.Collections.Generic;
using System.Text;

namespace LogSender.Models
{
    internal class MogRow : LogRow
    {
        ///**********************************************
        ///             Members Section
        ///**********************************************

        #region Members section

        enum _fileExtractDataIndexs
        {
            PROTOCOL, STATUS_REASON_LOG, SOURCE_PORT, DESTINATION_PORT, DIRECTION, ADDRESS_FAMILY,
            PROCESS_NAME, PROCESS_PATH, FLOW_HANDLE, FLOW_STATE, CAST_TYPE,SCRAMBLE_STATE,
            SOURCE_IP, DESTINATION_IP, SEQ_NUM, SUB_SEQ_NUM, USER_NAME, MOG_COUNTER
        };
        private ReasonLog _reasonMog = new ReasonLog();
        private string _realStatusMog = "";

        #endregion Member section

        ///**********************************************
        ///             Functions Section
        ///**********************************************

        #region Function section

        /// <summary>
        /// ctor of LogRow class
        /// </summary>
        /// <param name="serverClientDelta"></param>
        /// <param name="reportingComputer"></param>
        /// <param name="headerVersion"></param>
        /// <param name="rowNumber"></param>
        public MogRow(Int64 serverClientDelta, string reportingComputer, ushort headerVersion)
        {
            _expandSVCHost = new List<ExpandSVCHostRow>();
            //insert host name to row
            _reportingComputer = reportingComputer;
            _headerVersion = headerVersion;
            TimeStamp = new TimeStamp(serverClientDelta);

            _fileExtractData = new List<FileData>
            {
                new Protocol(),
                new StatusReasonLog(),
                new Port(), //source port
                new Port(), //destination port
                new Direction(),
                new AddressFamily(),
                new ApplicationName(),
                new FilePath(),
                new FlowHandle(),
                new FlowState(),
                new CastType(),
                new ScrambleState(),
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
            TimeStamp.ExtractData(loopIndex, expandedFileByteArray, ref fileIndex);

            //extract Data
            for (int index = 0; index < _fileExtractData.Count; index++)
            {
                //source ip need address family for extraction
                if (index == (int)_fileExtractDataIndexs.SOURCE_IP)
                {
                    _fileExtractData[index] = new IP(_fileExtractData[(int)_fileExtractDataIndexs.ADDRESS_FAMILY].GetData());
                }

                //destination ip need address family for extraction
                if (index == (int)_fileExtractDataIndexs.DESTINATION_IP)
                {
                    _fileExtractData[index] = new IP(_fileExtractData[(int)_fileExtractDataIndexs.ADDRESS_FAMILY].GetData());
                }

                _fileExtractData[index].ExtractData(loopIndex, expandedFileByteArray, ref fileIndex);
            }

            //The agent is suppling us with status and reason in the same field
            //we need to separate them to 2 fields: reason, real status.(the
            //extracted status is not the real status)

            _reasonMog.GetReasonFromExtractedData(_fileExtractData[(int)_fileExtractDataIndexs.STATUS_REASON_LOG].GetData());

            _realStatusMog = StatusReasonMap.Map(_fileExtractData[(int)_fileExtractDataIndexs.STATUS_REASON_LOG].GetData());

            if (_fileExtractData[(int)_fileExtractDataIndexs.FLOW_STATE].GetData() == "END")
            {
                SetEmptyString();
            }
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
        //public string GetFullAccTime()
        //{
        //    return _timeStamp.GetFullServerTime();
        //}

        /// <summary>
        /// when flow state parameter == END reset some parameters to empty string
        /// </summary>
        private void SetEmptyString()
        {
            _fileExtractData[(int)_fileExtractDataIndexs.PROCESS_NAME].SetData("");
            _fileExtractData[(int)_fileExtractDataIndexs.PROCESS_PATH].SetData("");
            _fileExtractData[(int)_fileExtractDataIndexs.DESTINATION_IP].SetData("");
            _fileExtractData[(int)_fileExtractDataIndexs.DESTINATION_PORT].SetData("");
            _fileExtractData[(int)_fileExtractDataIndexs.DIRECTION].SetData("");
            _fileExtractData[(int)_fileExtractDataIndexs.PROTOCOL].SetData("");
            _fileExtractData[(int)_fileExtractDataIndexs.SOURCE_IP].SetData("");
            _fileExtractData[(int)_fileExtractDataIndexs.SOURCE_PORT].SetData("");
            _fileExtractData[(int)_fileExtractDataIndexs.SCRAMBLE_STATE].SetData("");
            _fileExtractData[(int)_fileExtractDataIndexs.STATUS_REASON_LOG].SetData("");
            _fileExtractData[(int)_fileExtractDataIndexs.CAST_TYPE].SetData("");
        }

        /// <summary>
        /// This function add current row to data output
        /// </summary>
        /// <param name="dataAsString"></param>
        public StringBuilder AddRowToDataOutput()
        {
            List<string> paramList = GetAsList();

            return BuildAsCsv(paramList);
        }

        /// <summary>
        /// expand svchost data - using the service table
        /// </summary>
        /// <param name="serviceRow"></param>
        public void ExpandSvc(MogRow serviceRow)
        {
            ExpandSVCHostRow newExpandRow = new ExpandSVCHostRow
            {
                _appName = serviceRow._fileExtractData[(int)_fileExtractDataIndexs.PROCESS_NAME].GetData(),
                _fullPath = serviceRow._fileExtractData[(int)_fileExtractDataIndexs.PROCESS_PATH].GetData(),
                _reason = _reasonMog._reason
            };

            //for testing
            //if (string.IsNullOrEmpty(newExpandRow._appName) && string.IsNullOrEmpty(newExpandRow._fullPath) && string.IsNullOrEmpty(newExpandRow._reason))
            //{
            //    string what = "what?";
            //}
            _expandSVCHost.Add(newExpandRow);
        }

        /// <summary>
        /// Return all parameters as list
        /// </summary>
        /// <returns>list of parameters </returns>
        private List<string> GetAsList()
        {
            List<string> list = new List<string>
            {
                Constant.OPERATING_SYSTEM, //OS field
                _reportingComputer,
                TimeStamp._clientTimeStamp,
                TimeStamp._fullServerTimeStamp,
                "",//process id
                _fileExtractData[(int)_fileExtractDataIndexs.PROCESS_NAME].GetData(),
                _fileExtractData[(int)_fileExtractDataIndexs.PROCESS_PATH].GetData(),
                _fileExtractData[(int)_fileExtractDataIndexs.PROTOCOL].GetData(),
                _realStatusMog,
                _fileExtractData[(int)_fileExtractDataIndexs.SOURCE_PORT].GetData(),
                _fileExtractData[(int)_fileExtractDataIndexs.DESTINATION_PORT].GetData(),
                _fileExtractData[(int)_fileExtractDataIndexs.DIRECTION].GetData(),
                _fileExtractData[(int)_fileExtractDataIndexs.CAST_TYPE].GetData(),
                _fileExtractData[(int)_fileExtractDataIndexs.SCRAMBLE_STATE].GetData(),
                _fileExtractData[(int)_fileExtractDataIndexs.SOURCE_IP].GetData(),
                _fileExtractData[(int)_fileExtractDataIndexs.DESTINATION_IP].GetData(),
                _fileExtractData[(int)_fileExtractDataIndexs.SEQ_NUM].GetData(),
                _fileExtractData[(int)_fileExtractDataIndexs.SUB_SEQ_NUM].GetData(),
                _fileExtractData[(int)_fileExtractDataIndexs.USER_NAME].GetData(),
                _fileExtractData[(int)_fileExtractDataIndexs.MOG_COUNTER].GetData(),
                "",//destination path
               _reasonMog._reason,//reason
                "",//dll path
                "",//dll name
                ""//chain array
            };
            return list;
        }

        #endregion Function section

    }
}