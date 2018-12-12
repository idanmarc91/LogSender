using System;
using System.IO;
using LogSender.Utilities;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace LogSender_Tests
{
    [TestClass]
    public class FileMaintenanceTests
    {
        [TestMethod]
        public void DirectorySizeTest()
        {
            //Arrange
            string path = AppDomain.CurrentDomain.BaseDirectory;
            DirectoryInfo _directory = new DirectoryInfo(path + @"\\..\\..\\Maintenance Test" + "\\FolderSizeTest\\");

            //Act
            long folderSize = FileMaintenance.DirSize(_directory.GetFiles());

            long trueFolderSize = 2744474;
            ////Assert
            Assert.AreEqual(folderSize, trueFolderSize);
        }
    }
}
