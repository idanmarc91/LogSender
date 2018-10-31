using System;

namespace BinaryFileToTextFile.Data
{
    class TimeStamp 
    {
        ///**********************************************
        ///             Members Section
        ///**********************************************
        private const int TIME_STR_LEN = 8;

        private Int64 _serverClientDelta;
        private string _clientTimeStamp;
        private string _fullServerTimeStamp;

        ///**********************************************
        ///             Functions Section
        ///**********************************************

        /// <summary>
        /// Ctor of TimeStemp Class
        /// </summary>
        public TimeStamp(Int64 serverClientDelta)
        {
            _serverClientDelta = serverClientDelta;
        }

        /// <summary>
        /// This function extract data form binary file
        /// </summary>
        /// <param name="loopIndex"></param>
        /// <param name="expandedFileByteArray"></param>
        /// <param name="fileIndex"></param>
        public void ExtractData(int loopIndex, byte[] expandedFileByteArray, ref int fileIndex)
        {
            byte[] data = new byte[TIME_STR_LEN];

            for (int j = 0; j < TIME_STR_LEN; j++, fileIndex++)
                data[j] = expandedFileByteArray[loopIndex + fileIndex];

            Int64 ticksNumber = BitConverter.ToInt64(data, 0);
            var epochToAdd = new DateTime(1601, 1, 1, 0, 0, 0, DateTimeKind.Utc);
            ticksNumber = ticksNumber + epochToAdd.Ticks;
            DateTime time = new DateTime(ticksNumber).ToLocalTime();
            _clientTimeStamp = time.ToString("dd/MM HH:mm:ss");
            _fullServerTimeStamp = time.AddMilliseconds(_serverClientDelta).ToString("dd/MM HH:mm:ss.fff");
        }

        /// <summary>
        /// get client time
        /// </summary>
        /// <returns>string - client time</returns>
        public string GetClientTime()
        {
            return _clientTimeStamp;
        }

        /// <summary>
        /// get full server time
        /// </summary>
        /// <returns>string - full server time</returns>
        public string GetFullServerTime()
        {
            return _fullServerTimeStamp;
        }
    }
}
