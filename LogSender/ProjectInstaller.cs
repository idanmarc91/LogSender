using System;
using System.ComponentModel;
using System.Configuration.Install;
using System.Diagnostics;
using System.ServiceProcess;

namespace LogSender
{
    [RunInstaller(true)]
    public partial class ProjectInstaller : System.Configuration.Install.Installer
    {
        public ProjectInstaller()
        {
            InitializeComponent();
        }

        private void serviceInstaller1_AfterInstall(object sender, InstallEventArgs e)
        {
            int exitCode;
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

            new ServiceController(serviceInstaller1.ServiceName).Start();
        }
    }
}
