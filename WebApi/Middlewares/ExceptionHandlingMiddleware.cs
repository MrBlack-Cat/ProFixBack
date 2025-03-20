using Common.Exceptions;
using Common.GlobalResponse;
using FluentValidation;
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
            catch (Exception ex)
            {
                await HandleExceptionAsync(context, ex);
            }
        }

        private static async Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            context.Response.ContentType = "application/json";
            var responseModel = new ResponseModel();

            switch (exception)
            {
                case NotFoundException nf:
                    context.Response.StatusCode = (int)HttpStatusCode.NotFound;
                    responseModel.Errors = new List<string> { nf.Message };
                    break;

                case BadRequestException br:
                    context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
                    responseModel.Errors = br.Errors ?? new List<string> { br.Message };
                    break;

                case FluentValidation.ValidationException ve:
                    context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
                    responseModel.Errors = ve.Errors.Select(e => $"{e.PropertyName}: {e.ErrorMessage}").ToList();
                    break;

                case ConflictException cf:
                    context.Response.StatusCode = (int)HttpStatusCode.Conflict;
                    responseModel.Errors = new List<string> { cf.Message };
                    break;

                case ForbiddenException ff:
                    context.Response.StatusCode = (int)HttpStatusCode.Forbidden;
                    responseModel.Errors = new List<string> { ff.Message };
                    break;

                case InternalServerException ise:
                    context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                    responseModel.Errors = new List<string> { ise.Message };
                    break;

                default:
                    context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                    responseModel.Errors = new List<string> { "An unexpected error occurred." };
                    break;
            }

            var json = JsonSerializer.Serialize(responseModel);
            await context.Response.WriteAsync(json);
        }
    }
}
