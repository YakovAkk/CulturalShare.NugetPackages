using CulturalShare.Foundation.AspNetCore.Extensions.Extensions;
using Grpc.Core;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;

namespace CulturalShare.Foundation.AspNetCore.Extensions.Helpers;

public static class HttpHelper
{
    public static int GetUserId(HttpContext httpContext)
    {
        var customerIdClaim = httpContext.User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier);

        if (customerIdClaim != null && int.TryParse(customerIdClaim.Value, out int customerId))
        {
            return customerId;
        }

        throw new UnauthorizedAccessException("Unauthorized: Missing or invalid CustomerId claim");
    }

    public static int GetUserIdOrThrowRpcException(HttpContext httpContext)
    {
        if (httpContext?.User?.Identity?.IsAuthenticated != true)
        {
            throw new RpcException(new Status(StatusCode.Unauthenticated, "Unauthorized: Missing or invalid CustomerId claim"));
        }

        var userIdClaim = httpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

        if (!int.TryParse(userIdClaim, out var userId))
        {
            throw new RpcException(new Status(StatusCode.Unauthenticated, "Unauthorized: Missing or invalid CustomerId claim"));
        }

        return userId;
    }

    public static string GetCustomerEmail(HttpContext httpContext)
    {
        var customerEmailClaim = httpContext.User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Email);

        if (customerEmailClaim != null)
        {
            return customerEmailClaim.Value;
        }

        throw new UnauthorizedAccessException("Unauthorized: Missing or invalid Email claim");
    }

    public static Metadata CreateHeaderWithCorrelationId()
    {
        var header = new Metadata().AddCorrelationIdHeader();
        return header;
    }

    public static Metadata CreateHeaderWithCorrelationId(string correlationId)
    {
        var header = new Metadata().AddCorrelationIdHeader(correlationId);
        return header;
    }

    public static Metadata CreateHeaderWithCorrelationId(HttpContext context)
    {
        var header = new Metadata().AddCorrelationIdHeader(context);
        return header;
    }
}
