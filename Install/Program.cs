using LogSender.Utilities;
using System;

namespace Install
{
    class Program
    {
        static void Main(string[] args)
        {
            //StreamWriter logFile = new StreamWriter(TempFolder + "Installation Log.txt", true);
            //logFile.WriteLine("\n" + DateTime.Now + "   Uninstalling LogSender service" + "\n");
            //logFile.Close();


            //uninstall old
            //System.IO.File.AppendAllText(@"C:\Users\Cyber\Desktop\TEST.txt", "before uninstall old" + Environment.NewLine);

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
            catch (Exception ex)
            {
                System.IO.File.AppendAllText(@"C:\Users\Cyber\Desktop\error.txt", "error in delete old service" + ex.Message + Environment.NewLine);

            }


            //install new

            try
            {
                System.IO.File.Delete(@"C:\Program Files\Cyber 2.0\Cyber 2.0 Agent\LogSender.exe");
                System.IO.File.Delete(@"C:\Program Files\Cyber 2.0\Cyber 2.0 Agent\LogSender.exe.config");

                System.IO.File.Move(@"C:\Program Files\Cyber 2.0\Cyber 2.0 Agent\Cyber20LogSender.exe", @"C:\Program Files\Cyber 2.0\Cyber 2.0 Agent\LogSender.exe");
                System.IO.File.Move(@"C:\Program Files\Cyber 2.0\Cyber 2.0 Agent\Cyber20LogSender.exe.config", @"C:\Program Files\Cyber 2.0\Cyber 2.0 Agent\LogSender.exe.config");
            }
            catch (Exception ex)
            {
                System.IO.File.AppendAllText(@"C:\Users\Cyber\Desktop\error.txt", "error in delete" + ex.Message + Environment.NewLine);

            }

            try
            {
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
                    // Log("LogSender Service uninstalled successfully");
                    // returnValue = true;
                }
                else
                {
                    //Log("LogSender Service uninstallation failed - " + ServiceProcess.StandardError.ReadToEnd());
                    //returnValue = false;
                }
                ServiceProcess1.Dispose();
            }
            catch (Exception ex)
            {
                System.IO.File.AppendAllText(@"C:\Users\Cyber\Desktop\error.txt", "error in install new service" + ex.Message + Environment.NewLine);

            }


            InstallService.SetServiceRecovery(string.Format("failure \"{0}\" reset= 0 actions= restart/60000", "Cyber20LogSender"));
            InstallService.StartService("Cyber20LogSender");

        }
    }
}
