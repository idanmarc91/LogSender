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
                    _status = "Ok";
                    break;
                case "2":
                    _status = "Not in policy";
                    break;
                case "3":
                    _status = "Config exculuded";
                    break;
                case "4":
                    _status = "Scramble off";
                    break;
                case "5":
                    _status = "NoKey";
                    break;
                case "6":
                    _status = "Policy MM";
                    break;
                case "7":
                    _status = "Flow Error";
                    break;
                case "8":
                    _status = "Unknown";
                    break;
                case "9":
                    _status = "Not enough params";
                    break;
                case "10":
                    _status = "Policy MM date";
                    break;
                case "11":
                    _status = "Policy MM size";
                    break;
                case "12":
                    _status = "Policy MM bad MD5";
                    break;
                case "13":
                    _status = "Dropped";
                    break;
                case "14":
                    _status = "Policy MM MD5 corrupt";
                    break;
                case "15":
                    _status = "Policy MM size corrupt";
                    break;
                case "16":
                    _status = "Chain";
                    break;
                case "17":
                    _status = "Policy MM DLL MM";
                    break;
                case "18":
                    _status = "Policy MM DLL ambiguity";
                    break;
                case "19":
                    _status = "Wait for SUP";
                    break;
                case "20":
                    _status = "Chain block";
                    break;
                case "21":
                    _status = "NotPD";
                    break;
                case "22":
                    _status = "Fake localhost";
                    break;
                case "23":
                    _status = "Network no check";
                    break;
                case "24":
                    _status = "TCP MM";
                    break;
                case "25":
                    _status = "UDP MM";
                    break;
                case "26":
                    _status = "Svchost MM";
                    break;
                default:
                    _status = "Error Val";
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
