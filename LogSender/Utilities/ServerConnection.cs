using System;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace LogSender.Utilities
{
    public class ServerConnection
    {
        ///**********************************************
        ///             Members Section
        ///**********************************************

        private static readonly log4net.ILog log = log4net.LogManager.GetLogger("ServerConnection.cs");
        private static HttpClient _client;

        ///**********************************************
        ///             Functions Section
        ///**********************************************

        /// <summary>
        /// This function send the compressed data to the log stash server.
        /// data Content-Type must be application/json
        /// data content-encoding is gzip
        /// </summary>
        /// <param name="url"></param>
        /// <param name="data"></param>
        public static async Task<bool> SendDataToServer(string hostIp, MemoryStream compressedData)
        {
            try
            {
                using (HttpClientHandler handler = new HttpClientHandler())
                {
                    handler.AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate;
                    using (_client = new HttpClient(handler, false))
                    {

                        StreamContent content = new StreamContent(compressedData);
                        content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
                        content.Headers.ContentEncoding.Add("gzip");

                        using (HttpResponseMessage response = await _client.PostAsync(hostIp + "/input", content))
                        {
                            //check what to do when response is not 200.
                            log.Debug("server response " + response.StatusCode);
                            log.Debug("sending process ended");
                            return response.IsSuccessStatusCode;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                log.Error("problem occurred while sending data to the server ", ex);
                return false;
            }

        }

        /// <summary>
        /// This function check if the server is online 
        /// </summary>
        /// <param name="hostIp"></param>
        /// <returns>true if online, false if offline</returns>
        public static async Task<bool> IsServerAlive(string hostIp)
        {
            try
            {
                log.Debug("check if server (" + hostIp + ") is alive");

                using (HttpClient client = new HttpClient())
                {
                    using (HttpResponseMessage response = await client.GetAsync(hostIp + "/ping"))
                    {
                        log.Info("The server (" + hostIp + ") is online");
                        return true;
                    }
                }
            }
            catch (Exception ex)
            {
                log.Error("The server (" + hostIp + ") is offline");
                return false;
            }
            finally
            {
                log.Debug("exit serverIsAlive function");
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="retry"></param>
        /// <param name="hostIp"></param>
        /// <param name="compressedData"></param>
        /// <returns></returns>
        public static async Task<bool> ServerManager(int retry, string hostIp, int delayTime, MemoryStream compressedData)
        {
            while (retry != 0)//retry loop
            {
                if (await ServerConnection.SendDataToServer(hostIp, compressedData))
                {
                    return true;//when file sent sucessfuly exit while loop
                }
                await Task.Delay(delayTime);
                retry--;
            }
            return false;
        }
    }
}

