using System;
using System.IO;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using LogSender.Utilities;

namespace LogSender_Tests
{
    [TestClass]
    public class GZipCompressionTest
    {
        [TestMethod]
        public void CompressionTest()
        {
            //Arrange
            string path = AppDomain.CurrentDomain.BaseDirectory;
            string testStr = File.ReadAllText( path + @"\Compress Test\" + "CompressTest.txt" );

            //Act
            MemoryStream ms = GZipCompresstion.CompressString( testStr );
            string decompessedString = GZipCompresstion.DecompressString( ms.ToArray() );

            ////Assert
            Assert.AreEqual( testStr , decompessedString );
        }
    }
}
