using LogSender.Utilities;
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Management;
using System.Net.NetworkInformation;
using System.ServiceModel;

namespace LogSender
{
    public class RecSender
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger("RecSender.cs");

        //static string pathToRepositoryFiles = System.Reflection.Assembly.GetEntryAssembly().Location + "\\RepositoryFiles";
        static string pathToRepositoryFiles = "C:\\Program Files\\Cyber 2.0\\Cyber 2.0 Agent" + "\\RepositoryFiles";
        static string pathToFilesToZipRepository = pathToRepositoryFiles + "\\Files To Zip";
        static DirectoryInfo directoryRepository = new DirectoryInfo(pathToRepositoryFiles);

        static List<FileToSend> filesToSendListRepository = new List<FileToSend>();
        static bool responseFromServerRepository = false;

        public static void SendRecFiles()
        {
            try
            {

                //Check if directory exist
                if (!Directory.Exists(pathToRepositoryFiles))
                {
                    log.Error(pathToRepositoryFiles + " folder not exist, cannot send files.");
                    return;
                }

                if (directoryRepository.EnumerateFiles("*.xml").Where(f => (f.Length <= 6291456) && (f.Length > 0)).ToArray().Length >= 1)
                {

                    // if the directory does not exists, create it.
                    if (!Directory.Exists(pathToFilesToZipRepository))
                    {
                        Directory.CreateDirectory(pathToFilesToZipRepository);
                    }
                    //make sure that the folder is empty
                    DirectoryInfo di = new DirectoryInfo(pathToFilesToZipRepository);
                    log.Debug("Delete zip files in " + pathToFilesToZipRepository);
                    foreach (FileInfo file in di.GetFiles())
                    {
                        try
                        {
                            file.Delete();
                        }
                        catch (Exception ex)
                        {
                            //string cs = "Cyber 2.0 - Log Sender";
                            log.Error("Cannot delete zip file in file to zip folder", ex);
                            //  if (!EventLog.SourceExists(cs))
                            //  EventLog.CreateEventSource(cs, "Application");

                            //EventLog.WriteEntry(cs, e.Message, EventLogEntryType.Error);
                            break;
                        }

                    }

                    foreach (DirectoryInfo dir in di.GetDirectories())
                    {
                        try
                        {
                            dir.Delete(true);
                        }
                        catch (Exception ex)
                        {
                            //string cs = "Cyber 2.0 - Log Sender";
                            log.Error("Cannot delete zip folder", ex);

                            // if (!EventLog.SourceExists(cs))
                            //    EventLog.CreateEventSource(cs, "Application");

                            // EventLog.WriteEntry(cs, e.Message, EventLogEntryType.Error);
                            break;
                        }
                    }

                    filesToSendListRepository.Clear();

                    responseFromServerRepository = false;
                    //first take care of zip files if last service was stopped
                    string[] filetosend = Directory.EnumerateFiles(pathToRepositoryFiles, "*.zip").Where(f => (f.Length <= 10485760)).ToArray();
                    log.Debug("Checking if there are waiting zip files to be sent to the server");
                    foreach (string fname in filetosend)
                    {
                        string name = Path.GetFileName(fname);
                        name = name.Substring(0, name.Length - 4);
                        FileToSend find = filesToSendListRepository.Find(r => r.nameOfFile.ToLower() == fname.ToLower());
                        if (find == null)
                        {
                            FileToSend filezip = new FileToSend();
                            filezip.nameOfFile = Path.GetFileName(fname);

                            filezip.nameOfFile = filezip.nameOfFile.Substring(0, filezip.nameOfFile.Length - 4);

                            filezip.timeOfCreation = File.GetCreationTime(fname);
                            byte[] zipToBytesold = StreamFile(pathToRepositoryFiles + "\\" + filezip.nameOfFile + ".zip");

                            filezip.SetByteData(zipToBytesold);
                            filesToSendListRepository.Add(filezip);
                        }
                    }
                    //get the files in the directory
                    FileInfo[] allfiles = directoryRepository.EnumerateFiles("*.xml").Where(f => (f.Length > 0) && (f.Length <= 6291456)).ToArray();

                    //save the most recent file
                    //FileInfo mostRecentFile = GetLatestWritenFileFileInDirectory(directoryRepository, ".xml");

                    //check if the filesize is greater than 50 - need to check!
                    FileInfo[] files = allfiles;
                    if (allfiles.Length > 50)
                    {
                        // take all the files and order them by their write time, take the first 50 with the newest write time.
                        files = allfiles.OrderByDescending(f => f.LastWriteTime).Take(50).ToArray();
                    }

                    //if the file is not the latest file Move Files to new folder 
                    string fileName;
                    foreach (FileInfo s in files)
                    {
                        fileName = Path.GetFileName(s.FullName);
                        //if (fileName != mostRecentFile.Name)
                        //{
                        try
                        {
                            //if (CheckIfFileIsBeingUsed(pathToFolder + "\\" + fileName) == false)

                            File.Move(pathToRepositoryFiles + "\\" + fileName, pathToFilesToZipRepository + "\\" + fileName);

                        }
                        catch (IOException)
                        {
                            if (File.Exists(pathToFilesToZipRepository + "\\" + fileName))
                            {
                                File.Delete(pathToRepositoryFiles + "\\" + fileName);
                            }
                        }

                        //}

                    }
                    log.Debug("Deleting empty files");
                    //delete the empty files
                    FileInfo[] emptyFiles = directoryRepository.GetFiles().Where(f => (f.FullName.EndsWith(".xml")) && (f.Length == 0)).ToArray();
                    foreach (FileInfo fi in emptyFiles)
                    {
                        try
                        {
                            fi.Delete();
                        }
                        catch (Exception ex)
                        {
                            log.Error("Cannot delete empty file", ex);
                        }
                    }

                    // create object of a file to send
                    FileToSend fileziptosend = new FileToSend();
                    DateTime timeNowOfFileCreation = DateTime.Now;
                    fileziptosend.timeOfCreation = timeNowOfFileCreation;         // the name of the file and the file creation date are the same,
                    fileziptosend.nameOfFile = timeNowOfFileCreation.ToString().Replace('/', '-').Replace(':', '-');  // but it is safer that we keep DateTime in case that someone tires to change the name of the file



                    //create zip file from sub-folder
                    try
                    {
                        ZipFile.CreateFromDirectory(pathToFilesToZipRepository, pathToRepositoryFiles + "\\" + fileziptosend.nameOfFile + ".zip");
                    }
                    catch (Exception ex)
                    {
                        log.Error("Cannot create zip file", ex);
                    }

                    //it is much easier to pass byte array than sending a file
                    byte[] zipToBytes = StreamFile(pathToRepositoryFiles + "\\" + fileziptosend.nameOfFile + ".zip");

                    fileziptosend.SetByteData(zipToBytes);
                    filesToSendListRepository.Add(fileziptosend);


                    //send the byte array to the server
                    SendFilesToServerRepository();

                    //delete the files that are already zipped
                    try
                    {
                        foreach (FileInfo file in di.GetFiles())
                        {
                            file.Delete();
                        }

                    }
                    catch (IOException ex)
                    {
                        log.Error("Cannot delete the files that are already zipped", ex);
                    }
                    //if (stopFlag == true)
                    //   break;

                }
            }
            catch(Exception ex)
            {
                log.Error("Error occurred while sending rec files", ex);
            }
        }
        private static byte[] StreamFile(string filename)
        {
            FileStream fs = new FileStream(filename, FileMode.Open, FileAccess.Read);

            // Create a byte array of file stream length
            byte[] ImageData = new byte[fs.Length];

            //Read block of bytes from stream into the byte array
            fs.Read(ImageData, 0, System.Convert.ToInt32(fs.Length));

            //Close the File Stream
            fs.Close();
            return ImageData; //return the byte data
        }

