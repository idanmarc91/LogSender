using System;
using System.Collections.Generic;
using System.Text;

namespace BinaryFileToTextFile.Models
{
    class MogTable :Table
    {       ///**********************************************
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
        public MogTable(byte[] expandedFileByteArray, string hostName, Int64 serverClientDelta, UInt16 headerVersion)
        {
            try
            {
                _mogTable = new List<MogRow>();
                _serviceMogTable = new List<MogRow>();

                int bytesInRow = DefineRowSize(headerVersion, BYTES_IN_ROW);

                for (int loopIndex = 0; loopIndex < expandedFileByteArray.Length; loopIndex = loopIndex + bytesInRow)
                {

                    //create new row
                    MogRow row = new MogRow(serverClientDelta, hostName, headerVersion);

                    row.ExtractData(loopIndex, expandedFileByteArray);

                    if (row.CheckSubSeqNum())
                        _mogTable.Add(row);
                    else
                        _serviceMogTable.Add(row);
                }


                //ExpandSVCHost();
            }
            catch (Exception ex)
            {
                //TODO:Create logger
                System.IO.StreamWriter logFile = new System.IO.StreamWriter(AppDomain.CurrentDomain.BaseDirectory + "log.txt", true);
                logFile.WriteLine("Mog Table exception\n" + ex.Message);
                logFile.Close();
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
                        if ((serviceRow.GetSubSeqNum() == row.GetSeqNum()) && (serviceRow.GetFullAccTime() == row.GetFullAccTime()))
                            row.ExpandSvc(serviceRow);

                    }
                    _serviceMogTable.RemoveAll(i => i.GetFullAccTime() == row.GetFullAccTime() && i.GetSubSeqNum() == row.GetSeqNum());
                    //add counter of svchost process name
                    if (row.GetAppName() != "")
                        row.ChangeAppName();
                }
            }
        }

        /// <summary>
        /// This function convert table to json array
        /// </summary>
        /// <returns>json log array </returns>
        public JsonLog[] GetAsJson()
        {
            JsonLog[] jsonArray = new JsonLog[_mogTable.Count];

            for (int index = 0; index < _mogTable.Count; index++)
                jsonArray[index] = _mogTable[index].GetRowAsJson();

            return jsonArray;
        }
    }

}


