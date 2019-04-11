using System;
using System.ServiceProcess;
using System.Threading;

namespace LogSender
{

    public partial class LogSenderService : ServiceBase
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger("LogSenderService.cs");

        LogSender _logSender;
        static Thread MainThread;

        public LogSenderService()
        {
            InitializeComponent();
        }

        public void LogSenderServiceOnDebug()
        {
            OnStart(null);
        }

        public void Main()
        {
            try
            {
                _logSender = new LogSender();
                _logSender.RunService();
            }
            catch (Exception ex)
            {
                log.Fatal("Creation of log sender class problem", ex);

                log.Info("Stopping the service.");
                string serviceName = "Cyber20LogSender";
                using (var sc = new ServiceController(serviceName))
                {
                    if (sc.Status != ServiceControllerStatus.Stopped)
                    {
                        sc.Stop();
                    }
                }
            }
        }

        protected override void OnStart(string[] args)
        {
            try
            {
                log.Debug("Log Sender Service OnStart Function started");


                MainThread = new Thread(new ThreadStart(Main));
                MainThread.IsBackground = true; // impotent!
                MainThread.Start();

                log.Debug("Log Sender Service OnStart Function finished");
            }
            catch (Exception ex)
            {

            }
        }

        protected override void OnStop()
        {
            log.Debug("Log Sender Service enter OnStop Function");
            log.Warn("The service has stopped");
        }
    }
}
