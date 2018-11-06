using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace LogSender.Utilities
{
    public class FileMaintenance
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger( "FileMaintenance.cs" );

        public static bool IsFileLocked(FileInfo file)
        {
            FileStream stream = null;

            try
            {
                stream = file.Open( FileMode.Open , FileAccess.Read , FileShare.None );
            }
            catch( IOException ex)
            {
                //the file is unavailable because it is:
                //still being written to
                //or being processed by another thread
                //or does not exist (has already been processed)
                log.Error( file.Name + " file is in writing mode. cannot be access!",ex );
                return true;
            }
            finally
            {
                if( stream != null )
                {
                    stream.Close();
                }
            }

            //file is not locked
            return false;
        }

        public static void FileDelete(List<FileInfo> listOfFileToDelete)
        {
            log.Debug( "starting delete files process" );

            foreach( FileInfo file in listOfFileToDelete )
            {
                file.Delete();
                log.Debug( file.Name + " file has deleted" );
            }
        }

        public static long DirSize(FileInfo[] fileArray)
        {
            long size = 0;
            // Add file sizes.
            foreach( FileInfo fi in fileArray )
            {
                size += fi.Length;
            }
            return size;
        }

        public static void DeleteOldFiles(DirectoryInfo dir , long binaryFileMaxSize)
        {
            log.Debug( "starting delete old file" );

            FileInfo[] files = dir.GetFiles().OrderByDescending( p => p.CreationTime ).ToArray();

            for(int index = 0 ; FileMaintenance.DirSize( dir.GetFiles() ) > binaryFileMaxSize ; index++ )
            {
                files[index].Delete();
            }
        }
    }
}
