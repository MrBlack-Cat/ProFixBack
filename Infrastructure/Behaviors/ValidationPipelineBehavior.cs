//using FluentValidation;
//using MediatR;
//using System.Globalization;

//namespace Infrastructure.Behaviors;

//public class ValidationPipelineBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse> where TRequest : notnull
//{
//    private readonly IEnumerable<IValidator<TRequest>> _validators;

//    public ValidationPipelineBehavior(IEnumerable<IValidator<TRequest>> validators)
//    {
//        _validators = validators;
//        ValidatorOptions.Global.LanguageManager.Culture = new CultureInfo("az");
//    }

//    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
//    {
//        var context = new ValidationContext<TRequest>(request);
//        var validationResults = await Task.WhenAll(_validators.Select(v => v.ValidateAsync(context, cancellationToken)));
//        var failures = validationResults.SelectMany(r => r.Errors).Where(f => f != null).ToList();

//        if (failures.Any())
//            throw new ValidationException(failures);

//        return await next();
//    }
//}



