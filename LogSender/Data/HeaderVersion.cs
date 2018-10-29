using System;

namespace BinaryFileToTextFile.Data
{
    public class HeaderVersion
    {
        ///**********************************************
        ///             Members Section
        ///**********************************************

        private const int HEADER_VERSION_LEN = 2;
        private UInt16 _headerVersion;

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
            byte[] data = new byte[HEADER_VERSION_LEN];
            Array.Copy(headerArray,fileIndex, data, 0, HEADER_VERSION_LEN);

            //update curr index
            fileIndex += HEADER_VERSION_LEN;
            _headerVersion = BitConverter.ToUInt16(data, 0);
        }

        /// <summary>
        /// get header version data
        /// </summary>
        /// <returns>UInt16 - header version</returns>
        public UInt16 GetData()
        {
            return _headerVersion;
        }
    }
}
