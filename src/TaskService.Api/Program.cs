using Serilog;
using TaskManager.Api.Middleware;
using TaskManager.Infrastructure;
using TaskManager.Application;
using TaskManager.Application.Common.Interfaces;
using TaskManager.Api.Services;
using TaskManager.Api.Subscribers;
using Microsoft.AspNetCore.Authentication;
using Prometheus; 

Log.Logger = new LoggerConfiguration()
    .WriteTo.Console()
    .CreateBootstrapLogger();

Log.Information("Initialing web host builder");

try
{
    var builder = WebApplication.CreateBuilder(args);

    builder.Host.UseSerilog((context, configuration) => 
        configuration.ReadFrom.Configuration(context.Configuration));

    builder.Services.AddApplicationServices(); 
    builder.Services.AddInfrastructureServices(builder.Configuration);

    builder.Services.AddControllers();
    builder.Services.AddEndpointsApiExplorer();
    builder.Services.AddSwaggerGen();
    builder.Services.AddHttpContextAccessor();
    builder.Services.AddScoped<ICurrentUserService, CurrentUserService>();
    builder.Services.AddHostedService<UserCreatedSubscriber>();

    builder.Services.AddAuthorization();
    builder.Services.AddAuthentication("DefaultScheme")
        .AddScheme<AuthenticationSchemeOptions, BasicAuthenticationHandler>("DefaultScheme", null);


    var app = builder.Build();
    app.UseMiddleware<ExceptionHandlingMiddleware>();
    app.UseSerilogRequestLogging();

    app.UseMetricServer();
    app.UseHttpMetrics();

    // Configure the HTTP request pipeline.
    if (app.Environment.IsDevelopment())
    {
        app.UseSwagger();
        app.UseSwaggerUI();
    }

    app.UseAuthentication();
    app.UseAuthorization();

    app.MapControllers();

    // app.UseHttpsRedirection();

    app.Run();
}
catch (Exception ex)
{
    Log.Fatal(ex, "The application failed to start correctly");
}
finally
{
    Log.CloseAndFlush();
}
