using System.Text;

namespace LogSender.Data
{
    class ApplicationName : FileData
    {
        private const int APP_NAME_STR_LEN = 512;
        private string _appName;

        /// <summary>
        /// This function extract app name name
        /// </summary>
        /// <param name="loopIndex"></param>
        /// <param name="expandedFileByteArray"></param>
        /// <param name="fileIndex"></param>
        public override void ExtractData(int loopIndex, byte[] expandedFileByteArray, ref int fileIndex)
        {
            byte[] data = GetData(loopIndex, expandedFileByteArray, ref fileIndex, APP_NAME_STR_LEN);
            _appName = (Encoding.Unicode.GetString(data)).TrimEnd('\0');
        }

        /// <summary>
        /// get app name data
        /// </summary>
        /// <returns>string - app name</returns>
        public override string GetData()
        {
            return _appName;
        }
        /// <summary>
        /// set application name
        /// </summary>
        public override void SetData(string appName )
        {
            _appName = appName;
        }
    }
}
