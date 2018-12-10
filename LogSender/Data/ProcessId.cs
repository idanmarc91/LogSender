
using System;

namespace LogSender.Data
{
    class ProcessId : FileData
    {
        ///**********************************************
        ///             Members Section
        ///**********************************************
        private const int PROCESS_ID_LEN = 4;
        private string _processId;

        ///**********************************************
        ///             Functions Section
        ///**********************************************

        /// <summary>
        /// get process id value
        /// </summary>
        public override string GetData()
        {
            return _processId;
        }
        /// <summary>
        /// extract data from binary file
        /// </summary>
        /// <param name="loopIndex"></param>
        /// <param name="expandedFileByteArray"></param>
        /// <param name="fileIndex"></param>
        public override void ExtractData(int loopIndex, byte[] expandedFileByteArray, ref int fileIndex)
        {
           byte[] data = GetData(loopIndex, expandedFileByteArray, ref fileIndex, PROCESS_ID_LEN);
           _processId = BitConverter.ToUInt32(data, 0).ToString();
        }

        /// <summary>
        /// set new process id value
        /// </summary>
        /// <param name="processId"></param>
        public override void SetData(string processId)
        {
            _processId = processId;
        }
    }
}
