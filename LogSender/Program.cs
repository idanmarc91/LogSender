using System;
using System.Diagnostics;

#if !DEBUG
using System.ServiceProcess;
#else
using System.Threading;
#endif


[assembly: log4net.Config.XmlConfigurator(Watch = true)]

namespace LogSender
{
    static class Program
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger("Program.cs");

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        static void Main(string[] args)
        {
            try
            {

#if DEBUG
                log.Info("===========================");
                log.Info("Cyber 2.0");
                log.Info("Log Sender application started, application version: " + GetApplicationVersion());
                log.Info("Service starting in DEBUG mode");
                log.Info("===========================");

                LogSenderService service = new LogSenderService();
                service.LogSenderServiceOnDebug();
                Thread.Sleep(Timeout.Infinite);

#else
                //CosturaUtility.Initialize();

                if (args != null && args.Length >= 1 && args[0].Length > 1 && (args[0][0] == '-'))
                {
                    switch (args[0].Substring(1).ToLower())
                    {
                        case "install":
                            log.Info("Service starting install process");


                            SelfInstaller.InstallService(System.Reflection.Assembly.GetExecutingAssembly().Location);
                            break;
                        case "uninstall":

                            log.Info("Service starting uninstall process");
                            SelfInstaller.UninstallService(System.Reflection.Assembly.GetExecutingAssembly().Location);
                            break;
                    }
                }
                else
                {
                    log.Info("===========================");
                    log.Info("Cyber 2.0");
                    log.Info("Log Sender application started, application version: " + GetApplicationVersion());
#if QA_PRNT_FSA

                    log.Info("Service starting in QA RELEASE mode");
                    log.Info("NOT FOR PRODUCTION PURPOSE!");
#endif

#if RELEASE
                    log.Info("Service starting in RELEASE mode");

#endif
                    log.Info("===========================");


                    ServiceBase[] ServicesToRun;
                    ServicesToRun = new ServiceBase[]
                    {
                    new LogSenderService()
                    };
                    ServiceBase.Run(ServicesToRun);
                }
#endif
            }
            catch (Exception ex)
            {
                log.Fatal("Problem with service creation", ex);
            }
        }

        static string GetApplicationVersion()
        {
            System.Reflection.Assembly assembly = System.Reflection.Assembly.GetExecutingAssembly();
            FileVersionInfo fvi = FileVersionInfo.GetVersionInfo(assembly.Location);
            return fvi.FileVersion;
        }
    }
}
