using Microsoft.AspNetCore.Http;
using System.Data;
using Grpc.Core;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using FluentValidation;

namespace CulturalShare.Common.Helper.MiddlewareClasses;

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

    private static async Task HandleExceptionAsync(HttpContext httpContext, Exception exception)
    {
        var statusCode = GetStatusCode(exception);
        var response = new
        {
            status = statusCode,
            details = GetError(exception),
            errors = GetErrors(exception)
        };
        httpContext.Response.ContentType = "application/json";
        httpContext.Response.StatusCode = statusCode;
        await httpContext.Response.WriteAsync(JsonConvert.SerializeObject(response));
    }

    #region Private
    private static string GetError(Exception exception) =>
        exception switch
        {
            RpcException rpc => rpc.Status.Detail,
            ValidationException => "Validation exception has occurred.",
            _ when exception.Message == "An error occurred" && exception.InnerException != null => exception.InnerException.Message,
            _ => exception.Message
        };

    private static int GetStatusCode(Exception exception) =>
        exception switch
        {
            RpcException rpcException => GetStatusCodeFromRpcException(rpcException.StatusCode),
            DuplicateNameException => StatusCodes.Status400BadRequest,
            RowNotInTableException => StatusCodes.Status404NotFound,
            _ => StatusCodes.Status500InternalServerError
        };

    private static int GetStatusCodeFromRpcException(StatusCode statusCode) =>
        statusCode switch
        {
            StatusCode.InvalidArgument => StatusCodes.Status400BadRequest,
            StatusCode.Internal => StatusCodes.Status500InternalServerError,
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