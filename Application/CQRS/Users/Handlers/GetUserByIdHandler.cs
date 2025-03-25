using Application.CQRS.Users.DTOs;
using AutoMapper;
using Common.Exceptions;
using Common.GlobalResponse;
using Domain.Entities;
using MediatR;
using Repository.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.CQRS.Users.Handlers;

public class GetUserByIdHandler
{

    public record struct Query : IRequest<ResponseModel<GetUserByIdDto>>
    {
        public int Id { get; set; } 
    }

    public sealed class Handler(IUnitOfWork unitOfWork, IMapper mapper) : IRequestHandler<Query, ResponseModel<GetUserByIdDto>>
    {

        private readonly IUnitOfWork _unitOfWork = unitOfWork; 
        private readonly IMapper _mapper = mapper; 

        public async Task<ResponseModel<GetUserByIdDto>> Handle(Query request, CancellationToken cancellationToken)
        {
            var currentUser = await _unitOfWork.UserRepository.GetByIdAsync(request.Id);
            if (currentUser == null) throw new NotFoundException("User not found");


            var mappedResponse  = _mapper.Map<GetUserByIdDto>(currentUser);

            return new ResponseModel<GetUserByIdDto> { Data = mappedResponse, Errors = [], IsSuccess = true };



        }
    }
}
