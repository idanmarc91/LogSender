using System;
using System.ServiceProcess;
using System.Threading;

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
                log.Debug( "Log Sender Service OnStart Fucntion" );
                _logSender = new LogSender();
                if (_logSender == null )
                    throw new Exception("log sender could not be created");
                _logSender.RunService();
            }
            catch( Exception ex )
            {
                log.Fatal( "creation of log sender class problem "+ ex.Message , ex );
                Thread.CurrentThread.Abort();
            }
        }

        protected override void OnStop()
        {
            log.Debug( "Log Sender Service enter OnStop Fucntion" );
            log.Warn( "The service has stoped" );
        }
    }
}
