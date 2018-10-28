using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BinaryFileToTextFile.Data
{
    class ParentPath : FileData
    {
        const int PARANT_PATH_STR_LEN = 1024;

        private string _parentPath;

        public override void ExtractData(int loopIndex, byte[] expandedFileByteArray, ref int fileIndex)
        {
            byte[] data = GetData(loopIndex, expandedFileByteArray, ref fileIndex, PARANT_PATH_STR_LEN);
            _parentPath = Encoding.Unicode.GetString(data).TrimEnd('\0');
        }

        public override string GetData()
        {
            return _parentPath;
        }
        public override void SetData(string parentPath)
        {
            _parentPath = parentPath;
        }
    }
}
