using LogSender.Utilities;
using System;
using System.Linq;
using System.ServiceProcess;
using System.Management;


[assembly: log4net.Config.XmlConfigurator(Watch = true)]

namespace Install
{
    class Program
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger("Program.cs");

        private static readonly string OldService = @"LogSender.exe";
        private static readonly string OldServiceConfig = @"LogSender.exe.config";
        private static readonly string OldServiceConfigurationFile = @"Log Sender Configuration.cfg";

        static void Main(string[] args)
        {
            string pathToService = "";
            string pathToServiceConfig = "";
            string pathToServiceConfigFile = "";

            log.Info("Starting to install the LogSender windows service");

            log.Info("Deleting old service");

            if (DoesServiceExist("Cyber20LogSender"))//check if old service is installed
            {
                string pathToExecutable = GetServicePath("Cyber20LogSender");

                if(string.IsNullOrEmpty(pathToExecutable))
                {
                    return;
                }

                pathToService = pathToExecutable + OldService;
                pathToServiceConfig = pathToExecutable + OldServiceConfig;
                pathToServiceConfigFile = pathToExecutable + OldServiceConfigurationFile;

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
                        log.Info("Old service deleted");
                    }
                    else
                    {
                        log.Fatal("Problem occurred while trying to delete old service");
                    }
                    ServiceProcess.Dispose();

                }
                catch (Exception ex)
                {
                    log.Fatal("Problem occurred while trying to delete old service, installation could not proceed", ex);
                    return;
                }
            }



            try
            {
                log.Info("Starting to delete old service files");

                if (System.IO.File.Exists(pathToService) &&
                   System.IO.File.Exists(pathToServiceConfig) &&
                   System.IO.File.Exists(pathToServiceConfigFile))
                {
                    System.IO.File.Delete(pathToService);
                    System.IO.File.Delete(pathToServiceConfig);
                    System.IO.File.Delete(pathToServiceConfigFile);
                    log.Debug("Delete old files has ended");
                }
            }
            catch (Exception ex)
            {
                log.Fatal("Error occurred while trying to delete old service files", ex);
                return;
            }

            try
            {
                log.Debug("Starting to rename new service name to the old service name");
                System.IO.File.Move(@"C:\Program Files\Cyber 2.0\Cyber 2.0 Agent\Cyber20LogSender.exe.config", @"C:\Program Files\Cyber 2.0\Cyber 2.0 Agent\LogSender.exe.config");
                System.IO.File.Move(@"C:\Program Files\Cyber 2.0\Cyber 2.0 Agent\Cyber20LogSender.exe", @"C:\Program Files\Cyber 2.0\Cyber 2.0 Agent\LogSender.exe");
                log.Debug("Rename ended");
            }
            catch (Exception ex)
            {
                log.Fatal("Error occurred while trying rename new service", ex);
                return;
            }


            try
            {
                log.Info("Starting to install new service");
                System.Diagnostics.Process ServiceProcess1 = new System.Diagnostics.Process();
                System.Diagnostics.ProcessStartInfo startInfo1 = new System.Diagnostics.ProcessStartInfo();
                startInfo1.WindowStyle = System.Diagnostics.ProcessWindowStyle.Hidden;
                startInfo1.CreateNoWindow = true;
                startInfo1.RedirectStandardError = false;
                startInfo1.RedirectStandardOutput = false;
                startInfo1.FileName = "cmd.exe";
                startInfo1.Arguments = "user:Administrator cmd /c \"" + @"C:\Program Files\Cyber 2.0\Cyber 2.0 Agent\" + "LogSender.exe\" -install";
                startInfo1.Verb = "runas"; //The process should start with elevated permissions
                ServiceProcess1.StartInfo = startInfo1;
                ServiceProcess1.Start();
                ServiceProcess1.WaitForExit();
                if (ServiceProcess1.ExitCode == 0)
                {
                    log.Info("Installation new service has completed");
                }
                else
                {
                    log.Fatal("Error has occurred while trying to install new service");
                    return;
                }
                ServiceProcess1.Dispose();
            }
            catch (Exception ex)
            {
                log.Fatal("Error has occurred while trying to install new service", ex);
                return;
            }

            log.Info("Define and start new service");

            InstallService.SetServiceRecovery(string.Format("failure \"{0}\" reset= 0 actions= restart/60000", "Cyber20LogSender"));
            InstallService.StartService("Cyber20LogSender");

            log.Info("End of installation");
        }

        private static string GetServicePath(string ServiceName)
        {
            string currentserviceExePath = "";

            try
            {
                using (ManagementObject wmiService = new ManagementObject("Win32_Service.Name='" + ServiceName + "'"))
                {
                    wmiService.Get();
                    currentserviceExePath = wmiService["PathName"].ToString();
                }

            }
            catch(Exception ex) //if service not found
            {
                log.Fatal("Cannot find service.");
                return "";
            }
            int index = currentserviceExePath.LastIndexOf('\\');
            return currentserviceExePath.Substring(1,index); // remove \"  \" from string 
        }

        static bool DoesServiceExist(string serviceName)
        {
            return ServiceController.GetServices().Any(serviceController => serviceController.ServiceName.Equals(serviceName));
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
