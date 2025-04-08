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

    public record UpdateCommand(int Id, int UpdatedByUserId, UpdateUserDto Dto)
        : IRequest<ResponseModel<UpdateUserDto>>;

    public int Id { get; set; }
    public string UserName { get; set; }
    public string Email { get; set; }
    public string? PhoneNumber { get; set; }



    public sealed class Handler(
    IUnitOfWork unitOfWork,
    IMapper mapper,
    ILoggerService logger,
    IActivityLoggerService activityLogger)
    : IRequestHandler<UpdateCommand, ResponseModel<UpdateUserDto>>
    {
        public async Task<ResponseModel<UpdateUserDto>> Handle(UpdateCommand request, CancellationToken cancellationToken)
        {
            var user = await unitOfWork.UserRepository.GetByIdAsync(request.Id);
            if (user == null)
                throw new NotFoundException("User does not exist with provided Id");

            user.UserName = request.Dto.UserName;
            user.Email = request.Dto.Email;
            user.PhoneNumber = request.Dto.PhoneNumber;
            user.UpdatedAt = DateTime.UtcNow;
            user.UpdatedBy = request.UpdatedByUserId;

            await unitOfWork.UserRepository.UpdateAsync(user);
            await unitOfWork.SaveChangesAsync();

            await activityLogger.LogAsync(
                userId: user.Id,
                action: "Update",
                entityType: "User",
                entityId: user.Id,
                performedBy: request.UpdatedByUserId
            );

            var response = mapper.Map<UpdateUserDto>(user);
            return ResponseModel<UpdateUserDto>.Success(response);
        }
    }


}

