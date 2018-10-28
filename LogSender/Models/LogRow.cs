using System.Collections.Generic;

namespace BinaryFileToTextFile.Models
{
    abstract class LogRow :Row
    {
        ///**********************************************
        ///             Members Section
        ///**********************************************
        protected string _hostName;
        protected ushort _headerVersion;
        protected List<ExpandSVCHostRow> _expandSVCHost;
    }
}
