using System.Net;
using System.Text.Json;

namespace StefaniniPedido.API.Middlewares;

public sealed class ExceptionHandlingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<ExceptionHandlingMiddleware> _logger;

    private static readonly JsonSerializerOptions JsonOptions = new()
    {
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase
    };

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
            _logger.LogError(ex, "Exceção não tratada: {Message}", ex.Message);
            await HandleExceptionAsync(context, ex);
        }
    }

    private static Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        var (statusCode, error, message) = exception switch
        {
            ArgumentNullException ex => (HttpStatusCode.BadRequest, "Bad Request", ex.Message),
            ArgumentException ex => (HttpStatusCode.BadRequest, "Bad Request", ex.Message),
            KeyNotFoundException ex => (HttpStatusCode.NotFound, "Not Found", ex.Message),
            InvalidOperationException ex => (HttpStatusCode.UnprocessableEntity, "Unprocessable Entity", ex.Message),
            UnauthorizedAccessException => (HttpStatusCode.Unauthorized, "Unauthorized", "Acesso não autorizado."),
            NotImplementedException => (HttpStatusCode.NotImplemented, "Not Implemented", "Funcionalidade não implementada."),
            _ => (HttpStatusCode.InternalServerError, "Internal Server Error", "Ocorreu um erro interno. Tente novamente mais tarde.")
        };

        var response = new ErrorResponse
        {
            StatusCode = (int)statusCode,
            Error      = error,
            Message    = message,
            Detail     = exception is not InvalidOperationException and not ArgumentException
                             ? null
                             : exception.InnerException?.Message
        };

        context.Response.ContentType = "application/json";
        context.Response.StatusCode = (int)statusCode;

        var json = JsonSerializer.Serialize(response, JsonOptions);
        return context.Response.WriteAsync(json);
    }
}
