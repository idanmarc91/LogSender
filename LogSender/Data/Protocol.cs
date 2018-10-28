
namespace BinaryFileToTextFile.Data
{
    class Protocol : FileData
    {
        private const int PROTOCOL_LEN = 1;

        private string _protocol;

        /// <summary>
        /// this function extract protocol string
        /// </summary>
        /// <param name="loopIndex"></param>
        /// <param name="expandedFileByteArray"></param>
        /// <param name="fileIndex"></param>
        public override void ExtractData(int loopIndex, byte[] expandedFileByteArray, ref int fileIndex)
        {
            System.Text.StringBuilder builder = new System.Text.StringBuilder();
            builder.Append(GetData(loopIndex, expandedFileByteArray, ref fileIndex, PROTOCOL_LEN)[0]);
            switch (builder.ToString())
            {
                case "1":
                    _protocol = "ICMP";
                    break;
                case "2":
                    _protocol = "IGMP";
                    break;
                case "3":
                    _protocol = "GGP";
                    break;
                case "4":
                    _protocol = "IPV4";
                    break;
                case "5":
                    _protocol = "ST";
                    break;
                case "6":
                    _protocol = "TCP";
                    break;
                case "7":
                    _protocol = "CBT";
                    break;
                case "8":
                    _protocol = "EGP";
                    break;
                case "9":
                    _protocol = "IGP";
                    break;
                case "12":
                    _protocol = "PUP";
                    break;
                case "17":
                    _protocol = "UDP";
                    break;
                case "22":
                    _protocol = "IDP";
                    break;
                case "27":
                    _protocol = "RDP";
                    break;
                case "41":
                    _protocol = "IPV6";
                    break;
                case "43":
                    _protocol = "ROUTING";
                    break;
                case "44":
                    _protocol = "FRAGMENT";
                    break;
                case "50":
                    _protocol = "ESP";
                    break;
                case "51":
                    _protocol = "AH";
                    break;
                case "58":
                    _protocol = "ICMPV6";
                    break;
                case "59":
                    _protocol = "NONE";
                    break;
                case "60":
                    _protocol = "DSTOPTS";
                    break;
                case "77":
                    _protocol = "ND";
                    break;
                case "78":
                    _protocol = "ICLFXBM";
                    break;
                case "103":
                    _protocol = "PIM";
                    break;
                case "113":
                    _protocol = "PGM";
                    break;
                case "115":
                    _protocol = "L2TP";
                    break;
                case "132":
                    _protocol = "SCTP";
                    break;
                case "255":
                    _protocol = "RAW";
                    break;
                case "256":
                    _protocol = "MAX";
                    break;
                default:
                    _protocol = "ERROR!!!";
                    break;
            }
        }

        /// <summary>
        ///  Get data protocol
        /// </summary>
        /// <returns>string - protocol </returns>
        public override string GetData()
        {
            return _protocol;
        }

        /// <summary>
        /// set protocol
        /// </summary>
        public override void SetData(string protocol)
        {
            _protocol = protocol;
        }
    }
}
