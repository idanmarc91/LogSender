using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;

[assembly: log4net.Config.XmlConfigurator(Watch = true)]

namespace LogSender
{
    static class Program
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger("Program.cs");

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        static void Main()
        {
#if DEBUG
            LogSenderService service = new LogSenderService();
            service.LogSenderServiceOnDebug();
            System.Threading.Thread.Sleep(System.Threading.Timeout.Infinite);

#else
            ServiceBase[] ServicesToRun;
            ServicesToRun = new ServiceBase[]
            {
                new LogSenderService()
            };
            ServiceBase.Run(ServicesToRun);
#endif
        }
    }
}
