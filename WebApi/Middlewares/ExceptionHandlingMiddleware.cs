
using Common.Exceptions;
using Common.GlobalResponse;
using System.Net;
using System.Text.Json;

namespace Api.Middleware
{
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
            catch (Exception error)
            {
                if (context.Request.Path.StartsWithSegments("/Error"))
                {
                    throw;
                }

                var message = new List<string> { error.Message };

                switch (error)
                {

                    case BadRequestException:
                        await WriteError(context, HttpStatusCode.BadRequest, message);
                        break;

                    case NotFoundException:
                        await WriteError(context, HttpStatusCode.NotFound, message);
                        break;

                    case FluentValidation.ValidationException ex :
                        await WriteValidationErrors(context, HttpStatusCode.BadRequest, ex);
                        break;

                    case ConflictException:
                        await WriteError(context, HttpStatusCode.Conflict, message);
                        break;

                    case ForbiddenException:
                        await WriteError(context, HttpStatusCode.Forbidden, message);
                        break;

                    case InternalServerException ise:
                        await WriteError(context, HttpStatusCode.InternalServerError, message);
                        break;

                    default:
                        await WriteError(context, HttpStatusCode.BadRequest, message);
                        break;
                }

            }
        }

        private static async Task WriteError(HttpContext context, HttpStatusCode statusCode, List<string> messages)
        {
            context.Response.Clear();
            context.Response.StatusCode = (int)statusCode;
            context.Response.ContentType = "application/json";

            var json = JsonSerializer.Serialize(new ResponseModel(messages));
            await context.Response.WriteAsync(json);
        }

      

        private static async Task WriteValidationErrors(HttpContext context, HttpStatusCode statusCode,FluentValidation. ValidationException ex)
        {
            context.Response.Clear();
            context.Response.StatusCode = (int)statusCode;
            context.Response.ContentType = "application/json";

            var validationErrors = ex.Errors.Select(e => new { field = e.PropertyName, message = e.ErrorMessage });
            var json = JsonSerializer.Serialize(new { errors = validationErrors });

            await context.Response.WriteAsync(json);
        }


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