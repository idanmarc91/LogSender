using System;
using System.Text;

namespace LogSender.Data
{
    public class HeaderReportingComputer
    {
        ///**********************************************
        ///             Members Section
        ///**********************************************

        private string _reportingComputer;

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
            int reportingComputerLen = 0;
            int tempIndex = fileIndex;
            while (!((headerArray[tempIndex] == '\0') && (headerArray[tempIndex + 1] == '\0')))
            {
                reportingComputerLen = reportingComputerLen + 2;
                tempIndex = tempIndex + 2;
            }

            byte[] tempReportingComputer = new byte[reportingComputerLen];

            Array.Copy(headerArray, fileIndex, tempReportingComputer, 0, reportingComputerLen);

            //update curr index
            fileIndex += reportingComputerLen + 2;
            _reportingComputer = Encoding.Unicode.GetString(tempReportingComputer);
        }

        /// <summary>
        /// get reporting computer data
        /// </summary>
        /// <returns>string - _reportingComputer</returns>
        public string GetData()
        {
            return _reportingComputer;
        }
    }
}
