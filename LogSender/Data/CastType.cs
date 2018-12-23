using LogSender.Utilities;
using System;
using System.Net;

namespace LogSender.Data
{
    internal class CastType : FileData
    {
        ///**********************************************
        ///             Members Section
        ///**********************************************
        private const int CAST_TYPE_LEN = 1;

        private string _castType;

        ///**********************************************
        ///             Functions Section
        ///**********************************************

        /// <summary>
        /// This function extract x cast string
        /// </summary>
        /// <param name="loopIndex"></param>
        /// <param name="expandedFileByteArray"></param>
        /// <param name="fileIndex"></param>
        public override void ExtractData(int loopIndex, byte[] expandedFileByteArray, ref int fileIndex)
        {
            System.Text.StringBuilder builder = new System.Text.StringBuilder();
            builder.Append(GetData(loopIndex, expandedFileByteArray, ref fileIndex, CAST_TYPE_LEN)[0]);
            switch (builder.ToString())
            {
                case "0":
                    _castType = "Unicast";
                    break;

                case "1":
                    _castType = "NotUnicast";
                    break;

                default:
                    _castType = "ERROR";
                    break;
            }
        }

        /// <summary>
        /// get X cast
        /// </summary>
        /// <returns>string - x cast</returns>
        public override string GetData()
        {
            return _castType;
        }

        /// <summary>
        /// set xCast
        /// </summary>
        public override void SetData(string xCast)
        {
            _castType = xCast;
        }


        #region Future features

        /// <summary>
        /// The agent provide us with not accurate data about the cast type so we calculate it from
        /// the destination ip addres
        /// </summary>
        /// <param name="destIpAddress"></param>
        internal static void CalcTypeFromDestIp(string destIpAddress)
        {
            try
            {
                IPAddress address;

                //check if ip address is IPV6 or IPV4
                if (IPAddress.TryParse(destIpAddress, out address))
                {
                    switch (address.AddressFamily)
                    {
                        case System.Net.Sockets.AddressFamily.InterNetwork:
                            if (IsIpBroadcast(destIpAddress))
                            {

                            }
                            else if (IsIpMulticast(destIpAddress))
                            {

                            }
                            else
                            {
                                //unicast
                            }

                            break;

                        case System.Net.Sockets.AddressFamily.InterNetworkV6:
                            break;
                    }
                }
            }
            catch (Exception ex)
            {
            }
        }


        
        public static bool IsIpBroadcast(string destIpAddress)
        {
            IPAddress subNetMask = IpAddressCalculator.GetSubnetMask(IPAddress.Parse("255.255.255.0"));
            throw new NotImplementedException();
        }

        public static bool IsIpMulticast(string ip)
        {
            string[] ipArr = ip.Split('.');
            int i = int.Parse(ipArr[0]);

            return (i >= 224 && i <= 239) ? true : false;
        }

        #endregion
    }
}