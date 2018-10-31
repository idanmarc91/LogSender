using System.Text;

namespace BinaryFileToTextFile.Data
{
    class ProcessName : FileData
    {
        ///**********************************************
        ///             Members Section
        ///**********************************************
        const int PROCESS_NAME_LEN = 512;
        private string _processName;

        ///**********************************************
        ///             Functions Section
        ///**********************************************

        /// <summary>
        ///  Extract process name fron binary file
        /// </summary>
        /// <param name="loopIndex"></param>
        /// <param name="expandedFileByteArray"></param>
        /// <param name="fileIndex"></param>
        public override void ExtractData(int loopIndex, byte[] expandedFileByteArray, ref int fileIndex)
        {
            byte[] data = GetData(loopIndex, expandedFileByteArray, ref fileIndex, PROCESS_NAME_LEN);
            _processName = (Encoding.Unicode.GetString(data)).TrimEnd('\0');
        }

        /// <summary>
        /// get process name
        /// </summary>
        /// <returns>string - process name</returns>
        public override string GetData()
        {
            return _processName;
        }

        /// <summary>
        /// set new process name
        /// </summary>
        /// <param name="data"></param>
        public override void SetData(string data)
        {
            _processName = data;
        }
    }
}
