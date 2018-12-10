using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LogSender.Data
{
    class DestinationPath : FileData
    {
        ///**********************************************
        ///             Members Section
        ///**********************************************
        private const int DESTANATION_PATH_STR_LEN = 1024;
        private string _destinationPath;

        ///**********************************************
        ///             Functions Section
        ///**********************************************
        ///
         
        /// <summary>
        /// extract destination path data from binary file
        /// </summary>
        /// <param name="loopIndex"></param>
        /// <param name="expandedFileByteArray"></param>
        /// <param name="fileIndex"></param>
        public override void ExtractData(int loopIndex, byte[] expandedFileByteArray, ref int fileIndex)
        {
            byte[] data = GetData(loopIndex, expandedFileByteArray, ref fileIndex, DESTANATION_PATH_STR_LEN);
            _destinationPath = (Encoding.Unicode.GetString(data)).TrimEnd('\0');
            _destinationPath = _destinationPath.Replace(@"Device\Mup", "");
        }

        /// <summary>
        /// get destination path
        /// </summary>
        /// <returns>string destination path</returns>
        public override string GetData()
        {
            return _destinationPath;
        }

        /// <summary>
        /// set new value to destination path
        /// </summary>
        /// <param name="data"></param>
        public override void SetData(string data)
        {
            _destinationPath = data;
        }
    }
}
