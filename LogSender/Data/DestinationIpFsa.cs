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
        public DestinationIpFsa(string path)
        {
            string destHostName = ExtractHostNameFromDestPath(path);
            _destinationIp = ResolveHostName(destHostName);
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
                        hostEntry = Dns.GetHostEntry(Dns.GetHostName());
                        break;

                    case ";Csc":
                        hostEntry = Dns.GetHostEntry(Dns.GetHostName());
                        break;

                    default:
                        hostEntry = Dns.GetHostEntry(hostName);
                        break;
                }
                log.Debug("Resolve ok");

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
                log.Error("Error has occurred while resolving the host name");
                return "Error while resolving";
            }
        }
    }
}