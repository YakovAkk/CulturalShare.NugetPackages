using AuthenticationBackProto;
using CulturalShare.Foundation.EnvironmentHelper.EnvHelpers;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Service.Helper;

namespace CulturalShare.Interaction.Microservices.Extensions;

public static class AuthWrapperExtension
{
    public static void AddAuthWrapper(this IServiceCollection services, IConfiguration configuration)
    {
        var sortOutCredentialsHelper = new SortOutCredentialsHelper(configuration);
        var grpcClientsUrlConfiguration = sortOutCredentialsHelper.GetGrpcClientsUrlConfiguration();

        var jwtServicesConfig = sortOutCredentialsHelper.GetJwtServicesConfiguration();
        services.AddSingleton(jwtServicesConfig);

        services.AddGrpcClient<AuthenticationBackGrpcService.AuthenticationBackGrpcServiceClient>(options =>
        {
            options.Address = new Uri(grpcClientsUrlConfiguration.AuthClientUrl);
        });

        services.AddScoped<IMicroserviceAuthWrapper, MicroserviceAuthWrapper>();
    }
}
