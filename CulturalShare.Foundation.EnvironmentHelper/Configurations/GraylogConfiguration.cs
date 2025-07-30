using Serilog.Sinks.Graylog.Core.Transport;

namespace CulturalShare.Foundation.EnvironmentHelper.Configurations;

public class GraylogConfiguration
{
    public string Host { get; set; }
    public int Port { get; set; }
    public TransportType TransportType { get; set; }
}
