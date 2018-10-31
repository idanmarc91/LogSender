using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
