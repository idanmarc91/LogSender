using BinaryFileToTextFile.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogSender.Utilities
{
    public class JsonDataConvertion
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="jsonArray"></param>
        /// <returns></returns>
        public static string JsonSerialization(List<JsonLog> jsonArray)
        {
            try
            {
                using (StringWriter jsonStrWriter = new StringWriter())
                {
                    JsonSerializer serializer = new JsonSerializer();
                    serializer.Serialize(jsonStrWriter, jsonArray);

                    return jsonStrWriter.ToString();
                    //string Compressed = CompressString(jsonAsString);
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
