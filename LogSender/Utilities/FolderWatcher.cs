using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogSender.Utilities
{
    class FolderWatcher
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger( "FolderWatcher.cs" );

        /// <summary>
        /// This function watch the binary folder when server is online
        /// </summary>
        /// <param name="dir"></param>
        /// <returns>true if sending process should begin else false</returns>
        public static bool FolderOnlineWatcher(KeyValuePair<string , DirectoryInfo> dir,int binaryFileMaxSize,int  minNumOfFilesToSend)
        {
            log.Debug( "Watching online\'" + dir.Value.Name + "\' folder" );

            if( dir.Value.EnumerateFiles( "*." + dir.Key )
                         .Where( file => ( file.Length <= binaryFileMaxSize ) && ( file.Length > 0 ) )
                         .ToArray()
                         .Length > minNumOfFilesToSend )
            {
                log.Debug( "there are files to send in " + dir.Value.Name + " folder" );
                return true;
            }
            else
            {
                log.Debug( "there are no enough files to send in " + dir.Value.Name + " folder" );
                return false;
            }
        }

        /// <summary>
        /// This function maintaine folder when server is offline 
        /// </summary>
        /// <param name="dir"></param>
        public static void FolderOfflineWatcher(KeyValuePair<string , DirectoryInfo> dir, long maxBinaryFolderSize)
        {
            log.Debug( "Watching offline \'" + dir.Value.Name + "\' folder" );

            long lenght = FileMaintenance.DirSize( dir.Value.GetFiles() );

            if( lenght > maxBinaryFolderSize )
            {
                log.Error( "The binary folder " + dir.Value.Name + " has reached size limit" );
                FileMaintenance.DeleteOldFiles( dir.Value , maxBinaryFolderSize );
            }
        }

    }
}
