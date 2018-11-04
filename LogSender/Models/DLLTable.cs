using BinaryFileToTextFile.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace BinaryFileToTextFile
{
    public class DLLTable : Table
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger( "DLLTable.cs" );

        ///**********************************************
        ///             Members Section
        ///**********************************************

        //Constant section
        const int BYTES_IN_ROW = 2064;

        //Private Section
        private readonly List<DLLRow> _DLLTable;

        ///**********************************************
        ///             Functions Section
        ///**********************************************

        //Ctor of DLLTable class
        public DLLTable(byte[] expandedFileByteArray , string hostName , Int64 serverClientDelta)
        {
            try
            {
                //create new DLL list
                _DLLTable = new List<DLLRow>();


                for( int loopIndex = 0 ; loopIndex < expandedFileByteArray.Length ; loopIndex = loopIndex + BYTES_IN_ROW )
                {
                    //create new row
                    DLLRow row = new DLLRow( serverClientDelta );
                    row.ExtractData( loopIndex , expandedFileByteArray );
                    _DLLTable.Add( row );
                }
            }
            catch( Exception ex )
            {
                log.Error( "Problem with creating DLL table for one of the binary files" , ex );
            }
        }

        /// <summary>
        /// This function convert table to csv array
        /// </summary>
        public override void ConvertRowsToCsvFormat()
        {
            csvFormat = new StringBuilder();

            foreach( DLLRow row in _DLLTable )
            {
                csvFormat.Append( row.AddRowToDataOutput() );
            }
        }
    }
}
