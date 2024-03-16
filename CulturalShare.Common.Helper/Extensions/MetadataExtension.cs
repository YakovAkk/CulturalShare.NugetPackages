using CulturalShare.Common.Helper.Constants;
using Grpc.Core;
using Microsoft.AspNetCore.Http;

namespace CulturalShare.Common.Helper.Extensions;

public static class MetadataExtension
{
    public static Metadata AddAuthHeader(this Metadata metadata, string accessToken)
    {
        var key = "Authorization";
        var value = $"Bearer {accessToken}";

        metadata.Add(key, value);

        return metadata;
    }

    public static Metadata AddCorrelationIdHeader(this Metadata metadata)
    {
        var key = LoggingConsts.CorrelationIdHeaderName;
        var value = Guid.NewGuid().ToString();

        metadata.Add(key, value);

        return metadata;
    }

    public static Metadata AddCorrelationIdHeader(this Metadata metadata, string correlationId)
    {
        var key = LoggingConsts.CorrelationIdHeaderName;
        var value = correlationId;

        metadata.Add(key, value);

        return metadata;
    }

    public static Metadata AddCorrelationIdHeader(this Metadata metadata, HttpContext context)
    {
        var isValueExist = context.Request.Headers.TryGetValue(LoggingConsts.CorrelationIdHeaderName, out var correlationId);
        if (isValueExist)
        {
            return metadata.AddCorrelationIdHeader(correlationId);
        }

        return metadata.AddCorrelationIdHeader();
    }
}
