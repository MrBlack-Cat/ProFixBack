using Common.GlobalResponse;
using MediatR;
using Application.CQRS.Posts.DTOs;

namespace Application.CQRS.Posts.Queries.Requests
{
    public class GetPostsByLikedQuery : IRequest<ResponseModel<List<PostDto>>>
    {
    }
}
