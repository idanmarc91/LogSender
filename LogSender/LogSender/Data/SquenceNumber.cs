using System;

namespace BinaryFileToTextFile.Data
{
    class SquenceNumber :FileData
    {
        ///**********************************************
        ///             Members Section
        ///**********************************************
        const int SEQ_NUM_LEN = 2;
        private string _sequenceNumber;


        ///**********************************************
        ///             Members Section
        ///**********************************************
        /// <summary>
        /// This function extract sequence Number
        /// </summary>
        /// <param name="loopIndex"></param>
        /// <param name="expandedFileByteArray"></param>
        /// <param name="fileIndex"></param>
        public override void ExtractData(int loopIndex, byte[] expandedFileByteArray, ref int fileIndex)
        {
            byte[] data = GetData(loopIndex, expandedFileByteArray, ref fileIndex, SEQ_NUM_LEN);
            _sequenceNumber = BitConverter.ToUInt16(data, 0).ToString();
        }

        /// <summary>
        /// get sequence number
        /// </summary>
        /// <returns>string - sequence number</returns>
        public override string GetData()
        {
            return _sequenceNumber;
        }

        /// <summary>
        /// set sequence number
        /// </summary>
        public override void SetData(string seqNum)
        {
            _sequenceNumber = seqNum;
        }

    }
}
