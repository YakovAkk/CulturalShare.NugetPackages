using CulturalShare.Foundation.AspNetCore.Extensions.Constants;
using Microsoft.AspNetCore.Http;

namespace CulturalShare.Foundation.AspNetCore.Extensions.MiddlewareClasses;

public class CorrelationIdMiddleware
{
    private readonly RequestDelegate _next;

    public CorrelationIdMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task Invoke(HttpContext context)
    {
        if (!context.Request.Headers.ContainsKey(LoggingConsts.CorrelationIdHeaderName))
        {
            var correlationId = Guid.NewGuid().ToString();

            context.Request.Headers.Add(LoggingConsts.CorrelationIdHeaderName, correlationId);
        }

        await _next(context);
    }
}