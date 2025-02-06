namespace CulturalShare.Common.Helper.EnvHelpers;

internal class EnvironmentVariables
{
    public string Host { get; set; }
    public string Port { get; set; }
    public string DbName { get; set; }
    public string UserName { get; set; }
    public string Password { get; set; }
    public string MongoHost { get; set; }
    public string MongoPort { get; set; }
    public string MongoDbName { get; set; }
    public bool IsRunningInContainer { get; set; }
    public string KafkaUrl { get; set; }
    public string KafkaGroupId { get; set; }
    public string DebeziumUrl { get; set; }
    public string AuthServiceUrl { get; set; }
    public string PostReadServiceUrl { get; set; }
    public string PostWriteServiceUrl { get; set; }
    public string GraylogHost { get; set; }
    public string GraylogPort { get; set; }
    public string GraylogTransportType { get; set; }
}
