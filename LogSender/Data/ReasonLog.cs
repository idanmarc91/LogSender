namespace LogSender.Data
{
    class ReasonLog : FileData
    {
        ///**********************************************
        ///             Members Section
        ///**********************************************
        private const int STATUS_LEN = 1;
        private string _reason;

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
                    _reason = "Ok"; //ok
                    break;
                case "2":
                    _reason = "Not in policy"; // blocked
                    break;
                case "3":
                    _reason = "Config excluded"; //if chain exist  status= chain else status = excluded
                    break;
                case "4":
                    _reason = "Scramble off"; // not in use
                    break;
                case "5":
                    _reason = "NoKey"; //not in use
                    break;
                case "6":
                    _reason = "Policy MM"; //blocked
                    break;
                case "7":
                    _reason = "Flow Error"; // Error
                    break;
                case "8":
                    _reason = "Unknown"; //Unknown
                    break;
                case "9":
                    _reason = "Not enough params";//Erez need to answer what is the status for this condition
                    break;
                case "10":
                    _reason = "Policy MM date"; // status blocked
                    break;
                case "11":
                    _reason = "Policy MM size"; //blocked
                    break;
                case "12":
                    _reason = "Policy MM MD5";//blocked. was Policy MM bad MD5
                    break;
                case "13":
                    _reason = "Dropped";//dropped
                    break;
                case "14":
                    _reason = "Policy MM MD5 corrupt"; //blocked
                    break;
                case "15":
                    _reason = "Policy MM size corrupt";//blocked
                    break;
                case "16":
                    _reason = "Chain";//chain
                    break;
                case "17":
                    _reason = "Policy MM DLL MM";//blocked
                    break;
                case "18":
                    _reason = "Policy MM DLL ambiguity";//blocked
                    break;
                case "19":
                    _reason = "Wait for SUP";//Error
                    break;
                case "20":
                    _reason = "Chain break"; //Chain
                    break;
                case "21":
                    _reason = "NotPD";//Error
                    break;
                case "22":
                    _reason = "Fake localhost"; //Error Erez need to answer
                    break;
                case "23":
                    _reason = "Network no check"; //blocked Erez need to answer
                    break;
                case "24":
                    _reason = "TCP MM"; //Info 
                    break;
                case "25":
                    _reason = "UDP MM"; //Info
                    break;
                case "26":
                    _reason = "Svchost MM"; // blocked
                    break;
                default:
                    _reason = "Error"; //error
                    break;
            }
        }

        /// <summary>
        /// Get data _status
        /// </summary>
        /// <returns>string - status</returns>
        public override string GetData()
        {
            return _reason;
        }

        /// <summary>
        /// set status
        /// </summary>
        public override void SetData(string status)
        {
            _reason = status;
        }
    }
}
