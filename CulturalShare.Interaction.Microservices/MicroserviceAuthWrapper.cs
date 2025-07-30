using AuthenticationBackProto;
using CulturalShare.Foundation.AspNetCore.Extensions.Extensions;
using CulturalShare.Foundation.EnvironmentHelper.Configurations;
using Grpc.Core;
using Microsoft.Extensions.Logging;

namespace Service.Helper;

public class MicroserviceAuthWrapper : IMicroserviceAuthWrapper
{
    private readonly AuthenticationBackGrpcService.AuthenticationBackGrpcServiceClient _authenticationBackGrpcServiceClient;
    private readonly JwtServicesConfig _jwtServicesConfig;
    private readonly ILogger<MicroserviceAuthWrapper> _logger;

    public MicroserviceAuthWrapper(
        AuthenticationBackGrpcService.AuthenticationBackGrpcServiceClient authenticationBackGrpcServiceClient,
        JwtServicesConfig jwtServicesConfig,
        ILogger<MicroserviceAuthWrapper> logger)
    {
        _authenticationBackGrpcServiceClient = authenticationBackGrpcServiceClient;
        _jwtServicesConfig = jwtServicesConfig;
        _logger = logger;
    }

    public Task<TResponse?> ExecuteWithUnaryCallAsync<TResponse>(
       Func<Metadata, AsyncUnaryCall<TResponse>> grpcCall)
       where TResponse : class
    {
        return ExecuteWithServiceTokenAsync(headers => grpcCall(headers).ResponseAsync);
    }

    public async Task<TResponse?> ExecuteWithServiceTokenAsync<TResponse>(
        Func<Metadata, Task<TResponse>> grpcRequest) where TResponse : class
    {
        var jwtConfig = _jwtServicesConfig.ServicesJwtConfigs.FirstOrDefault(x => x.ServiceId == _jwtServicesConfig.CurrentServiceId);
        if (jwtConfig == null)
        {
            _logger.LogError($"JWT configuration for the service '{_jwtServicesConfig.CurrentServiceId}' not found.");
            return null;
        }

        var serviceToken = await _authenticationBackGrpcServiceClient.GetServiceTokenAsync(new ServiceTokenRequest()
        {
            ServiceId = jwtConfig.ServiceId,
            ServiceSecret = jwtConfig.ServiceSecret,
        });

        if (serviceToken == null)
        {
            _logger.LogError("Failed to retrieve service token from AuthenticationBackGrpcService.");
            return null;
        }

        var headers = new Metadata().AddAuthHeader(serviceToken.AccessToken);
        return await grpcRequest(headers);
    }
}
