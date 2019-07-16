using System;
using System.Linq;
using System.ServiceProcess;

namespace LogSender
{
    internal class SelfInstaller
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger("SelfInstaller.cs");

        public static void InstallService(string exeFilename)
        {
            try
            {
                Console.WriteLine("Starting to install LogSender Service.");

                // check if the service is already installed
                ServiceController ctl = ServiceController.GetServices().FirstOrDefault(s => s.ServiceName == "Cyber20LogSender");
                if (ctl != null)
                {
                    log.Fatal("Log Sender Service is already installed. Please uninstall the service in order to uninstall it");
                    return;
                }
                else
                {
                    System.Configuration.Install.AssemblyInstaller installer = new System.Configuration.Install.AssemblyInstaller(exeFilename, null);
                    installer.UseNewContext = true;
                    installer.Install(null);
                    installer.Commit(null);
                }
            }
            catch (Exception ex)
            {
                log.Fatal("SelfInstaller- Install Service couldn't install LogSender Service", ex);
            }
        }

        public static void UninstallService(string exeFilename)
        {
            try
            {
                Console.WriteLine("Starting to uninstall LogSender Service.");

                // check if the service is already installed
                ServiceController ctl = ServiceController.GetServices().FirstOrDefault(s => s.ServiceName == "Cyber20LogSender");
                if (ctl == null)
                {
                    log.Fatal("LogSender Service is not installed.");
                }
                else
                {
                    System.Configuration.Install.AssemblyInstaller installer = new System.Configuration.Install.AssemblyInstaller(exeFilename, null);
                    installer.UseNewContext = true;
                    installer.Uninstall(null);
                }
            }
            catch (Exception ex)
            {
                log.Fatal("SelfInstaller - Uninstall Service could'nt uninstall LogSender Service", ex);
            }
        }
    }
}