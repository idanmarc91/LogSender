using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace LogSender.Utilities
{
    public class FileSorting
    {
        /// <summary>
        /// This function sort the file in folder by configuration setting
        /// </summary>
        /// <param name="dir"></param>
        /// <returns></returns>
        public static FileInfo[] GetAllFilesByConfigSettings(KeyValuePair<string, DirectoryInfo> dir)
        {
            return dir.Value.EnumerateFiles("*." + dir.Key)
                             .Where(file => (file.Length <= ConfigFile.Instance._configData._binaryFileMaxSize) && (file.Length > 0))
                             .ToArray();
        }

    }
}
