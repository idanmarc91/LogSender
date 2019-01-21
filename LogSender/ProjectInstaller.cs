using LogSender.Utilities;
using System.Collections;
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
            log.Debug("After install event started");
            InstallService.SetServiceRecovery(string.Format("failure \"{0}\" reset= 0 actions= restart/60000", serviceInstaller1.ServiceName));

            //create config file
            ConfigFile cnf = ConfigFile.Instance;

            //start the service
            InstallService.StartService(serviceInstaller1.ServiceName);
        }

        private void serviceInstaller1_BeforeUninstall(object sender, InstallEventArgs e)
        {
            //stop the service -not working goods
            //UninstallService.StopService(serviceInstaller1.ServiceName);

            string exeDirPath = Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location);

            UninstallService.DeleteFileByName(Path.Combine(exeDirPath, "LogSenderConfiguration.cfg"));

            string logPath = Path.Combine(exeDirPath, "ls-logs");
            UninstallService.DeleteLogFiles(logPath);
        }
    }
}