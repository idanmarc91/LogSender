using System;
using System.IO;
using System.IO.Compression;
using System.Text;

namespace LogSender.Utilities
{
    class JsonCompress
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger( "JsonCompress.cs" );

        /// <summary>
        /// This function copmpress as Gzip
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        public static string CompressString(string text)
        {
            try
            {
                log.Debug( "starting GZip compression before sending data to server" );
                byte[] buffer = Encoding.UTF8.GetBytes( text );
                var memoryStream = new MemoryStream();
                using( var gZipStream = new GZipStream( memoryStream , CompressionMode.Compress , true ) )
                {
                    gZipStream.Write( buffer , 0 , buffer.Length );
                }

                memoryStream.Position = 0;

                var compressedData = new byte[memoryStream.Length];
                memoryStream.Read( compressedData , 0 , compressedData.Length );

                var gZipBuffer = new byte[compressedData.Length + 4];
                Buffer.BlockCopy( compressedData , 0 , gZipBuffer , 4 , compressedData.Length );
                Buffer.BlockCopy( BitConverter.GetBytes( buffer.Length ) , 0 , gZipBuffer , 0 , 4 );
                return Convert.ToBase64String( gZipBuffer );

            }
            catch( Exception ex )
            {
                log.Error( "Error occurred while compressing the data" , ex );
                return "";
            }
        }

        /// <summary>
        /// This function decopmpress Gzip 
        /// </summary>
        /// <param name="compressedText"></param>
        /// <returns></returns>
        public static string DecompressString(string compressedText)
        {
            try
            {
                log.Debug( "starting GZip decompression the data " );

                byte[] gZipBuffer = Convert.FromBase64String( compressedText );
                using( var memoryStream = new MemoryStream() )
                {
                    int dataLength = BitConverter.ToInt32( gZipBuffer , 0 );
                    memoryStream.Write( gZipBuffer , 4 , gZipBuffer.Length - 4 );

                    var buffer = new byte[dataLength];

                    memoryStream.Position = 0;
                    using( var gZipStream = new GZipStream( memoryStream , CompressionMode.Decompress ) )
                    {
                        gZipStream.Read( buffer , 0 , buffer.Length );
                    }

                    return Encoding.UTF8.GetString( buffer );
                }
            }
            catch( Exception ex )
            {
                log.Error( "Error occurred while decompressing the data" , ex );

                return "";
            }
        }
    }
}
