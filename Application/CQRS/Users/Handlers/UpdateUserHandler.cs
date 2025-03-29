using Application.Common.Interfaces;
using Application.CQRS.Users.DTOs;
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

namespace Application.CQRS.Users.Handlers;

public class UpdateUserHandler
{

    public record struct UpdateCommand : IRequest<ResponseModel<UpdateUserDto>>
    {
        public int Id { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public string? PhoneNumber { get; set; }
    }


    public sealed class Handler(IUnitOfWork unitOfWork, IMapper mapper, ILoggerService logger, IActivityLoggerService activityLogger) : IRequestHandler<UpdateCommand, ResponseModel<UpdateUserDto>>
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        private readonly IMapper _mapper = mapper;
        private readonly ILoggerService _logger = logger;
        private readonly IActivityLoggerService _activityLogger = activityLogger;


        public async Task<ResponseModel<UpdateUserDto>> Handle(UpdateCommand request, CancellationToken cancellationToken)
        {

            var currentUser = await _unitOfWork.UserRepository.GetByIdAsync(request.Id);
            if (currentUser == null) { throw new BadRequestException("User does not exist with provided Id"); }

            currentUser.UserName = request.UserName;
            currentUser.Email = request.Email;
            currentUser.PhoneNumber = request.PhoneNumber;

            _unitOfWork.UserRepository.UpdateAsync(currentUser);
            await _unitOfWork.SaveChangesAsync();

            await _activityLogger.LogAsync(
                userId: currentUser.Id,
                action: "Update",
                entityType: "User",
                entityId: currentUser.Id
            );


            var response = _mapper.Map<UpdateUserDto>(currentUser);

            return new ResponseModel<UpdateUserDto> { Data = response, Errors = [], IsSuccess = true };

        }
    }

}

