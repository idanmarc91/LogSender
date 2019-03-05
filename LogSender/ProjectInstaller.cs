using LogSender.Utilities;
using System.ComponentModel;
using System.Configuration.Install;
using System.IO;

namespace LogSender
{
    [RunInstaller(true)]
    public partial class ProjectInstaller : System.Configuration.Install.Installer
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger("ProjectInstaller.cs");

        public ProjectInstaller()
        {
            InitializeComponent();
        }

        private void serviceInstaller1_AfterInstall(object sender, InstallEventArgs e)
        {
            //System.IO.File.AppendAllText(@"C:\Program Files\Cyber 2.0\Cyber 2.0 Agent\TEST.txt", "in after install function" + Environment.NewLine);

            log.Debug("After install event started");
            //InstallService.SetServiceRecovery(string.Format("failure \"{0}\" reset= 0 actions= restart/60000", serviceInstaller1.ServiceName));

            //create config file
            //ConfigFile cnf = ConfigFile.Instance;

            ////start the service
            //InstallService.StartService(serviceInstaller1.ServiceName);
        }

        private void serviceInstaller1_BeforeUninstall(object sender, InstallEventArgs e)
        {
            //stop the service -not working goods
            UninstallService.StopService(serviceInstaller1.ServiceName);

            string exeDirPath = Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location);

            UninstallService.DeleteFileByName(Path.Combine(exeDirPath, "LogSenderConfiguration.cfg"));

            string logPath = Path.Combine(exeDirPath, "ls-logs");
            UninstallService.DeleteLogFiles(logPath);
        }

        private void serviceInstaller1_BeforeInstall(object sender, InstallEventArgs e)
        {
            //System.IO.File.AppendAllText(@"C:\Program Files\Cyber 2.0\Cyber 2.0 Agent\TEST.txt", "in serviceInstaller1_BeforeInstall function" + Environment.NewLine);

            //UninstallService.StopService(serviceInstaller1.ServiceName);

            //InstallService.UninstallService(serviceInstaller1.ServiceName);//uninstall old service with same service name

            //System.Diagnostics.Process ServiceProcess = new System.Diagnostics.Process();
            //System.Diagnostics.ProcessStartInfo startInfo = new System.Diagnostics.ProcessStartInfo();
            //startInfo.WindowStyle = System.Diagnostics.ProcessWindowStyle.Hidden;
            //startInfo.CreateNoWindow = true;
            //startInfo.RedirectStandardError = false;
            //startInfo.RedirectStandardOutput = false;
            //startInfo.FileName = "cmd.exe";
            //startInfo.Arguments = "user:Administrator cmd /c \"" + @"C:\Program Files\Cyber 2.0\Cyber 2.0 Agent\" + "LogSender.exe\" -uninstall";
            //startInfo.Verb = "runas"; //The process should start with elevated permissions
            //ServiceProcess.StartInfo = startInfo;
            //ServiceProcess.Start();
            //ServiceProcess.WaitForExit();
            //if (ServiceProcess.ExitCode == 0)
            //{
            //    // Log("LogSender Service uninstalled successfully");
            //    // returnValue = true;
            //}
            //else
            //{
            //    //Log("LogSender Service uninstallation failed - " + ServiceProcess.StandardError.ReadToEnd());
            //    //returnValue = false;
            //}
            //ServiceProcess.Dispose();
            //System.IO.File.AppendAllText(@"C:\Program Files\Cyber 2.0\Cyber 2.0 Agent\TEST.txt", "Finished uninstalling old service" + Environment.NewLine);
        }

        private void serviceProcessInstaller1_AfterInstall(object sender, InstallEventArgs e)
        {

        }
    }
}