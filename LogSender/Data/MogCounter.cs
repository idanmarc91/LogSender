using System;

namespace LogSender.Data
{
    class MogCounter : FileData
    {
        ///**********************************************
        ///             Members Section
        ///**********************************************
        const int MOG_COUNTER_LEN = 2;
        private string _mogCounter;

        ///**********************************************
        ///             Functions Section
        ///**********************************************
        
        /// <summary>
        /// This function extract mog counter
        /// </summary>
        /// <param name="loopIndex"></param>
        /// <param name="expandedFileByteArray"></param>
        /// <param name="fileIndex"></param>
        public override void ExtractData(int loopIndex, byte[] expandedFileByteArray, ref int fileIndex)
        {
            byte[] data = GetData(loopIndex, expandedFileByteArray, ref fileIndex, MOG_COUNTER_LEN);
            _mogCounter = BitConverter.ToUInt16(data, 0).ToString();
            fileIndex += 6; // ignore 6 byte 
        }

        /// <summary>
        /// get mog counter
        /// </summary>
        /// <returns>string - mog counter</returns>
        public override string GetData()
        {
            return _mogCounter;
        }

        /// <summary>
        /// set new mog counter
        /// </summary>
        /// <param name="data"></param>
        public override void SetData(string data)
        {
            _mogCounter = data;
        }
    }
}