        private static async void SendFilesToServerRepository()
        {
            if (await ServerConnection.IsServerAliveAsync())
            {
                //List<FileToSend> filesToDeleteRepository = new List<FileToSend>();
                foreach (FileToSend fts in filesToSendListRepository)
                {
                    //check the stop flag first
                    //if (stopFlag == true)
                    //   break;
                    responseFromServerRepository = SendByteArrayToServer(fts.GetByteData());
                    if (responseFromServerRepository == true)
                    {
                        try
                        {
                            log.Info("Sending process is ended successfully");
                            log.Debug("Delete zip file");
                            //delete the zip file
                            FileInfo fi = new FileInfo(pathToRepositoryFiles + "\\" + fts.nameOfFile + ".zip");
                            fi.Delete();
                            //filesToDeleteRepository.Add(fts);
                        }
                        catch (Exception ex)
                        {
                            log.Error("Cannot delete zip file after sending it to server", ex);

                            return;
                        }
                    }
                    else
                    {
                        log.Error("Error occurred while sending message to the server. server response is false");
                    }
                    if (filesToSendListRepository.Count > 1)
                    {
                        //  for (int i = 0; i < 10; i++)
                        // {
                        //  if (stopFlag == true) // check every second if the stop flag is triggered
                        //   break;
                        //Thread.Sleep(1000);
                        // }
                    }

                }


                //delete the files that were sent from the list
                foreach (FileToSend fi in filesToSendListRepository.ToList())
                {
                    try
                    {
                        filesToSendListRepository.Remove(fi);
                    }
                    catch (Exception ex)
                    {
                        log.Error("Cannot delete the .xml files after sending to server", ex);
                    }

                }
            }
            else
            {
                log.Warn("The server (log insert) is offline cannot send repository files");
            }

        }

