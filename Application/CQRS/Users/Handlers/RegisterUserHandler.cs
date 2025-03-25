//using Application.CQRS.Users.DTOs;
//using Application.Services;
//using AutoMapper;
//using Common.Exceptions;
//using Common.GlobalResponse;
//using Domain.Entities;
//using MediatR;
//using Repository.Common;


//namespace Application.CQRS.Users.Handlers;

//public class RegisterUserHandler
//{
//    public record struct RegisterCommand : IRequest<ResponseModel<RegisterUserDto>>
//    {
//        public RegisterCommand(string userName, string email, string password)
//        {
//            UserName = userName;
//            Email = email;
//            Password = password;
//        }

//        public string UserName { get; set; } = null!;
//        public string Email { get; set; } = null!;
//        public string Password { get; set; } = null!;
//    }

//    public sealed class Handler(IUnitOfWork unitOfWork, IMapper mapper) : IRequestHandler<RegisterCommand, ResponseModel<RegisterUserDto>>
//    {


//        private readonly IUnitOfWork _unitOfWork = unitOfWork;
//        private readonly IMapper _mapper = mapper;

//        public async Task<ResponseModel<RegisterUserDto>> Handle(RegisterCommand request, CancellationToken cancellationToken)
//        {

//            var currentUser = await _unitOfWork.UserRepository.GetByEmailAsync(request.Email);
//            if (currentUser != null) { throw new BadRequestException("User already exist with provided Email"); }

//            var user = _mapper.Map<User>(request);

//            PasswordHasher passwordHasher = new PasswordHasher();

//            var hashPassword = passwordHasher.HashPassword(request.Password);
//            user.PasswordHash = hashPassword;
//            await _unitOfWork.UserRepository.RegisterAsync(user);
//            await _unitOfWork.SaveChangesAsync();

//            var response = _mapper.Map<RegisterUserDto>(user);

//            return new ResponseModel<RegisterUserDto> { Data = response, Errors = [], IsSuccess = true };



//        }
//    }


//}



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
    public record struct RegisterCommand(string UserName, string Email, string Password, int RoleId)
        : IRequest<ResponseModel<RegisterUserDto>>;

    public sealed class Handler(IUnitOfWork unitOfWork, IMapper mapper)
        : IRequestHandler<RegisterCommand, ResponseModel<RegisterUserDto>>
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        private readonly IMapper _mapper = mapper;

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
                RoleId = request.RoleId, // 👈 обязательно указываем роль!
                CreatedAt = DateTime.UtcNow
            };

            try
            {
                await _unitOfWork.UserRepository.RegisterAsync(user);
                await _unitOfWork.SaveChangesAsync();

                var responseDto = _mapper.Map<RegisterUserDto>(user);
                return new ResponseModel<RegisterUserDto>
                {
                    Data = responseDto,
                    IsSuccess = true
                };
            }
            catch (Exception ex)
            {
                throw new InternalServerException($"Registration failed: {ex.Message}");
            }
        }
    }


}




