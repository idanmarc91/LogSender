using System;
using System.IO;
using System.IO.Compression;
using System.Text;

namespace LogSender.Utilities
{
    public class GZipCompresstion
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger( "JsonCompress.cs" );

        /// <summary>
        /// This function copmpress as Gzip
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        public static MemoryStream CompressString(string text)
        {
            try
            {

                log.Debug("starting GZip compression before sending data to server");

                byte[] jsonBytes = Encoding.UTF8.GetBytes(text);
                MemoryStream ms = new MemoryStream();
                using (GZipStream gzip = new GZipStream(ms, CompressionMode.Compress, true))
                {
                    gzip.Write(jsonBytes, 0, jsonBytes.Length);
                }
                ms.Position = 0;

                log.Debug("GZip compression prccess ended successfully");

                return ms;
                

                #region old compression code code

                //log.Debug( "starting GZip compression before sending data to server" );
                //byte[] buffer = Encoding.UTF8.GetBytes( text );
                //var memoryStream = new MemoryStream();
                //using( var gZipStream = new GZipStream( memoryStream , CompressionMode.Compress , true ) )
                //{
                //    gZipStream.Write( buffer , 0 , buffer.Length );
                //}

                //memoryStream.Position = 0;

                //var compressedData = new byte[memoryStream.Length];
                //memoryStream.Read( compressedData , 0 , compressedData.Length );

                //byte[] gZipBuffer = new byte[compressedData.Length + 4];
                //Buffer.BlockCopy( compressedData , 0 , gZipBuffer , 4 , compressedData.Length );
                //Buffer.BlockCopy( BitConverter.GetBytes( buffer.Length ) , 0 , gZipBuffer , 0 , 4 );

                //log.Debug( "GZip compression prccess ended successfully" );

                //return gZipBuffer;
                //return Convert.ToBase64String( gZipBuffer );
                #endregion

            }
            catch ( Exception ex )
            {
                log.Error( "Error occurred while compressing the data" , ex );
                return null;
            }
        }

        /// <summary>
        /// This function decopmpress Gzip 
        /// </summary>
        /// <param name="compressedText"></param>
        /// <returns></returns>
        public static string DecompressString(byte [] compressedText)
        {
            try
            {
                log.Debug("starting GZip decompression the data ");

                using (GZipStream stream = new GZipStream(new MemoryStream(compressedText),
                CompressionMode.Decompress))
                {
                    const int size = 4096;
                    byte[] buffer = new byte[size];
                    using (MemoryStream memory = new MemoryStream())
                    {
                        int count = 0;
                        do
                        {
                            count = stream.Read(buffer, 0, size);
                            if (count > 0)
                            {
                                memory.Write(buffer, 0, count);
                            }
                        }
                        while (count > 0);
                        log.Debug( "GZip decompression process ended successfully" );
                        return Encoding.Default.GetString(memory.ToArray());
                    }
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
