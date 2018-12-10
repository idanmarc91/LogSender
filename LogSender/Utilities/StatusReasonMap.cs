using System.Collections.Generic;

namespace LogSender.Utilities
{
    public static class StatusReasonMap
    {
        private static Dictionary<string, string> _map = new Dictionary<string, string>
        {
            {"NotWL","Blocked"},
            {"WLMM","Blocked"},
            {"NEP","Blocked"},
            {"WLMM DATE","Blocked"},
            {"WLMM MD5","Blocked"},
            {"WLMM MD5 COR","Blocked"},
            {"WLMM SIZE COR","Blocked"},
            {"CHAIN","Blocked"},
            {"Chain","Blocked"},
            {"WLMM DLL MM","Blocked"},
            {"WLMM DLL AMBG","Blocked"},
            {"ChainBreak","Blocked"},
            {"FAKE","Blocked"},
            {"WFS","UNKNOWN"},
            {"NotPD","UNKNOWN"},
            {"Other","UNKNOWN"},
            {"ERROR VAL","ERROR"},
            {"POL","ERROR"}
        };

        public static string Map(string reason)
        {
            if (_map.ContainsKey(reason))
            {
                return _map[reason];
            }
            else
            {
                return reason;
            }
        }
    }
}
