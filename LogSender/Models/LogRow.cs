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
            StringBuilder svcTemp = new StringBuilder();

            if (_expandSVCHost.Count > 0)
            {
                for (int index = 0; index < _expandSVCHost.Count; index++)
                {
                    svcTemp.Append(_expandSVCHost[index]._appName + "," + _expandSVCHost[index]._fullPath + "," + _expandSVCHost[index]._status);

                    if (index + 1 != _expandSVCHost.Count)
                        svcTemp.Append("||");
                }
            }

            paramList.Add(svcTemp.ToString());
            string test = ServiceStack.Text.CsvSerializer.SerializeToCsv(paramList);

            dataAsString.Append(test);

            //dataAsString.Append(Environment.NewLine);
            return dataAsString;
        }
    }
}
