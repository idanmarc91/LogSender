

namespace LogSender.Data
{
    class Direction :FileData
    {
        const int DIRECTION_LEN = 1;
        private string _direction;

        /// <summary>
        /// This function extract direction
        /// </summary>
        /// <param name="loopIndex"></param>
        /// <param name="expandedFileByteArray"></param>
        /// <param name="fileIndex"></param>
        public override void ExtractData(int loopIndex, byte[] expandedFileByteArray, ref int fileIndex)
        {
            System.Text.StringBuilder builder = new System.Text.StringBuilder();
            builder.Append(GetData(loopIndex, expandedFileByteArray, ref fileIndex, DIRECTION_LEN)[0]);
            switch (builder.ToString())
            {
                case "0":
                    _direction = "Outbound";
                    break;
                case "1":
                    _direction = "Inbound";
                    break;
                default:
                    _direction = "Error";
                    break;
            }
        }

        /// <summary>
        /// get direction
        /// </summary>
        /// <returns>string - direction</returns>
        public override string GetData()
        {
            return _direction;
        }

        /// <summary>
        /// set direction
        /// </summary>
        public override void SetData(string direction)
        {
            _direction = direction;
        }
    }
}
