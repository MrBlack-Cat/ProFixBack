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

public class UpdatePostHandler
{

    public record struct Command  ( UpdatePostDto PostDto):IRequest<ResponseModel<UpdatePostDto>>;

    public sealed class Handler(IUnitOfWork unitWork, IMapper mapper) : IRequestHandler<Command, ResponseModel<UpdatePostDto>>
    {

        private readonly IUnitOfWork _unitWork = unitWork;
        private readonly IMapper _mapper = mapper;

        public async Task<ResponseModel<UpdatePostDto>> Handle(Command request, CancellationToken cancellationToken)
        {

            var post = await _unitWork.PostRepository.GetByIdAsync(request.PostDto.Id); 

            if(post == null) throw new NotFoundException("Post does not exist with the provided ID.");

            _mapper.Map(request.PostDto, post); 
            post.UpdatedAt = DateTime.UtcNow;

            await _unitWork.PostRepository.UpdateAsync(post);
            await _unitWork.SaveChangesAsync(); 


            var response = _mapper.Map<UpdatePostDto>(post);

            return new ResponseModel<UpdatePostDto>
            {
                Data = response,
                IsSuccess = true
            };


        }
    }


}
