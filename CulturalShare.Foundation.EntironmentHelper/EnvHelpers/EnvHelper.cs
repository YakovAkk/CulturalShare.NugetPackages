using Microsoft.Extensions.Configuration;

namespace CulturalShare.Foundation.EntironmentHelper.EnvHelpers;

internal class EnvHelper
{
    public static bool TryGetEnvironmentalVariables(IConfiguration configuration, out EnvironmentVariables envVars)
    {
        envVars = new EnvironmentVariables();

        if (!IsRunningInContainer(configuration))
        {
            return false;
        }

        envVars.IsRunningInContainer = true;
        envVars.Port = GetEnvVariable(EnvVariablesConstants.Port, configuration);
        envVars.Host = GetEnvVariable(EnvVariablesConstants.Host, configuration);
        envVars.UserName = GetEnvVariable(EnvVariablesConstants.UserName, configuration);
        envVars.Password = GetEnvVariable(EnvVariablesConstants.Password, configuration);
        envVars.DbName = GetEnvVariable(EnvVariablesConstants.DbName, configuration);
        envVars.MongoDbName = GetEnvVariable(EnvVariablesConstants.MongoDbName, configuration);
        envVars.MongoPort = GetEnvVariable(EnvVariablesConstants.MongoPort, configuration);
        envVars.MongoHost = GetEnvVariable(EnvVariablesConstants.MongoHost, configuration);
        envVars.KafkaUrl = GetEnvVariable(EnvVariablesConstants.KafkaUrl, configuration);
        envVars.KafkaGroupId = GetEnvVariable(EnvVariablesConstants.KafkaGroupId, configuration);
        envVars.DebeziumUrl = GetEnvVariable(EnvVariablesConstants.DebeziumUrl, configuration);
        envVars.AuthServiceUrl = GetEnvVariable(EnvVariablesConstants.AuthServiceUrl, configuration);
        envVars.PostReadServiceUrl = GetEnvVariable(EnvVariablesConstants.PostReadServiceUrl, configuration);
        envVars.PostWriteServiceUrl = GetEnvVariable(EnvVariablesConstants.PostWriteServiceUrl, configuration);
        envVars.GraylogHost = GetEnvVariable(EnvVariablesConstants.GraylogHost, configuration);
        envVars.GraylogPort = GetEnvVariable(EnvVariablesConstants.GraylogPort, configuration);
        envVars.GraylogTransportType = GetEnvVariable(EnvVariablesConstants.GraylogTransportType, configuration);

        return true;
    }

    private static bool IsRunningInContainer(IConfiguration configuration)
    {
        var dotnetRunningInContainer = GetEnvVariable(EnvVariablesConstants.DotnetRunningInContainer, configuration);
        return bool.TryParse(dotnetRunningInContainer, out bool isRunningInContainer) && isRunningInContainer;
    }

    private static string GetEnvVariable(string envVariablesConstant, IConfiguration configuration)
    {
        return configuration[envVariablesConstant] ?? Environment.GetEnvironmentVariable(envVariablesConstant);
    }
}
