using System.Diagnostics;

namespace GameStore.Api.MiddleWares;

public class RequestLoggingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<RequestLoggingMiddleware> _logger;
    public RequestLoggingMiddleware(
        RequestDelegate next,
        ILogger<RequestLoggingMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }
    public async Task InvokeAsync(HttpContext context)
    {
        var request = context.Request;
        var stopwatch = Stopwatch.StartNew();
        _logger.LogInformation(
            "Request started: {Method} {Path} {QueryString}",
            request.Method,
            request.Path,
            request.QueryString
        );
        try
        {
            await _next(context);
        }
        finally
        {
            stopwatch.Stop();

            _logger.LogInformation(
            "Request finished: {Method} {Path} → {StatusCode} in {ElapsedMs}ms",
            request.Method,
            request.Path,
            context.Response.StatusCode,
            stopwatch.ElapsedMilliseconds
        );
        }
        
    }
}
// Extension method
public static class RequestLoggingMiddlewareExtensions
{
    public static IApplicationBuilder UseRequestLogging(
        this IApplicationBuilder builder)
        => builder.UseMiddleware<RequestLoggingMiddleware>();
}
