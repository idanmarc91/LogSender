using System;

namespace BinaryFileToTextFile.Data
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
                    _reason = "OK";
                    break;
                case "1":
                    _reason = "NotWL";
                    break;
                case "2":
                    _reason = "UNKNOWN";
                    break;
                case "3":
                    _reason = "POL";
                    break;
                case "5":
                    _reason = "WLMM DATE";
                    break;
                case "6":
                    _reason = "WLMM SIZE";
                    break;
                case "7":
                    _reason = "WLMM MD5";
                    break;
                case "8":
                    _reason = "EXCLUDED";
                    break;
                case "9":
                    _reason = "FAKE";
                    break;
                case "10":
                    _reason = "CHAIN";
                    break;
                case "11":
                    _reason = "WLMM MD5 COR";
                    break;
                case "12":
                    _reason = "WLMM SIZE COR";
                    break;
                case "13":
                    _reason = "WFS";
                    break;
                case "14":
                    _reason = "WLMM DLL MM";
                    break;
                case "15":
                    _reason = "WLMM DLL AMBG";
                    break;
                case "16":
                    _reason = "WFS";
                    break;
                case "17":
                    _reason = "ChainBreak";
                    break;
                case "18":
                    _reason = "NotPD";
                    break;
                default:
                    _reason = "ERROR";
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
