
namespace LogSender.Data
{
    class FlowState : FileData
    {
        private const int FLOW_STATE_STR_LEN = 1;
        private string _flowState;

        /// <summary>
        /// This function extract flow state string 
        /// </summary>
        /// <param name="loopIndex"></param>
        /// <param name="expandedFileByteArray"></param>
        /// <param name="fileIndex"></param>
        public override void ExtractData(int loopIndex, byte[] expandedFileByteArray, ref int fileIndex)
        {
            System.Text.StringBuilder builder = new System.Text.StringBuilder();
            builder.Append(GetData(loopIndex, expandedFileByteArray, ref fileIndex, FLOW_STATE_STR_LEN)[0]);
            switch (builder.ToString())
            {
                case "1":
                    _flowState = "START";
                    break;
                case "2":
                    _flowState = "END";
                    break;
                default:
                    _flowState = "UNSPECIFIED";
                    break;
            }
        }

        /// <summary>
        /// get flow handle
        /// </summary>
        /// <returns>string - flow state</returns>
        public override string GetData()
        {
            return _flowState;
        }

        /// <summary>
        /// set flow state
        /// </summary>
        public override void SetData(string flowState)
        {
            _flowState = flowState;
        }
    }
}
