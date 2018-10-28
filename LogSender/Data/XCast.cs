
namespace BinaryFileToTextFile.Data
{
    class XCast :FileData
    {
        const int X_CAST_LEN = 1;
        private string _xCast;

        /// <summary>
        /// This function extract x cast string 
        /// </summary>
        /// <param name="loopIndex"></param>
        /// <param name="expandedFileByteArray"></param>
        /// <param name="fileIndex"></param>
        public override void ExtractData(int loopIndex, byte[] expandedFileByteArray, ref int fileIndex)
        {
            System.Text.StringBuilder builder = new System.Text.StringBuilder();
            builder.Append(GetData(loopIndex, expandedFileByteArray, ref fileIndex, X_CAST_LEN)[0]);
            switch (builder.ToString())
            {
                case "0":
                    _xCast = "Unicast";
                    break;
                case "1":
                    _xCast = "NotUnicast";
                    break;
                default:
                    _xCast = "ERROR";
                    break;
            }
        }

        /// <summary>
        /// get X cast
        /// </summary>
        /// <returns>string - x cast</returns>
        public override string GetData()
        {
            return _xCast;
        }

        /// <summary>
        /// set xCast
        /// </summary>
        public override void SetData(string xCast)
        {
            _xCast = xCast;
        }
    }
}
