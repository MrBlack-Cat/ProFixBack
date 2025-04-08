using MediatR;
using Common.GlobalResponse;

namespace Application.CQRS.Posts.Commands.Requests;

public record DeletePostCommand(int PostId, int DeletedByUserId, string Reason) : IRequest<ResponseModel<string>>;
