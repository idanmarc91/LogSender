using LogSender.Utilities;

namespace Uninstall
{
    class Program
    {
        static void Main(string[] args)
        {

            UninstallService.StopService("Cyber20LogSender");
            try
            {

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
                    // Log("LogSender Service uninstalled successfully");
                    // returnValue = true;
                }
                else
                {
                    //Log("LogSender Service uninstallation failed - " + ServiceProcess.StandardError.ReadToEnd());
                    //returnValue = false;
                }
                ServiceProcess.Dispose();
            }
            catch (System.Exception)
            {
            }

            System.IO.File.Move(@"C:\Program Files\Cyber 2.0\Cyber 2.0 Agent\LogSender.exe", @"C:\Program Files\Cyber 2.0\Cyber 2.0 Agent\Cyber20LogSender.exe");
            System.IO.File.Move(@"C:\Program Files\Cyber 2.0\Cyber 2.0 Agent\LogSender.exe.config", @"C:\Program Files\Cyber 2.0\Cyber 2.0 Agent\Cyber20LogSender.exe.config");

        }
    }
}
