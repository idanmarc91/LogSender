using System;
using System.Linq;

namespace BinaryFileToTextFile.Data
{
    class ImageName :FileData
    {
        ///**********************************************
        ///             Members Section
        ///**********************************************
        private string _imageName;

        ///**********************************************
        ///             Functions Section
        ///**********************************************

        /// <summary>
        /// empty ctor for image name class
        /// </summary>
        public ImageName()
        {
        }

        /// <summary>
        /// ctor for ImmageName class
        /// </summary>
        /// <param name="imagePath"></param>
        public ImageName(string imagePath)
        {
            _imageName = imagePath.Split('\\').LastOrDefault();
        }

        /// <summary>
        /// retrun image name
        /// </summary>
        /// <returns></returns>
        public override string GetData()
        {
            return _imageName;
        }

        /// <summary>
        /// set new image name
        /// </summary>
        /// <param name="data"></param>
        public override void SetData(string data)
        {
            _imageName = data;
        }

        public override void ExtractData(int loopIndex, byte[] expandedFileByteArray, ref int fileIndex)
        {
            throw new NotImplementedException();
        }
    }
}
