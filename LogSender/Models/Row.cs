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


        public virtual void BuildAsCsv(List<string> paramList , StringBuilder dataAsString)
        {
            dataAsString.Append( String.Join( "," , paramList ) );

            dataAsString.Append( "," );

            dataAsString.Append( "\n " );
        }
    }
}
