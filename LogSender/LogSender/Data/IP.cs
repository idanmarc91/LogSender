using System.Text;

namespace BinaryFileToTextFile.Data
{
    class IP : FileData
    {
        ///**********************************************
        ///             Members Section
        ///**********************************************
        private const int IP_LEN = 16;
        private string _addressFamily;
        private string _ip;

        ///**********************************************
        ///             Functions Section
        ///**********************************************
        ///empty ctor
        public IP()
        {}

        /// <summary>
        /// Ctor of IP Class
        /// </summary>
        /// <param name="addressFamily"></param>
        public IP(string addressFamily)
        {
            _addressFamily = addressFamily;
        }

        /// <summary>
        /// This function extract ip
        /// </summary>
        /// <param name="loopIndex"></param>
        /// <param name="expandedFileByteArray"></param>
        /// <param name="fileIndex"></param>
        public override void ExtractData(int loopIndex, byte[] expandedFileByteArray, ref int fileIndex)
        {
            StringBuilder builder = new StringBuilder();

            if (_addressFamily == "2")
            {
                _ip =  "Wrong address family flag!!!";
            }
            else if (_addressFamily == "1")
            {
                for (int j = 0; j < IP_LEN; j++)
                {
                    byte b = expandedFileByteArray[fileIndex + loopIndex];
                    if ((j % 2) != 0)
                    {
                        builder.Append(b + ":");
                    }
                    fileIndex++;
                }
                System.Net.IPAddress IPAdd = System.Net.IPAddress.Parse(builder.ToString().Remove(builder.ToString().Length - 1));
                _ip =  IPAdd.ToString();
            }
            else if (_addressFamily == "0")
            {
                for (int j = 0; j < IP_LEN; j++)
                {
                    byte b = expandedFileByteArray[fileIndex + loopIndex];
                    if (j < 4)
                        builder.Append(b + ".");
                    fileIndex++;
                }
                System.Net.IPAddress IPAdd = System.Net.IPAddress.Parse(builder.ToString().Remove(builder.ToString().Length - 1));
                _ip = IPAdd.ToString();
            }
        }

        /// <summary>
        /// get ip data
        /// </summary>
        /// <returns>string - ip</returns>
        public override string GetData()
        {
            return _ip;
        }

        /// <summary>
        /// set ip
        /// </summary>
        public override void SetData(string ip)
        {
            _ip = ip;
        }
    }
}
