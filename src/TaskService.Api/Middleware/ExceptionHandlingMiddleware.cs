
using System.Net;
using System.Text.Json;
using TaskManager.Application.Common.Exceptions;

namespace TaskManager.Api.Middleware;
public class ExceptionHandlingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<ExceptionHandlingMiddleware> _logger;

    public ExceptionHandlingMiddleware(RequestDelegate next, ILogger<ExceptionHandlingMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception ex)
        {
            await HandleExceptionAsync(context, ex);
        }
    }

    private async Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        HttpStatusCode statusCode;
        object response;

        switch (exception)
        {
            case ValidationRequestException validationException:
                statusCode = HttpStatusCode.BadRequest;
                response = new { errors = validationException.Errors };
                break;
            case NotFoundException notFoundException:
                statusCode = HttpStatusCode.NotFound;
                response = new { error = notFoundException.Message };
                break;
            default:
                statusCode = HttpStatusCode.InternalServerError;
                response = new { error = "Ocorreu um erro inesperado." };
                _logger.LogError(exception, "Erro não tratado: {Message}", exception.Message);
                break;
        }

        context.Response.ContentType = "application/json";
        context.Response.StatusCode = (int)statusCode;

        var jsonResponse = JsonSerializer.Serialize(response);
        await context.Response.WriteAsync(jsonResponse);
    }
}