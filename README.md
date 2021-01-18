![.NET-shield](https://img.shields.io/badge/.NET-5.0-blueviolet?style=for-the-badge&logo=.net)
![Postgres-shield](https://img.shields.io/badge/Postgres-12+-blue?style=for-the-badge&logo=postgresql)
# lotus-server
A RESTful backend API for the LOTUS 2021 project

### [Requirements](#Requirements) | [Installation](#Installation) | [Docker](#Docker) 

## Requirements
* .NET 5.0
* Postgres 12 (or later)

## Installation
In order to start future development of this .NET Core project, you'll have to follow
the following steps:

1. Clone this project using `git clone https://github.com/Crypit-Coders-Inc/lotus-server.git`
2. Open the folder where the `lotus-server.sln` is located
3. Restore the packages using `dotnet restore`
4. Open your favourite editor and start developing

## Docker
If you want to build the Docker container yourself, you'll have to follow the following steps:
1. Clone this project using `git clone git clone https://github.com/Crypit-Coders-Inc/lotus-server.git -b docker`
2. Open the `RestApi/appsettings.Docker.json` file and change the _Key_ and _Issuer_ in the **Jwt** section
3. Build the Docker container using `docker-compose build`
4. As soon as the container is build, you can run the container by running `docker-compose up [-d]` (_use `-d` if you want to run it as Daemon_)
