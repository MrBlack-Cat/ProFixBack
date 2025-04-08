using Application.CQRS.Certificates.Commands.Requests;
using FluentValidation;

namespace Application.Validators.Certificates;

public class DeleteCertificateCommandValidator : AbstractValidator<DeleteCertificateCommand>
{
    public DeleteCertificateCommandValidator()
    {
        RuleFor(x => x.Id).GreaterThan(0);
        RuleFor(x => x.DeletedByUserId).GreaterThan(0);
        RuleFor(x => x.Reason)
            .NotEmpty().WithMessage("Reason is required.")
            .MaximumLength(1000);
    }
}
