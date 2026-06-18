using System.Net;
using NoteAppApi.Common.Exceptions;

namespace NoteAppApi.Middlewares;

public class ExceptionMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<ExceptionMiddleware> _logger;

    public ExceptionMiddleware(RequestDelegate next, ILogger<ExceptionMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task Invoke(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (AppException ex)
        {
            _logger.LogWarning(ex, "Application exception occurred");

            await HandleAppException(context, ex);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Unhandled exception occurred");

            await HandleUnknownException(context, ex);
        }
    }

    private static async Task HandleAppException(HttpContext ctx, AppException ex)
    {
        if (ctx.Response.HasStarted) return;

        ctx.Response.ContentType = "application/json";
        ctx.Response.StatusCode = (int)ex.StatusCode;

        await ctx.Response.WriteAsJsonAsync(new
        {
            error = ex.ErrorCode,
            message = ex.Message
        });
    }

    private static async Task HandleUnknownException(HttpContext ctx, Exception ex)
    {
        if (ctx.Response.HasStarted) return;

        ctx.Response.ContentType = "application/json";
        ctx.Response.StatusCode = 500;

        await ctx.Response.WriteAsJsonAsync(new
        {
            error = "INTERNAL_SERVER_ERROR",
            message = "Unexpected error"
        });
    }
}