using Application.Common.Interfaces;
using Application.CQRS.Users.DTOs;
using Application.Services;
using AutoMapper;
using Common.Exceptions;
using Common.GlobalResponse;
using Domain.Entities;
using MediatR;
using Microsoft.Extensions.Logging;
using Repository.Common;

namespace Application.CQRS.Users.Handlers;

public class RegisterUserHandler
{
    public record struct RegisterCommand(string UserName, string Email, string Password, int RoleId)
        : IRequest<ResponseModel<RegisterUserDto>>;

    public sealed class Handler(IUnitOfWork unitOfWork, IMapper mapper, ILoggerService logger, IActivityLoggerService activityLogger)
        : IRequestHandler<RegisterCommand, ResponseModel<RegisterUserDto>>
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        private readonly IMapper _mapper = mapper;
        private readonly ILoggerService _logger = logger;
        private readonly IActivityLoggerService _activityLogger = activityLogger;

        public async Task<ResponseModel<RegisterUserDto>> Handle(RegisterCommand request, CancellationToken cancellationToken)
        {
            var existingUser = await _unitOfWork.UserRepository.GetByEmailAsync(request.Email.ToLower());
            if (existingUser is not null)
                throw new BadRequestException("User already exists with provided Email");

            var user = new User
            {
                UserName = request.UserName,
                Email = request.Email.ToLower(),
                PasswordHash = new PasswordHasher().HashPassword(request.Password),
                RoleId = request.RoleId,
                CreatedAt = DateTime.UtcNow
            };

            try
            {
                await _unitOfWork.UserRepository.RegisterAsync(user);
                await _unitOfWork.SaveChangesAsync();

                #region ActivityLog


                await _activityLogger.LogAsync(
                   userId: user.Id,
                   action: "Register",
                   entityType: "User",
                   entityId: user.Id
               );

                _logger.LogInfo($"New user registered: {user.Email}");

                #endregion

                var responseDto = _mapper.Map<RegisterUserDto>(user);
                return new ResponseModel<RegisterUserDto>
                {
                    Data = responseDto,
                    IsSuccess = true
                };
            }
            catch (Exception ex)
            {

                return new ResponseModel<RegisterUserDto>
                {
                    Errors = new List<string> { $"Registration failed: {ex.Message}" },
                    IsSuccess = false
                };
                //throw new InternalServerException($"Registration failed: {ex.Message}");
            }
        }
    }


}



