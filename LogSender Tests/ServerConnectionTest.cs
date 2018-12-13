using System;
using System.IO;
using System.Threading.Tasks;
using LogSender.Utilities;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace LogSender_Tests
{
    [TestClass]
    public class ServerConnectionTest
    {
        [TestMethod]
        public async Task CheckIfServerIsAliveTest()
        {
            //Act
            //bool isServerOnline = await ServerConnection.IsServerAliveAsync("http://10.0.0.40:8080");

            //Assert
            //Assert.IsTrue(isServerOnline);
        }

        [TestMethod]
        public async Task SendDataToServerTest()
        {
            //Arrange
            string path = AppDomain.CurrentDomain.BaseDirectory;

            string testString = File.ReadAllText(path + "\\..\\..\\Server Connection Test\\file_to_send.txt");

            MemoryStream stringAsStream = new MemoryStream();
            var writer = new StreamWriter(stringAsStream);
            writer.Write(testString);
            writer.Flush();
            stringAsStream.Position = 0;

            MemoryStream compressedData = GZipCompresstion.CompressString(testString);
            //Act
            ServerConnection con = new ServerConnection();

            bool isServerOnline = await con.SendDataToServerAsync("http://10.0.0.40:8080", compressedData);

            //Assert
            Assert.IsTrue(isServerOnline);

        }
    }
}
