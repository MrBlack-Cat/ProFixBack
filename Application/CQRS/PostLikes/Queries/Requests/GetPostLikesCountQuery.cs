using MediatR;
using Common.GlobalResponse;

namespace Application.CQRS.PostLikes.Queries.Requests;

public record GetPostLikesCountQuery(int PostId) : IRequest<ResponseModel<int>>;
