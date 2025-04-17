using Application.CQRS.Posts.DTOs;
using Common.GlobalResponse;
using MediatR;

namespace Application.CQRS.Posts.Queries.Requests;

public record GetPostsListQuery : IRequest<ResponseModel<List<PostDto>>>;
