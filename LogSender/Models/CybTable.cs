using LogSender.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace LogSender
{
    public class CybTable : Table
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger("CybTable.cs");

        ///**********************************************
        ///             Members Section
        ///**********************************************

        //Constant Section
        const int BYTES_IN_ROW = 1600;

        //Private Sction
        private readonly List<CybRow> _cybTable;
        private readonly List<CybRow> _servicesCybTable;

        ///**********************************************
        ///             Functions Section
        ///**********************************************

        //Ctor of CybTable Class
        public CybTable(byte[] expandedFileByteArray, string reportinComputer, Int64 serverClientDelta, UInt16 headerVersion)
        {
            try
            {
                _cybTable = new List<CybRow>();
                _servicesCybTable = new List<CybRow>();

                int bytesInRow = DefineRowSize(headerVersion, BYTES_IN_ROW);

                for (int loopIndex = 0; loopIndex < expandedFileByteArray.Length; loopIndex = loopIndex + bytesInRow)
                {
                    //create new row
                    CybRow row = new CybRow(serverClientDelta, reportinComputer, headerVersion);
                    row.ExtractData(loopIndex, expandedFileByteArray);

                    if (row.CheckSubSeqNum())
                    {
                        _cybTable.Add(row);
                    }
                    else
                    {
                        _servicesCybTable.Add(row);
                    }
                }
                ExpandSVCHost();
            }
            catch (Exception ex)
            {
                log.Error("Problem with creating cyb table for one of the binary files", ex);
            }

        }

        /// <summary>
        ///  this function expand SVC Host
        /// </summary>
        private void ExpandSVCHost()
        {
            foreach (CybRow row in _cybTable)
            {
                if (row.GetSeqNum() != "0")
                {
                    foreach (CybRow serviceRow in _servicesCybTable)
                    {
                        if ((serviceRow.GetSubSeqNum() == row.GetSeqNum()) && (serviceRow.GetFullAccTime() == row.GetFullAccTime()))
                        {
                            row.ExpandSvc(serviceRow);
                        }
                    }
                    _servicesCybTable.RemoveAll(i => i.GetFullAccTime() == row.GetFullAccTime() && i.GetSubSeqNum() == row.GetSeqNum());
                }
            }
        }

        /// <summary>
        /// This function convert table to csv array
        /// </summary>
        public override void ConvertRowsToCsvFormat()
        {
            csvFormat = new StringBuilder();

            foreach (CybRow row in _cybTable)
            {
                csvFormat.Append(row.AddRowToDataOutput());
            }
        }
    }
}


