using System;
using System.Text;

namespace BinaryFileToTextFile.Data
{
    class HeaderHostName
    {
        ///**********************************************
        ///             Members Section
        ///**********************************************

        private string _hostName;

        ///**********************************************
        ///             Functions Section
        ///**********************************************

        /// <summary>
        /// This function extract the Host name from header file
        /// </summary>
        /// <param name="headerArray"></param>
        /// <param name="fileIndex"></param>
        public void ExtractData(byte[] headerArray, ref int fileIndex)
        {
            int hostNameLenght = 0;
            int tempIndex = fileIndex;
            while (!((headerArray[tempIndex] == '\0') && (headerArray[tempIndex + 1] == '\0')))
            {
                hostNameLenght = hostNameLenght + 2;
                tempIndex = tempIndex + 2;
            }

            byte[] tempHostName = new byte[hostNameLenght];

            Array.Copy(headerArray, fileIndex, tempHostName, 0, hostNameLenght);

            //update curr index
            fileIndex += hostNameLenght + 2;
            _hostName = Encoding.Unicode.GetString(tempHostName);
        }

        /// <summary>
        /// get host name data
        /// </summary>
        /// <returns>string - host name</returns>
        public string GetData()
        {
            return _hostName;
        }
    }
}
