using System;
using System.ServiceProcess;

namespace LogSender
{

    public partial class LogSenderService : ServiceBase
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger( "LogSenderService.cs" );

        LogSender _logSender;
        public LogSenderService()
        {
            InitializeComponent();
        }

        public void LogSenderServiceOnDebug()
        {
            log.Debug( "Log Sender Service On Debug" );
            OnStart( null );
        }

        protected override void OnStart(string[] args)
        {
            try
            {
                log.Debug( "Log Sender Service OnStart Function" );
                _logSender = new LogSender();
                _logSender.RunService();
                
            }
            catch( Exception ex )
            {
                log.Fatal( "creation of log sender class problem" , ex );
            }
        }

        protected override void OnStop()
        {
            log.Debug( "Log Sender Service enter OnStop Function" );
            log.Warn( "The service has stopped" );
        }
    }
}
