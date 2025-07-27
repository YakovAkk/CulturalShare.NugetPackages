using Grpc.Core;

namespace Service.Helper;

public interface IMicroserviceAuthWrapper
{
    Task<TResponse?> ExecuteWithServiceTokenAsync<TResponse>(Func<Metadata, Task<TResponse>> grpcRequest) where TResponse : class;
    Task<TResponse?> ExecuteWithUnaryCallAsync<TResponse>(Func<Metadata, AsyncUnaryCall<TResponse>> grpcCall) where TResponse : class;
}