using BinaryFileToTextFile.Data;
using System;
using System.Collections.Generic;

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
                    _fileExtractData[index] = new ImageName(_fileExtractData[(int)_fileExtractDataIndexs.PARENT_PATH].GetData());
                else
                    _fileExtractData[index].ExtractData(loopIndex, expandedFileByteArray, ref fileIndex);
            }
        }

        /// <summary>
        /// This function return the parameters as json
        /// </summary>
        /// <returns>jsonlog format object</returns>
        public JsonLog GetRowAsJson()
        {
            JsonLog row = new JsonLog
            {
                ClientTime = _timeStamp.GetClientTime(),
                FullServerTime = _timeStamp.GetFullServerTime(),
                ImagePath = _fileExtractData[(int)_fileExtractDataIndexs.IMAGE_PATH].GetData(),
                ImageName = _fileExtractData[(int)_fileExtractDataIndexs.IMAGE_NAME].GetData(),
                ParentPath = _fileExtractData[(int)_fileExtractDataIndexs.PARENT_PATH].GetData(),
                ParentName = _fileExtractData[(int)_fileExtractDataIndexs.PARENT_NAME].GetData(),
                Reason = _fileExtractData[(int)_fileExtractDataIndexs.REASON].GetData(),
                OS = "win"
            };

            if (_fileExtractData[(int)_fileExtractDataIndexs.PROCESS_ID].GetData() != "")
                row.ProcessID = Int32.Parse(_fileExtractData[(int)_fileExtractDataIndexs.PROCESS_ID].GetData());
            return row;
        }
    }
}