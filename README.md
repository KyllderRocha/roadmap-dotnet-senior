# roadmap-dotnet-senior Fase 3

# Raiz
dotnet ef migrations add AddPasswordHashToUser --project src/TaskManager.Infrastructure --startup-project src/TaskManager.Api
dotnet ef database update --project src/TaskManager.Infrastructure --startup-project src/TaskManager.Api
dotnet add src/TaskManager.Infrastructure/TaskManager.Infrastructure.csproj package System.IdentityModel.Tokens.Jwt
dotnet add src/TaskManager.Application/TaskManager.Application.csproj package BCrypt.Net-Next
dotnet add src/TaskManager.Api/TaskManager.Api.csproj package Microsoft.AspNetCore.Authentication.JwtBearer --version 8.*
dotnet add src/TaskManager.Application/TaskManager.Application.csproj package Microsoft.AspNetCore.Http.Abstractions

