using CulturalShare.Common.Helper.Extensions;
using CulturalShare.Foundation.EntironmentHelper.Configurations;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using Serilog.Sinks.Graylog.Core.Transport;

namespace CulturalShare.Foundation.EntironmentHelper.EnvHelpers;

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
        if (_isRunningInDocker)
        {
            return ConnectionStringCreator.CreatePostgresConnectionString(
                _enviromentVariables.Host,
                _enviromentVariables.Port,
                _enviromentVariables.DbName,
                _enviromentVariables.UserName,
                _enviromentVariables.Password);
        }

        var dBConnectionString = _configuration.GetConnectionString(sectionName);
        return dBConnectionString;
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
                Issuer = _enviromentVariables.JwtIssuer,
                SecondsUntilExpireUserJwtToken = _enviromentVariables.SecondsUntilExpireUserJwtToken,
                SecondsUntilExpireUserRefreshToken = _enviromentVariables.SecondsUntilExpireUserRefreshToken,
                SecondsUntilExpireServiceJwtToken = _enviromentVariables.SecondsUntilExpireServiceJwtToken,
                JwtSecretTokenPairs = JsonConvert.DeserializeObject<Dictionary<string, string>>(_enviromentVariables.JwtSecretTokenPairs)
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
    private T GetConfiguration<T>(string sectionName, Func<T> createFromEnvVars) where T : new()
    {
        if (_isRunningInDocker)
        {
            T configFromEnvVars = createFromEnvVars();

            if (IsConfigComplete(configFromEnvVars))
            {
                return configFromEnvVars;
            }
        }

        var section = _configuration.GetSection(sectionName);
        return section.Get<T>() ?? new T();
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
    #endregion
}
