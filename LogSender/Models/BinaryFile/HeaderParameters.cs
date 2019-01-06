using LogSender.Data;

namespace LogSender.Models
{
    public class HeaderParameters
    {
        ///**********************************************
        ///             Members Section
        ///**********************************************
        public HeaderVersion _version = new HeaderVersion();
        public HeaderDllMode _dllMode = new HeaderDllMode();
        public HeaderClientZValue _clientZValue = new HeaderClientZValue();
        public HeaderServerClientDelta _serverClientDelta = new HeaderServerClientDelta();
        public HeaderReportingComputer _reportingComputer = new HeaderReportingComputer();
        public HeaderMacAddress _macAddress = new HeaderMacAddress();

        ///**********************************************
        ///             Functions Section
        ///**********************************************
        public void ExtractData(byte[] headerArray)
        {
            int fileIndex = 0;

            _version.ExtractData( headerArray , ref fileIndex );
            _dllMode.ExtractData( headerArray , ref fileIndex );
            _clientZValue.ExtractData( headerArray , ref fileIndex );
            _serverClientDelta.ExtractData( headerArray , ref fileIndex );
            _reportingComputer.ExtractData( headerArray , ref fileIndex );
            _macAddress.ExtractData( headerArray , ref fileIndex );
        }
    }
}
