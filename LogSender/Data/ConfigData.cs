
namespace LogSender.Data
{
    public class ConfigData
    {
        public int _jsonDataMaxSize { get; set; }
        public int _binaryFileMaxSize { get; set; }
        public int _threadSleepTime { get; set; } //in miliseconds
        public string _hostIp { get; set; }
        public int _minNumOfFilesToSend { get; set; }
        public int _numberOfTimesToRetry { get; set; }
        public int _waitTimeBeforeRetry { get; set; } //in milliseconds
        public long _maxBinaryFolderSize { get; set; }
    }
}
