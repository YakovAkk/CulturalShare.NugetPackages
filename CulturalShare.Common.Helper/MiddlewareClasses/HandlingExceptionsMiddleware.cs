using Microsoft.AspNetCore.Http;
using System.Data;
using System.Text.Json;
using FluentValidation;
using CulturalShare.Common.Helper.Model;

namespace CulturalShare.Common.Helper.MiddlewareClasses;

public class HandlingExceptionsMiddleware
{
    public RequestDelegate _next { get; }
    public HandlingExceptionsMiddleware(RequestDelegate next)
    {
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
        var response = new ErrorViewModel()
        {
            Status = statusCode,
            Error = GetError(exception),
            ValidationErrors = GetErrors(exception)
        };
        httpContext.Response.ContentType = "application/json";
        httpContext.Response.StatusCode = statusCode;
        await httpContext.Response.WriteAsync(JsonSerializer.Serialize(response));
    }

    private static string GetError(Exception exception)
    {
        if (exception is ValidationException)
            return "Validation exception has occured.";

        if (exception.Message == "An error occurred" && exception.InnerException != null)
            return exception.InnerException.Message;

        return exception.Message;
    }

    private static int GetStatusCode(Exception exception) =>
        exception switch
        {
            DuplicateNameException => StatusCodes.Status400BadRequest,
            RowNotInTableException => StatusCodes.Status404NotFound,
            ValidationException => StatusCodes.Status400BadRequest,
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
}
