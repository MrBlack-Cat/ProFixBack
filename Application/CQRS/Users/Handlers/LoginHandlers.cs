using Application.Common.Interfaces;
using Application.CQRS.Users.DTOs;
using Application.Services;
using AutoMapper;
using Common.Exceptions;
using Common.GlobalResponse;
using Domain.Entities;
using Domain.Entities.TokenSecurity;
using MediatR;
using Microsoft.Extensions.Configuration;
using Repository.Common;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Application.CQRS.Users.Handlers;

public class LoginHandlers
{

    public class LoginRequest : IRequest<ResponseModel<LoginResponseDto>>
    {
        public string Email { get; set; } 
        public string Password { get; set; } 
    }



    public sealed class Handler(IUnitOfWork unitOfWork, IConfiguration configuration, ILoggerService loggerService , ITokenService tokenService) : IRequestHandler<LoginRequest, ResponseModel<LoginResponseDto>>
    {

        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        private readonly ITokenService _tokenService = tokenService; 

        public async Task<ResponseModel<LoginResponseDto>> Handle(LoginRequest request, CancellationToken cancellationToken)
        {

            loggerService.LogInfo($"Attempted to log in with the email : {request.Email}");
            User currentUser = await _unitOfWork.UserRepository.GetByEmailAsync(request.Email);
            if(currentUser == null)
            {
                loggerService.LogWarning($" User doesn't exist with this email : {request.Email}");
                throw new BadRequestException("User doesn't exist with provided Email ");
            }

            PasswordHasher passwordHasher = new PasswordHasher();

            var hashedPassword = passwordHasher.HashPassword(request.Password);
            if(hashedPassword != currentUser.PasswordHash)
            {
                loggerService.LogWarning($"The wrong password was entered by the {request.Email}");
                throw new BadRequestException ("Wrong password!");

            }


            //jwt token yaratdiq 
            List<Claim> authClaim = 
                [
                    new Claim(ClaimTypes.NameIdentifier, currentUser.Id.ToString()),
                    new Claim(ClaimTypes.Name , currentUser.UserName),
                    new Claim(ClaimTypes.Email, currentUser.Email),
                    //new Claim(ClaimTypes.MobilePhone, currentUser.PhoneNumber ),
                    new Claim(ClaimTypes.Role, currentUser.Role.Name) 
                ];


            //JwtSecurityToken token  = _tokenService.CreateToken(authClaim , configuration);
            JwtSecurityToken token  = _tokenService.CreateToken(authClaim);
            string tokenString  = new JwtSecurityTokenHandler().WriteToken(token);

            string refreshTokenString  = _tokenService.GenerateRefreshToken();


            RefreshToken refreshToken = new()
            {
                Token = refreshTokenString,
                UserId = currentUser.Id,
                ExpiryDate = DateTime.Now.AddDays(Double.Parse(configuration.GetRequiredSection("JWT:RefreshTokenExpirationDays").Value!))
            };
            
            await _unitOfWork.RefreshTokenRepository.SaveRefreshToken(refreshToken);
            await _unitOfWork.SaveChangesAsync();

            LoginResponseDto response = new()
            {
                AccessToken = tokenString,
                RefreshToken = refreshTokenString
            };

            loggerService.LogInfo($"Logged into the system via email : {request.Email}");

            return new ResponseModel<LoginResponseDto> { Data = response };





        }
    }

}
