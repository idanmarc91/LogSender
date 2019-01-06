using System;
using System.Collections.Generic;
using System.Text;

namespace LogSender.Models
{
    public class MogTable : Table
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger( "MogTable.cs" );

        ///**********************************************
        ///             Members Section
        ///**********************************************

        #region Members section

        //Constant Section
        const int BYTES_IN_ROW = 1608;

        //Private Section
        private readonly List<MogRow> _mogTable;
        private readonly List<MogRow> _serviceMogTable;

        #endregion Member section

        ///**********************************************
        ///             Functions Section
        ///**********************************************

        #region Function section

        //Ctor of MogTable Class
        public MogTable(byte[] expandedFileByteArray , string reportingComputer , Int64 serverClientDelta , UInt16 headerVersion)
        {
            try
            {
                _mogTable = new List<MogRow>();
                _serviceMogTable = new List<MogRow>();

                int bytesInRow = DefineRowSize( headerVersion , BYTES_IN_ROW );

                for( int loopIndex = 0 ; loopIndex < expandedFileByteArray.Length ; loopIndex = loopIndex + bytesInRow )
                {
                    //create new row
                    MogRow row = new MogRow( serverClientDelta , reportingComputer , headerVersion );

                    row.ExtractData( loopIndex , expandedFileByteArray );

                    if( row.CheckSubSeqNum() )
                    {
                        _mogTable.Add( row );
                    }
                    else
                    {
                        _serviceMogTable.Add( row );
                    }
                }
                ExpandSVCHost();

            }
            catch ( Exception ex )
            {
                log.Error( "Problem with creating MOG table for one of the binary files" , ex );
            }
        }

        /// <summary>
        ///  this function expand SVC Host
        /// </summary>
        private void ExpandSVCHost()
        {
            foreach (MogRow row in _mogTable)
            {
                if (row.GetSeqNum() != "0")
                {
                    foreach (MogRow serviceRow in _serviceMogTable)
                    {
                        if ((serviceRow.GetSubSeqNum() == row.GetSeqNum()) && (serviceRow.TimeStamp._fullServerTimeStamp == row.TimeStamp._fullServerTimeStamp))
                        {
                            row.ExpandSvc(serviceRow);
                        }
                    }
                    _serviceMogTable.RemoveAll(i => i.TimeStamp._fullServerTimeStamp == row.TimeStamp._fullServerTimeStamp && i.GetSubSeqNum() == row.GetSeqNum());
                }
            }
        }

        /// <summary>
        /// This function convert table to csv array
        /// </summary>
        public override void ConvertRowsToCsvFormat()
        {
            csvFormat = new StringBuilder();

            foreach( MogRow row in _mogTable )
            {
                csvFormat.Append( row.AddRowToDataOutput() );

            }
        }

        #endregion Function section
    }
}


