namespace LogSender.Data
{
    class ReasonLog
    {
        public string _reason { get; set; }

        internal void GetReasonFromExtractedData(string status)
        {
            switch (status)
            {
                case "OK":
                case "ERROR":
                case "UNKNOWN":
                case "Dropped":
                case "Exculuded":
                    _reason = status;
                    break;
                default:
                    _reason = status;
                    break;

            }
        }
    }
}
