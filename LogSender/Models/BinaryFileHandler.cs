using LogSender.Models;
using System;
using System.IO;

namespace LogSender
{
    public class BinaryFileHandler
    {
        ///**********************************************
        ///             Members Section
        ///**********************************************
        //const section
        const int SIZE_OF_HEADER = 2;

        // Private section
        //private System.IO.FileStream _fs;
        private readonly int _fileLenght;
        private readonly byte[] _file;

        ///**********************************************
        ///             Functions Section
        ///**********************************************

        /// <summary>
        /// Ctor of BinaryFileHandler Class
        /// </summary>
        public BinaryFileHandler(FileInfo file)
        {
            _file = new byte[file.Length];
            _file = File.ReadAllBytes( file.FullName );
            _fileLenght = ( int ) file.Length;

            //old code 
            //_fs = new System.IO.FileStream( file.FullName , FileMode.Open );
            //_file = new byte[_fileLenght];
            //_fs.Read( _file , 0 , _fileLenght );
            //_fs.Close();
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
