using System;

namespace LogSender.Data
{
    class Port : FileData
    {
        ///**********************************************
        ///             Members Section
        ///**********************************************
        private const int SRC_PORT_LEN = 2;
        private string _port;

        ///**********************************************
        ///             Functions Section
        ///**********************************************

        /// <summary>
        /// This function extract port
        /// </summary>
        /// <param name="loopIndex"></param>
        /// <param name="expandedFileByteArray"></param>
        /// <param name="fileIndex"></param>
        public override void ExtractData(int loopIndex, byte[] expandedFileByteArray, ref int fileIndex)
        {
            byte[] data = GetData(loopIndex, expandedFileByteArray, ref fileIndex, SRC_PORT_LEN);
            UInt16 ConvertedSourcePort = BitConverter.ToUInt16(data, 0);
            _port = ConvertedSourcePort.ToString();
        }

        /// <summary>
        /// Get  Port
        /// </summary>
        /// <returns>string - source port</returns>
        public override string GetData()
        {
            return _port;
        }

        /// <summary>
        /// set port
        /// </summary>
        public override void SetData(string port )
        {
            _port = port;
        }
    }
}
