using Common.GlobalResponse;
using MediatR;

namespace Application.CQRS.PostLikes.Commands.Requests;

public record UnlikePostCommand(int PostId) : IRequest<ResponseModel<string>>;
