using Microsoft.Extensions.Primitives;
using Serilog.Context;

namespace Bookify.Api.Middleware;

internal sealed class RequestContextLoggingMiddleware(RequestDelegate next)
{
    private const string CorrelationIdHeaderName = "X-Correlation-Id";

    public Task Invoke(HttpContext httpContext)
    {
        using (LogContext.PushProperty("CorrelationId", GetCorrelationId(httpContext)))
        {
            return next(httpContext);
        }
    }

    private static string GetCorrelationId(HttpContext httpContext)
    {
        httpContext.Request.Headers.TryGetValue(CorrelationIdHeaderName, out StringValues correlationId);

        return correlationId.FirstOrDefault() ?? httpContext.TraceIdentifier;
    }
}