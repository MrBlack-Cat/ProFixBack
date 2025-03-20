//using Application.CQRS.Users.Commands.Requests;
//using FluentValidation;

//namespace Application.Validators.Users;

//public class UpdateUserCommandValidator : AbstractValidator<UpdateUserCommand>
//{
//    public UpdateUserCommandValidator()
//    {
//        RuleFor(x => x.Id)
//            .GreaterThan(0).WithMessage("User ID must be provided");

//        RuleFor(x => x.UserName)
//            .NotEmpty().WithMessage("Username is required")
//            .MinimumLength(3).WithMessage("Username must be at least 3 characters");

//        RuleFor(x => x.Email)
//            .NotEmpty().WithMessage("Email is required")
//            .EmailAddress().WithMessage("Invalid email format");

//        RuleFor(x => x.UserRoleId)
//            .GreaterThan(0).WithMessage("UserRoleId must be specified");
//    }
//}
