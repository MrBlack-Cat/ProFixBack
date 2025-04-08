//using MediatR;
//using Application.CQRS.Posts.DTOs;
//using System.Collections.Generic;
//using Common.GlobalResponse;

//namespace Application.CQRS.Posts.Queries.Requests;

//public record GetPostsByProviderQuery(int UserId) : IRequest<ResponseModel<List<PostListDto>>>;

using MediatR;
using Application.CQRS.Posts.DTOs;
using Common.GlobalResponse;

namespace Application.CQRS.Posts.Queries.Requests;

public record GetPostsByProviderQuery(int UserId) : IRequest<ResponseModel<List<PostListDto>>>;
