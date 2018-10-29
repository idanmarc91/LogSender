using BinaryFileToTextFile.Models;
using System;
using System.Collections.Generic;

namespace BinaryFileToTextFile
{
    class CybTable : Table
    {
        ///**********************************************
        ///             Members Section
        ///**********************************************

        //Constant Section
        const int BYTES_IN_ROW = 1600;

        //Private Sction
        private List<CybRow> _cybTable;
        private List<CybRow> _servicesCybTable;

        ///**********************************************
        ///             Functions Section
        ///**********************************************

        //Ctor of CybTable Class
        public CybTable(byte[] expandedFileByteArray, string hostName, Int64 serverClientDelta, UInt16 headerVersion)
        {
            try
            {
                _cybTable = new List<CybRow>();
                _servicesCybTable = new List<CybRow>();

                int bytesInRow = DefineRowSize(headerVersion, BYTES_IN_ROW);
               
                for (int loopIndex = 0; loopIndex < expandedFileByteArray.Length; loopIndex = loopIndex + bytesInRow)
                {
                    //create new row
                    CybRow row = new CybRow(serverClientDelta, hostName, headerVersion);
                    row.ExtractData(loopIndex, expandedFileByteArray);

                    if (row.CheckSubSeqNum())
                        _cybTable.Add(row);
                    else
                        _servicesCybTable.Add(row);
                }
                ExpandSVCHost();
            }
            catch (Exception ex)
            {
                //TODO:Create logger
                System.IO.StreamWriter logFile = new System.IO.StreamWriter(AppDomain.CurrentDomain.BaseDirectory + "log.txt", true);
                logFile.WriteLine("Cyb Table exception\n" + ex.Message);
                logFile.Close();
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
                            row.ExpandSvc(serviceRow);

                    }
                    _servicesCybTable.RemoveAll(i => i.GetFullAccTime() == row.GetFullAccTime() && i.GetSubSeqNum() == row.GetSeqNum());
                    //add counter of svchost process name
                    if (row.GetAppName() != "")
                        row.ChangeAppName();
                }
            }
        }

        /// <summary>
        /// This function convert table to json array
        /// </summary>
        /// <returns>json log array</returns>
        public void GetAsJson(List<JsonLog> array)
        {
            //JsonLog[] jsonArray = new JsonLog[_cybTable.Count];

            //for (int index = 0; index < _cybTable.Count; index++)
            //    jsonArray[index] = _cybTable[index].GetRowAsJson();

            //return jsonArray;

            foreach(CybRow row in _cybTable)
            {
                array.Add(row.GetRowAsJson());
            }
        }
    }

}


