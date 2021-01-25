![.NET-shield](https://img.shields.io/badge/.NET-5.0-blueviolet?style=for-the-badge&logo=.net)
![Postgres-shield](https://img.shields.io/badge/Postgres-12+-blue?style=for-the-badge&logo=postgresql)
# lotus-server
A RESTful backend API for the LOTUS 2021 project
<br/>_https://lotus21-api.herokuapp.com_

### [Requirements](#Requirements) | [Installation](#Installation)

## Requirements
* .NET 5.0
* Postgres 12 (or later)

## Installation
### Option 1 (normal):
1. Unzip the `.zip` file containing this project.
2. Open your terminal in the folder containing the `.sln` file.
3. Execute the following command: `dotnet run -p RestApi`.

### Options 2 (containerized):
1. Unzip the `.zip` file containing this project.
2. Open your terminal in the folder containing the `.sln` file.
3. Execute the following command: `docker build .`.
4. Execute the following command: `docker run --name lotus21_api -p 5000:80 -d [container_id]`.
