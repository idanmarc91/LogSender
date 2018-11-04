using System;
using System.Collections.Generic;
using System.Text;

namespace BinaryFileToTextFile.Models
{
    class LogRow :Row
    {
        ///**********************************************
        ///             Members Section
        ///**********************************************
        protected string _hostName;
        protected ushort _headerVersion;
        protected List<ExpandSVCHostRow> _expandSVCHost;


        public override StringBuilder BuildAsCsv(List<string> paramList)
        {
            StringBuilder dataAsString = new StringBuilder();
            dataAsString.Append(String.Join(",", paramList));

            dataAsString.Append(",");

            if (_expandSVCHost.Count > 0)
            {
                StringBuilder svcTemp = new StringBuilder();
                //svcTemp.Append("\"");
                for (int index = 0; index < _expandSVCHost.Count; index++)
                {
                    svcTemp.Append(_expandSVCHost[index]._appName + "," + _expandSVCHost[index]._fullPath + "," + _expandSVCHost[index]._status);

                    if (index + 1 != _expandSVCHost.Count)
                        svcTemp.Append("||");
                }
                //svcTemp.Append("\"");
                dataAsString.Append(svcTemp);
            }
            else
                dataAsString.Append(",");
            dataAsString.Append("\n ");
            return dataAsString;
        }
    }
}
