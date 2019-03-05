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
                if (sc.CanStop)
                {
                    sc.Stop();
                    sc.WaitForStatus(ServiceControllerStatus.Stopped);
                }
            }
        }

        /// <summary>
        /// Delete the log file directory
        /// </summary>
        /// <param name="path"></param>
        public static void DeleteLogFiles(string path)
        {
            try
            {

                if (Directory.Exists(path))
                {
                    DirectoryInfo logDir = new DirectoryInfo(path);
                    logDir.Delete(true);
                }
            }
            catch (System.Exception ex)
            {
                //cannot delete files because files are open
            }

        }


        public static void DeleteFileByName(string filePath)
        {
            try
            {
                if (File.Exists(filePath))
                {
                    File.Delete(filePath);
                }
            }
            catch (System.Exception ex)
            {
                //cannot delete files because files are open
            }
        }
    }
}