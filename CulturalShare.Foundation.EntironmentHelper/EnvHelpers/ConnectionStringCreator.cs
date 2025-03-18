using CulturalShare.Foundation.EntironmentHelper.Configurations;

namespace CulturalShare.Foundation.EntironmentHelper.EnvHelpers;

internal static class ConnectionStringCreator
{
    public static string CreatePostgresConnectionString(string host, string port, string dbName, string userName, string password)
    {
        return $"Host={host};Port={port};Database={dbName};Username={userName};Password={password};";
    }

    public static string CreateMongoConnectionString(string host, string port)
    {
        return $"mongodb://{host}:{port}";
    }

    public static string CreateRegisConnectionString(RedisConfiguration configuration)
    {
        return $"{configuration.Host}:{configuration.Port},password={configuration.Password}";
    }
}
