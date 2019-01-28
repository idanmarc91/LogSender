﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace LogSender1.ServiceReference {
    
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ServiceModel.ServiceContractAttribute(ConfigurationName="ServiceReference.Cyber20WebServiceSoap")]
    public interface Cyber20WebServiceSoap {
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/GetNewCertFile", ReplyAction="*")]
        [System.ServiceModel.XmlSerializerFormatAttribute(SupportFaults=true)]
        string GetNewCertFile(string encCurrentPubKey, string GUID, string compName);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/GetNewCertFile", ReplyAction="*")]
        System.Threading.Tasks.Task<string> GetNewCertFileAsync(string encCurrentPubKey, string GUID, string compName);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/ManualyCreateCertFile", ReplyAction="*")]
        [System.ServiceModel.XmlSerializerFormatAttribute(SupportFaults=true)]
        int ManualyCreateCertFile();
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/ManualyCreateCertFile", ReplyAction="*")]
        System.Threading.Tasks.Task<int> ManualyCreateCertFileAsync();
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/CanGetCertificate", ReplyAction="*")]
        [System.ServiceModel.XmlSerializerFormatAttribute(SupportFaults=true)]
        string CanGetCertificate(
                    string currentClientPubKey, 
                    string MAC, 
                    string whiteListVersionNumber, 
                    string SubWhiteListsVersions, 
                    string compName, 
                    bool gotCert, 
                    string GUID, 
                    string UIVersion, 
                    string driverVersion, 
                    string SUPVersion, 
                    string serviceVersion, 
                    string reconVersion, 
                    string LogSender1Version, 
                    string OSVersion, 
                    int IsScrambled, 
                    string LogedInUser);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/CanGetCertificate", ReplyAction="*")]
        System.Threading.Tasks.Task<string> CanGetCertificateAsync(
                    string currentClientPubKey, 
                    string MAC, 
                    string whiteListVersionNumber, 
                    string SubWhiteListsVersions, 
                    string compName, 
                    bool gotCert, 
                    string GUID, 
                    string UIVersion, 
                    string driverVersion, 
                    string SUPVersion, 
                    string serviceVersion, 
                    string reconVersion, 
                    string LogSender1Version, 
                    string OSVersion, 
                    int IsScrambled, 
                    string LogedInUser);
        
        // CODEGEN: Parameter 'logFile' requires additional schema information that cannot be captured using the parameter mode. The specific attribute is 'System.Xml.Serialization.XmlElementAttribute'.
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/GetLogFileFromClient", ReplyAction="*")]
        [System.ServiceModel.XmlSerializerFormatAttribute(SupportFaults=true)]
        LogSender1.ServiceReference.GetLogFileFromClientResponse GetLogFileFromClient(LogSender1.ServiceReference.GetLogFileFromClientRequest request);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/GetLogFileFromClient", ReplyAction="*")]
        System.Threading.Tasks.Task<LogSender1.ServiceReference.GetLogFileFromClientResponse> GetLogFileFromClientAsync(LogSender1.ServiceReference.GetLogFileFromClientRequest request);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/TestConnectionToWebService", ReplyAction="*")]
        [System.ServiceModel.XmlSerializerFormatAttribute(SupportFaults=true)]
        bool TestConnectionToWebService();
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/TestConnectionToWebService", ReplyAction="*")]
        System.Threading.Tasks.Task<bool> TestConnectionToWebServiceAsync();
        
        // CODEGEN: Parameter 'whiteListZipFile' requires additional schema information that cannot be captured using the parameter mode. The specific attribute is 'System.Xml.Serialization.XmlElementAttribute'.
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/GetWhiteListZipFileToInsertDB", ReplyAction="*")]
        [System.ServiceModel.XmlSerializerFormatAttribute(SupportFaults=true)]
        LogSender1.ServiceReference.GetWhiteListZipFileToInsertDBResponse GetWhiteListZipFileToInsertDB(LogSender1.ServiceReference.GetWhiteListZipFileToInsertDBRequest request);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/GetWhiteListZipFileToInsertDB", ReplyAction="*")]
        System.Threading.Tasks.Task<LogSender1.ServiceReference.GetWhiteListZipFileToInsertDBResponse> GetWhiteListZipFileToInsertDBAsync(LogSender1.ServiceReference.GetWhiteListZipFileToInsertDBRequest request);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/GetNewWhiteList", ReplyAction="*")]
        [System.ServiceModel.XmlSerializerFormatAttribute(SupportFaults=true)]
        string GetNewWhiteList(string GUID, string compName);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/GetNewWhiteList", ReplyAction="*")]
        System.Threading.Tasks.Task<string> GetNewWhiteListAsync(string GUID, string compName);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/GetNewSubWhiteList", ReplyAction="*")]
        [System.ServiceModel.XmlSerializerFormatAttribute(SupportFaults=true)]
        string GetNewSubWhiteList(string GUID, string compName);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/GetNewSubWhiteList", ReplyAction="*")]
        System.Threading.Tasks.Task<string> GetNewSubWhiteListAsync(string GUID, string compName);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/GetNewConfigFile", ReplyAction="*")]
        [System.ServiceModel.XmlSerializerFormatAttribute(SupportFaults=true)]
        string GetNewConfigFile(string GUID, string compName);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/GetNewConfigFile", ReplyAction="*")]
        System.Threading.Tasks.Task<string> GetNewConfigFileAsync(string GUID, string compName);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/ReturnWhiteListByVersionNumber", ReplyAction="*")]
        [System.ServiceModel.XmlSerializerFormatAttribute(SupportFaults=true)]
        string ReturnWhiteListByVersionNumber(int versionNumber);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/ReturnWhiteListByVersionNumber", ReplyAction="*")]
        System.Threading.Tasks.Task<string> ReturnWhiteListByVersionNumberAsync(int versionNumber);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/ReturnWhiteListXMLByVersionNumber", ReplyAction="*")]
        [System.ServiceModel.XmlSerializerFormatAttribute(SupportFaults=true)]
        string ReturnWhiteListXMLByVersionNumber(int versionNumber);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/ReturnWhiteListXMLByVersionNumber", ReplyAction="*")]
        System.Threading.Tasks.Task<string> ReturnWhiteListXMLByVersionNumberAsync(int versionNumber);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/GetSubWhitelistTable", ReplyAction="*")]
        [System.ServiceModel.XmlSerializerFormatAttribute(SupportFaults=true)]
        string[] GetSubWhitelistTable();
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/GetSubWhitelistTable", ReplyAction="*")]
        System.Threading.Tasks.Task<string[]> GetSubWhitelistTableAsync();
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/GetSubWhitelist", ReplyAction="*")]
        [System.ServiceModel.XmlSerializerFormatAttribute(SupportFaults=true)]
        string GetSubWhitelist(string groupName, int fileVersion, int mode);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/GetSubWhitelist", ReplyAction="*")]
        System.Threading.Tasks.Task<string> GetSubWhitelistAsync(string groupName, int fileVersion, int mode);
        
        // CODEGEN: Parameter 'xmlfile' requires additional schema information that cannot be captured using the parameter mode. The specific attribute is 'System.Xml.Serialization.XmlElementAttribute'.
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/UploadSubWhiteList", ReplyAction="*")]
        [System.ServiceModel.XmlSerializerFormatAttribute(SupportFaults=true)]
        LogSender1.ServiceReference.UploadSubWhiteListResponse UploadSubWhiteList(LogSender1.ServiceReference.UploadSubWhiteListRequest request);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/UploadSubWhiteList", ReplyAction="*")]
        System.Threading.Tasks.Task<LogSender1.ServiceReference.UploadSubWhiteListResponse> UploadSubWhiteListAsync(LogSender1.ServiceReference.UploadSubWhiteListRequest request);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/ActivateSubWhiteList", ReplyAction="*")]
        [System.ServiceModel.XmlSerializerFormatAttribute(SupportFaults=true)]
        bool ActivateSubWhiteList(string groupName, int Version);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/ActivateSubWhiteList", ReplyAction="*")]
        System.Threading.Tasks.Task<bool> ActivateSubWhiteListAsync(string groupName, int Version);
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.ServiceModel.MessageContractAttribute(WrapperName="GetLogFileFromClient", WrapperNamespace="http://tempuri.org/", IsWrapped=true)]
    public partial class GetLogFileFromClientRequest {
        
        [System.ServiceModel.MessageBodyMemberAttribute(Namespace="http://tempuri.org/", Order=0)]
        [System.Xml.Serialization.XmlElementAttribute(DataType="base64Binary")]
        public byte[] logFile;
        
        [System.ServiceModel.MessageBodyMemberAttribute(Namespace="http://tempuri.org/", Order=1)]
        public string creator;
        
        [System.ServiceModel.MessageBodyMemberAttribute(Namespace="http://tempuri.org/", Order=2)]
        public string hostname;
        
        [System.ServiceModel.MessageBodyMemberAttribute(Namespace="http://tempuri.org/", Order=3)]
        public string UID;
        
        [System.ServiceModel.MessageBodyMemberAttribute(Namespace="http://tempuri.org/", Order=4)]
        public string clientGUID;
        
        public GetLogFileFromClientRequest() {
        }
        
        public GetLogFileFromClientRequest(byte[] logFile, string creator, string hostname, string UID, string clientGUID) {
            this.logFile = logFile;
            this.creator = creator;
            this.hostname = hostname;
            this.UID = UID;
            this.clientGUID = clientGUID;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.ServiceModel.MessageContractAttribute(WrapperName="GetLogFileFromClientResponse", WrapperNamespace="http://tempuri.org/", IsWrapped=true)]
    public partial class GetLogFileFromClientResponse {
        
        [System.ServiceModel.MessageBodyMemberAttribute(Namespace="http://tempuri.org/", Order=0)]
        public bool GetLogFileFromClientResult;
        
        public GetLogFileFromClientResponse() {
        }
        
        public GetLogFileFromClientResponse(bool GetLogFileFromClientResult) {
            this.GetLogFileFromClientResult = GetLogFileFromClientResult;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.ServiceModel.MessageContractAttribute(WrapperName="GetWhiteListZipFileToInsertDB", WrapperNamespace="http://tempuri.org/", IsWrapped=true)]
    public partial class GetWhiteListZipFileToInsertDBRequest {
        
        [System.ServiceModel.MessageBodyMemberAttribute(Namespace="http://tempuri.org/", Order=0)]
        [System.Xml.Serialization.XmlElementAttribute(DataType="base64Binary")]
        public byte[] whiteListZipFile;
        
        [System.ServiceModel.MessageBodyMemberAttribute(Namespace="http://tempuri.org/", Order=1)]
        public string creator;
        
        [System.ServiceModel.MessageBodyMemberAttribute(Namespace="http://tempuri.org/", Order=2)]
        public System.DateTime fileCreationTime;
        
        [System.ServiceModel.MessageBodyMemberAttribute(Namespace="http://tempuri.org/", Order=3)]
        public string whiteListName;
        
        [System.ServiceModel.MessageBodyMemberAttribute(Namespace="http://tempuri.org/", Order=4)]
        public string whiteListDescription;
        
        public GetWhiteListZipFileToInsertDBRequest() {
        }
        
        public GetWhiteListZipFileToInsertDBRequest(byte[] whiteListZipFile, string creator, System.DateTime fileCreationTime, string whiteListName, string whiteListDescription) {
            this.whiteListZipFile = whiteListZipFile;
            this.creator = creator;
            this.fileCreationTime = fileCreationTime;
            this.whiteListName = whiteListName;
            this.whiteListDescription = whiteListDescription;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.ServiceModel.MessageContractAttribute(WrapperName="GetWhiteListZipFileToInsertDBResponse", WrapperNamespace="http://tempuri.org/", IsWrapped=true)]
    public partial class GetWhiteListZipFileToInsertDBResponse {
        
        [System.ServiceModel.MessageBodyMemberAttribute(Namespace="http://tempuri.org/", Order=0)]
        public bool GetWhiteListZipFileToInsertDBResult;
        
        public GetWhiteListZipFileToInsertDBResponse() {
        }
        
        public GetWhiteListZipFileToInsertDBResponse(bool GetWhiteListZipFileToInsertDBResult) {
            this.GetWhiteListZipFileToInsertDBResult = GetWhiteListZipFileToInsertDBResult;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.ServiceModel.MessageContractAttribute(WrapperName="UploadSubWhiteList", WrapperNamespace="http://tempuri.org/", IsWrapped=true)]
    public partial class UploadSubWhiteListRequest {
        
        [System.ServiceModel.MessageBodyMemberAttribute(Namespace="http://tempuri.org/", Order=0)]
        public string groupName;
        
        [System.ServiceModel.MessageBodyMemberAttribute(Namespace="http://tempuri.org/", Order=1)]
        [System.Xml.Serialization.XmlElementAttribute(DataType="base64Binary")]
        public byte[] xmlfile;
        
        [System.ServiceModel.MessageBodyMemberAttribute(Namespace="http://tempuri.org/", Order=2)]
        [System.Xml.Serialization.XmlElementAttribute(DataType="base64Binary")]
        public byte[] cwmffile;
        
        [System.ServiceModel.MessageBodyMemberAttribute(Namespace="http://tempuri.org/", Order=3)]
        public string description;
        
        public UploadSubWhiteListRequest() {
        }
        
        public UploadSubWhiteListRequest(string groupName, byte[] xmlfile, byte[] cwmffile, string description) {
            this.groupName = groupName;
            this.xmlfile = xmlfile;
            this.cwmffile = cwmffile;
            this.description = description;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.ServiceModel.MessageContractAttribute(WrapperName="UploadSubWhiteListResponse", WrapperNamespace="http://tempuri.org/", IsWrapped=true)]
    public partial class UploadSubWhiteListResponse {
        
        [System.ServiceModel.MessageBodyMemberAttribute(Namespace="http://tempuri.org/", Order=0)]
        public bool UploadSubWhiteListResult;
        
        public UploadSubWhiteListResponse() {
        }
        
        public UploadSubWhiteListResponse(bool UploadSubWhiteListResult) {
            this.UploadSubWhiteListResult = UploadSubWhiteListResult;
        }
    }
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public interface Cyber20WebServiceSoapChannel : LogSender1.ServiceReference.Cyber20WebServiceSoap, System.ServiceModel.IClientChannel {
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public partial class Cyber20WebServiceSoapClient : System.ServiceModel.ClientBase<LogSender1.ServiceReference.Cyber20WebServiceSoap>, LogSender1.ServiceReference.Cyber20WebServiceSoap {
        
        public Cyber20WebServiceSoapClient() {
        }
        
        public Cyber20WebServiceSoapClient(string endpointConfigurationName) : 
                base(endpointConfigurationName) {
        }
        
        public Cyber20WebServiceSoapClient(string endpointConfigurationName, string remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public Cyber20WebServiceSoapClient(string endpointConfigurationName, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public Cyber20WebServiceSoapClient(System.ServiceModel.Channels.Binding binding, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(binding, remoteAddress) {
        }
        
        public string GetNewCertFile(string encCurrentPubKey, string GUID, string compName) {
            return base.Channel.GetNewCertFile(encCurrentPubKey, GUID, compName);
        }
        
        public System.Threading.Tasks.Task<string> GetNewCertFileAsync(string encCurrentPubKey, string GUID, string compName) {
            return base.Channel.GetNewCertFileAsync(encCurrentPubKey, GUID, compName);
        }
        
        public int ManualyCreateCertFile() {
            return base.Channel.ManualyCreateCertFile();
        }
        
        public System.Threading.Tasks.Task<int> ManualyCreateCertFileAsync() {
            return base.Channel.ManualyCreateCertFileAsync();
        }
        
        public string CanGetCertificate(
                    string currentClientPubKey, 
                    string MAC, 
                    string whiteListVersionNumber, 
                    string SubWhiteListsVersions, 
                    string compName, 
                    bool gotCert, 
                    string GUID, 
                    string UIVersion, 
                    string driverVersion, 
                    string SUPVersion, 
                    string serviceVersion, 
                    string reconVersion, 
                    string LogSender1Version, 
                    string OSVersion, 
                    int IsScrambled, 
                    string LogedInUser) {
            return base.Channel.CanGetCertificate(currentClientPubKey, MAC, whiteListVersionNumber, SubWhiteListsVersions, compName, gotCert, GUID, UIVersion, driverVersion, SUPVersion, serviceVersion, reconVersion, LogSender1Version, OSVersion, IsScrambled, LogedInUser);
        }
        
        public System.Threading.Tasks.Task<string> CanGetCertificateAsync(
                    string currentClientPubKey, 
                    string MAC, 
                    string whiteListVersionNumber, 
                    string SubWhiteListsVersions, 
                    string compName, 
                    bool gotCert, 
                    string GUID, 
                    string UIVersion, 
                    string driverVersion, 
                    string SUPVersion, 
                    string serviceVersion, 
                    string reconVersion, 
                    string LogSender1Version, 
                    string OSVersion, 
                    int IsScrambled, 
                    string LogedInUser) {
            return base.Channel.CanGetCertificateAsync(currentClientPubKey, MAC, whiteListVersionNumber, SubWhiteListsVersions, compName, gotCert, GUID, UIVersion, driverVersion, SUPVersion, serviceVersion, reconVersion, LogSender1Version, OSVersion, IsScrambled, LogedInUser);
        }
        
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
        LogSender1.ServiceReference.GetLogFileFromClientResponse LogSender1.ServiceReference.Cyber20WebServiceSoap.GetLogFileFromClient(LogSender1.ServiceReference.GetLogFileFromClientRequest request) {
            return base.Channel.GetLogFileFromClient(request);
        }
        
        public bool GetLogFileFromClient(byte[] logFile, string creator, string hostname, string UID, string clientGUID) {
            LogSender1.ServiceReference.GetLogFileFromClientRequest inValue = new LogSender1.ServiceReference.GetLogFileFromClientRequest();
            inValue.logFile = logFile;
            inValue.creator = creator;
            inValue.hostname = hostname;
            inValue.UID = UID;
            inValue.clientGUID = clientGUID;
            LogSender1.ServiceReference.GetLogFileFromClientResponse retVal = ((LogSender1.ServiceReference.Cyber20WebServiceSoap)(this)).GetLogFileFromClient(inValue);
            return retVal.GetLogFileFromClientResult;
        }
        
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
        System.Threading.Tasks.Task<LogSender1.ServiceReference.GetLogFileFromClientResponse> LogSender1.ServiceReference.Cyber20WebServiceSoap.GetLogFileFromClientAsync(LogSender1.ServiceReference.GetLogFileFromClientRequest request) {
            return base.Channel.GetLogFileFromClientAsync(request);
        }
        
        public System.Threading.Tasks.Task<LogSender1.ServiceReference.GetLogFileFromClientResponse> GetLogFileFromClientAsync(byte[] logFile, string creator, string hostname, string UID, string clientGUID) {
            LogSender1.ServiceReference.GetLogFileFromClientRequest inValue = new LogSender1.ServiceReference.GetLogFileFromClientRequest();
            inValue.logFile = logFile;
            inValue.creator = creator;
            inValue.hostname = hostname;
            inValue.UID = UID;
            inValue.clientGUID = clientGUID;
            return ((LogSender1.ServiceReference.Cyber20WebServiceSoap)(this)).GetLogFileFromClientAsync(inValue);
        }
        
        public bool TestConnectionToWebService() {
            return base.Channel.TestConnectionToWebService();
        }
        
        public System.Threading.Tasks.Task<bool> TestConnectionToWebServiceAsync() {
            return base.Channel.TestConnectionToWebServiceAsync();
        }
        
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
        LogSender1.ServiceReference.GetWhiteListZipFileToInsertDBResponse LogSender1.ServiceReference.Cyber20WebServiceSoap.GetWhiteListZipFileToInsertDB(LogSender1.ServiceReference.GetWhiteListZipFileToInsertDBRequest request) {
            return base.Channel.GetWhiteListZipFileToInsertDB(request);
        }
        
        public bool GetWhiteListZipFileToInsertDB(byte[] whiteListZipFile, string creator, System.DateTime fileCreationTime, string whiteListName, string whiteListDescription) {
            LogSender1.ServiceReference.GetWhiteListZipFileToInsertDBRequest inValue = new LogSender1.ServiceReference.GetWhiteListZipFileToInsertDBRequest();
            inValue.whiteListZipFile = whiteListZipFile;
            inValue.creator = creator;
            inValue.fileCreationTime = fileCreationTime;
            inValue.whiteListName = whiteListName;
            inValue.whiteListDescription = whiteListDescription;
            LogSender1.ServiceReference.GetWhiteListZipFileToInsertDBResponse retVal = ((LogSender1.ServiceReference.Cyber20WebServiceSoap)(this)).GetWhiteListZipFileToInsertDB(inValue);
            return retVal.GetWhiteListZipFileToInsertDBResult;
        }
        
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
        System.Threading.Tasks.Task<LogSender1.ServiceReference.GetWhiteListZipFileToInsertDBResponse> LogSender1.ServiceReference.Cyber20WebServiceSoap.GetWhiteListZipFileToInsertDBAsync(LogSender1.ServiceReference.GetWhiteListZipFileToInsertDBRequest request) {
            return base.Channel.GetWhiteListZipFileToInsertDBAsync(request);
        }
        
        public System.Threading.Tasks.Task<LogSender1.ServiceReference.GetWhiteListZipFileToInsertDBResponse> GetWhiteListZipFileToInsertDBAsync(byte[] whiteListZipFile, string creator, System.DateTime fileCreationTime, string whiteListName, string whiteListDescription) {
            LogSender1.ServiceReference.GetWhiteListZipFileToInsertDBRequest inValue = new LogSender1.ServiceReference.GetWhiteListZipFileToInsertDBRequest();
            inValue.whiteListZipFile = whiteListZipFile;
            inValue.creator = creator;
            inValue.fileCreationTime = fileCreationTime;
            inValue.whiteListName = whiteListName;
            inValue.whiteListDescription = whiteListDescription;
            return ((LogSender1.ServiceReference.Cyber20WebServiceSoap)(this)).GetWhiteListZipFileToInsertDBAsync(inValue);
        }
        
        public string GetNewWhiteList(string GUID, string compName) {
            return base.Channel.GetNewWhiteList(GUID, compName);
        }
        
        public System.Threading.Tasks.Task<string> GetNewWhiteListAsync(string GUID, string compName) {
            return base.Channel.GetNewWhiteListAsync(GUID, compName);
        }
        
        public string GetNewSubWhiteList(string GUID, string compName) {
            return base.Channel.GetNewSubWhiteList(GUID, compName);
        }
        
        public System.Threading.Tasks.Task<string> GetNewSubWhiteListAsync(string GUID, string compName) {
            return base.Channel.GetNewSubWhiteListAsync(GUID, compName);
        }
        
        public string GetNewConfigFile(string GUID, string compName) {
            return base.Channel.GetNewConfigFile(GUID, compName);
        }
        
        public System.Threading.Tasks.Task<string> GetNewConfigFileAsync(string GUID, string compName) {
            return base.Channel.GetNewConfigFileAsync(GUID, compName);
        }
        
        public string ReturnWhiteListByVersionNumber(int versionNumber) {
            return base.Channel.ReturnWhiteListByVersionNumber(versionNumber);
        }
        
        public System.Threading.Tasks.Task<string> ReturnWhiteListByVersionNumberAsync(int versionNumber) {
            return base.Channel.ReturnWhiteListByVersionNumberAsync(versionNumber);
        }
        
        public string ReturnWhiteListXMLByVersionNumber(int versionNumber) {
            return base.Channel.ReturnWhiteListXMLByVersionNumber(versionNumber);
        }
        
        public System.Threading.Tasks.Task<string> ReturnWhiteListXMLByVersionNumberAsync(int versionNumber) {
            return base.Channel.ReturnWhiteListXMLByVersionNumberAsync(versionNumber);
        }
        
        public string[] GetSubWhitelistTable() {
            return base.Channel.GetSubWhitelistTable();
        }
        
        public System.Threading.Tasks.Task<string[]> GetSubWhitelistTableAsync() {
            return base.Channel.GetSubWhitelistTableAsync();
        }
        
        public string GetSubWhitelist(string groupName, int fileVersion, int mode) {
            return base.Channel.GetSubWhitelist(groupName, fileVersion, mode);
        }
        
        public System.Threading.Tasks.Task<string> GetSubWhitelistAsync(string groupName, int fileVersion, int mode) {
            return base.Channel.GetSubWhitelistAsync(groupName, fileVersion, mode);
        }
        
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
        LogSender1.ServiceReference.UploadSubWhiteListResponse LogSender1.ServiceReference.Cyber20WebServiceSoap.UploadSubWhiteList(LogSender1.ServiceReference.UploadSubWhiteListRequest request) {
            return base.Channel.UploadSubWhiteList(request);
        }
        
        public bool UploadSubWhiteList(string groupName, byte[] xmlfile, byte[] cwmffile, string description) {
            LogSender1.ServiceReference.UploadSubWhiteListRequest inValue = new LogSender1.ServiceReference.UploadSubWhiteListRequest();
            inValue.groupName = groupName;
            inValue.xmlfile = xmlfile;
            inValue.cwmffile = cwmffile;
            inValue.description = description;
            LogSender1.ServiceReference.UploadSubWhiteListResponse retVal = ((LogSender1.ServiceReference.Cyber20WebServiceSoap)(this)).UploadSubWhiteList(inValue);
            return retVal.UploadSubWhiteListResult;
        }
        
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
        System.Threading.Tasks.Task<LogSender1.ServiceReference.UploadSubWhiteListResponse> LogSender1.ServiceReference.Cyber20WebServiceSoap.UploadSubWhiteListAsync(LogSender1.ServiceReference.UploadSubWhiteListRequest request) {
            return base.Channel.UploadSubWhiteListAsync(request);
        }
        
        public System.Threading.Tasks.Task<LogSender1.ServiceReference.UploadSubWhiteListResponse> UploadSubWhiteListAsync(string groupName, byte[] xmlfile, byte[] cwmffile, string description) {
            LogSender1.ServiceReference.UploadSubWhiteListRequest inValue = new LogSender1.ServiceReference.UploadSubWhiteListRequest();
            inValue.groupName = groupName;
            inValue.xmlfile = xmlfile;
            inValue.cwmffile = cwmffile;
            inValue.description = description;
            return ((LogSender1.ServiceReference.Cyber20WebServiceSoap)(this)).UploadSubWhiteListAsync(inValue);
        }
        
        public bool ActivateSubWhiteList(string groupName, int Version) {
            return base.Channel.ActivateSubWhiteList(groupName, Version);
        }
        
        public System.Threading.Tasks.Task<bool> ActivateSubWhiteListAsync(string groupName, int Version) {
            return base.Channel.ActivateSubWhiteListAsync(groupName, Version);
        }
    }
}
