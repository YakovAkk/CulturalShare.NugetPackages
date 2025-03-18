using CulturalShare.Foundation.AspNetCore.Extensions.MiddlewareClasses;
using Microsoft.AspNetCore.Builder;

namespace CulturalShare.Foundation.AspNetCore.Extensions.Extensions;

public static class MiddlewareExtensions
{
    public static IApplicationBuilder UseExceptionsHandler(this IApplicationBuilder app)
    {
        return app.UseMiddleware<HandlingExceptionsMiddleware>();
    }

    public static IApplicationBuilder UseSecureHeaders(this IApplicationBuilder app)
    {
        return app.UseMiddleware<SecureHeadersMiddleware>();
    }

    public static IApplicationBuilder UseCorrelationIdMiddleware(this IApplicationBuilder app)
    {
        return app.UseMiddleware<CorrelationIdMiddleware>();
    }
}
