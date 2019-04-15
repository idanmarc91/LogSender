using LogSender.Utilities;
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
            _destinationIp = ResolveHostName(path);
            if (_destinationIp == "")
            {
                _destinationIp = sourceIp;
            }
        }

        /// <summary>
        /// This function extract the host name from the destination path with regex pattern.
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        private string ExtractHostNameFromDestPath(string path)
        {
            //string pattern = @"[^\\][\w]*[^\\]"; //with no ip recognize
            string pattern = @"[^\\][\w\d.]*[^\\]"; //include ip recognize
            string match = Regex.Match(path, pattern).Value;
            return (match == "") ? path : match;
        }

        private string ResolveHostName(string path)
        {
            string destHostName = ExtractHostNameFromDestPath(path);

            try
            {
                IPHostEntry hostEntry = null;
                switch (destHostName)
                {
                    case "localhost":
                        return "";

                    case ";Csc":
                        return "";

                    case "\\":
                        return "";

                    default:
                        try
                        {
                            //check if the destHostName is equal to the domain full name
                            if (Constant.DOMAIN_FULL_NAME.Equals(destHostName, StringComparison.InvariantCultureIgnoreCase))
                            {
                                return Constant.DOMAIN_IP;
                            }
                            //resolve the destHostName with dns domain cache
                            hostEntry = Dns.GetHostEntry(destHostName);
                        }
                        catch (SocketException ex)//catch special host name
                        {
//#if QARELEASE || DEBUG
                            string ip = SpecialHostName(destHostName);

                            if (ip == null)//if we cannot resolve the destHostName we write it to a log file for further investigation
                            {
                                log.Warn(ex.Message);
                                System.IO.File.AppendAllText(System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location) + "\\ls-logs\\destination_ip.txt",
                                                            DateTime.Now.ToString("dd/MM HH:mm:ss") + " Error occurred while resolving: " + ex.Message + " host name: " + destHostName + " destination path: " + path + Environment.NewLine);
                                return (destHostName == null) ? "Cannot Resolve IP" : destHostName;
                            }
                            else
                            {
                                return ip;
                            }
//#endif
                        }
                        break;
                }

                IPAddress[] ips = hostEntry.AddressList;
                return (ips.Length > 0) ? ips[ips.Length - 1].ToString() : destHostName;
            }
            catch (Exception ex)
            {
                log.Error("Error has occurred while resolving the host name", ex);
                return "Error while resolving";
            }
        }

        private string SpecialHostName(string hostName)
        {
            if (SystemFunctions.CheckedIfDomain(hostName))
            {
                if (hostName.Equals(Constant.DOMAIN_NAME, StringComparison.InvariantCultureIgnoreCase))
                {
                    return Constant.DOMAIN_IP;
                }
            }
            return null;
        }
    }
}