# AACSB Portal API
## Environment
- Jetbrains Rider 2022.2.2
- .NET Core 6
- .NET CLI

## Pre-requirement

## DB Migration
0. You can use Inmemory DB for test!!
1. Install ef tool
   `dotnet tool install --global dotnet-ef`
2. Start a MSSQL
   - For common users
   `docker run -e 'ACCEPT_EULA=Y' -e 'SA_PASSWORD=P@$$w0rd' -p 1433:1433 -d mcr.microsoft.com/mssql/server`
   - For Apple Silicon users
   `docker run -e 'ACCEPT_EULA=Y' -e 'SA_PASSWORD=P@$$w0rd' -p 1433:1433 -d mcr.microsoft.com/azure-sql-edge`
3. Set environment string (DB)
4. Migrate DB
> This project will migrate db automatically.

## DB Add Migration
```
dotnet ef migrations add <CommitMessage> --project ./src/Migrators/Migrators.<DBProvider>/ --context ApplicationDbContext -o Migrations/Application
```

## Environment Variables
All the environment variables are located at `src/Host`, there are all the configuration files, one for each area.

## Others
This project is based on [codewithmukesh](https://codewithmukesh.com/blog/introducing-fullstackhero/), which can be download from [here](https://github.com/fullstackhero/dotnet-webapi-boilerplate).
Detailed architecture introduction can be found [here](https://codewithmukesh.com/blog/onion-architecture-in-aspnet-core/) and [CQRS Design](https://codewithmukesh.com/blog/cqrs-in-aspnet-core-3-1/).