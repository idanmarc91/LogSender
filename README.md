# v5-LogSender-Client
v5 LogSender Client 

The Log Sender Windows Service send log file (cyb,mog,fsa,cimg) located in Cyber 2.0 folder to Cyber 2.0 managment server.
The log sender also responsible for managing the log folders and cleaning them when server is offline.

## Getting Started

These instructions will get you a copy of the project up and running on your local machine for development and testing purposes.
See deployment for notes on how to deploy the project on a live system.

### Prerequisites

* Visual studio
* log4net
* newtonsoft.Json
* Cyber 2.0 managment server for logs receiving

### Installing

This solution include a Service Installer project. the output of this project is an msi that install the service

## Running the tests

The Log Sender solution include a unit test projet.
The unit testing will run only in visual studio.

## Deployment

To install a production ready application you can follow the [Dev Installing](#dev-installing) after installing/validating Prerequisites are met.

## Built With

* Visual studio

## Versioning

See vestion notes [Version notes Google docs](https://docs.google.com/document/u/1/d/15fN9bL6YFy0ZJIrxfmFL691V4POe_7zb4SqEcy7h5pk/edit?usp=drive_web&ouid=107850385867994278819) 

## Authors

* Idan Marciano

## License

This project is licensed under Cyber 2.0 (2015) Inc. All rights reserved.

## Acknowledgments

* Hat tip to anyone whose code was used
