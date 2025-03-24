using Application.CQRS.Users.DTOs;
using AutoMapper;
using Common.GlobalResponse;
using MediatR;
using Repository.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.CQRS.Users.Handlers;

public class GetAllUserHandler
{


    public record struct Command : IRequest<ResponseModel<List<GetAllUserDto>>>
    {
    }

    public class GetAllUsersHandler(IUnitOfWork unitOfWork, IMapper mapper) : IRequestHandler<Command, ResponseModel<List<GetAllUserDto>>>
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        private readonly IMapper _mapper = mapper;

        public async Task<ResponseModel<List<GetAllUserDto>>> Handle(Command request, CancellationToken cancellationToken)
        {
            var users = await _unitOfWork.UserRepository.GetAllAsync();

            if (users == null || !users.Any())
            {
                return new ResponseModel<List<GetAllUserDto>>
                {
                    Data = new List<GetAllUserDto>(),
                    Errors = [],
                    IsSuccess = false
                };
            }



            var userDtos = _mapper.Map<List<GetAllUserDto>>(users);

            return new ResponseModel<List<GetAllUserDto>>
            {
                Data = userDtos,
                Errors = [],
                IsSuccess = true
            };
        }
    }

}
