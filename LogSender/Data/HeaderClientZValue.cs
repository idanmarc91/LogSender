using System;

namespace BinaryFileToTextFile.Data
{
    class HeaderClientZValue
    {
        ///**********************************************
        ///             Members Section
        ///**********************************************

        private const int CLIENT_Z_VALUE_LEN = 8;
        private Int64 _clientZValue;

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
            byte[] tempClientZValue = new byte[CLIENT_Z_VALUE_LEN];

            Array.Copy(headerArray, fileIndex, tempClientZValue, 0, CLIENT_Z_VALUE_LEN);
            //update curr index
            fileIndex += CLIENT_Z_VALUE_LEN;

            UInt64 UHeaderClientZValue = BitConverter.ToUInt64(tempClientZValue, 0);
            _clientZValue = unchecked((Int64)UHeaderClientZValue);
        }

        /// <summary>
        /// get client z value data
        /// </summary>
        /// <returns>Int64 - client z value</returns>
        public Int64 GetData()
        {
            return _clientZValue;
        }
    }
}
