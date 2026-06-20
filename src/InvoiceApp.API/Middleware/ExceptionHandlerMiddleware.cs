using System.Net;
using System.Text.Json;

namespace InvoiceApp.API.Middleware;

public class ExceptionHandlerMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<ExceptionHandlerMiddleware> _logger;

    public ExceptionHandlerMiddleware(RequestDelegate next, ILogger<ExceptionHandlerMiddleware> logger)
    {
        _next   = next;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (KeyNotFoundException ex)
        {
            await WriteError(context, (int)HttpStatusCode.NotFound, ex.Message);
        }
        catch (InvalidOperationException ex)
        {
            await WriteError(context, (int)HttpStatusCode.BadRequest, ex.Message);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error no controlado");
            await WriteError(context, (int)HttpStatusCode.InternalServerError, "Ocurrió un error inesperado.");
        }
    }

    private static Task WriteError(HttpContext ctx, int statusCode, string message)
    {
        ctx.Response.StatusCode  = statusCode;
        ctx.Response.ContentType = "application/json";
        var body = JsonSerializer.Serialize(new { success = false, error = message, statusCode });
        return ctx.Response.WriteAsync(body);
    }
}
