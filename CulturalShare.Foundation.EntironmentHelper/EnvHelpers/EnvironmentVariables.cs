
namespace CulturalShare.Foundation.EntironmentHelper.EnvHelpers;

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
    public string RedisHost { get; set; }
    public string RedisPort { get; set; }
    public string RedisPassword { get; set; }
    public string JwtIssuer { get; set; }
    public int SecondsUntilExpireUserJwtToken { get; set; }
    public int SecondsUntilExpireUserRefreshToken { get; set; }
    public int SecondsUntilExpireServiceJwtToken { get; set; }
    public string JwtSecretTokenPairs { get; set; }
}
