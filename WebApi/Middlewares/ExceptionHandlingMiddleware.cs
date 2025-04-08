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

            case ConflictException:
                statusCode = HttpStatusCode.Conflict;
                messages = [exception.Message];
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


//private static async Task HandleExceptionAsync(HttpContext context, Exception exception)
//{
//    //yeni elave 
//    if (context.Response.HasStarted)
//    {
//        Console.WriteLine("Response in fact has started , exception handling does't");
//        return;
//    }


//    context.Response.ContentType = "application/json";
//    var responseModel = new ResponseModel();

//    switch (exception)
//    {
//        case NotFoundException nf:
//            context.Response.StatusCode = (int)HttpStatusCode.NotFound;
//            responseModel.Errors = new List<string> { nf.Message };
//            break;

//        case BadRequestException br:
//            context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
//            responseModel.Errors = br.Errors ?? new List<string> { br.Message };
//            break;

//        case FluentValidation.ValidationException ve:
//            context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
//            responseModel.Errors = ve.Errors.Select(e => $"{e.PropertyName}: {e.ErrorMessage}").ToList();
//            break;

//        case ConflictException cf:
//            context.Response.StatusCode = (int)HttpStatusCode.Conflict;
//            responseModel.Errors = new List<string> { cf.Message };
//            break;

//        case ForbiddenException ff:
//            context.Response.StatusCode = (int)HttpStatusCode.Forbidden;
//            responseModel.Errors = new List<string> { ff.Message };
//            break;

//        case InternalServerException ise:
//            context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
//            responseModel.Errors = new List<string> { ise.Message };
//            break;

//        default:
//            context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
//            responseModel.Errors = new List<string> { "An unexpected error occurred." };
//            break;
//    }


//    //deyishiklik eledim 
//    var json = JsonSerializer.Serialize(responseModel , new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase });

//    await context.Response.WriteAsync(json);
//}