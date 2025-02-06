namespace CulturalShare.Common.Helper.EnvHelpers;

public class EnvVariablesConstants
{
    public const string DotnetRunningInContainer = "DOTNET_RUNNING_IN_CONTAINER";
    public const string Host = "HOST";
    public const string Port = "PORT";
    public const string DbName = "DB_NAME";
    public const string UserName = "USER_NAME";
    public const string Password = "PASSWORD";
    public const string MongoHost = "MONGO_HOST";
    public const string MongoPort = "MONGO_PORT";
    public const string MongoDbName = "MONGO_DB_NAME";
    public const string KafkaUrl = "KAFKA_URL";
    public const string KafkaGroupId = "KAFKA_GROUP_ID";
    public const string DebeziumUrl = "DEBEZIUM_URL";
    public const string AuthServiceUrl = "AUTH_SERVICE_URL";
    public const string PostReadServiceUrl = "POST_READ_SERVICE_URL";
    public const string PostWriteServiceUrl = "POST_WRITE_SERVICE_URL";
    public const string GraylogHost = "GRAYLOG_HOST";
    public const string GraylogPort = "GRAYLOG_PORT";
    public const string GraylogTransportType = "GRAYLOG_TRANSPORT_TYPE";
}
