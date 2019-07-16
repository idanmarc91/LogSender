using System;
using System.Configuration.Install;
using System.Diagnostics;
using System.ServiceProcess;

namespace LogSender.Utilities
{
    public class InstallService
    {
        /// <summary>
        /// This function set recovery options to the service
        /// </summary>
        /// <param name="serviceName"></param>
        public static void SetServiceRecovery(string recoveryArg)
        {
            int exitCode;
            //add recovery option to log sender service
            using (var process = new Process())
            {
                var startInfo = process.StartInfo;
                startInfo.FileName = "sc";
                startInfo.WindowStyle = ProcessWindowStyle.Hidden;

                // tell Windows that the service should restart if it fails
                startInfo.Arguments = recoveryArg;
                process.Start();
                process.WaitForExit();

                exitCode = process.ExitCode;
            }

            if (exitCode != 0)
            {
                throw new InvalidOperationException();
            }
        }

        /// <summary>
        /// This function start the service
        /// </summary>
        /// <param name="serviceName"></param>
        public static void StartService(string serviceName)
        {
            try
            {

                using (var sc = new ServiceController(serviceName))
                {
                    sc.Start();
                }
            }
            catch (Exception)
            {

                //throw;
            }
        }

        public static void UninstallService(string serviceName)
        {
            ServiceInstaller ServiceInstallerObj = new ServiceInstaller();
            InstallContext Context = new InstallContext("<<log file path>>", null);
            ServiceInstallerObj.Context = Context;
            ServiceInstallerObj.ServiceName = serviceName;
            ServiceInstallerObj.Uninstall(null);
        }
    }
}