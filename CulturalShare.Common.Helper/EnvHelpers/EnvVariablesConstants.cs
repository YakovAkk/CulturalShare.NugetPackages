namespace CulturalShare.Common.Helper.EnvHelpers;

public class EnvVariablesConstants
{
    /// <summary>
    /// Indicates whether the application is running in a container.
    /// </summary>
    public const string DotnetRunningInContainer = "DOTNET_RUNNING_IN_CONTAINER";

    /// <summary>
    /// Host server environment variable.
    /// </summary>
    public const string Host = "HOST";

    /// <summary>
    /// Port environment variable.
    /// </summary>
    public const string Port = "PORT";

    /// <summary>
    /// Database name environment variable.
    /// </summary>
    public const string DbName = "DB_NAME";

    /// <summary>
    /// User name environment variable.
    /// </summary>
    public const string UserName = "USER_NAME";

    /// <summary>
    /// Password environment variable.
    /// </summary>
    public const string Password = "PASSWORD";

    /// <summary>
    /// MongoHost environment variable.
    /// </summary>
    public const string MongoHost = "MONGO_HOST";

    /// <summary>
    /// MongoPort environment variable.
    /// </summary>
    public const string MongoPort = "MONGO_PORT";

    /// <summary>
    /// MongoDbName environment variable.
    /// </summary>
    public const string MongoDbName = "MONGO_DB_NAME";

    /// <summary>
    /// KafkaUrl environment variable.
    /// </summary>
    public const string KafkaUrl = "KAFKA_URL";

    /// <summary>
    /// KafkaGroupId environment variable.
    /// </summary>
    public const string KafkaGroupId = "KAFKA_GROUP_ID";

    /// <summary>
    /// DebeziumUrl environment variable.
    /// </summary>
    public const string DebeziumUrl = "DEBEZIUM_URL";

    /// <summary>
    /// AuthServiceUrl environment variable.
    /// </summary>
    public const string AuthServiceUrl = "AUTH_SERVICE_URL";

    /// <summary>
    /// KafkaGroupId environment variable.
    /// </summary>
    public const string PostReadServiceUrl = "POST_READ_SERVICE_URL";

    /// <summary>
    /// DebeziumUrl environment variable.
    /// </summary>
    public const string PostWriteServiceUrl = "POST_WRITE_SERVICE_URL";
}
