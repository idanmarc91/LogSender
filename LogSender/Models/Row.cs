using LogSender.Data;
using System.Collections.Generic;
using System.Text;

namespace LogSender.Models
{
    abstract class Row
    {
        ///**********************************************
        ///             Members Section
        ///**********************************************
        protected string _reportingComputer;
        protected List<FileData> _fileExtractData;
        protected TimeStamp _timeStamp;

        ///**********************************************
        ///             Functions Section
        ///**********************************************
        public virtual StringBuilder BuildAsCsv(List<string> paramList)
        {
            StringBuilder dataAsString = new StringBuilder();
            string test = ServiceStack.Text.CsvSerializer.SerializeToCsv(paramList);

            dataAsString.Append(test);

            return dataAsString;
        }
    }
}
