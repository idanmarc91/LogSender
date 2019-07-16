using LogSender.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace LogSender
{
    public class DLLTable : Table
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger( "DLLTable.cs" );

        ///**********************************************
        ///             Members Section
        ///**********************************************

        #region Members section

        //Private Section
        private readonly List<DLLRow> _DLLTable;

        #endregion Member section

        ///**********************************************
        ///             Functions Section
        ///**********************************************

        #region Function section

        //Ctor of DLLTable class
        public DLLTable(byte[] expandedFileByteArray , string reportingComputer , Int64 serverClientDelta)
        {
            try
            {
                //create new DLL list
                _DLLTable = new List<DLLRow>();

                for( int loopIndex = 0 ; loopIndex < expandedFileByteArray.Length ; loopIndex = loopIndex + Utilities.Constant.DLL_ROW_SIZE)
                {
                    //create new row
                    DLLRow row = new DLLRow( serverClientDelta, reportingComputer);
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

        #endregion Function section

    }
}
