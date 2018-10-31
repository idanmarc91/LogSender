using LogSender.Models;
using Newtonsoft.Json;
using System;
using System.IO;
using System.Text;

namespace LogSender.Utilities
{
    public class JsonDataConvertion
    {
        /// <summary>
        /// This function get data as json with one payload attribute
        /// </summary>
        /// <param name="jsonArray"></param>
        /// <returns></returns>
        public static string JsonSerialization(StringBuilder dataAsString)
        {
            try
            {
                using (StringWriter jsonStrWriter = new StringWriter())
                {
                    Payload payload = new Payload(dataAsString);
                    JsonSerializer serializer = new JsonSerializer();

                    serializer.Serialize(jsonStrWriter, payload);

                    return jsonStrWriter.ToString();
                }
            }
            catch (Exception ex)
            {
                //TODO:Create logger
                System.IO.StreamWriter logFile = new System.IO.StreamWriter(AppDomain.CurrentDomain.BaseDirectory + "log.txt", true);
                logFile.WriteLine("Json Serializetion error\n" + ex.Message);
                logFile.Close();
                return string.Empty;
            }
        }
    }
}
