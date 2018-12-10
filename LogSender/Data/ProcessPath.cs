using System.Text;

namespace LogSender.Data
{
    class ProcessPath : FileData
    {
        ///**********************************************
        ///             Members Section
        ///**********************************************
        private const int PROCESS_PATH_STR_LEN = 1024;
        private string _processName;
        private string _processPath;

        ///**********************************************
        ///             Functions Section
        ///**********************************************
        
        /// <summary>
        /// ctor of process path class
        /// </summary>
        /// <param name="processName"></param>
        public ProcessPath(string processName)
        {
            _processName = processName;
        }
        
        //empty ctor
        public ProcessPath()
        {}

        /// <summary>
        /// extract process path data from binary file
        /// </summary>
        /// <param name="loopIndex"></param>
        /// <param name="expandedFileByteArray"></param>
        /// <param name="fileIndex"></param>
        public override void ExtractData(int loopIndex, byte[] expandedFileByteArray, ref int fileIndex)
        {
            byte[] data = GetData(loopIndex, expandedFileByteArray, ref fileIndex, PROCESS_PATH_STR_LEN);
            _processPath = (Encoding.Unicode.GetString(data)).TrimEnd('\0');
            _processPath = _processPath.Replace(_processName, "");
        }

        /// <summary>
        /// get process path value
        /// </summary>
        /// <returns>string - process path</returns>
        public override string GetData()
        {
            return _processPath;
        }

        /// <summary>
        /// set new value to process path
        /// </summary>
        /// <param name="data"></param>
        public override void SetData(string data)
        {
            _processPath = data;
        }
    }
}
