namespace LogSender.Data
{
    class StatusFsa : FileData
    {
        ///**********************************************
        ///             Members Section
        ///**********************************************
        private const int STATUS_FSA_LEN = 1;
        private string _status;

        ///**********************************************
        ///             Functions Section
        ///**********************************************

        /// <summary>
        /// This function extract status string
        /// </summary>
        /// <param name="loopIndex"></param>
        /// <param name="expandedFileByteArray"></param>
        /// <param name="fileIndex"></param>
        public override void ExtractData(int loopIndex, byte[] expandedFileByteArray, ref int fileIndex)
        {
            System.Text.StringBuilder builder = new System.Text.StringBuilder();
            builder.Append(GetData(loopIndex, expandedFileByteArray, ref fileIndex, STATUS_FSA_LEN)[0]);
            switch (builder.ToString())
            {
                case "1":
                    _status = "Ok";
                    break;
                case "2":
                    _status = "Blocked";
                    break;
                default:
                    _status = "Error";
                    _status = builder.ToString();
                    break;
            }

        }
        /// <summary>
        /// get status data
        /// </summary>
        /// <returns>string status</returns>
        public override string GetData()
        {
            return _status;
        }

        /// <summary>
        /// set new status string
        /// </summary>
        /// <param name="data"></param>
        public override void SetData(string data)
        {
            _status  = data;
        }
    }
}
