using BinaryFileToTextFile.Models;
using System.Collections.Generic;
using System.Text;

namespace BinaryFileToTextFile
{
    public abstract class Table
    {
        ///**********************************************
        ///             Members Section
        ///**********************************************

        //Constant Section
        const int NEW_VER_BYTE_EXTENTION = 512;

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

        public abstract void GetAsJson(StringBuilder dataAsString);
    }
}
