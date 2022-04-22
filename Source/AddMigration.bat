@echo off
SET migrationName=""
set /p migrationName=Enter a migration name: 
dotnet ef migrations add "%migrationName%" --project=DomainDrivenDesignWithCqrs.AppLayer --startup-project=DomainDrivenDesignWithCqrs.Server
SET migrationName=""
