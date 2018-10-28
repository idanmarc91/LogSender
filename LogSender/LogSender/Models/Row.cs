using BinaryFileToTextFile.Data;
using System.Collections.Generic;

namespace BinaryFileToTextFile.Models
{
    abstract class Row
    {
        ///**********************************************
        ///             Members Section
        ///**********************************************
        protected List<FileData> _fileExtractData;
        protected TimeStamp _timeStamp;
    }
}
