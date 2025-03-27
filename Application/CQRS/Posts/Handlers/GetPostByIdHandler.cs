using Application.CQRS.Posts.DTOs;
using AutoMapper;
using Common.Exceptions;
using Common.GlobalResponse;
using MediatR;
using Repository.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.CQRS.Posts.Handlers;

public class GetPostByIdHandler
{

    public record struct Command (int Id) : IRequest<ResponseModel<GetPostByIdDto>>;


    public sealed class Handler(IUnitOfWork unitWork, IMapper mapper) : IRequestHandler<Command, ResponseModel<GetPostByIdDto>>
    {
        private readonly IUnitOfWork _unitWork = unitWork;
        private readonly IMapper _mapper = mapper;

        public async Task<ResponseModel<GetPostByIdDto>> Handle(Command request, CancellationToken cancellationToken)
        {

            var post  = await _unitWork.PostRepository.GetByIdAsync(request.Id);
            
            if (post == null) throw new BadRequestException("Post does not exist with the provided ID.");
            
            var postDto = _mapper.Map<GetPostByIdDto>(post);
            return new ResponseModel<GetPostByIdDto>()
            {
                Data = postDto,
                IsSuccess = true
            };



        }
    }

}
