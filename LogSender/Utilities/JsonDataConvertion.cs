using LogSender.Models;
using Newtonsoft.Json;
using System;
using System.IO;
using System.Text;

namespace LogSender.Utilities
{
    public class JsonDataConvertion
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger( "JsonDataConvertion.cs" );

        /// <summary>
        /// This function get data as json with one payload attribute
        /// </summary>
        /// <param name="jsonArray"></param>
        /// <returns></returns>
        public static string JsonSerialization(StringBuilder dataAsString)
        {
            try
            {
                using( StringWriter jsonStrWriter = new StringWriter() )
                {
                    Payload payload = new Payload( dataAsString );
                    JsonSerializer serializer = new JsonSerializer();

                    serializer.Serialize( jsonStrWriter , payload );

                    return jsonStrWriter.ToString();
                }
            }
            catch( Exception ex )
            {
                log.Error( "problem with json serialization process" , ex );
            }
            return "";
        }
    }
}
