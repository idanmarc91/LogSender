using System;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Sockets;
using System.Threading.Tasks;

namespace LogSender.Utilities
{
    public class ServerConnection
    {
        ///**********************************************
        ///             Members Section
        ///**********************************************

        private static readonly log4net.ILog log = log4net.LogManager.GetLogger("ServerConnection.cs");
        private HttpClient _client;

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
        public async Task<bool> SendDataToServerAsync(string hostIp, MemoryStream compressedData)
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
        public static async Task<bool> IsServerAliveAsync()
        {
            try
            {
                log.Debug("check if server (" + ConfigFile.Instance._configData._hostIp + ") is alive");

                using (HttpClient client = new HttpClient())
                {
                    using (HttpResponseMessage response = await client.GetAsync(ConfigFile.Instance._configData._hostIp + "/ping"))
                    {
                        log.Info("The server (" + ConfigFile.Instance._configData._hostIp + ") is online");
                        return true;
                    }
                }
            }
            catch (Exception ex)
            {
                log.Error("The server (" + ConfigFile.Instance._configData._hostIp + ") is offline", ex);
                return false;
            }
        }

        public static async Task<bool> IsServerAliveAsync(string ip)
        {
            try
            {
                log.Debug("check if server (" + ip + ") is alive");

                using (HttpClient client = new HttpClient())
                {
                    using (HttpResponseMessage response = await client.GetAsync(ip + "/ping"))
                    {
                        log.Info("The server (" + ip + ") is online");
                        return true;
                    }
                }
            }
            catch (Exception ex)
            {
                log.Error("The server (" + ip + ") is offline", ex);
                return false;
            }
        }

        /// <summary>
        /// This function is responsible for server connection logic.
        /// if the sending process fail this function will manage the delay and resend process 
        /// depent on the configuration file data
        /// </summary>
        /// <param name="compressedData"></param>
        /// <returns></returns>
        public async Task<bool> ServerManagerAsync(MemoryStream compressedData)
        {
            int retry = ConfigFile.Instance._configData._numberOfTimesToRetry;
            while (retry != 0)//retry loop
            {
                if (await SendDataToServerAsync(ConfigFile.Instance._configData._hostIp, compressedData))
                {
                    return true;//when file sent sucessfuly exit while loop
                }
                await Task.Delay(ConfigFile.Instance._configData._waitTimeBeforeRetry);
                retry--;
            }
            return false;
        }

        /// <summary>
        /// Get local ip address
        /// </summary>
        /// <returns></returns>
        public static string GetLocalIPAddress()
        {
            var host = Dns.GetHostEntry(Dns.GetHostName());
            foreach (var ip in host.AddressList)
            {
                if (ip.AddressFamily == AddressFamily.InterNetwork)
                {
                    return ip.ToString();
                }
            }
            throw new Exception("No network adapters with an IPv4 address in the system!");
        }
    }
}

