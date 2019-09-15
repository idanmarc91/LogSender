# LogSender Windows Service
LogSender Windows Service 

The Log Sender Windows Service send log files to a management server.
There are 4 kind of logs cyb, mog, fsa, cimg (binary file deserialized in the log sender sending process).
The log sending process includes desearialized, data mapping and gziping the data before sending it to node service located on the server. 
The logs are created by another services that collect data from the Network and File System on the machine meaning the folder contain new logs every few seconds.
The log sender service is also responsible for managing the log folders and cleaning them when server is offline.
To controll the log sender configuration there is a config file called "LogSenderConfiguration.cfg". this file is a text file that the service is reading during start up process


### Prerequisites

* Visual Studio
* log4net
* newtonsoft.Json


## Running the tests

The Log Sender solution include a unit test projet.
The unit testing will run only in visual studio.


## Built With

* Visual studio

## Versioning
**if a new version is made, update "AssemblyVersion" and "AssemblyFileVersion" at "AssemblyInfo" file**

## Authors

* Idan Marciano

## License

This project is licensed under Cyber 2.0 (2015) Inc. All rights reserved.

## Acknowledgments

* Hat tip to anyone whose code was used
