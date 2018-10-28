
using Newtonsoft.Json;

namespace BinaryFileToTextFile.Models
{
    [JsonObject(MemberSerialization.OptIn)]
    class JsonLog
    {
        [JsonProperty]
        public string OS { get; set; }
        [JsonProperty]
        public string HostName { get; set; }
        [JsonProperty]
        public string ClientTime { get; set; }
        [JsonProperty]
        public string FullServerTime { get; set; }
        [JsonProperty]
        public int ProcessID { get; set; }
        [JsonProperty]
        public string ProcessName { get; set; }
        [JsonProperty]
        public string ProcessPath { get; set; }
        [JsonProperty]
        public string ApplicationName { get; set; }
        [JsonProperty]
        public string Protocol { get; set; }
        [JsonProperty]
        public string Status { get; set; }
        [JsonProperty]
        public int SourcePort { get; set; }
        [JsonProperty]
        public int DestinationPort { get; set; }
        [JsonProperty]
        public string Direction { get; set; }
        [JsonProperty]
        public string FilePath { get; set; }
        [JsonProperty]
        public string XCast { get; set; }
        [JsonProperty]
        public string State { get; set; }
        [JsonProperty]
        public string SourceIP { get; set; }
        [JsonProperty]
        public string DestinationIP { get; set; }
        [JsonProperty]
        public int SequanceNumber { get; set; }
        [JsonProperty]
        public int SubSequanceNumber { get; set; }
        [JsonProperty]
        public string UserName { get; set; }
        [JsonProperty]
        public int MogCounter { get; set; }
        [JsonProperty]
        public string DestinationPath { get; set; }
        [JsonProperty]
        public string Reason { get; set; }
        [JsonProperty]
        public string ImagePath { get; set; }
        [JsonProperty]
        public string ImageName { get; set; }
        [JsonProperty]
        public string ParentPath { get; set; }
        [JsonProperty]
        public string ParentName { get; set; }
        [JsonProperty]
        public ExpandSVCHostRow[] ChainArray { get; set; }
    }
}
