using BinaryFileToTextFile.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace BinaryFileToTextFile
{
    class DLLTable : Table
    {
        ///**********************************************
        ///             Members Section
        ///**********************************************
        
        //Constant section
        const int BYTES_IN_ROW = 2064;

        //Private Section
        private List<DLLRow> _DLLTable;

        ///**********************************************
        ///             Functions Section
        ///**********************************************

        //Ctor of DLLTable class
        public DLLTable(byte[] expandedFileByteArray, string hostName, Int64 serverClientDelta)
        {
            try
            {
                //create new DLL list
                _DLLTable = new List<DLLRow>();
                

                for (int loopIndex = 0; loopIndex < expandedFileByteArray.Length; loopIndex = loopIndex  + BYTES_IN_ROW)
                {
                    //create new row
                    DLLRow row = new DLLRow(serverClientDelta);
                    row.ExtractData(loopIndex, expandedFileByteArray);
                    _DLLTable.Add(row);
                }
            }
            catch (Exception ex)
            {
                //TODO:Create logger
                System.IO.StreamWriter logFile = new System.IO.StreamWriter(AppDomain.CurrentDomain.BaseDirectory + "log.txt", true);
                logFile.WriteLine("DLL Table exception\n" + ex.Message);
                logFile.Close();
            }
        }

        /// <summary>
        /// This function convert table to json array
        /// </summary>
        /// <returns>json log array </returns>
        public JsonLog[] GetAsJson()
        {
            JsonLog[] jsonArray = new JsonLog[_DLLTable.Count];

            for (int index = 0; index < _DLLTable.Count; index++)
                jsonArray[index] = _DLLTable[index].GetRowAsJson();

            return jsonArray;
        }
    }
}
