using Serilog.Sinks.Graylog.Core.Transport;

namespace CulturalShare.Common.Helper.Configurations;

public class GraylogConfiguration
{
    public string Host { get; set; }
    public int Port { get; set; }
    public TransportType TransportType { get; set; }
}
