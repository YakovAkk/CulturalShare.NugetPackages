using CulturalShare.Common.Helper.Extensions;
using Grpc.Core;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;

namespace CulturalShare.Common.Helper;

public static class HttpHelper
{
    public static int GetCustomerId(HttpContext httpContext)
    {
        var customerIdClaim = httpContext.User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier);

        if (customerIdClaim != null && int.TryParse(customerIdClaim.Value, out int customerId))
        {
            return customerId;
        }

        throw new UnauthorizedAccessException("Unauthorized: Missing or invalid CustomerId claim");
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
