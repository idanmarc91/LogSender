﻿using System;
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
            bool isServerOnline = await ServerConnection.IsServerAlive("http://10.0.0.174:8080");
            
            //Assert
            Assert.IsTrue(isServerOnline);
        }

        [TestMethod]
        public async Task SendDataToServerTest()
        {
            //Arrange

            string path = AppDomain.CurrentDomain.BaseDirectory;

            string testString = File.ReadAllText(path + "Server Connection Test\file_to_send.txt");

            MemoryStream stringAsStream = new MemoryStream();
            var writer = new StreamWriter(stringAsStream);
            writer.Write(testString);
            writer.Flush();
            stringAsStream.Position = 0;

            //Act
            bool isServerOnline = await ServerConnection.SendDataToServer("http://10.0.0.174:8080", stringAsStream);

            //Assert
            Assert.IsTrue(isServerOnline);
        }
    }
}