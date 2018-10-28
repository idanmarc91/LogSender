

namespace BinaryFileToTextFile.Data
{
    class AddressFamily:FileData
    {
        private const int ADD_FAM_LEN = 1;
        private string _addressFamily;

        /// <summary>
        /// This function extract address family
        /// </summary>
        /// <param name="loopIndex"></param>
        /// <param name="expandedFileByteArray"></param>
        /// <param name="fileIndex"></param>
        public override void ExtractData(int loopIndex, byte[] expandedFileByteArray, ref int fileIndex)
        {
            System.Text.StringBuilder builder = new System.Text.StringBuilder();
            builder.Append(GetData(loopIndex, expandedFileByteArray, ref fileIndex, ADD_FAM_LEN)[0]);
            switch (builder.ToString())
            {
                case "2":
                    _addressFamily = "0";
                    break;
                case "23":
                    _addressFamily = "1";
                    break;
                default:
                    _addressFamily = "2";
                    break;
            }
        }

        /// <summary>
        /// Get  _addressFamily
        /// </summary>
        /// <returns>string - address family</returns>
        public override string GetData()
        {
            return _addressFamily;
        }

        /// <summary>
        /// set address family
        /// </summary>
        public override void SetData(string addressFamily)
        {
            _addressFamily = addressFamily;
        }
    }
}
