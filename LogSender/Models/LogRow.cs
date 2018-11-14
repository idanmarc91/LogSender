using System.Collections.Generic;
using System.Text;

namespace BinaryFileToTextFile.Models
{
    class LogRow : Row
    {
        ///**********************************************
        ///             Members Section
        ///**********************************************
        protected string _hostName;
        protected ushort _headerVersion;
        protected List<ExpandSVCHostRow> _expandSVCHost;

        ///**********************************************
        ///             Functions Section
        ///**********************************************

        /// <summary>
        /// This function build the row data as csv string format
        /// </summary>
        /// <param name="paramList"></param>
        /// <returns></returns>
        public override StringBuilder BuildAsCsv(List<string> paramList)
        {
            StringBuilder dataAsString = new StringBuilder();

            if (_expandSVCHost.Count > 0)
            {
                paramList.Add(BuildSvcChainString());
            }
            else
            {
                paramList.Add("");
            }

            string test = ServiceStack.Text.CsvSerializer.SerializeToCsv(paramList);

            dataAsString.Append(test);

            return dataAsString;
        }

        /// <summary>
        /// This function build special string for svc chain string
        /// </summary>
        /// <returns>string - svc chain string  </returns>
        private string BuildSvcChainString()
        {
            StringBuilder svcTemp = new StringBuilder();

            for (int index = 0; index < _expandSVCHost.Count; index++)
            {
                svcTemp.Append(_expandSVCHost[index]._appName + "," + _expandSVCHost[index]._fullPath + "," + _expandSVCHost[index]._status);

                if (index + 1 != _expandSVCHost.Count)
                {
                    svcTemp.Append("||");
                }
            }
            return svcTemp.ToString();
        }
    }
}
