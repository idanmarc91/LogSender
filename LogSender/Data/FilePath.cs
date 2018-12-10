using System.Text;

namespace LogSender.Data
{
    class FilePath : FileData
    {
        ///**********************************************
        ///             Members Section
        ///**********************************************
        private const int FILE_PATH_STR_LEN = 1024;
        private string _filePath;


        ///**********************************************
        ///             Functions Section
        ///**********************************************
        /// <summary>
        /// This function extract file path string 
        /// </summary>
        /// <param name="loopIndex"></param>
        /// <param name="expandedFileByteArray"></param>
        /// <param name="fileIndex"></param>
        public override void ExtractData(int loopIndex, byte[] expandedFileByteArray, ref int fileIndex)
        {
            byte[] data = GetData(loopIndex, expandedFileByteArray, ref fileIndex, FILE_PATH_STR_LEN);
            _filePath = (Encoding.Unicode.GetString(data)).TrimEnd('\0');
        }

        /// <summary>
        /// get file path 
        /// </summary>
        /// <returns>string - file path</returns>
        public override string GetData()
        {
            return _filePath;
        }

        /// <summary>
        /// set file path
        /// </summary>
        public override void SetData(string filePath)
        {
            _filePath = filePath;
        }
    }
}
