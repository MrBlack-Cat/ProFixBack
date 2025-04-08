using Application.CQRS.Certificates.Commands.Requests;
using FluentValidation;

namespace Application.Validators.Certificates;

public class CreateCertificateCommandValidator : AbstractValidator<CreateCertificateCommand>
{
    public CreateCertificateCommandValidator()
    {
        RuleFor(x => x.CreatedByUserId).GreaterThan(0);

        RuleFor(x => x.Dto.Title)
            .NotEmpty().WithMessage("Title is required.")
            .MaximumLength(200);

        RuleFor(x => x.Dto.Description)
            .MaximumLength(1000);

        RuleFor(x => x.Dto.FileUrl)
            .NotEmpty().WithMessage("FileUrl is required.")
            .MaximumLength(500);

        RuleFor(x => x.Dto.IssuedAt)
            .NotEmpty().WithMessage("IssuedAt is required.");
    }
}
