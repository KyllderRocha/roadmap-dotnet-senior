# roadmap-dotnet-senior Fase 3

# Estando dentro de src/TaskManager.Api
dotnet add package BCrypt.Net-Next
dotnet add package Microsoft.AspNetCore.Authentication.JwtBearer

# Raiz
dotnet ef migrations add AddPasswordHashToUser --project src/TaskManager.Infrastructure --startup-project src/TaskManager.Api
dotnet ef database update --project src/TaskManager.Infrastructure --startup-project src/TaskManager.Api