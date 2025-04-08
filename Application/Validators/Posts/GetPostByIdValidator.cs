using Application.CQRS.Posts.Queries.Handlers;
using FluentValidation;

namespace Application.Validators.Posts;

public class GetPostByIdValidator : AbstractValidator<GetPostByIdHandler.Command>
{
    public GetPostByIdValidator()
    {
        RuleFor(x => x.Id)
            .GreaterThan(0).WithMessage("Post ID must be greater than 0.");
    }
}
