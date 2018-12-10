using System;

namespace LogSender.Data
{
    class FlowHandle :FileData
    {
        const int FLOW_HANDLE_STR_LEN = 8;
        private string _flowHandle;

        /// <summary>
        /// This function extract flow handle string 
        /// </summary>
        /// <param name="loopIndex"></param>
        /// <param name="expandedFileByteArray"></param>
        /// <param name="fileIndex"></param>
        public override void ExtractData(int loopIndex, byte[] expandedFileByteArray, ref int fileIndex)
        {
            byte[] data = GetData(loopIndex, expandedFileByteArray, ref fileIndex, FLOW_HANDLE_STR_LEN);
            UInt64 ConvertedFlowHandle = BitConverter.ToUInt64(data, 0);
            _flowHandle = ConvertedFlowHandle.ToString();
        }

        /// <summary>
        /// get flow handle
        /// </summary>
        /// <returns>string - flow handle</returns>
        public override string GetData()
        {
            return _flowHandle;
        }

        /// <summary>
        /// set flow handle
        /// </summary>
        public override void SetData(string flowHandle)
        {
            _flowHandle = flowHandle;
        }
    }
}
