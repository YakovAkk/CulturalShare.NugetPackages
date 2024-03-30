namespace CulturalShare.Common.Helper.EnvHelpers;

internal static class ConnectionStringCreator
{
    /// <summary>
    /// Creates a SQL Server connection string based on the configured parameters.
    /// </summary>
    /// <returns>A SQL Server connection string.</returns>
    public static string CreatePostgresConnectionString(string host, string port, string dbName, string userName, string password)
    {
        return $"Host={host};Port={port};Database={dbName};Username={userName};Password={password};";
    }

    /// <summary>
    /// Creates a SQL Server connection string based on the configured parameters.
    /// </summary>
    /// <returns>A SQL Server connection string.</returns>
    public static string CreateMongoConnectionString(string host, string port)
    {
        return $"mongodb://{host}:{port}";
    }
}
