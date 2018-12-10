using System.Text;

namespace LogSender.Data
{
    class UserName :FileData
    {
        ///**********************************************
        ///             Members Section
        ///**********************************************
        private const int USER_NAME_STR_LEN = 512;
        private string _userName;
        private ushort _headerVersion;

        ///**********************************************
        ///             Functions Section
        ///**********************************************

        /// <summary>
        /// Ctor of User Name class
        /// </summary>
        /// <param name="headerVersion"></param>
        public UserName(ushort headerVersion)
        {
            _headerVersion = headerVersion;
        }
        /// <summary>
        /// This function extract user name
        /// </summary>
        /// <param name="loopIndex"></param>
        /// <param name="expandedFileByteArray"></param>
        /// <param name="fileIndex"></param>
        public override void ExtractData(int loopIndex, byte[] expandedFileByteArray, ref int fileIndex)
        {
            //fileIndex += 16;
            if (_headerVersion > 2)
            {
                byte[] data = GetData(loopIndex, expandedFileByteArray, ref fileIndex, USER_NAME_STR_LEN);
                _userName = (Encoding.Unicode.GetString(data));
                _userName = _userName.Trim('\0');
            }
            else
                _userName = "";
        }

        /// <summary>
        /// get user name
        /// </summary>
        /// <returns>string - user name</returns>
        public override string GetData()
        {
            return _userName;
        }

        /// <summary>
        /// set user name
        /// </summary>
        public override void SetData(string userName)
        {
            _userName = userName;
        }
    }
}
