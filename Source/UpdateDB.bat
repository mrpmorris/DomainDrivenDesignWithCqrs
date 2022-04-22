@echo off
dotnet ef database update --project=DomainDrivenDesignWithCqrs.AppLayer --startup-project=DomainDrivenDesignWithCqrs.Server

