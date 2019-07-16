
namespace LogSender.Data
{
    class ReasonDll : FileData
    {
        ///**********************************************
        ///             Members Section
        ///**********************************************
        private const int REASON_DLL_LEN = 1;
        private string _reason;

        ///**********************************************
        ///             Functions Section
        ///**********************************************

        /// <summary>
        /// This function extract the reason data from the binary file
        /// </summary>
        /// <param name="loopIndex"></param>
        /// <param name="expandedFileByteArray"></param>
        /// <param name="fileIndex"></param>
        public override void ExtractData(int loopIndex, byte[] expandedFileByteArray, ref int fileIndex)
        {
            System.Text.StringBuilder builder = new System.Text.StringBuilder();
            builder.Append(GetData(loopIndex, expandedFileByteArray, ref fileIndex, REASON_DLL_LEN)[0]);
            switch (builder.ToString())
            {
                case "0":
                    _reason = "Ok"; // ok
                    break;
                case "1":
                    _reason = "Not in policy"; //blocked
                    break;
                case "2":
                    _reason = "Policy MM date";//blocked
                    break;
                case "3":
                    _reason = "Policy MM size";//blocked
                    break;
                case "4":
                    _reason = "Policy MM MD5";//blocked
                    break;
                case "5":
                    _reason = "Policy MM MD5 corrupt";//blocked
                    break;
                case "6":
                    _reason = "Policy MM size corrupt";//blocked
                    break;
                case "7":
                    _reason = "Policy MM DLL MM";//blocked
                    break;
                case "8":
                    _reason = "Policy MM DLL ambiguity";//blocked
                    break;
                case "9":
                    _reason = "Chain";//chain
                    break;
                case "10":
                    _reason = "Flow error"; //error
                    break;
                case "11":
                    _reason = "Wait for SUP"; //error
                    break;
                case "12":
                    _reason = "Network no check";
                    break;
                case "13":
                    _reason = "Error"; //unkkown
                    break;
                default:
                    _reason = "Error"; // Error
                    break;
            }
        }

        /// <summary>
        /// Get reason
        /// </summary>
        /// <returns>string -reason</returns>
        public override string GetData()
        {
            return _reason;
        }

        /// <summary>
        /// set new reason
        /// </summary>
        /// <param name="reason"></param>
        public override void SetData(string reason)
        {
            _reason = reason;
        }
    }
}
