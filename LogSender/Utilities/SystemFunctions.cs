using System.Linq;
using System.Management;

namespace LogSender.Utilities
{
    internal class SystemFunctions
    {
        public static void SetOperatingSystem()
        {
            var name = (from x in new ManagementObjectSearcher("SELECT Caption FROM Win32_OperatingSystem").Get().Cast<ManagementObject>()
                        select x.GetPropertyValue("Caption")).FirstOrDefault();
            Constant.OPERATINGSYSTEM = name != null ? name.ToString() : "Unknown";
        }
    }
}