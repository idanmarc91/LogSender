using System.Collections.Generic;

namespace LogSender.Models
{
    public class BinaryFileData
    {
        ///**********************************************
        ///             Members Section
        ///**********************************************
        public byte[] _headerArray;
        public byte[] _logArray;

        ///**********************************************
        ///             Functions Section
        ///**********************************************
        
        /// <summary>
        /// This function expend the log section: expend the '0' values 
        /// </summary>
        /// <returns> byte array of uncompressed log data </returns>
        public byte[] ExpendLogSection()
        {
            int i;
            try
            {
                List<byte> newFile = new List<byte>();

                for (i = 0; i < _logArray.Length  ; i++)
                {
                    if (_logArray[i] != 00)
                    {
                        newFile.Add(_logArray[i]);
                    }
                    else if(i + 1 != _logArray.Length)
                    {
                        
                        int addSize = _logArray[i + 1]; //the number of zeros

                        while (addSize-- != 0)
                        {
                            newFile.Add(0);
                        }
                        i++;
                    }
                }
                return newFile.ToArray();
            }
            catch(System.Exception ex)
            {
                return new byte[5];
            }
        }
    }
}
