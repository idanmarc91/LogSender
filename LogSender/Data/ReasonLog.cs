namespace LogSender.Data
{
    class ReasonLog
    {
        public string _reason { get; set; }

        internal void GetReasonFromExtractedData(string status)
        {
            switch (status)
            {
                case "Ok":
                case "Error":
                case "Unknown":
                case "Dropped":
                case "Excluded":
                    _reason = status;
                    break;
                default:
                    _reason = status;
                    break;

            }
        }
    }
}
