using BinaryFileToTextFile;
using BinaryFileToTextFile.Models;
using LogSender.Utilities;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.IO;
using System.Text;

namespace LogSender_Tests
{
    [TestClass]
    public class LogTests
    {
        [TestMethod]
        public void TestCybLog()
        {
            //Arrange
            string path = AppDomain.CurrentDomain.BaseDirectory;
            DirectoryInfo _directory = new DirectoryInfo(path + @"\\..\\..\\Test Logs\");
            FileInfo[] logFiles = _directory.GetFiles("*.cyb");

            BinaryFileHandler bf = new BinaryFileHandler(logFiles[0]);

            string jsonTest = string.Empty;
            string jsonTrueOutput = File.ReadAllText(path + @"\\..\\..\\Test Logs\" + "cyb json output.txt");

            //Act
            if (!bf.IsFileNull())
            {
                //preprocessing file
                BinaryFileData data = bf.SeparateHeaderAndFile();

                //expend log section
                byte[] expandedFileByteArray = data.ExpendLogSection();

                //get header parameters
                HeaderParameters headerParameters = new HeaderParameters();
                headerParameters.ExtractData(data._headerArray);

                //extract data from binary file
                CybTable log = new CybTable(expandedFileByteArray, headerParameters._hostName.GetData(), headerParameters._serverClientDelta.GetData(), headerParameters._version.GetData());

                log.ConvertRowsToCsvFormat();

                StringBuilder test = new StringBuilder();
                ParsingBinaryFile.AddOutputHeader(test);
                test.Append(log.GetDataAsString());

                jsonTest = JsonDataConvertion.JsonSerialization(test);
            }

            //Assert
            Assert.AreEqual(jsonTest, jsonTrueOutput);
        }

        [TestMethod]
        public void TestFsaLog()
        {
            //Arrange
            string path = AppDomain.CurrentDomain.BaseDirectory;
            DirectoryInfo _directory = new DirectoryInfo(path + @"\\..\\..\\Test Logs\");
            FileInfo[] logFiles = _directory.GetFiles("*.fsa");

            BinaryFileHandler bf = new BinaryFileHandler(logFiles[0]);

            string jsonTest = string.Empty;
            string jsonTrueOutput = File.ReadAllText(path + @"\\..\\..\\Test Logs\" + "fsa json output.txt");

            //Act
            if (!bf.IsFileNull())
            {
                //preprocessing file
                BinaryFileData data = bf.SeparateHeaderAndFile();

                //expend log section
                byte[] expandedFileByteArray = data.ExpendLogSection();

                //get header parameters
                HeaderParameters headerParameters = new HeaderParameters();
                headerParameters.ExtractData(data._headerArray);

                //extract data from binary file
                FsaTable log = new FsaTable(expandedFileByteArray, headerParameters._hostName.GetData(), headerParameters._serverClientDelta.GetData(), headerParameters._version.GetData());

                log.ConvertRowsToCsvFormat();

                StringBuilder test = new StringBuilder();
                ParsingBinaryFile.AddOutputHeader(test);
                test.Append(log.GetDataAsString());

                jsonTest = JsonDataConvertion.JsonSerialization(test);
            }

            //Assert
            Assert.AreEqual(jsonTest, jsonTrueOutput);
        }

        [TestMethod]
        public void TestCimgLog()
        {
            //Arrange
            string path = AppDomain.CurrentDomain.BaseDirectory;
            DirectoryInfo _directory = new DirectoryInfo(path + @"\\..\\..\\Test Logs\");
            FileInfo[] logFiles = _directory.GetFiles("*.cimg");

            BinaryFileHandler bf = new BinaryFileHandler(logFiles[0]);

            string jsonTest = string.Empty;
            string jsonTrueOutput = File.ReadAllText(path + @"\\..\\..\\Test Logs\" + "cimg json output.txt");

            //Act
            if (!bf.IsFileNull())
            {
                //preprocessing file
                BinaryFileData data = bf.SeparateHeaderAndFile();

                //expend log section
                byte[] expandedFileByteArray = data.ExpendLogSection();

                //get header parameters
                HeaderParameters headerParameters = new HeaderParameters();
                headerParameters.ExtractData(data._headerArray);

                //extract data from binary file
                DLLTable log = new DLLTable(expandedFileByteArray, headerParameters._hostName.GetData(), headerParameters._serverClientDelta.GetData());

                log.ConvertRowsToCsvFormat();

                StringBuilder test = new StringBuilder();
                ParsingBinaryFile.AddOutputHeader(test);
                test.Append(log.GetDataAsString());

                jsonTest = JsonDataConvertion.JsonSerialization(test);
            }

            //Assert
            Assert.AreEqual(jsonTest, jsonTrueOutput);
        }

        [TestMethod]
        public void TestMogLog()
        {
            //Arrange
            string path = AppDomain.CurrentDomain.BaseDirectory;
            DirectoryInfo _directory = new DirectoryInfo(path + @"\\..\\..\\Test Logs\");
            FileInfo[] logFiles = _directory.GetFiles("*.mog");

            BinaryFileHandler bf = new BinaryFileHandler(logFiles[0]);

            string jsonTest = string.Empty;
            string jsonTrueOutput = File.ReadAllText(path + @"\\..\\..\\Test Logs\" + "mog json output.txt");

            //Act
            if (!bf.IsFileNull())
            {
                //preprocessing file
                BinaryFileData data = bf.SeparateHeaderAndFile();

                //expend log section
                byte[] expandedFileByteArray = data.ExpendLogSection();

                //get header parameters
                HeaderParameters headerParameters = new HeaderParameters();
                headerParameters.ExtractData(data._headerArray);

                //extract data from binary file
                MogTable log = new MogTable(expandedFileByteArray, headerParameters._hostName.GetData(), headerParameters._serverClientDelta.GetData(), headerParameters._version.GetData());

                log.ConvertRowsToCsvFormat();

                StringBuilder test = new StringBuilder();
                ParsingBinaryFile.AddOutputHeader(test);
                test.Append(log.GetDataAsString());

                jsonTest = JsonDataConvertion.JsonSerialization(test);
            }

            //Assert
            Assert.AreEqual(jsonTest, jsonTrueOutput);
        }
    }
}
