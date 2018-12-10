using System;

namespace LogSender.Data
{
    public class HeaderServerClientDelta
    {
        ///**********************************************
        ///             Members Section
        ///**********************************************

        private const int SERVER_CLIENT_DELTA_LEN = 8;
        private Int64 _serverClientDelta;

        ///**********************************************
        ///             Functions Section
        ///**********************************************

        /// <summary>
        /// This function extract the Server Client Delta from header file
        /// </summary>
        /// <param name="headerArray"></param>
        /// <param name="fileIndex"></param>
        public void ExtractData(byte[] headerArray, ref int fileIndex)
        {
            byte[] tempServerClientDelta = new byte[SERVER_CLIENT_DELTA_LEN];
            Array.Copy(headerArray, fileIndex, tempServerClientDelta, 0, SERVER_CLIENT_DELTA_LEN);

            //update curr index
            fileIndex += SERVER_CLIENT_DELTA_LEN;

            UInt64 UHeaderServerClientDelta = BitConverter.ToUInt64(tempServerClientDelta, 0);
            _serverClientDelta = unchecked((Int64)UHeaderServerClientDelta);
        }

        /// <summary>
        /// get server client delta data
        /// </summary>
        /// <returns>Int64 - server client delta</returns>
        public Int64 GetData()
        {
            return _serverClientDelta;
        }
    }
}
