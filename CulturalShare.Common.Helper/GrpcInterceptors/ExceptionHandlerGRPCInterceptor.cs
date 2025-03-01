using Grpc.Core.Interceptors;
using Grpc.Core;
using Microsoft.Extensions.Logging;
using CultureShare.Foundation.Exceptions;

namespace CulturalShare.Common.Helper.GrpcInterceptors;

public class ExceptionHandlerGRPCInterceptor : Interceptor
{
    private readonly ILogger<ExceptionHandlerGRPCInterceptor> _logger;

    public ExceptionHandlerGRPCInterceptor(ILogger<ExceptionHandlerGRPCInterceptor> logger)
    {
        _logger = logger;
    }

    public override async Task<TResponse> UnaryServerHandler<TRequest, TResponse>(TRequest request,
        ServerCallContext context,
        UnaryServerMethod<TRequest, TResponse> continuation)
    {
        try
        {
            var response = await base.UnaryServerHandler(request, context, continuation);
            return response;
        }
        catch (Exception ex)
        {
            var exception = GetProperException(ex);
            throw exception;
        }
    }

    private Exception GetProperException(Exception ex)
    {
        var status = GetStatus(ex);
        var message = ex.Message;

        return new RpcException(new Status(status, message)); ;
    }

    private StatusCode GetStatus(Exception exception)
        => exception switch
        {
            BadRequestException => StatusCode.InvalidArgument,
            _ => StatusCode.Internal
        };
}
