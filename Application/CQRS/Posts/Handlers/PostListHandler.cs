using Application.CQRS.Posts.DTOs;
using AutoMapper;
using Common.GlobalResponse;
using MediatR;
using Repository.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.CQRS.Posts.Handlers;

public class PostListHandler
{

    public record struct Command :IRequest<ResponseModel<List<PostListDto>>>;


    public sealed class Handler(IUnitOfWork unitWork) : IRequestHandler<Command, ResponseModel<List<PostListDto>>>
    {


        private readonly IUnitOfWork _unitWork = unitWork;
        private readonly IMapper _mapper;

        public async Task<ResponseModel<List<PostListDto>>> Handle(Command request, CancellationToken cancellationToken)
        {

            var posts = await _unitWork.PostRepository.GetAllAsync();

            var postListDto = _mapper.Map<List<PostListDto>>(posts);    

            return new ResponseModel<List<PostListDto>>()
            {
                Data = postListDto,
                IsSuccess = true
            };  


        }
    }

}
