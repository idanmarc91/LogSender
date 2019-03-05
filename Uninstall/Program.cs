using System;

namespace Uninstall
{
    class Program
    {
        static void Main(string[] args)
        {
            //StreamWriter logFile = new StreamWriter(TempFolder + "Installation Log.txt", true);
            //logFile.WriteLine("\n" + DateTime.Now + "   Uninstalling LogSender service" + "\n");
            //logFile.Close();


            //uninstall old
            System.IO.File.AppendAllText(@"C:\Users\Cyber\Desktop\TEST.txt", "before uninstall old" + Environment.NewLine);

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


            System.IO.File.AppendAllText(@"C:\Users\Cyber\Desktop\TEST.txt", "after uninstall old" + Environment.NewLine);


            //install new



            System.IO.File.AppendAllText(@"C:\Users\Cyber\Desktop\TEST.txt", "before install new" + Environment.NewLine);


            System.Diagnostics.Process ServiceProcess1 = new System.Diagnostics.Process();
            System.Diagnostics.ProcessStartInfo startInfo1 = new System.Diagnostics.ProcessStartInfo();
            startInfo1.WindowStyle = System.Diagnostics.ProcessWindowStyle.Hidden;
            startInfo1.CreateNoWindow = true;
            startInfo1.RedirectStandardError = false;
            startInfo1.RedirectStandardOutput = false;
            startInfo1.FileName = "cmd.exe";
            startInfo1.Arguments = "user:Administrator cmd /c \"" + @"C:\Program Files\Cyber 2.0\Cyber 2.0 Agent\temp\" + "LogSender.exe\" -install";
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

            System.IO.File.AppendAllText(@"C:\Users\Cyber\Desktop\TEST.txt", "after install new" + Environment.NewLine);



            //tempLogFile = new StreamWriter(@"C:\Installation Log.txt", true);
            //tempLogFile.WriteLine("\n" + DateTime.Now + "   LogSender uninstalled successfully" + "\n");
            //tempLogFile.Close();

        }
    }
}
