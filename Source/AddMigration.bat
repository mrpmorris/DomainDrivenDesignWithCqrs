@echo off
dotnet ef migrations add "%1" --project=DomainDrivenDesignWithCqrs.AppLayer --startup-project=DomainDrivenDesignWithCqrs.Server
