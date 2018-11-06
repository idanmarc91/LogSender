using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace LogSender.Utilities
{
    class ServerConnection
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger( "ServerConnection.cs" );

        /// <summary>
        /// This function send the compressed data to the log stash server.
        /// data Content-Type must be application/json
        /// data content-encoding is gzip
        /// </summary>
        /// <param name="url"></param>
        /// <param name="data"></param>
        public static async Task<bool> SendDataToServer(string hostIp , byte[] compressedData)
        {
            try
            {
                log.Debug( "Sending process has started" );
                using( HttpClient client = new HttpClient() )
                {
                    client.BaseAddress = new Uri( hostIp + "/input" );
                    client.DefaultRequestHeaders
                          .Accept
                          .Add( new MediaTypeWithQualityHeaderValue( "application/json" ) );

                    using( HttpRequestMessage request = new HttpRequestMessage( HttpMethod.Post , hostIp + "/input" ) )
                    {
                        request.Method = HttpMethod.Post;

                        request.Content = new ByteArrayContent( compressedData );

                        //request.Headers.
                        using( HttpResponseMessage response =  await client.SendAsync( request ) )
                        {
                            //check what to do when response is not 200.
                            log.Debug( "server response " + response.StatusCode );
                            log.Debug( "sending process ended" );
                            return response.IsSuccessStatusCode;
                        }
                    }
                }
            }
            catch( Exception ex )
            {
                log.Error( "problem occurred while sending data to the server " , ex );
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
                log.Debug( "check if server is alive" );

                using( HttpClient client = new HttpClient() )
                {
                    using( HttpResponseMessage response = await client.GetAsync( hostIp +"/ping" ) )
                    {
                        log.Info( "The server is online" );
                        return true;
                    }
                }
            }
            catch (Exception ex)
            {
                log.Error( "The server is offline");
                return false;
            }
            finally
            {
                log.Debug( "exit serverIsAlive function" );
            }
        }
    }
}

