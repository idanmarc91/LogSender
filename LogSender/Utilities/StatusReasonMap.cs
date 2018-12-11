using System.Collections.Generic;

namespace LogSender.Utilities
{
    public static class StatusReasonMap
    {
        private static Dictionary<string, string> _map = new Dictionary<string, string>
        {
            {"Not in policy","Blocked"},
            {"Policy MM","Blocked"},
            {"Not enough params","Blocked"},
            {"Policy MM date","Blocked"},
            {"Policy MM MD5","Blocked"},
            {"Policy MM MD5 corrupt","Blocked"},
            {"Policy MM size corrupt","Blocked"},
            {"Policy MM size","Blocked"},
            {"Chain","Blocked"},
            {"Policy MM DLL MM","Blocked"},
            {"Policy MM DLL ambiguity","Blocked"},
            {"Chain block","Blocked"},
            {"Fake localhost","Blocked"},
            {"Network no check","Blocked"},
            {"Wait for SUP","Unknown"},
            {"NotPD","Unknown"},
            {"Other","Unknown"},
            {"Error val","Error"},
            {"TCP MM","Error"},
            {"UDP MM","Error"},
            {"Path overflow","Error"},
            {"Config exculuded", "Excluded"}

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
