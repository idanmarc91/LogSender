﻿
namespace LogSender.Data
{
    class ScrambleState : FileData
    {
        ///**********************************************
        ///             Members Section
        ///**********************************************
        const int STATE_LEN = 1;
        private string _state;

        ///**********************************************
        ///             Functions Section
        ///**********************************************   
        /// <summary>
        /// This function extract state string 
        /// </summary>
        /// <param name="loopIndex"></param>
        /// <param name="expandedFileByteArray"></param>
        /// <param name="fileIndex"></param>
        public override void ExtractData(int loopIndex, byte[] expandedFileByteArray, ref int fileIndex)
        {
            System.Text.StringBuilder builder = new System.Text.StringBuilder();
            builder.Append(GetData(loopIndex, expandedFileByteArray, ref fileIndex, STATE_LEN)[0]);
            switch (builder.ToString())
            {
                case "1":
                    _state = "Defense";
                    break;
                case "2":
                    _state = "Monitor";
                    break;
                case "3":
                    _state = "NoKey";
                    break;
                default:
                    _state = "Error";
                    break;
            }
            
            //ignore one byte
            fileIndex++;
        }

        /// <summary>
        /// get state
        /// </summary>
        /// <returns>string - state</returns>
        public override string GetData()
        {
            return _state;
        }

        /// <summary>
        /// set state 
        /// </summary>
        public override void SetData(string state)
        {
            _state = state;
        }
    }
}
