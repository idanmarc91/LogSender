using System.Text;

namespace LogSender
{
    public abstract class Table
    {
        ///**********************************************
        ///             Members Section
        ///**********************************************

        //Constant Section
        private const int NEW_VER_BYTE_EXTENTION = 512;

        protected StringBuilder csvFormat;
        ///**********************************************
        ///             Functions Section
        ///**********************************************

        /// <summary>
        /// This function calculate Byte in row depend on header version
        /// </summary>
        /// <param name="headerVersion"></param>
        /// <returns>int - Byte in row</returns>
        protected int DefineRowSize(ushort headerVersion, int byte_in_row)
        {
            return (headerVersion > 2) ? (byte_in_row + NEW_VER_BYTE_EXTENTION) : byte_in_row;
        }

        public abstract void ConvertRowsToCsvFormat();

        /// <summary>
        /// get csv format data
        /// </summary>
        /// <returns></returns>
        public StringBuilder GetDataAsString()
        {
            return csvFormat;
        }

        /// <summary>
        /// Get csv format string lenght (in bytes)
        /// </summary>
        /// <returns>int -  size of csv lenght</returns>
        internal int GetDataSize()
        {
            return csvFormat.Length;
        }
    }
}
