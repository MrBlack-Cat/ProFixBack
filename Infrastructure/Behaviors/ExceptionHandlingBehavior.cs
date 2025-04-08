using Common.Exceptions;
using Common.GlobalResponse;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Infrastructure.Behaviors;

public class ExceptionHandlingBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    where TRequest : IRequest<TResponse>
{
    private readonly ILogger<ExceptionHandlingBehavior<TRequest, TResponse>> _logger;

    public ExceptionHandlingBehavior(ILogger<ExceptionHandlingBehavior<TRequest, TResponse>> logger)
    {
        _logger = logger;
    }

    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        try
        {
            return await next();
        }
        catch (BadRequestException ex)
        {
            _logger.LogWarning(ex, $"BadRequest: {ex.Message}");
            return CreateErrorResponse<TResponse>(ex.Errors ?? [ex.Message]);
        }
        catch (ValidationException ex)
        {
            _logger.LogWarning(ex, $"Validation failed: {string.Join(", ", ex.Errors)}");
            return CreateErrorResponse<TResponse>(ex.Errors);
        }
        catch (NotFoundException ex)
        {
            _logger.LogWarning(ex, $"NotFound: {ex.Message}");
            return CreateErrorResponse<TResponse>([ex.Message]);
        }
        catch (UnauthorizedException ex)
        {
            _logger.LogWarning(ex, $"Unauthorized: {ex.Message}");
            return CreateErrorResponse<TResponse>([ex.Message]);
        }
        catch (ForbiddenException ex)
        {
            _logger.LogWarning(ex, $"Forbidden: {ex.Message}");
            return CreateErrorResponse<TResponse>([ex.Message]);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Unhandled exception in {typeof(TRequest).Name}");
            return CreateErrorResponse<TResponse>(["An unexpected error occurred."]);
        }
    }

    private static TResp CreateErrorResponse<TResp>(List<string> errors)
    {
        var responseType = typeof(TResp);

        // ResponseModel<T>
        if (responseType.IsGenericType && responseType.GetGenericTypeDefinition() == typeof(ResponseModel<>))
        {
            var genericType = responseType.GetGenericArguments()[0];
            var instance = Activator.CreateInstance(typeof(ResponseModel<>).MakeGenericType(genericType));

            if (instance is not null)
            {
                var isSuccessProp = responseType.GetProperty("IsSuccess");
                var errorsProp = responseType.GetProperty("Errors");

                isSuccessProp?.SetValue(instance, false);
                errorsProp?.SetValue(instance, errors);

                return (TResp)instance;
            }
        }

        // ResponseModel
        if (typeof(ResponseModel).IsAssignableFrom(responseType))
        {
            var response = Activator.CreateInstance(responseType) as ResponseModel;

            if (response != null)
            {
                response.IsSuccess = false;
                response.Errors = errors;
                return (TResp)(object)response;
            }
        }

        throw new InvalidOperationException($"TResponse type {responseType.Name} must be ResponseModel or ResponseModel<T>.");
    }
}
