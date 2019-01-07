using LogSender.Models;
using LogSender.Utilities;
using System;
using System.Collections.Generic;
using System.Text;

namespace LogSender
{
    public class FsaTable : Table
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger( "FSATable.cs" );

        ///**********************************************
        ///             Members Section
        ///**********************************************

        #region Members section

        //Private Section
        private readonly List<FSARow> _FsaTable;
        private readonly List<FSARow> _servicesFsaTable;

        #endregion Member section

        ///**********************************************
        ///             Functions Section
        ///**********************************************

        #region Function section

        //Ctor of FSATable
        public FsaTable(byte[] expandedFileByteArray , string reportingComputer , Int64 serverClientDelta , UInt16 headerVersion)
        {
            try
            {
                _FsaTable = new List<FSARow>();
                _servicesFsaTable = new List<FSARow>();

                //define how much bytes in each FSA row 
                int bytesInRow = DefineRowSize( headerVersion , Utilities.Constant.FSA_ROW_SIZE);

                string _sourceIP = ServerConnection.GetLocalIPAddress();

                //main loop iteration binary file and extract data from it
                for( int loopIndex = 0 ; loopIndex < expandedFileByteArray.Length ; loopIndex = loopIndex + bytesInRow )
                {
                    FSARow row = new FSARow( serverClientDelta , reportingComputer , headerVersion, _sourceIP);
                    row.ExtractData( loopIndex , expandedFileByteArray );

                    if( row.GetSubSeqNum() == "0" )
                    {
                        _FsaTable.Add( row );
                    }
                    else
                    {
                        _servicesFsaTable.Add( row );
                    }
                }

                ExpandSVCHost();
            }
            catch( Exception ex )
            {
                log.Error( "Problem with creating FSA table for one of the binary files" , ex );
            }
        }

        /// <summary>
        ///  this function expand SVC Host
        /// </summary>
        private void ExpandSVCHost()
        {
            foreach( FSARow row in _FsaTable )
            {
                if( row.GetSeqNum() != "0" )
                {
                    foreach( FSARow serviceRow in _servicesFsaTable )
                    {
                        if( ( serviceRow.GetSubSeqNum() == row.GetSeqNum() ) && ( serviceRow.TimeStamp._fullServerTimeStamp == row.TimeStamp._fullServerTimeStamp) )
                        {
                            row.ExpandSvc( serviceRow );
                        }
                    }
                    _servicesFsaTable.RemoveAll( i => i.TimeStamp._fullServerTimeStamp == row.TimeStamp._fullServerTimeStamp && i.GetSubSeqNum() == row.GetSeqNum() );
                }
            }
        }

        /// <summary>
        /// This function convert table to csv array
        /// </summary>
        public override void ConvertRowsToCsvFormat()
        {
            csvFormat = new StringBuilder();

            foreach( FSARow row in _FsaTable )
            {
                csvFormat.Append( row.AddRowToDataOutput() );
            }
        }
        
        #endregion Function section
    }
}
