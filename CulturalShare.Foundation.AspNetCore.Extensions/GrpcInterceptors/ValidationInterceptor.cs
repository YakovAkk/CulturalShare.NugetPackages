using FluentValidation;
using Grpc.Core.Interceptors;
using Grpc.Core;

namespace CulturalShare.Foundation.AspNetCore.Extensions.GrpcInterceptors;

public class ValidationInterceptor : Interceptor
{
    private readonly IServiceProvider _serviceProvider;

    public ValidationInterceptor(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    public override async Task<TResponse> UnaryServerHandler<TRequest, TResponse>(
        TRequest request,
        ServerCallContext context,
        UnaryServerMethod<TRequest, TResponse> continuation)
    {
        var validator = _serviceProvider.GetService(typeof(IValidator<TRequest>)) as IValidator<TRequest>;

        if (validator != null)
        {
            var validationResult = await validator.ValidateAsync(request);

            if (!validationResult.IsValid)
            {
                var errors = string.Join("; ", validationResult.Errors);
                throw new RpcException(new Status(StatusCode.InvalidArgument, errors));
            }
        }

        return await continuation(request, context);
    }
}