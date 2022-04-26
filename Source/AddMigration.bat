@echo off
dotnet ef migrations add "%1" --project=DomainDrivenDesignWithCqrs.AppLayer --output-dir "Persistence\DbMigrations" --startup-project=DomainDrivenDesignWithCqrs.Server
