using System;

namespace LogSender
{
    internal class FileToSend
    {
        public string nameOfFile;
        public DateTime timeOfCreation;
        private byte[] bytedata;


        public byte[] GetByteData()
        {
            return bytedata;
        }
        public void SetByteData(byte[] bytearray)
        {
            bytedata = bytearray;
        }
    }
}