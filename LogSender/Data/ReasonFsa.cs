using System;

namespace LogSender.Data
{
    class ReasonFsa : FileData
    {
        ///**********************************************
        ///             Members Section
        ///**********************************************
        private const int REASON_STR_LEN = 1;
        private string _reason;

        ///**********************************************
        ///             Members Section
        ///**********************************************
        /// <summary>
        /// This function extract reason string
        /// </summary>
        /// <param name="loopIndex"></param>
        /// <param name="expandedFileByteArray"></param>
        /// <param name="fileIndex"></param>
        public override void ExtractData(int loopIndex, byte[] expandedFileByteArray, ref int fileIndex)
        {
            System.Text.StringBuilder builder = new System.Text.StringBuilder();
            builder.Append(GetData(loopIndex, expandedFileByteArray, ref fileIndex, REASON_STR_LEN)[0]);
            switch (builder.ToString())
            {
                case "0":
                    _reason = "Ok";
                    break;
                case "1":
                    _reason = "Not in policy";
                    break;
                case "2":
                    _reason = "Unknown";
                    break;
                case "3":
                    _reason = "Path overflow";
                    break;
                case "5":
                    _reason = "Policy MM date";
                    break;
                case "6":
                    _reason = "Policy MM size";
                    break;
                case "7":
                    _reason = "Policy MM MD5";
                    break;
                case "8":
                    _reason = "Dropped";
                    break;
                case "9":
                    _reason = "Fake localhost";
                    break;
                case "10":
                    _reason = "Chain";
                    break;
                case "11":
                    _reason = "Policy MM MD5 corrupt";
                    break;
                case "12":
                    _reason = "Policy MM size corrupt";
                    break;
                case "13":
                    _reason = "Wait for SUP";
                    break;
                case "14":
                    _reason = "Policy MM DLL MM";
                    break;
                case "15":
                    _reason = "Policy MM DLL ambiguity";
                    break;
                case "16":
                    _reason = "Wait for SUP";
                    break;
                case "17":
                    _reason = "Chain block";
                    break;
                case "18":
                    _reason = "NotPD";
                    break;
                case "19":
                    _reason = "Error";
                    break;
                case "20":
                    _reason = "Service MM";
                    break;
                default:
                    _reason = "Error Val";
                    break;
            }
        }

        /// <summary>
        /// get reason string
        /// </summary>
        /// <returns>string reason</returns>
        public override string GetData()
        {
            return _reason;
        }

        /// <summary>
        /// Set new reason string
        /// </summary>
        /// <param name="data"></param>
        public override void SetData(string data)
        {
            _reason = data;
        }
    }
}
