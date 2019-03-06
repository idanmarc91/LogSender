using System;
using System.IO;
using System.Linq;
using System.ServiceProcess;

[assembly: log4net.Config.XmlConfigurator(Watch = true)]


namespace Uninstall
{
    class Program
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger("Program.cs");

        static void Main(string[] args)
        {
            log.Info("Starting to uninstall log sender service");


            if (DoesServiceExist("Cyber20LogSender"))//check if old service is installed
            {
                try
                {
                    StopService("Cyber20LogSender");
                    System.Diagnostics.Process ServiceProcess = new System.Diagnostics.Process();
                    System.Diagnostics.ProcessStartInfo startInfo = new System.Diagnostics.ProcessStartInfo();
                    startInfo.WindowStyle = System.Diagnostics.ProcessWindowStyle.Hidden;
                    startInfo.CreateNoWindow = true;
                    startInfo.RedirectStandardError = false;
                    startInfo.RedirectStandardOutput = false;
                    startInfo.FileName = "cmd.exe";
                    startInfo.Arguments = "user:Administrator cmd /c \"" + @"C:\Program Files\Cyber 2.0\Cyber 2.0 Agent\" + "LogSender.exe\" -uninstall";
                    startInfo.Verb = "runas"; //The process should start with elevated permissions
                    ServiceProcess.StartInfo = startInfo;
                    ServiceProcess.Start();
                    ServiceProcess.WaitForExit();
                    if (ServiceProcess.ExitCode == 0)
                    {
                        log.Info("LogSender service uninstall process has succeeded");
                    }
                    else
                    {
                        log.Fatal("Error occurred while trying to uninstall the service");
                        return;
                    }
                    ServiceProcess.Dispose();
                }
                catch (Exception ex)
                {
                    log.Fatal("Error occurred while trying to uninstall the service", ex);
                    return;
                }
            }


            log.Debug("Reverting names for log senders service files");
            try
            {
                System.IO.File.Move(@"C:\Program Files\Cyber 2.0\Cyber 2.0 Agent\LogSender.exe", @"C:\Program Files\Cyber 2.0\Cyber 2.0 Agent\Cyber20LogSender.exe");
                System.IO.File.Move(@"C:\Program Files\Cyber 2.0\Cyber 2.0 Agent\LogSender.exe.config", @"C:\Program Files\Cyber 2.0\Cyber 2.0 Agent\Cyber20LogSender.exe.config");

            }
            catch (Exception ex)
            {
                log.Fatal("Problem occurred while trying to revert service files name", ex);
                return;
            }


            log.Info("Deleting service files");
            DeleteFileByName(@"C:\Program Files\Cyber 2.0\Cyber 2.0 Agent\LogSenderConfiguration.cfg");
            DeleteLogFiles(@"C:\Program Files\Cyber 2.0\Cyber 2.0 Agent\ls-logs");
            log.Info("Uninstall process ended");

        }

        static bool DoesServiceExist(string serviceName)
        {
            return ServiceController.GetServices().Any(serviceController => serviceController.ServiceName.Equals(serviceName));
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
                log.Error("Cannot delete log sender configuration file");
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
                log.Error("Cannot delete log sender logs folder");
            }
        }

        public static void StopService(string serviceName)
        {
            using (var sc = new ServiceController(serviceName))
            {
                if (sc.Status != ServiceControllerStatus.Stopped)
                {
                    sc.Stop();
                    sc.WaitForStatus(ServiceControllerStatus.Stopped);
                }
            }
        }
    }
}
