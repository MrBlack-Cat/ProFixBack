using Application.CQRS.Certificates.Commands.Requests;
using FluentValidation;

namespace Application.Validators.Certificates;

public class UpdateCertificateCommandValidator : AbstractValidator<UpdateCertificateCommand>
{
    public UpdateCertificateCommandValidator()
    {
        RuleFor(x => x.Id).GreaterThan(0);
        RuleFor(x => x.UpdatedByUserId).GreaterThan(0);

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
