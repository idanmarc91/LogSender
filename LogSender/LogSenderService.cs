using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;

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
            //System.IO.File.Create(AppDomain.CurrentDomain.BaseDirectory + "OnStart.txt");
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
