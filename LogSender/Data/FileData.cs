
using System;

namespace BinaryFileToTextFile.Data
{
    abstract class FileData
    {
        public abstract void ExtractData(int loopIndex, byte[] expandedFileByteArray, ref int fileIndex);
        public abstract string GetData();
        public abstract void SetData(string data);

        public byte[] GetData(int loopIndex, byte[] expandedFileByteArray, ref int fileIndex, int size)
        {
            byte[] data = new byte[size];

            Array.Copy(expandedFileByteArray, loopIndex + fileIndex, data, 0, size);

            fileIndex += size;

            return data;
        }
    }
}
