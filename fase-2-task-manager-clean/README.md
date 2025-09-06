- Create new solution
dotnet new sln --name YourApiName

- Create the projects
dotnet new classlib --name TaskManager.Domain
dotnet new classlib --name TaskManager.Application
dotnet new classlib --name TaskManager.Infrastructure

dotnet new webapi --name TaskManager.Api


dotnet sln add src/TaskManager.Domain/TaskManager.Domain.csproj
dotnet sln add src/TaskManager.Application/TaskManager.Application.csproj
dotnet sln add src/TaskManager.Infrastructure/TaskManager.Infrastructure.csproj
dotnet sln add src/TaskManager.Api/TaskManager.Api.csproj


dotnet add src/TaskManager.Application/TaskManager.Application.csproj reference src/TaskManager.Domain/TaskManager.Domain.csproj

dotnet add src/TaskManager.Infrastructure/TaskManager.Infrastructure.csproj reference src/TaskManager.Application/TaskManager.Application.csproj

dotnet add src/TaskManager.Api/TaskManager.Api.csproj reference src/TaskManager.Infrastructure/TaskManager.Infrastructure.csproj
dotnet add src/TaskManager.Api/TaskManager.Api.csproj reference src/TaskManager.Application/TaskManager.Application.csproj


dotnet add package Npgsql.EntityFrameworkCore.PostgreSQL

cd src/TaskManager.Application

dotnet add package FluentValidation
dotnet add package FluentValidation.DependencyInjectionExtensions

# Estando na pasta raiz da solução (fase-2-task-manager-clean)
cd src/TaskManager.Api

dotnet add package Serilog.AspNetCore
dotnet add package Serilog.Sinks.File
dotnet add package Serilog.Sinks.Console

---

# No terminal, dentro da pasta fase-2-task-manager-clean
cd src/TaskManager.Application/
dotnet add package MediatR

---
# Migrations 
## Na pasta src 
dotnet add TaskManager.Api package Microsoft.EntityFrameworkCore.Design
dotnet ef migrations add InitialCreate --project TaskManager.Infrastructure --startup-project TaskManager.Api
dotnet ef database update --project TaskManager.Infrastructure --startup-project TaskManager.Api


# Docker
docker-compose up --build