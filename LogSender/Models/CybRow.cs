using LogSender.Data;
using LogSender.Utilities;
using System;
using System.Collections.Generic;
using System.Text;

namespace LogSender.Models
{
    class CybRow : LogRow
    {
        ///**********************************************
        ///             Members Section
        ///**********************************************
        enum _fileExtractDataIndexs
        {
            PROTOCOL, STATUS_REASON_LOG, SOURCE_PORT, DESTINATION_PORT, DIRECTION, ADDRESS_FAMILY,
            PROCESS_NAME, PROCESS_PATH, FLOW_HANDLE, FLOW_STATE, CAST_TYPE, SCRAMBLE_STATE, SOURCE_IP,
            DESTINATION_IP, SEQ_NUM, SUB_SEQ_NUM, USER_NAME, REASON
        };

        private ReasonLog _reasonCyb = new ReasonLog();
        private string _realStatusCyb = "";

        ///**********************************************
        ///             Functions Section
        ///**********************************************

        /// <summary>
        /// ctor of LogRow class
        /// </summary>
        /// <param name="serverClientDelta"></param>
        /// <param name="reportingComputer"></param>
        /// <param name="headerVersion"></param>
        /// <param name="rowNumber"></param>
        public CybRow(Int64 serverClientDelta, string reportingComputer, ushort headerVersion)
        {
            _expandSVCHost = new List<ExpandSVCHostRow>();
            //insert host name to row
            _reportingComputer = reportingComputer;
            _headerVersion = headerVersion;
            _timeStamp = new TimeStamp(serverClientDelta);

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
                {
                    _fileExtractData[index] = new IP(_fileExtractData[(int)_fileExtractDataIndexs.ADDRESS_FAMILY].GetData());
                }

                if (index == (int)_fileExtractDataIndexs.DESTINATION_IP)
                {
                    _fileExtractData[index] = new IP(_fileExtractData[(int)_fileExtractDataIndexs.ADDRESS_FAMILY].GetData());
                }

                _fileExtractData[index].ExtractData(loopIndex, expandedFileByteArray, ref fileIndex);
            }

            //The agent is suppling us with status and reason in the same field
            //we need to separate them to 2 fields: reason, real status.(the
            //extracted status is not the real status)
            
            _reasonCyb.GetReasonFromExtractedData(_fileExtractData[(int)_fileExtractDataIndexs.STATUS_REASON_LOG].GetData());

            _realStatusCyb = StatusReasonMap.Map(_fileExtractData[(int)_fileExtractDataIndexs.STATUS_REASON_LOG].GetData());

            if (_fileExtractData[(int)_fileExtractDataIndexs.FLOW_STATE].GetData() == "END")
            {
                SetEmptyString();
            }

            //DEPT
            //The agent provide us with not accurate cast type so we calculate it manually by the destination ip
            //CastType.CalcTypeFromDestIp(_fileExtractData[(int)_fileExtractDataIndexs.DESTINATION_IP].GetData());
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
        /// when flow state parameter == END reset some parameters to empty string
        /// </summary>
        private void SetEmptyString()
        {
            _fileExtractData[(int)_fileExtractDataIndexs.PROCESS_NAME].SetData("");
            _fileExtractData[(int)_fileExtractDataIndexs.DESTINATION_IP].SetData("");
            _fileExtractData[(int)_fileExtractDataIndexs.DESTINATION_PORT].SetData("");
            _fileExtractData[(int)_fileExtractDataIndexs.DIRECTION].SetData("");
            _fileExtractData[(int)_fileExtractDataIndexs.PROTOCOL].SetData("");
            _fileExtractData[(int)_fileExtractDataIndexs.SOURCE_IP].SetData("");
            _fileExtractData[(int)_fileExtractDataIndexs.SOURCE_PORT].SetData("");
            _fileExtractData[(int)_fileExtractDataIndexs.SCRAMBLE_STATE].SetData("");
            _fileExtractData[(int)_fileExtractDataIndexs.STATUS_REASON_LOG].SetData("");
            _realStatusCyb = "";
            _reasonCyb._reason = "";
            _fileExtractData[(int)_fileExtractDataIndexs.CAST_TYPE].SetData("");
            _fileExtractData[(int)_fileExtractDataIndexs.PROCESS_PATH].SetData("");
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
        public StringBuilder AddRowToDataOutput()
        {
            List<string> paramList = GetAsList();
            return BuildAsCsv(paramList);
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
                _reportingComputer,
                _timeStamp.GetClientTime(),
                _timeStamp.GetFullServerTime(),
                "",//process_id
                _fileExtractData[(int)_fileExtractDataIndexs.PROCESS_NAME].GetData(),
                _fileExtractData[(int)_fileExtractDataIndexs.PROCESS_PATH].GetData(),
                _fileExtractData[(int)_fileExtractDataIndexs.PROTOCOL].GetData(),
                _realStatusCyb,
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
                "",//mog_counter
                "",//destination_path
                _reasonCyb._reason,//reason
                "",//dll_path
                "",//dll_name
                ""//chain_array
            };
            return list;
        }

        ///// <summary>
        ///// Get application name
        ///// </summary>
        ///// <returns>string - app name</returns>
        //public string GetAppName()
        //{
        //    return _fileExtractData[(int)_fileExtractDataIndexs.PROCESS_NAME].GetData();
        //}

        /// <summary>
        /// expand svchost data - using the service table
        /// </summary>
        /// <param name="serviceRow"></param>
        public void ExpandSvc(CybRow serviceRow)
        {
            ExpandSVCHostRow newExpandRow = new ExpandSVCHostRow
            {
                _appName = serviceRow._fileExtractData[(int)_fileExtractDataIndexs.PROCESS_NAME].GetData(),
                _fullPath = serviceRow._fileExtractData[(int)_fileExtractDataIndexs.PROCESS_PATH].GetData(),
                _status = serviceRow._realStatusCyb
                //_status = serviceRow._fileExtractData[(int)_fileExtractDataIndexs.STATUS_REASON_CYB].GetData()
            };
            _expandSVCHost.Add(newExpandRow);
        }
    }
}