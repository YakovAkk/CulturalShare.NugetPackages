using CulturalShare.Common.Helper.Extensions;
using CulturalShare.Foundation.EnvironmentHelper.Configurations;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using Serilog.Sinks.Graylog.Core.Transport;

namespace CulturalShare.Foundation.EnvironmentHelper.EnvHelpers;

public class SortOutCredentialsHelper
{
    private readonly IConfiguration _configuration;
    private readonly EnvironmentVariables _enviromentVariables;
    private readonly bool _isRunningInDocker;

    public SortOutCredentialsHelper(IConfiguration configuration)
    {
        _configuration = configuration;

        if (EnvHelper.TryGetEnvironmentalVariables(_configuration, out var envVars))
        {
            _enviromentVariables = envVars;
            _isRunningInDocker = true;
        }
        else
        {
            _isRunningInDocker = false;
        }
    }

    public string GetPostgresConnectionString(string sectionName = "DefaultConnection")
    {
        if (!HasCompletePostgresEnv())
        {
            return _configuration.GetConnectionString(sectionName);
        }

        return ConnectionStringCreator.CreatePostgresConnectionString(
            _enviromentVariables.Host,
            _enviromentVariables.Port,
            _enviromentVariables.DbName,
            _enviromentVariables.UserName,
            _enviromentVariables.Password);
    }

    public string GetRedisConnectionString(string sectionName = "Redis")
    {
        var configuration = GetConfiguration(sectionName,
            () => new RedisConfiguration
            {
                Host = _enviromentVariables.RedisHost,
                Port = Convert.ToInt32(_enviromentVariables.RedisPort),
                Password = _enviromentVariables.RedisPassword
            });

        return ConnectionStringCreator.CreateRegisConnectionString(configuration);
    }

    public JwtServicesConfig GetJwtServicesConfiguration(string sectionName = nameof(JwtServicesConfig))
    {
        return GetConfiguration(sectionName,
            () => new JwtServicesConfig
            {
                CurrentServiceId = _enviromentVariables.CurrentServiceId,
                Issuer = _enviromentVariables.JwtIssuer,
                SecondsUntilExpireUserJwtToken = _enviromentVariables.SecondsUntilExpireUserJwtToken,
                SecondsUntilExpireUserRefreshToken = _enviromentVariables.SecondsUntilExpireUserRefreshToken,
                SecondsUntilExpireServiceJwtToken = _enviromentVariables.SecondsUntilExpireServiceJwtToken,
                ServicesJwtConfigs = DeserializeJwtSecretPairs(_enviromentVariables.JwtSecretTokenPairs)
            });
    }

    public MongoConfiguration GetMongoConfiguration(string sectionName = nameof(MongoConfiguration))
    {
        return GetConfiguration(sectionName,
            () => new MongoConfiguration
            {
                ConnectionString = ConnectionStringCreator.CreateMongoConnectionString(
                    _enviromentVariables.MongoHost, 
                    _enviromentVariables.MongoPort),

                DatabaseName = _enviromentVariables.MongoDbName
            });
    }

    public GraylogConfiguration GetGraylogConfiguration(string sectionName = nameof(GraylogConfiguration))
    {
        return GetConfiguration(sectionName,
            () => new GraylogConfiguration
            {
                Port = Convert.ToInt32(_enviromentVariables.GraylogPort),
                Host = _enviromentVariables.GraylogHost,
                TransportType = _enviromentVariables.GraylogTransportType.ToEnum<TransportType>()
            });
    }

    public KafkaConfiguration GetKafkaConfiguration(string sectionName = nameof(KafkaConfiguration))
    {
        return GetConfiguration(sectionName,
            () => new KafkaConfiguration
            {
                GroupId = _enviromentVariables.KafkaGroupId,
                Url = _enviromentVariables.KafkaUrl
            });
    }

    public DebesiumConfiguration GetDebesiumConfiguration(string sectionName = nameof(DebesiumConfiguration))
    {
        return GetConfiguration(sectionName,
            () => new DebesiumConfiguration
            {
                Url = _enviromentVariables.DebeziumUrl
            });
    }

    public GrpcClientsUrlConfiguration GetGrpcClientsUrlConfiguration(string sectionName = nameof(GrpcClientsUrlConfiguration))
    {
        return GetConfiguration(sectionName,
            () => new GrpcClientsUrlConfiguration
            {
                AuthClientUrl = _enviromentVariables.AuthServiceUrl,
                PostReadClientUrl = _enviromentVariables.PostReadServiceUrl,
                PostWriteClientUrl = _enviromentVariables.PostWriteServiceUrl,
            });
    }

    #region Private
    private static List<ServiceJwtConfig> DeserializeJwtSecretPairs(string? rawJson) =>
        string.IsNullOrWhiteSpace(rawJson)
            ? new List<ServiceJwtConfig>()
            : JsonConvert.DeserializeObject<List<ServiceJwtConfig>>(rawJson)!;

    private T GetConfiguration<T>(string sectionName, Func<T> createFromEnvVars) where T : class, new()
    {
        if (_isRunningInDocker)
        {
            var envConfig = TryCreateFromEnvVars(createFromEnvVars);

            if (envConfig is not null && IsConfigComplete(envConfig))
            {
                return envConfig;
            }
        }

        return _configuration.GetSection(sectionName).Get<T>() ?? new T();
    }

    private T TryCreateFromEnvVars<T>(Func<T> create) where T : class
    {
        try
        {
            return create();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"[{nameof(GetConfiguration)}] Failed to create config {typeof(T).Name} from environment variables: {ex.Message}");
            return null;
        }
    }

    private bool IsConfigComplete<T>(T config)
    {
        foreach (var property in typeof(T).GetProperties())
        {
            if (property.GetValue(config) == null)
            {
                return false;
            }
        }
        return true;
    }

    private bool HasCompletePostgresEnv()
    {
        if (_enviromentVariables == null)
        {
            return false;
        }

        return !string.IsNullOrWhiteSpace(_enviromentVariables.Host)
            && !string.IsNullOrWhiteSpace(_enviromentVariables.Port)
            && !string.IsNullOrWhiteSpace(_enviromentVariables.DbName)
            && !string.IsNullOrWhiteSpace(_enviromentVariables.UserName)
            && !string.IsNullOrWhiteSpace(_enviromentVariables.Password);
    }

    #endregion
}
