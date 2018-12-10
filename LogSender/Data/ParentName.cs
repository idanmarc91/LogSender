using System;
using System.Linq;

namespace LogSender.Data
{
    class ParentName :FileData
    {
        ///**********************************************
        ///             Members Section
        ///**********************************************
        private string _parentName;

        ///**********************************************
        ///             Functions Section
        ///**********************************************

        /// <summary>
        /// empty Ctor fot Parent name class
        /// </summary>
        public ParentName()
        {}

        /// <summary>
        /// Ctor fot Parent name class
        /// </summary>
        /// <param name="parentPath"></param>
        public ParentName(string parentPath)
        {
            _parentName = parentPath.Split('\\').LastOrDefault();
        }

        /// <summary>
        /// retrun parent name
        /// </summary>
        /// <returns>string - parent name</returns>
        public override string GetData()
        {
            return _parentName;
        }

        /// <summary>
        /// set new parent name
        /// </summary>
        /// <param name="data"></param>
        public override void SetData(string data)
        {
            _parentName = data;
        }

        public override void ExtractData(int loopIndex, byte[] expandedFileByteArray, ref int fileIndex)
        {
            throw new NotImplementedException();
        }
    }
}
