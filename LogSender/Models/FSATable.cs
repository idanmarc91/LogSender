﻿using BinaryFileToTextFile.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace BinaryFileToTextFile
{
    public class FSATable : Table
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger( "FSATable.cs" );

        ///**********************************************
        ///             Members Section
        ///**********************************************

        //Constant Section
        const int BYTES_IN_ROW = 2584;

        //Private Section
        private List<FSARow> _FsaTable;
        private List<FSARow> _servicesFsaTable;


        ///**********************************************
        ///             Functions Section
        ///**********************************************

        //Ctor of FSATable
        public FSATable(byte[] expandedFileByteArray , string hostName , Int64 serverClientDelta , UInt16 headerVersion)
        {
            try
            {
                _FsaTable = new List<FSARow>();
                _servicesFsaTable = new List<FSARow>();

                //define how much bytes in each FSA row 
                int bytesInRow = DefineRowSize( headerVersion , BYTES_IN_ROW );

                //main loop itaration binary file and extract data from it
                for( int loopIndex = 0 ; loopIndex < expandedFileByteArray.Length ; loopIndex = loopIndex + bytesInRow )
                {
                    FSARow row = new FSARow( serverClientDelta , hostName , headerVersion );
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
                        if( ( serviceRow.GetSubSeqNum() == row.GetSeqNum() ) && ( serviceRow.GetFullAccTime() == row.GetFullAccTime() ) )
                        {
                            row.ExpandSvc( serviceRow );
                        }
                    }
                    _servicesFsaTable.RemoveAll( i => i.GetFullAccTime() == row.GetFullAccTime() && i.GetSubSeqNum() == row.GetSeqNum() );
                    //add counter of svchost process name
                    if( row.GetProcessName() != "" )
                    {
                        row.ChangeProcessName();
                    }
                }
            }
        }

        /// <summary>
        /// This function convert table to json array
        /// </summary>
        /// <returns>json log array</returns>
        public override void GetAsJson(StringBuilder dataAsString)
        {
            foreach( FSARow row in _FsaTable )
            {
                row.AddRowToDataOutput( dataAsString );
            }
        }
    }
}
