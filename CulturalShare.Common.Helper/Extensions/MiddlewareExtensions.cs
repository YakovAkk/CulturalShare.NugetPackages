using CulturalShare.Common.Helper.MiddlewareClasses;
using Microsoft.AspNetCore.Builder;

namespace CulturalShare.Common.Helper.Extensions;

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
