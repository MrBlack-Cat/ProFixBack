using Application.CQRS.Users.Handlers;
using FluentValidation;

namespace Application.Validators.Users;

public class UpdateUserCommandValidator : AbstractValidator<UpdateUserHandler.UpdateCommand>
{
    public UpdateUserCommandValidator()
    {
        RuleFor(x => x.Id).GreaterThan(0);
        RuleFor(x => x.Dto.UserName).NotEmpty().MaximumLength(100);
        RuleFor(x => x.Dto.Email).NotEmpty().EmailAddress();
        RuleFor(x => x.Dto.PhoneNumber).MaximumLength(20);
    }
}

