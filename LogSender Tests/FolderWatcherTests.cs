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
        [TestMethod]
        public void IsFolderReadyToSendTest_CheckIfFolderIsReady()
        {
            //Arrange
            string path = AppDomain.CurrentDomain.BaseDirectory;

            int minNumOfFilesToSend = 2; //2 files
            long binaryFileMaxSize = 6291456; // 6 mega
            long maxBinaryFolderSize = 104857600; //100 mega

            KeyValuePair<string, DirectoryInfo> FolderReady = new KeyValuePair<string, DirectoryInfo>
                                                            ("fsa", new DirectoryInfo(path + @"\\..\\..\\Watcher Tests\FolderReady"));
            KeyValuePair<string, DirectoryInfo> FolderNotReady = new KeyValuePair<string, DirectoryInfo>
                                                           ("fsa", new DirectoryInfo(path + @"\\..\\..\\Watcher Tests\FolderNotReady"));
            //Act
            bool folderReadyValueTrue = FolderWatcher.IsFolderReadyToSendWatcher(FolderReady, binaryFileMaxSize, minNumOfFilesToSend, maxBinaryFolderSize);
            bool folderReadyValueFalse = FolderWatcher.IsFolderReadyToSendWatcher(FolderNotReady, binaryFileMaxSize, minNumOfFilesToSend, maxBinaryFolderSize);

            //Assert
            Assert.AreNotEqual(folderReadyValueTrue, folderReadyValueFalse);
        }

        [TestMethod]
        public void IsFolderReadyToSendTest_OverWeightFile()
        {
            //Arrange
            string path = AppDomain.CurrentDomain.BaseDirectory;

            int minNumOfFilesToSend = 2; //2 files
            long binaryFileMaxSize = 100; // 100 bytes
            long maxBinaryFolderSize = 104857600; ////100 mega

            KeyValuePair<string, DirectoryInfo> overWeightFileInFolder = new KeyValuePair<string, DirectoryInfo>
                                                            ("fsa", new DirectoryInfo(path + @"\\..\\..\\Watcher Tests\FolderReady"));

            //Act
            bool FileOverWeightFalse = FolderWatcher.IsFolderReadyToSendWatcher(overWeightFileInFolder, binaryFileMaxSize, minNumOfFilesToSend, maxBinaryFolderSize);

            //Assert
            Assert.IsFalse(FileOverWeightFalse);
        }

        [TestMethod]
        public void IsFolderReadyToSendTest_CheckFolderSize()
        {
            //Arrange
            string path = AppDomain.CurrentDomain.BaseDirectory;
            long maxBinaryFolderSize = 1000; //100 bytes
            KeyValuePair<string, DirectoryInfo> overWeightFolder = new KeyValuePair<string, DirectoryInfo>
                                                            ("fsa", new DirectoryInfo(path + @"\\..\\..\\Watcher Tests\FolderReady"));
            //Act
            bool FolderOverWeightTrue = FolderWatcher.FolderSizeWatcher(overWeightFolder, maxBinaryFolderSize);

            //Assert
            Assert.IsTrue(FolderOverWeightTrue);
        }
    }
}
