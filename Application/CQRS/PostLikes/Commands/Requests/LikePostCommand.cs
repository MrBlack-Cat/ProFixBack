using MediatR;
using Common.GlobalResponse;

namespace Application.CQRS.PostLikes.Commands.Requests;

public record LikePostCommand(int PostId) : IRequest<ResponseModel<string>>;
