using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace LogSender.Utilities
{
    class ServerConnection
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger( "ServerConnection.cs" );

        public static async Task ConnectionController(string hostIp, byte[] compressedData)
        {

            //send data to elastic server
            HttpResponseMessage serverResponse = await SendDataToServer( hostIp , compressedData );


        }



        /// <summary>
        /// This function send the compressed data to the log stash server.
        /// data Content-Type must be application/json
        /// data content-encoding is gzip
        /// </summary>
        /// <param name="url"></param>
        /// <param name="data"></param>
        public static async Task<HttpResponseMessage> SendDataToServer(string url , byte[] data)
        {
            try
            {
                log.Debug( "Sending process has started" );
                using( HttpClient client = new HttpClient() )
                {
                    client.BaseAddress = new Uri( url );
                    client.DefaultRequestHeaders
                          .Accept
                          .Add( new MediaTypeWithQualityHeaderValue( "application/json" ) );

                    using( HttpRequestMessage request = new HttpRequestMessage( HttpMethod.Post , url ) )
                    {
                        request.Method = HttpMethod.Post;

                        request.Content = new ByteArrayContent( data );
                        
                        //request.Headers.
                        using( HttpResponseMessage response = await client.SendAsync( request ) )
                        {
                            //check what to do when response is not 200.
                            log.Debug( "server response " + response.StatusCode );
                            log.Debug( "sending process ended" );
                            return response;
                        }
                    }
                }
            }
            catch( Exception ex )
            {
                log.Error( "problem occurred while sending data to the server " ,ex);
                return null;
            }

        }
    }
}

