using System.IO;
using System.ServiceProcess;

namespace LogSender.Utilities
{
    public class UninstallService
    {
        /// <summary>
        /// This function is stopping the service
        /// </summary>
        /// <param name="serviceName">Name of the service we want to stop</param>
        public static void StopService(string serviceName)
        {
            using (var sc = new ServiceController(serviceName))
            {
                if(sc.CanStop)
                {
                    sc.Stop();
                }
            }
        }

        /// <summary>
        /// Delete the log file directory
        /// </summary>
        /// <param name="path"></param>
        public static void DeleteLogFiles(string path)
        {
            if (Directory.Exists(path))
            {
                DirectoryInfo logDir = new DirectoryInfo(path);
                logDir.Delete(true);
            }
        }

        public static void DeleteFileByName(string filePath)
        {
            if (File.Exists(filePath))
            { 
                File.Delete(filePath);
            }
        }
    }
}