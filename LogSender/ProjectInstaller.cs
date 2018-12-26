using LogSender.Utilities;
using System;
using System.Collections;
using System.ComponentModel;
using System.Configuration.Install;
using System.Diagnostics;
using System.IO;
using System.ServiceProcess;

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
            string path = Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location);
            System.IO.File.WriteAllText(path, "test");
            int exitCode;
            log.Debug("After install event started");
            //add recovery option to log sender service
            using (var process = new Process())
            {
                var startInfo = process.StartInfo;
                startInfo.FileName = "sc";
                startInfo.WindowStyle = ProcessWindowStyle.Hidden;

                // tell Windows that the service should restart if it fails
                startInfo.Arguments = string.Format("failure \"{0}\" reset= 0 actions= restart/60000", "Cyber 2.0 Log Sender v5");
                process.Start();
                process.WaitForExit();

                exitCode = process.ExitCode;
            }

            if (exitCode != 0)
            {
                throw new InvalidOperationException();
            }

            //create config file
            ConfigFile cnf = ConfigFile.Instance;

            //start the service right after installation process complete
            new ServiceController(serviceInstaller1.ServiceName).Start();
        }

        protected override void OnBeforeUninstall(IDictionary savedState)
        {

            using (var sc = new ServiceController(serviceInstaller1.ServiceName))
            {
                sc.Stop();
            }
        }
    }
      
}
