using BinaryFileToTextFile.Models;
using System;
using System.IO;

namespace BinaryFileToTextFile
{
    public class BinaryFileHandler
    {
        ///**********************************************
        ///             Members Section
        ///**********************************************
        //const section
        const int SIZE_OF_HEADER = 2;

        // Private section
        private System.IO.FileStream _fs;
        private int _fileLenght;
        private byte[] _file;

        ///**********************************************
        ///             Functions Section
        ///**********************************************

        /// <summary>
        /// Ctor of BinaryFileHandler Class
        /// </summary>
        public BinaryFileHandler(string filePath)
        {

            _fs = new System.IO.FileStream(filePath, FileMode.Open);

            _fileLenght = (int)_fs.Length;
            _file = new byte[_fileLenght];
            _fs.Read(_file, 0, _fileLenght);

        }

        /// <summary>
        /// This function separate the header section and the data section from the binary file
        /// </summary>
        /// <returns></returns>
        public BinaryFileData SeparateHeaderAndFile()
        {
            BinaryFileData data = new BinaryFileData();

            //convert size to uint16
            UInt16 headerSize = GetHeaderSize();

            data._headerArray = new byte[headerSize - 2];

            //copy bits from buffer to header array depend on size of header
            for (int i = 0; i < headerSize - 2; i++)
                data._headerArray[i] = _file[i + 2];

            //Array.Copy(_file, 0, data._headerArray, 0, headerSize);

            data._logArray = new byte[_fileLenght - headerSize];

            Array.Copy(_file, headerSize , data._logArray, 0, data._logArray.Length);

            return data;
        }

        /// <summary>
        /// This function extract header size from binary file
        /// </summary>
        /// <returns></returns>
        public ushort GetHeaderSize()
        {
            byte[] tempHeaderSize = new byte[SIZE_OF_HEADER];

            //Check the header size. first two byte are header size
            for (int i = 0; i < SIZE_OF_HEADER; i++)
                tempHeaderSize[i] = _file[i];

            return BitConverter.ToUInt16(tempHeaderSize, 0);
        }


        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public bool IsFileNull()
        {
            return ( _file == null );
        }
    }
}
