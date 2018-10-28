
namespace BinaryFileToTextFile.Data
{
    class ReasonDll : FileData
    {
        private const int REASON_DLL_LEN = 1;

        private string _reason;

        public override void ExtractData(int loopIndex, byte[] expandedFileByteArray, ref int fileIndex)
        {
            System.Text.StringBuilder builder = new System.Text.StringBuilder();
            builder.Append(GetData(loopIndex, expandedFileByteArray, ref fileIndex, REASON_DLL_LEN)[0]);
            switch (builder.ToString())
            {
                case "0":
                    _reason = "OK";
                    break;
                case "1":
                    _reason = "NotWL";
                    break;
                case "2":
                    _reason = "WLMM DATE";
                    break;
                case "3":
                    _reason = "WLMM SIZE";
                    break;
                case "4":
                    _reason = "WLMM MD5";
                    break;
                case "5":
                    _reason = "WLMM MD5 COR";
                    break;
                case "6":
                    _reason = "WLMM SIZE COR";
                    break;
                case "7":
                    _reason = "WLMM DLL MM";
                    break;
                case "8":
                    _reason = "WLMM DLL AMBG";
                    break;
                case "9":
                    _reason = "Chain";
                    break;
                case "10":
                    _reason = "Error";
                    break;
                case "11":
                    _reason = "WFS";
                    break;
                case "12":
                    _reason = "NoCheck";
                    break;
                case "13":
                    _reason = "Other";
                    break;
                default:
                    _reason = "ERROR VAL";
                    break;

            }
        }

        public override string GetData()
        {
            return _reason;
        }

        public override void SetData(string reason)
        {
            _reason = reason;
        }
    }
}
