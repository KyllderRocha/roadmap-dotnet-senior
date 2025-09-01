- Create new solution
dotnet new sln --name YourApiName

- Create the projects
dotnet new classlib --name YourApiName.Domain
dotnet new classlib --name YourApiName.Application
dotnet new classlib --name YourApiName.Infrastructure

dotnet new webapi --name YourApiName.Api


dotnet sln add src/YourApiName.Domain/YourApiName.Domain.csproj
dotnet sln add src/YourApiName.Application/YourApiName.Application.csproj
dotnet sln add src/YourApiName.Infrastructure/YourApiName.Infrastructure.csproj
dotnet sln add src/YourApiName.Api/YourApiName.Api.csproj


dotnet add src/YourApiName.Application/YourApiName.Application.csproj reference src/YourApiName.Domain/YourApiName.Domain.csproj

dotnet add src/YourApiName.Infrastructure/YourApiName.Infrastructure.csproj reference src/YourApiName.Application/YourApiName.Application.csproj

dotnet add src/YourApiName.Api/YourApiName.Api.csproj reference src/YourApiName.Infrastructure/YourApiName.Infrastructure.csproj
dotnet add src/YourApiName.Api/YourApiName.Api.csproj reference src/YourApiName.Application/YourApiName.Application.csproj


dotnet add package Npgsql.EntityFrameworkCore.PostgreSQL

cd src/TaskManagerClean.Application

dotnet add package FluentValidation
dotnet add package FluentValidation.DependencyInjectionExtensions

---

# No terminal, dentro da pasta fase-2-task-manager-clean
cd src/TaskManagerClean.Application/
dotnet add package MediatR

---
# Migrations 
## Na pasta src 
dotnet add TaskManager.Api package Microsoft.EntityFrameworkCore.Design
dotnet ef migrations add InitialCreate --project TaskManager.Infrastructure --startup-project TaskManager.Api
dotnet ef database update --project TaskManager.Infrastructure --startup-project TaskManager.Api


# Docker
docker-compose up --build