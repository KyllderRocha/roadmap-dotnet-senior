using Serilog;
using TaskManager.Api.Middleware;
using TaskManager.Infrastructure;
using TaskManager.Application; 


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

    // builder.Services.AddValidatorsFromAssembly(typeof(TaskManager.Application.AssemblyReference).Assembly);

    // builder.Services.AddMediatR(cfg =>
    // {
    //     cfg.RegisterServicesFromAssembly(typeof(TaskManager.Application.AssemblyReference).Assembly);
    //     cfg.AddBehavior(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));
    // });

    // var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

    // builder.Services.AddDbContext<ApplicationDbContext>(options =>
    //     options.UseNpgsql(connectionString, npgsqlOptions =>
    //     {
    //         npgsqlOptions.EnableRetryOnFailure(
    //             maxRetryCount: 5, 
    //             maxRetryDelay: TimeSpan.FromSeconds(30), 
    //             errorCodesToAdd: null);
    //     }));

    // builder.Services.AddScoped<IUserRepository, UserRepository>();
    // builder.Services.AddScoped<ITaskRepository, TaskRepository>();
    // builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
    // builder.Services.AddScoped<IUserService, UserService>();
    // builder.Services.AddScoped<ITaskService, TaskService>();

    builder.Services.AddControllers();
    builder.Services.AddEndpointsApiExplorer();
    builder.Services.AddSwaggerGen();


    var app = builder.Build();
    app.MapControllers();

    app.UseSerilogRequestLogging();

    app.UseMiddleware<ExceptionHandlingMiddleware>();

    // Configure the HTTP request pipeline.
    if (app.Environment.IsDevelopment())
    {
        app.UseSwagger();
        app.UseSwaggerUI();
    }

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
