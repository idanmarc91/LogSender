using System;

namespace LogSender.Data
{
    class StatusReasonLog : FileData
    {
        ///**********************************************
        ///             Members Section
        ///**********************************************
        private const int STATUS_LEN = 1;
        private string _status;

        ///**********************************************
        ///             Functions Section
        ///**********************************************

        /// <summary>
        /// This function create status string
        /// </summary>
        /// <param name="loopIndex"></param>
        /// <param name="expandedFileByteArray"></param>
        /// <param name="fileIndex"></param>
        public override void ExtractData(int loopIndex, byte[] expandedFileByteArray, ref int fileIndex)
        {
            System.Text.StringBuilder builder = new System.Text.StringBuilder();
            builder.Append(GetData(loopIndex, expandedFileByteArray, ref fileIndex, STATUS_LEN)[0]);
            switch (builder.ToString())
            {
                case "1":
                    _status = "OK";
                    break;
                case "2":
                    _status = "NotWL";
                    break;
                case "3":
                    _status = "Exculuded";
                    break;
                case "4":
                    _status = "Off";
                    break;
                case "5":
                    _status = "NoKey";
                    break;
                case "6":
                    _status = "WLMM";
                    break;
                case "7":
                    _status = "ERROR";
                    break;
                case "8":
                    _status = "UNKNOWN";
                    break;
                case "9":
                    _status = "NEP";
                    break;
                case "10":
                    _status = "WLMM DATE";
                    break;
                case "11":
                    _status = "WLMM SIZE";
                    break;
                case "12":
                    _status = "WLMM MD5";
                    break;
                case "13":
                    _status = "Dropped";
                    break;
                case "14":
                    _status = "WLMM MD5 COR";
                    break;
                case "15":
                    _status = "WLMM SIZE COR";
                    break;
                case "16":
                    _status = "Chain";
                    break;
                case "17":
                    _status = "WLMM DLL MM";
                    break;
                case "18":
                    _status = "WLMM DLL AMBG";
                    break;
                case "19":
                    _status = "WFS";
                    break;
                case "20":
                    _status = "ChainBreak";
                    break;
                case "21":
                    _status = "NotPD";
                    break;
                default:
                    _status = builder.ToString();
                    break;
            }
        }

        /// <summary>
        /// Get data _status
        /// </summary>
        /// <returns>string - status</returns>
        public override string GetData()
        {
            return _status;
        }

        /// <summary>
        /// set status
        /// </summary>
        public override void SetData(string status)
        {
            _status = status;
        }
    }
}
