using Application.Common.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Application.Services
{
    public class TokenService : ITokenService
    {
        private readonly IConfiguration _configuration;

        public TokenService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        //yeni elave eledim 
        public JwtSecurityToken CreateToken(List<Claim> authClaims , IConfiguration configuration)
        {
            var authSignInKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["Jwt:Secret"]!));

            var token = new JwtSecurityToken(
                issuer: configuration["Jwt:ValidIssuer"],
                audience: configuration["Jwt:ValidAudience"],
                claims: authClaims,
                expires: DateTime.Now.AddMinutes(Double.Parse(configuration.GetRequiredSection("JWT:AccessTokenExpirationMinutes").Value!)),
                signingCredentials: new SigningCredentials(authSignInKey, SecurityAlgorithms.HmacSha256
                ));
            return token;   
        }

        public string GenerateRefreshToken()
        {
            var randomNumber = new byte[32];
            using var rng = System.Security.Cryptography.RandomNumberGenerator.Create();
            rng.GetBytes(randomNumber);
            return Convert.ToBase64String(randomNumber);
        }
    }
}


        //public string GenerateToken(string userId, string role)
        //{
        //    var claims = new[]
        //    {
        //        new Claim(ClaimTypes.NameIdentifier, userId),
        //        new Claim(ClaimTypes.Role, role)
        //    };

        //    var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
        //    var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        //    var token = new JwtSecurityToken(
        //        issuer: _configuration["Jwt:Issuer"],
        //        audience: _configuration["Jwt:Audience"],
        //        claims: claims,
        //        expires: DateTime.UtcNow.AddHours(2),
        //        signingCredentials: creds);

        //    return new JwtSecurityTokenHandler().WriteToken(token);
        //}