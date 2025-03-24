using Application.CQRS.Users.DTOs;
using Application.Services;
using AutoMapper;
using Common.Exceptions;
using Common.GlobalResponse;
using Domain.Entities;
using MediatR;
using Repository.Common;


namespace Application.CQRS.Users.Handlers;

public class RegisterUserHandler
{
    public record struct RegisterCommand : IRequest<ResponseModel<RegisterUserDto>>
    {
        public RegisterCommand(string userName, string email, string password, string? phoneNumber)
        {
            UserName = userName;
            Email = email;
            Password = password;
            PhoneNumber = phoneNumber;
        }

        public string UserName { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string Password { get; set; } = null!;
        public string? PhoneNumber { get; set; }
    }

        public sealed class Handler(IUnitOfWork unitOfWork, IMapper mapper) : IRequestHandler<RegisterCommand, ResponseModel<RegisterUserDto>>
        {


            private readonly IUnitOfWork _unitOfWork = unitOfWork;
            private readonly IMapper _mapper = mapper;

            public async  Task<ResponseModel<RegisterUserDto>> Handle(RegisterCommand request, CancellationToken cancellationToken)
            {

                var currentUser = _unitOfWork.UserRepository.GetByEmailAsync(request.Email);
                if(currentUser != null) { throw new BadRequestException("User already exist with provided Email"); }

                var user  = _mapper.Map<User>(request); 

                PasswordHasher passwordHasher = new PasswordHasher();

                var hashPassword = passwordHasher.HashPassword(request.Password);
                user.PasswordHash = hashPassword;
                user.CreatedBy = 1; 
                await _unitOfWork.UserRepository.RegisterAsync(user);
                await _unitOfWork.SaveChangesAsync();

                var response = _mapper.Map<RegisterUserDto>(user);  

                return new ResponseModel<RegisterUserDto> { Data = response, Errors = [], IsSuccess = true };   



            }
        }
    
}





