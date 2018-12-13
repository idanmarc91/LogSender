
namespace LogSender.Data
{
    public class ConfigData
    {
        public long _jsonDataMaxSize { get; set; }
        public long _binaryFileMaxSize { get; set; }
        public int _threadSleepTime { get; set; } //in miliseconds
        public string _hostPort { get; set; }
        public string _hostIp { get; set; }
        public int _minNumOfFilesToSend { get; set; }
        public int _numberOfTimesToRetry { get; set; }
        public int _waitTimeBeforeRetry { get; set; } //in milliseconds
        public long _binaryFolderMaxSize { get; set; }
        public string _fsaFolderPath { get; set; }
        public string _cybFolderPath { get; set; }
        public string _mogFolderPath { get; set; }
        public string _cimgFolderPath { get; set; }
    }
}
