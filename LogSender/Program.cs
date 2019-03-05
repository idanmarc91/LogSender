using System;
using System.ServiceProcess;
using System.Threading;

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
                LogSenderService service = new LogSenderService();
                service.LogSenderServiceOnDebug();
                Thread.Sleep(Timeout.Infinite );

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
                    log.Debug("Service starting in RELEASE mode");

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
                Thread.CurrentThread.Abort();
            }
        }
    }
}
