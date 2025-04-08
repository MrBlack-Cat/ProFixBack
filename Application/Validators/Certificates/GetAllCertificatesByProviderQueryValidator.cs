using Application.CQRS.Certificates.Queries.Requests;
using FluentValidation;

namespace Application.Validators.Certificates;

public class GetAllCertificatesByProviderQueryValidator : AbstractValidator<GetAllCertificatesByServiceProviderQuery>
{
    public GetAllCertificatesByProviderQueryValidator()
    {
        RuleFor(x => x.ServiceProviderProfileId)
            .GreaterThan(0)
            .WithMessage("ServiceProviderProfileId must be greater than 0.");
    }
}
