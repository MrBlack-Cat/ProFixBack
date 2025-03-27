using Application.Common.Interfaces;
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

public class DeletePostHandler
{
    public record struct Command : IRequest<ResponseModel<DeletePostDto>>
    {
        public int Id { get; set; }
        public int DeletedBy { get; set; }
        public string DeletedReason { get; set; }
    }


    public sealed class Handler(IUnitOfWork unitWork, IUserContext userContext, IMapper mapper) : IRequestHandler<Command, ResponseModel<DeletePostDto>>
    {
        private readonly IUnitOfWork _unitWork = unitWork;
        private readonly IUserContext _userContext = userContext;
        private readonly IMapper _mapper = mapper;



        public async Task<ResponseModel<DeletePostDto>> Handle(Command request, CancellationToken cancellationToken)
        {

            var currentUserId = _userContext.GetCurrentUserId();
            if (!currentUserId.HasValue) throw new UnauthorizedAccessException("User is not authenticated.");

            var userRole = _userContext.GetUserRole();
            if (userRole != "Admin") throw new ForbiddenException("Only Admins can delete posts.");

            if (string.IsNullOrWhiteSpace(request.DeletedReason)) throw new BadRequestException("Delete reason is required.");

            var post = await _unitWork.PostRepository.GetByIdAsync(request.Id);
            if (post == null) throw new BadRequestException("Post does not exist with the provided ID.");

            //command i posta ceviririk 
            _mapper.Map(request, post);
            post.DeletedBy = request.DeletedBy;


            await _unitWork.PostRepository.DeleteAsync(post);
            await _unitWork.SaveChangesAsync();

            return new ResponseModel<DeletePostDto>
            {
                Data = _mapper.Map<DeletePostDto>(post),
                IsSuccess = true
            };
        }
    }

    
}

