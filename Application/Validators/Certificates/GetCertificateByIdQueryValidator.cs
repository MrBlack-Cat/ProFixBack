using Application.CQRS.Certificates.Queries.Requests;
using FluentValidation;

namespace Application.Validators.Certificates;

public class GetCertificateByIdQueryValidator : AbstractValidator<GetCertificateByIdQuery>
{
    public GetCertificateByIdQueryValidator()
    {
        RuleFor(x => x.Id)
            .GreaterThan(0)
            .WithMessage("Certificate ID must be greater than 0.");
    }
}
