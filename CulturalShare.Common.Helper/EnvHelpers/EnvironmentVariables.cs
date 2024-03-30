namespace CulturalShare.Common.Helper.EnvHelpers;

internal class EnvironmentVariables
{
    /// <summary>
    /// Host server environment variable.
    /// </summary>
    public string Host { get; set; }

    /// <summary>
    /// Port environment variable.
    /// </summary>
    public string Port { get; set; }

    /// <summary>
    /// Database name environment variable.
    /// </summary>
    public string DbName { get; set; }

    /// <summary>
    /// User name environment variable.
    /// </summary>
    public string UserName { get; set; }

    /// <summary>
    /// Password environment variable.
    /// </summary>
    public string Password { get; set; }

    /// <summary>
    /// MongoHost environment variable.
    /// </summary>
    public string MongoHost { get; set; }

    /// <summary>
    /// MongoPort environment variable.
    /// </summary>
    public string MongoPort { get; set; }

    /// <summary>
    /// MongoDbName environment variable.
    /// </summary>
    public string MongoDbName { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether the application is running in a container.
    /// </summary>
    public bool IsRunningInContainer { get; set; }

    /// <summary>
    /// KafkaUrl environment variable.
    /// </summary>
    public string KafkaUrl { get; set; }

    /// <summary>
    /// KafkaGroupId environment variable.
    /// </summary>
    public string KafkaGroupId { get; set; }

    /// <summary>
    /// DebeziumUrl environment variable.
    /// </summary>
    public string DebeziumUrl { get; set; }

    /// <summary>
    /// AuthServiceUrl environment variable.
    /// </summary>
    public string AuthServiceUrl { get; set; }

    /// <summary>
    /// KafkaGroupId environment variable.
    /// </summary>
    public string PostReadServiceUrl { get; set; }

    /// <summary>
    /// DebeziumUrl environment variable.
    /// </summary>
    public string PostWriteServiceUrl { get; set; }
}
