using System.Text;

namespace LogSender.Models
{
    class Payload
    {
        public string payload { get; set; }

        public Payload(StringBuilder dataAsString)
        {
            payload = dataAsString.ToString();
        }
    }
}
