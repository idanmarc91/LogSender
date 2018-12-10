using System;

namespace LogSender.Data
{
    public class HeaderMacAddress
    {
        ///**********************************************
        ///             Members Section
        ///**********************************************

        private string _macAddress;

        ///**********************************************
        ///             Functions Section
        ///**********************************************

        /// <summary>
        /// This function extract the MAC Address from header file
        /// </summary>
        /// <param name="headerArray"></param>
        /// <param name="fileIndex"></param>
        public void ExtractData(byte[] headerArray, ref int fileIndex)
        {
            int MACAddressLenght = headerArray.Length - fileIndex - 1;
            byte[] MACHeaderAddress = new byte[MACAddressLenght];

            Array.Copy(headerArray, fileIndex, MACHeaderAddress, 0, MACAddressLenght);

            _macAddress = System.Text.Encoding.Default.GetString(MACHeaderAddress);
        }

        /// <summary>
        /// get mac address data
        /// </summary>
        /// <returns>string - mac address</returns>
        public string GetData()
        {
            return _macAddress;
        }
    }
}
