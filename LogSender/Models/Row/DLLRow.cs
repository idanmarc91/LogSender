using LogSender.Data;
using LogSender.Utilities;
using System;
using System.Collections.Generic;
using System.Text;

namespace LogSender.Models
{
    internal class DLLRow : Row
    {
        ///**********************************************
        ///             Members Section
        ///**********************************************

        #region Members section

        enum _fileExtractDataIndexs
        {
            PROCESS_ID, DLL_PATH, DLL_NAME, PARENT_PATH, PARENT_NAME, STATUS_REASON_LOG
        };

        private ReasonLog _reasonDll = new ReasonLog();
        private string _realStatusDll = "";

        #endregion Member section

        ///**********************************************
        ///             Functions Section
        ///**********************************************

        #region Function section

        /// <summary>
        /// Ctor of DLL Row class
        /// </summary>
        /// <param name="serverClientDelta"></param>
        /// <param name="rowNumber"></param>
        public DLLRow(Int64 serverClientDelta, string reportingComputer)
        {
            _timeStamp = new TimeStamp(serverClientDelta);
            _reportingComputer = reportingComputer;
            _fileExtractData = new List<FileData>
            {
                new ProcessId(),
                new DllPath(),
                new DllName(),
                new ParentPath(),
                new ParentName(),
                new StatusReasonDll()
            };
        }

        /// <summary>
        /// This function extract data from binary file
        /// </summary>
        /// <param name="loopIndex"></param>
        /// <param name="expandedFileByteArray"></param>
        public void ExtractData(int loopIndex, byte[] expandedFileByteArray)
        {
            int fileIndex = 0;
            _timeStamp.ExtractData(loopIndex, expandedFileByteArray, ref fileIndex);

            for (int index = 0; index < _fileExtractData.Count; index++)
            {
                if (index == (int)_fileExtractDataIndexs.DLL_NAME)
                {
                    _fileExtractData[index] = new DllName(_fileExtractData[(int)_fileExtractDataIndexs.DLL_PATH].GetData());
                }
                else if (index == (int)_fileExtractDataIndexs.PARENT_NAME)
                {
                    _fileExtractData[index] = new ParentName(_fileExtractData[(int)_fileExtractDataIndexs.PARENT_PATH].GetData());
                }
                else
                {
                    _fileExtractData[index].ExtractData(loopIndex, expandedFileByteArray, ref fileIndex);
                }
            }

            //The agent is suppling us with status and reason in the same field
            //we need to separate them to 2 fields: reason, real status.(the
            //extracted status is not the real status)

            _reasonDll.GetReasonFromExtractedData(_fileExtractData[(int)_fileExtractDataIndexs.STATUS_REASON_LOG].GetData());

            _realStatusDll = StatusReasonMap.Map(_fileExtractData[(int)_fileExtractDataIndexs.STATUS_REASON_LOG].GetData());
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
        /// Get dll row parameters as List of string
        /// </summary>
        /// <returns>list<string> - dll parameters</returns>
        private List<string> GetAsList()
        {
            List<string> list = new List<string>
            {
                Constant.OPERATINGSYSTEM,
                _reportingComputer,
                _timeStamp.GetClientTime(),
                _timeStamp.GetFullServerTime(),
                _fileExtractData[(int)_fileExtractDataIndexs.PROCESS_ID].GetData(),
                _fileExtractData[(int)_fileExtractDataIndexs.PARENT_NAME].GetData(),//process name  = parent name
                _fileExtractData[(int)_fileExtractDataIndexs.PARENT_PATH].GetData(),//process path = parent path
                "",//protocol
                _realStatusDll,//status
                "",//source_port
                "",//destination_port
                "",//direction
                "",//cast_type
                "",//scramble_state
                "",//source_ip
                "",//destination_ip
                "",//sequance_number
                "",//sub_sequance_number
                "",//user_name
                "",//mog_counter
                "",//destination_path
                _reasonDll._reason,
                _fileExtractData[(int)_fileExtractDataIndexs.DLL_PATH].GetData(),
                _fileExtractData[(int)_fileExtractDataIndexs.DLL_NAME].GetData(),
                "",//chain_array
            };
            return list;
        }

        #endregion Function section

    }
}