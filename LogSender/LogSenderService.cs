using System;
using System.ServiceProcess;

namespace LogSender
{
    public partial class LogSenderService : ServiceBase
    {
        LogSender _logSender;
        public LogSenderService()
        {
            InitializeComponent();
        }

        public void LogSenderServiceOnDebug()
        {
            OnStart(null);
        }

        protected override void OnStart(string[] args)
        {
            string path = AppDomain.CurrentDomain.BaseDirectory;
            _logSender = new LogSender(path);
            _logSender.RunService();
        }

        protected override void OnStop()
        {
            //System.IO.File.Create(AppDomain.CurrentDomain.BaseDirectory + "OnStop.txt");
        }
    }
}
