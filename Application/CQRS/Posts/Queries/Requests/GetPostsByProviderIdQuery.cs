using Common.GlobalResponse;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.CQRS.Posts.DTOs;


namespace Application.CQRS.Posts.Queries.Requests;

public record GetPostsByProviderIdQuery(int ServiceProviderProfileId) : IRequest<ResponseModel<List<PostDto>>>;
