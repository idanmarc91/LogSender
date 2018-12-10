using System.Text;

namespace LogSender.Data
{
    class DllPath : FileData
    {
        ///**********************************************
        ///             Members Section
        ///**********************************************
        const int IMG_PATH_STR_LEN = 1024;
        private string _dllPath;

        ///**********************************************
        ///             Functions Section
        ///**********************************************

        /// <summary>
        /// This function extract image path from binary file
        /// </summary>
        /// <param name="loopIndex"></param>
        /// <param name="expandedFileByteArray"></param>
        /// <param name="fileIndex"></param>
        public override void ExtractData(int loopIndex, byte[] expandedFileByteArray, ref int fileIndex)
        {
            byte [] data = GetData(loopIndex, expandedFileByteArray, ref fileIndex, IMG_PATH_STR_LEN);
            _dllPath = Encoding.Unicode.GetString(data).TrimEnd('\0');
        }

        /// <summary>
        /// get image path parameter function 
        /// </summary>
        /// <returns>string - image path</returns>
        public override string GetData()
        {
            return _dllPath;
        }

        /// <summary>
        /// set new image path
        /// </summary>
        /// <param name="dllPath"></param>
        public override void SetData(string dllPath)
        {
            _dllPath = dllPath;
        }
    }
}
