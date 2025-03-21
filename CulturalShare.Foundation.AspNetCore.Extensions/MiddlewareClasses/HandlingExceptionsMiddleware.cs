using Microsoft.AspNetCore.Http;
using System.Data;
using Grpc.Core;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using FluentValidation;

namespace CulturalShare.Foundation.AspNetCore.Extensions.MiddlewareClasses;

public class HandlingExceptionsMiddleware
{
    public ILogger<HandlingExceptionsMiddleware> _logger { get; }
    public RequestDelegate _next { get; }
    public HandlingExceptionsMiddleware(ILogger<HandlingExceptionsMiddleware> logger, RequestDelegate next)
    {
        _logger = logger;
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next.Invoke(context);
        }
        catch (Exception ex)
        {
            await HandleExceptionAsync(context, ex);
        }
    }

    #region Private

    private static async Task HandleExceptionAsync(HttpContext httpContext, Exception exception)
    {
        var response = new
        {
            Status = GetStatusCode(exception),
            Details = GetError(exception),
            Errors = GetErrors(exception)
        };

        httpContext.Response.ContentType = "application/json";
        httpContext.Response.StatusCode = response.Status;
        await httpContext.Response.WriteAsync(JsonConvert.SerializeObject(response));
    }

    private static string GetError(Exception exception) =>
        exception switch
        {
            RpcException rpc => rpc.Status.Detail,
            ValidationException => "Validation exception has occurred.",
            UnauthorizedAccessException => "Unauthorized access.",
            _ when exception.Message == "An error occurred" && exception.InnerException != null => exception.InnerException.Message,
            _ => exception.Message
        };

    private static int GetStatusCode(Exception exception) =>
        exception switch
        {
            RpcException rpcException => GetStatusCodeFromRpcException(rpcException.StatusCode),
            DuplicateNameException => StatusCodes.Status400BadRequest,
            RowNotInTableException => StatusCodes.Status404NotFound,
            UnauthorizedAccessException => StatusCodes.Status401Unauthorized,
            _ => StatusCodes.Status500InternalServerError
        };

    private static int GetStatusCodeFromRpcException(StatusCode statusCode) =>
        statusCode switch
        {
            StatusCode.InvalidArgument => StatusCodes.Status400BadRequest,
            StatusCode.Internal => StatusCodes.Status500InternalServerError,
            StatusCode.Unauthenticated => StatusCodes.Status401Unauthorized,
            StatusCode.NotFound => StatusCodes.Status404NotFound,
            _ => StatusCodes.Status500InternalServerError
        };

    private static IReadOnlyDictionary<string, string[]> GetErrors(Exception exception)
    {
        IReadOnlyDictionary<string, string[]> errors = null;
        if (exception is ValidationException validationException)
        {
            errors = validationException.Errors.GroupBy(
                x => x.PropertyName,
                x => x.ErrorMessage,
                (propertyName, errorMessages) => new
                {
                    Key = propertyName,
                    Values = errorMessages.Distinct().ToArray()
                })
            .ToDictionary(x => x.Key, x => x.Values);
        }

        return errors;
    }
    #endregion
}