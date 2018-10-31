using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BinaryFileToTextFile.Data
{
    class ParentPath : FileData
    {
        ///**********************************************
        ///             Members Section
        ///**********************************************
        const int PARANT_PATH_STR_LEN = 1024;
        private string _parentPath;

        ///**********************************************
        ///             Functions Section
        ///**********************************************

        /// <summary>
        /// This functio extract the parent path from binary file
        /// </summary>
        /// <param name="loopIndex"></param>
        /// <param name="expandedFileByteArray"></param>
        /// <param name="fileIndex"></param>
        public override void ExtractData(int loopIndex, byte[] expandedFileByteArray, ref int fileIndex)
        {
            byte[] data = GetData(loopIndex, expandedFileByteArray, ref fileIndex, PARANT_PATH_STR_LEN);
            _parentPath = Encoding.Unicode.GetString(data).TrimEnd('\0');
        }

        /// <summary>
        /// Get parent path data
        /// </summary>
        /// <returns>string - parent path</returns>
        public override string GetData()
        {
            return _parentPath;
        }

        /// <summary>
        /// this function set new value to parent path
        /// </summary>
        /// <param name="parentPath"></param>
        public override void SetData(string parentPath)
        {
            _parentPath = parentPath;
        }
    }
}
