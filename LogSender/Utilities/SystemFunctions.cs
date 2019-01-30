using System;
using System.Linq;
using System.Management;
using System.Net.NetworkInformation;
using System.Text.RegularExpressions;

namespace LogSender.Utilities
{
    public static class SystemFunctions
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger("SystemFunctions.cs");

        public static string GetOperatingSystem()
        {
            var name = (from x in new ManagementObjectSearcher("SELECT Caption FROM Win32_OperatingSystem").Get().Cast<ManagementObject>()
                        select x.GetPropertyValue("Caption")).FirstOrDefault();
            return name != null ? name.ToString() : "Unknown";
        }

        public static string GetFullDomainName()
        {
            return System.Net.NetworkInformation.IPGlobalProperties.GetIPGlobalProperties().DomainName;
        }

        internal static string GetDomainIp()
        {
            try
            {
                Ping pinger = new Ping();
                PingReply reply = pinger.Send(Constant.DOMAIN_FULL_NAME);
                return reply.Address.ToString();
            }
            catch(Exception ex)
            {
                log.Error("Problem occurred while resolving domain ip, domain name is: " + Constant.DOMAIN_FULL_NAME,ex);
                return "";
            }
        }

        internal static string ExtractDomainName()
        {
            string pattern = @".+?(?=\.)";
            return Regex.Match(Constant.DOMAIN_FULL_NAME, pattern).Value;
        }

        internal static bool CheckedIfDomain(string hostName)
        {
            string pattern = @"[a-zA-Z\d-]{0,63}(\.[a-zA-Z\d-]{0,63})*"; //valid domain name
            return Regex.IsMatch(hostName, pattern);
        }
    }
}