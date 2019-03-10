using System.Collections.Generic;

namespace LogSender.Data
{
    class StatusLogDLL
    {
        private static Dictionary<string, string> _reasonStatusMap = new Dictionary<string, string>
        {
            {"Not in policy","Blocked"},//
            {"Policy MM","Blocked"},//
            {"Not enough params","Blocked"},// Erez need to answer if its true
            {"Policy MM date","Blocked"},//
            {"Policy MM MD5","Blocked"},// DLL
            {"Policy MM MD5 corrupt","Blocked"},//
            {"Policy MM size corrupt","Blocked"},//
            {"Policy MM size","Blocked"},//
            {"Policy MM bad MD5","Blocked"},//
            {"Chain","Blocked"},//
            {"Policy MM DLL MM","Blocked"},//
            {"Policy MM DLL ambiguity","Blocked"},//
            {"Chain break","Chain"},// 
            {"Fake localhost","Blocked"},//
            {"Network no check","Blocked"},//
            {"Svchost MM", "Blocked"},//
            //{"Service MM", "Blocked"}, // duplicated with Svchost MM
            {"Wait for SUP","Error"},//
            {"NotPD","Error"},//
            //{"Other","Unknown"},
            //{"NoCheck", "Blocked"}, // nocheck = network no check duplicated
            {"Error","Error"},//
            {"TCP MM","Info"},//
            {"UDP MM","Info"},//
            //{"Path overflow","Error"},//FSA Reason
            {"Config excluded", "Excluded"},//
            { "Ok", "Ok"},//
            { "Flow Error", "Error"},//
            {"Unknown","Unknown" },//
            { "Dropped", "Dropped"}//
        };

        public string _status { get; set; }

        internal static string MapStatusFromReason(string reason)
        {
            if (_reasonStatusMap.ContainsKey(reason))
            {
                //_status = _reasonStatusMap[status];
                return _reasonStatusMap[reason];

            }
            else
            {
                return reason;
            }
        }

        internal void DefineStatusFromDataLog(string reason, string seqNum)
        {
            if (seqNum != "0")
            {
                _status = "Chain";
            }
            else
            {
                _status = MapStatusFromReason(reason);
            }
        }

        internal void DefineStatusFromDataDLL(string reason)
        {
            _status = MapStatusFromReason(reason);
        }
    }
}
