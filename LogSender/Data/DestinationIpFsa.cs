using System;
using System.Net;
using System.Net.Sockets;
using System.Text.RegularExpressions;

namespace LogSender.Data
{
    public class DestinationIpFsa
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger("DestinationIpFsa.cs");

        public string _destinationIp { get; private set; }

        /// <summary>
        /// Ctor
        /// </summary>
        public DestinationIpFsa(string path, string sourceIp)
        {
            string destHostName = ExtractHostNameFromDestPath(path);
            _destinationIp = ResolveHostName(destHostName);
            if(_destinationIp == "")
            {
                _destinationIp = sourceIp;
            }
        }

        private string ExtractHostNameFromDestPath(string path)
        {
            string pattern = @"[^\\][\w]*[^\\]";
            return Regex.Match(path, pattern).Value;
        }

        private string ResolveHostName(string hostName)
        {
            try
            {
                IPHostEntry hostEntry;

                switch (hostName)
                {
                    case "localhost":
                        return "";

                    case ";Csc":
                        return "";

                    case "\\":
                        return "";

                    default:
                        hostEntry = Dns.GetHostEntry(hostName);
                        break;
                }

                IPAddress[] ips = hostEntry.AddressList;
                return ips[ips.Length - 1].ToString();
            }
            catch(SocketException ex)
            {
                log.Warn(ex.Message);
                return ex.Message;
            }
            catch(Exception ex)
            {
                log.Error("Error has occurred while resolving the host name", ex);
                return "Error while resolving";
            }
        }
    }
}