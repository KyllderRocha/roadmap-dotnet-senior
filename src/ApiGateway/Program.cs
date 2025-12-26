using Ocelot.DependencyInjection;
using Ocelot.Middleware;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using System.IdentityModel.Tokens.Jwt;
using Prometheus;

var builder = WebApplication.CreateBuilder(args);

builder.Configuration.AddJsonFile("ocelot.json", optional: false, reloadOnChange: true);

var secret = builder.Configuration.GetSection("JwtSettings:Secret").Value!;
var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secret));

JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Clear();

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = key,
        ValidateIssuer = true,
        ValidIssuer = builder.Configuration.GetSection("JwtSettings:Issuer").Value,
        ValidateAudience = true,
        ValidAudience = builder.Configuration.GetSection("JwtSettings:Audience").Value,
        ValidateLifetime = true
    };
});



builder.Services.AddOcelot(builder.Configuration);

var app = builder.Build();
// app.UseAuthentication();

app.UseMetricServer();
app.UseHttpMetrics();

// app.Use(async (context, next) =>
// {
//     if (context.User.Identity is not null && context.User.Identity.IsAuthenticated)
//     {
//         var logger = context.RequestServices.GetRequiredService<ILogger<Program>>();
//         logger.LogInformation("--- Claims do Usuário Autenticado ---");
//         foreach (var claim in context.User.Claims)
//         {
//             logger.LogInformation("Claim Type: {Type}, Claim Value: {Value}", claim.Type, claim.Value);
//         }
//         logger.LogInformation("------------------------------------");
//     }
//     await next.Invoke();
// });

await app.UseOcelot();
app.Run();