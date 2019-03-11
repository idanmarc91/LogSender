using LogSender.Utilities;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.IO;

namespace LogSender_Tests
{
    [TestClass]
    public class FolderWatcherTests
    {

        /// <summary>
        /// This test is using the config file. the folder:"FolderReady" and "FolderNotReady" will give currect answer if
        /// minNumOfFilesToSend = 2, binaryFileMaxSize = 6291456, maxBinaryFolderSize = 104857600
        /// </summary>
        [TestMethod]
        public void IsFolderReadyToSendTest_CheckIfFolderIsReady()
        {
            //Arrange
            string path = AppDomain.CurrentDomain.BaseDirectory;

            ConfigFile _configFile = ConfigFile.Instance;
            _configFile._configData._minNumOfFilesToSend = 2;
            _configFile._configData._binaryFileMaxSize = 6291456;
            _configFile._configData._binaryFolderMaxSize = 104857600;

            KeyValuePair<string, DirectoryInfo> FolderReady = new KeyValuePair<string, DirectoryInfo>
                                                            ("fsa", new DirectoryInfo(path + @"\\..\\..\\Watcher Tests\FolderReady"));
            KeyValuePair<string, DirectoryInfo> FolderNotReady = new KeyValuePair<string, DirectoryInfo>
                                                           ("fsa", new DirectoryInfo(path + @"\\..\\..\\Watcher Tests\FolderNotReady"));
            //Act
            bool folderReadyValueTrue = FolderWatcher.IsFolderReadyToSendWatcher(FolderReady);
            bool folderReadyValueFalse = FolderWatcher.IsFolderReadyToSendWatcher(FolderNotReady);

            //Assert
            Assert.AreNotEqual(folderReadyValueTrue, folderReadyValueFalse);
        }

        [TestMethod]
        public void IsFolderReadyToSendTest_OverWeightFile()
        {
            //Arrange
            string path = AppDomain.CurrentDomain.BaseDirectory;

            ConfigFile _configFile = ConfigFile.Instance;
            _configFile._configData._minNumOfFilesToSend = 2;
            _configFile._configData._binaryFileMaxSize = 100;
            _configFile._configData._binaryFolderMaxSize = 104857600;

            KeyValuePair<string, DirectoryInfo> overWeightFileInFolder = new KeyValuePair<string, DirectoryInfo>
                                                            ("fsa", new DirectoryInfo(path + @"\\..\\..\\Watcher Tests\FolderReady"));

            //Act
            bool FileOverWeightFalse = FolderWatcher.IsFolderReadyToSendWatcher(overWeightFileInFolder);

            //Assert
            Assert.IsFalse(FileOverWeightFalse);
        }

        [TestMethod]
        public void IsFolderReadyToSendTest_CheckFolderSize()
        {
            //Arrange
            string path = AppDomain.CurrentDomain.BaseDirectory;
            ConfigFile _configFile = ConfigFile.Instance;

            _configFile._configData._binaryFolderMaxSize = 1000;

            KeyValuePair<string, DirectoryInfo> overWeightFolder = new KeyValuePair<string, DirectoryInfo>
                                                            ("fsa", new DirectoryInfo(path + @"\\..\\..\\Watcher Tests\FolderReady"));
            //Act
            long dirSize = FileMaintenance.DirSize(overWeightFolder.Value.GetFiles());// get directory size

            bool FolderOverWeightTrue = FolderWatcher.FolderSizeWatcher(overWeightFolder, dirSize);

            //Assert
            Assert.IsTrue(FolderOverWeightTrue);
        }
    }
}
