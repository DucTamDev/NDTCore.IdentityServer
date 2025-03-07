::rmdir /S /Q "NDTCore.IdentityServer.Infrastructure/Persistences/Migrations"

::dotnet ef migrations add AspNetIdentityMigration -c ApplicationDbContext -o Persistences/Migrations/AspNetIdentityDb --project ../NDTCore.IdentityServer.Infrastructure
::dotnet ef migrations add PersistedGrantMigration -c PersistedGrantDbContext -o Persistences/Migrations/PersistedGrantDb --project ../NDTCore.IdentityServer.Infrastructure
::dotnet ef migrations add IdentityServerConfigMigration -c ConfigurationDbContext -o Persistences/Migrations/ConfigurationDb --project ../NDTCore.IdentityServer.Infrastructure

::dotnet ef migrations script -c ApplicationDbContext -o ../NDTCore.IdentityServer.Infrastructure/Persistences/Migrations/AspNetIdentityDb.sql
::dotnet ef migrations script -c PersistedGrantDbContext -o ../NDTCore.IdentityServer.Infrastructure/Persistences/Migrations/PersistedGrantDb.sql
::dotnet ef migrations script -c ConfigurationDbContext -o ../NDTCore.IdentityServer.Infrastructure/Persistences/Migrations/ConfigurationDb.sql

::# Using Package Manager Console (PMC) #

::Add-Migration AspNetIdentityMigration -Context ApplicationDbContext -OutputDir Persistences/Migrations/AspNetIdentityDb -Project NDTCore.IdentityServer.Infrastructure
::Add-Migration PersistedGrantMigration -Context PersistedGrantDbContext -OutputDir Persistences/Migrations/PersistedGrantDb -Project NDTCore.IdentityServer.Infrastructure
::Add-Migration IdentityServerConfigMigration -Context ConfigurationDbContext -OutputDir Persistences/Migrations/ConfigurationDb -Project NDTCore.IdentityServer.Infrastructure

::Script-Migration -Context ApplicationDbContext -Output ./NDTCore.IdentityServer.Infrastructure/Persistences/Migrations/AspNetIdentityDb.sql
::Script-Migration -Context PersistedGrantDbContext -Output ./NDTCore.IdentityServer.Infrastructure/Persistences/Migrations/PersistedGrantDb.sql
::Script-Migration -Context ConfigurationDbContext -Output ./NDTCore.IdentityServer.Infrastructure/Persistences/Migrations/ConfigurationDb.sql

::Update-Database -Context ApplicationDbContext
::Update-Database -Context PersistedGrantDbContext
::Update-Database -Context ConfigurationDbContext

@echo off
setlocal enabledelayedexpansion

:: Set project variables
set PROJECT_NAME=NDTCore.IdentityServer.Infrastructure
set STARTUP_PROJECT=%CD%  :: The script runs inside the startup project
set PROJECT_PATH=%CD%\..\%PROJECT_NAME%
set MIGRATIONS_PATH=%PROJECT_PATH%\Persistences\Migrations

echo -----------------------------------
echo Starting EF Core Migrations...
echo -----------------------------------

:: Ensure Migrations folder exists
if not exist "%MIGRATIONS_PATH%" (
    mkdir "%MIGRATIONS_PATH%"
)

:: Run EF Core migration commands
echo Creating Initial Identity Database Migration...
dotnet ef migrations add AspNetIdentityMigration -c ApplicationDbContext -o Persistences/Migrations/AspNetIdentityDb --project %PROJECT_PATH%

echo Creating Persisted Grant Migration...
dotnet ef migrations add PersistedGrantMigration -c PersistedGrantDbContext -o Persistences/Migrations/PersistedGrantDb --project %PROJECT_PATH%

echo Creating Identity Server Configuration Migration...
dotnet ef migrations add IdentityServerConfigMigration -c ConfigurationDbContext -o Persistences/Migrations/ConfigurationDb --project %PROJECT_PATH%

:: Generate SQL scripts from migrations
echo Generating SQL Scripts...
dotnet ef migrations script -c ApplicationDbContext -o %PROJECT_PATH%/Persistences/Migrations/AspNetIdentityDb.sql
dotnet ef migrations script -c PersistedGrantDbContext -o %PROJECT_PATH%/Persistences/Migrations/PersistedGrantDb.sql
dotnet ef migrations script -c ConfigurationDbContext -o %PROJECT_PATH%/Persistences/Migrations/ConfigurationDb.sql

::Update database
echo Updating Database...
dotnet ef database update -c ApplicationDbContext
dotnet ef database update -c PersistedGrantDbContext
dotnet ef database update -c ConfigurationDbContext

echo -----------------------------------
echo EF Core Migrations Completed!
echo -----------------------------------

endlocal
