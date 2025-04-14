using Common.Exceptions;
using Common.GlobalResponse;
using FluentValidation;
using Microsoft.AspNetCore.Http;
using System.Net;
using System.Text.Json;
using FluentValidationException = FluentValidation.ValidationException;
using CommonValidationException = Common.Exceptions.ValidationException;

namespace Api.Middleware;

public class ExceptionHandlingMiddleware
{
    private readonly RequestDelegate _next;

    public ExceptionHandlingMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task Invoke(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception ex)
        {
            if (context.Request.Path.StartsWithSegments("/Error"))
                throw;

            await HandleExceptionAsync(context, ex);
        }
    }

    private static async Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        HttpStatusCode statusCode;
        List<string> messages;

        switch (exception)
        {
            case BadRequestException:
                statusCode = HttpStatusCode.BadRequest;
                messages = [exception.Message];
                break;

            case UnauthorizedException:
            case UnauthorizedAccessException:
                statusCode = HttpStatusCode.Unauthorized;
                messages = [exception.Message];
                break;


            case NotFoundException:
                statusCode = HttpStatusCode.NotFound;
                messages = [exception.Message];
                break;

            case ConflictException conflictEx:
                statusCode = HttpStatusCode.Conflict;
                messages = (conflictEx.Errors ?? new List<string> { conflictEx.Message });
                break;


            case ForbiddenException:
                statusCode = HttpStatusCode.Forbidden;
                messages = [exception.Message];
                break;

            case InternalServerException:
                statusCode = HttpStatusCode.InternalServerError;
                messages = [exception.Message];
                break;

            case FluentValidationException validationEx:
                await WriteFluentValidationErrorsAsync(context, validationEx);
                return;

            case CommonValidationException customValidationEx:
                statusCode = HttpStatusCode.BadRequest;
                messages = customValidationEx.Errors;
                break;

            default:
                statusCode = HttpStatusCode.InternalServerError;
                messages = ["An unexpected error occurred."];
                break;
        }

        await WriteErrorAsync(context, statusCode, messages);
    }

    private static async Task WriteErrorAsync(HttpContext context, HttpStatusCode statusCode, List<string> messages)
    {
        context.Response.Clear();
        context.Response.StatusCode = (int)statusCode;
        context.Response.ContentType = "application/json";

        var response = new ResponseModel(messages);

        var options = new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            WriteIndented = false
        };

        var json = JsonSerializer.Serialize(response, options);
        await context.Response.WriteAsync(json);
    }

    private static async Task WriteFluentValidationErrorsAsync(HttpContext context, FluentValidationException ex)
    {
        context.Response.Clear();
        context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
        context.Response.ContentType = "application/json";

        var errors = ex.Errors
            .GroupBy(e => e.PropertyName)
            .ToDictionary(
                g => g.Key,
                g => g.Select(e => e.ErrorMessage).Distinct().ToArray()
            );

        var response = new
        {
            isSuccess = false,
            errors
        };

        var options = new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            WriteIndented = false
        };

        var json = JsonSerializer.Serialize(response, options);
        await context.Response.WriteAsync(json);
    }
}