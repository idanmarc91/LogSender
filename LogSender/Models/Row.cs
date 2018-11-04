using BinaryFileToTextFile.Data;
using System;
using System.Collections.Generic;
using System.Text;

namespace BinaryFileToTextFile.Models
{
    abstract class Row
    {
        ///**********************************************
        ///             Members Section
        ///**********************************************
        protected List<FileData> _fileExtractData;
        protected TimeStamp _timeStamp;


        public virtual StringBuilder BuildAsCsv(List<string> paramList )
        {
            StringBuilder dataAsString = new StringBuilder();
            dataAsString.Append( String.Join( "," , paramList ) );

            dataAsString.Append( "," );

            dataAsString.Append( "\n " );

            return dataAsString;
        }
    }
}
