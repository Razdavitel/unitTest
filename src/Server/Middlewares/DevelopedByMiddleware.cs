namespace Squads.Server.Middlewares;

// mainly example for potential future middleware
public class DevelopedByMiddleware
{
    private readonly RequestDelegate _next;

    public DevelopedByMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        context.Response.Headers.Add("X-Developed-By", "HoGent");
        // Call the next delegate/middleware in the pipeline.
        await _next(context);
    }
}

public static class RequestPingMiddlewareExtensions
{
    public static IApplicationBuilder UsePong(
        this IApplicationBuilder builder)
    {
        return builder.UseMiddleware<DevelopedByMiddleware>();
    }
}