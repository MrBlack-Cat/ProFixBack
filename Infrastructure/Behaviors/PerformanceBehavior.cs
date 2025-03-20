using MediatR;
using Microsoft.Extensions.Logging;
using System.Diagnostics;

namespace Infrastructure.Behaviors;

public class PerformanceBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    where TRequest : IRequest<TResponse>
{
    private readonly ILogger<PerformanceBehavior<TRequest, TResponse>> _logger;

    public PerformanceBehavior(ILogger<PerformanceBehavior<TRequest, TResponse>> logger)
    {
        _logger = logger;
    }

    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        var stopwatch = Stopwatch.StartNew();
        var response = await next();
        stopwatch.Stop();

        var elapsed = stopwatch.ElapsedMilliseconds;
        if (elapsed > 500) 
        {
            _logger.LogWarning($"[PerformanceBehavior] {typeof(TRequest).Name} took {elapsed} ms.");
        }

        return response;
    }
}