        private static bool SendByteArrayToServer(byte[] byteArray)
        {
            bool responseFromServer = false;
            try
            {
                log.Info("Sending .xml zipped files to the server");
                LogSender1.ServiceReference.Cyber20WebServiceSoapClient client = new LogSender1.ServiceReference.Cyber20WebServiceSoapClient("Cyber20WebServiceSoap");
                client.Endpoint.Address = new EndpointAddress(@"http://" + ConfigFile.Instance._configData._hostIp + "/serverws/cyber20webservice.asmx");
                responseFromServer = client.GetLogFileFromClient(byteArray, GetMacs(), Environment.MachineName, GetWhitelistGUID(), GetComputerGUID() + GetCPUID());
            }
            catch (Exception ex)
            {
                log.Error("Cannot send zip file to server, sending failed", ex);
            }

            return responseFromServer;
        }

        private static string GetMacs()
        {
            List<string> macs =
            (
                from nic in NetworkInterface.GetAllNetworkInterfaces()
                where (nic.NetworkInterfaceType != NetworkInterfaceType.Loopback && nic.NetworkInterfaceType != NetworkInterfaceType.Tunnel) //nic.OperationalStatus == OperationalStatus.Up && 
                select nic.GetPhysicalAddress().ToString()
            ).ToList();

            //delete empty macs
            for (int i = 0; i < macs.Count; i++)
            {
                if (macs[i] == "")
                {
                    macs.Remove(macs[i]);
                }
            }

            macs = macs.OrderBy(q => q).ToList();

            if (macs.Count > 0)
            {
                return macs[0];
            }

            return "";

        }
        private static string GetWhitelistGUID()
        {
            try
            {
                string subkey = @"SYSTEM\CurrentControlSet\Services\Cyber20NetDriver";
                Microsoft.Win32.RegistryKey key = Microsoft.Win32.Registry.LocalMachine.OpenSubKey(subkey, true);
                string guid = (string)key.GetValue("GUID");
                key.Close();
                return guid;
            }
            catch
            {
                return "";
            }
        }

        private static string GetComputerGUID()
        {
            try
            {
                string subkey = @"SOFTWARE\Microsoft\Cryptography";
                Microsoft.Win32.RegistryKey key = Microsoft.Win32.Registry.LocalMachine.OpenSubKey(subkey, true);
                string guid = (string)key.GetValue("MachineGuid");
                key.Close();
                return guid;
            }
            catch
            {
                return "";
            }

        }
        private static string GetCPUID()
        {
            try
            {
                ManagementObjectSearcher searcher =
                    new ManagementObjectSearcher("root\\CIMV2",
                    "SELECT * FROM Win32_Processor");

                foreach (ManagementObject queryObj in searcher.Get())
                {
                    return queryObj["ProcessorId"].ToString();
                }
                return ""; //in case of error
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return "";
            }

        }
    }
}
