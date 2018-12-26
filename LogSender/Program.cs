using System;
using System.ServiceProcess;
using System.Threading;

[assembly: log4net.Config.XmlConfigurator( Watch = true )]

namespace LogSender
{
    static class Program
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger( "Program.cs" );

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
            ServiceBase[] ServicesToRun;
            ServicesToRun = new ServiceBase[]
            {
                new LogSenderService()
            };
            ServiceBase.Run(ServicesToRun);
#endif
            }
            catch (Exception ex)
            {
                log.Fatal( "Problem with service creation" , ex );
                Thread.CurrentThread.Abort();
            }
        }
    }
}
