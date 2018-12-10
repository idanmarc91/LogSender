using System;
using System.Linq;

namespace LogSender.Data
{
    class DllName :FileData
    {
        ///**********************************************
        ///             Members Section
        ///**********************************************
        private string _dllName;

        ///**********************************************
        ///             Functions Section
        ///**********************************************

        /// <summary>
        /// empty ctor for image name class
        /// </summary>
        public DllName()
        {}

        /// <summary>
        /// ctor for ImmageName class
        /// </summary>
        /// <param name="imagePath"></param>
        public DllName(string imagePath)
        {
            _dllName = imagePath.Split('\\').LastOrDefault();
        }

        /// <summary>
        /// retrun image name
        /// </summary>
        /// <returns></returns>
        public override string GetData()
        {
            return _dllName;
        }

        /// <summary>
        /// set new image name
        /// </summary>
        /// <param name="data"></param>
        public override void SetData(string data)
        {
            _dllName = data;
        }

        
        /// <summary>
        /// No extraction needed
        /// </summary>
        /// <param name="loopIndex"></param>
        /// <param name="expandedFileByteArray"></param>
        /// <param name="fileIndex"></param>
        public override void ExtractData(int loopIndex, byte[] expandedFileByteArray, ref int fileIndex)
        {
            throw new NotImplementedException();
        }
    }
}
