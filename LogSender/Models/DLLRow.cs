using BinaryFileToTextFile.Data;
using System;
using System.Collections.Generic;
using System.Text;

namespace BinaryFileToTextFile.Models
{
    class DLLRow :Row
    {
        ///**********************************************
        ///             Members Section
        ///**********************************************
        enum _fileExtractDataIndexs
        {
            PROCESS_ID, IMAGE_PATH, IMAGE_NAME, PARENT_PATH, PARENT_NAME, REASON
        };

        ///**********************************************
        ///             Functions Section
        ///**********************************************

        /// <summary>
        /// Ctor of DLL Row class
        /// </summary>
        /// <param name="serverClientDelta"></param>
        /// <param name="rowNumber"></param>
        public DLLRow (Int64 serverClientDelta)
        {
            _timeStamp = new TimeStamp(serverClientDelta);
            _fileExtractData = new List<FileData>
            {
                new ProcessId(),
                new ImagePath(),
                new ImageName(),
                new ParentPath(),
                new ParentName(),
                new ReasonDll(),
            };
        }

        /// <summary>
        /// This function extract data from binary file
        /// </summary>
        /// <param name="loopIndex"></param>
        /// <param name="expandedFileByteArray"></param>
        public void ExtractData(int loopIndex,byte [] expandedFileByteArray)
        {
            int fileIndex = 0;
            _timeStamp.ExtractData(loopIndex, expandedFileByteArray, ref fileIndex);

            for(int index = 0; index < _fileExtractData.Count; index++)
            {
                if (index == (int)_fileExtractDataIndexs.IMAGE_NAME)
                    _fileExtractData[index] = new ImageName(_fileExtractData[(int)_fileExtractDataIndexs.IMAGE_PATH].GetData());

                else if (index == (int)_fileExtractDataIndexs.PARENT_NAME)
                    _fileExtractData[index] = new ParentName(_fileExtractData[(int)_fileExtractDataIndexs.PARENT_PATH].GetData());
                else
                    _fileExtractData[index].ExtractData(loopIndex, expandedFileByteArray, ref fileIndex);
            }
        }

        /// <summary>
        /// Get dll row parameters as List of string
        /// </summary>
        /// <returns>list<string> - dll parameters</returns>
        private List<string> GetAsList()
        {
            List<string> list = new List<string>
            {
                "win",
                _timeStamp.GetClientTime(),
                _timeStamp.GetFullServerTime(),
                _fileExtractData[(int)_fileExtractDataIndexs.PROCESS_ID].GetData(),
                "",
                "",
                "",
                "",
                "",
                "",
                "",
                "",
                "",
                "",
                "",
                "",
                "",
                "",
                "",
                "",
                "",
                "",
                _fileExtractData[(int)_fileExtractDataIndexs.REASON].GetData(),
                _fileExtractData[(int)_fileExtractDataIndexs.IMAGE_PATH].GetData(),
                _fileExtractData[(int)_fileExtractDataIndexs.IMAGE_NAME].GetData(),
                _fileExtractData[(int)_fileExtractDataIndexs.PARENT_PATH].GetData(),
                _fileExtractData[(int)_fileExtractDataIndexs.PARENT_NAME].GetData(),
            };
            return list;
        }

        /// <summary>
        /// This function add current row to data output
        /// </summary>
        /// <param name="dataAsString"></param>
        public void AddRowToDataOutput(StringBuilder dataAsString)
        {
            List<string> paramList = GetAsList();
            BuildAsCsv(paramList, dataAsString);
        }
    }
}