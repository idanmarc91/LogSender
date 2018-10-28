
namespace BinaryFileToTextFile.Data
{
    class HeaderDllMode
    {
        ///**********************************************
        ///             Members Section
        ///**********************************************

        private byte _dllMode;

        ///**********************************************
        ///             Functions Section
        ///**********************************************

        /// <summary>
        /// this function extract data from header array
        /// </summary>
        /// <param name="headerArray"></param>
        /// <param name="fileIndex"></param>
        public void ExtractData(byte[] headerArray, ref int fileIndex)
        {
           _dllMode = headerArray[fileIndex++];
        }

        /// <summary>
        /// get dll mode data
        /// </summary>
        /// <returns>byte - dll mode </returns>
        public byte GetData()
        {
            return _dllMode;
        }
    }
}
