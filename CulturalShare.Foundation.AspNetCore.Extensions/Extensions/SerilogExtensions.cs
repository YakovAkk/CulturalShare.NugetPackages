using CulturalShare.Foundation.AspNetCore.Extensions.Constants;
using CulturalShare.Foundation.EntironmentHelper.Configurations;
using CulturalShare.Foundation.EntironmentHelper.EnvHelpers;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Serilog;
using Serilog.Sinks.Graylog;

namespace CulturalShare.Foundation.AspNetCore.Extensions.Extensions;

public static class SerilogExtensions
{
    public static WebApplicationBuilder UseCustomSerilog(this WebApplicationBuilder builder, IConfiguration configuration)
    {
        var sortOutCredentialsHelper = new SortOutCredentialsHelper(configuration);
        var graylogConfig = sortOutCredentialsHelper.GetGraylogConfiguration();

        builder.Host.UseSerilog((context, config) =>
        {
            var configuration = context.Configuration;

            config.Enrich.WithCorrelationIdHeader(LoggingConsts.CorrelationIdHeaderName);
            config.Enrich.WithProperty(LoggingConsts.Environment, Environment.GetEnvironmentVariable(EnvVariablesConstants.AspNetCoreEnvironment));
            config.ReadFrom.Configuration(configuration);

            config.WriteTo.Graylog(new GraylogSinkOptions
            {
                HostnameOrAddress = graylogConfig.Host,
                Port = graylogConfig.Port,
                TransportType = graylogConfig.TransportType
            });
        });

        return builder;
    }
}
