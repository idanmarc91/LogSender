namespace LogSender.Data
{
    class StatusLog : FileData
    {
        ///**********************************************
        ///             Members Section
        ///**********************************************
        private const int STATUS_LEN = 1;
        private string _status;

        ///**********************************************
        ///             Functions Section
        ///**********************************************

        /// <summary>
        /// This function create status string
        /// </summary>
        /// <param name="loopIndex"></param>
        /// <param name="expandedFileByteArray"></param>
        /// <param name="fileIndex"></param>
        public override void ExtractData(int loopIndex, byte[] expandedFileByteArray, ref int fileIndex)
        {
            System.Text.StringBuilder builder = new System.Text.StringBuilder();
            builder.Append(GetData(loopIndex, expandedFileByteArray, ref fileIndex, STATUS_LEN)[0]);
            switch (builder.ToString())
            {
                case "1":
                    _status = "OK";
                    break;
                case "2":
                    _status = "NotWL";
                    break;
                case "3":
                    _status = "Exculuded";
                    break;
                case "4":
                    _status = "Off";
                    break;
                default:
                    _status = builder.ToString();
                    break;
            }
        }

        /// <summary>
        /// Get data _status
        /// </summary>
        /// <returns>string - status</returns>
        public override string GetData()
        {
            return _status;
        }

        /// <summary>
        /// set status
        /// </summary>
        public override void SetData(string status)
        {
            _status = status;
        }
    }
}
