using FluentValidation;
using MediatR;
using Microsoft.Extensions.Options;
using System.Globalization;
using Common.Options;

namespace Application.Common.Behaviors;

public class LocalizedValidationBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    where TRequest : IRequest<TResponse>
{
    private readonly IEnumerable<IValidator<TRequest>> _validators;

    public LocalizedValidationBehavior(IEnumerable<IValidator<TRequest>> validators, IOptions<ValidationOptions> options)
    {
        _validators = validators;

        var culture = new CultureInfo(options.Value.Culture ?? "en");
        ValidatorOptions.Global.LanguageManager.Culture = culture;
    }

    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        if (!_validators.Any())
            return await next();

        var context = new ValidationContext<TRequest>(request);
        var validationResults = await Task.WhenAll(_validators.Select(v => v.ValidateAsync(context, cancellationToken)));
        var failures = validationResults.SelectMany(r => r.Errors).Where(f => f != null).ToList();

        if (failures.Count > 0)
            throw new ValidationException(failures);

        return await next();
    }
}
