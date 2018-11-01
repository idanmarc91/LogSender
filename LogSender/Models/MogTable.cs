using System;
using System.Collections.Generic;
using System.Text;

namespace BinaryFileToTextFile.Models
{
    public class MogTable : Table
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger( "MogTable.cs" );

        ///**********************************************
        ///             Members Section
        ///**********************************************

        //Constant Section
        const int BYTES_IN_ROW = 1608;

        //Private Sction
        private List<MogRow> _mogTable;
        private List<MogRow> _serviceMogTable;

        ///**********************************************
        ///             Functions Section
        ///**********************************************

        //Ctor of MogTable Class
        public MogTable(byte[] expandedFileByteArray , string hostName , Int64 serverClientDelta , UInt16 headerVersion)
        {
            try
            {
                _mogTable = new List<MogRow>();
                _serviceMogTable = new List<MogRow>();

                int bytesInRow = DefineRowSize( headerVersion , BYTES_IN_ROW );

                for( int loopIndex = 0 ; loopIndex < expandedFileByteArray.Length ; loopIndex = loopIndex + bytesInRow )
                {
                    //create new row
                    MogRow row = new MogRow( serverClientDelta , hostName , headerVersion );

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
            }
            catch( Exception ex )
            {
                log.Error( "Problem with creating MOG table for one of the binary files" , ex );
            }
        } 

        /// <summary>
        /// This function convert table to json array
        /// </summary>
        /// <returns>json log array</returns>
        public override void GetAsJson(StringBuilder dataAsString)
        {
            foreach( MogRow row in _mogTable )
            {
                row.AddRowToDataOutput( dataAsString );
            }
        }
    }
}


