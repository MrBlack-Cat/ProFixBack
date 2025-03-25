using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Application.Common.Interfaces
{
    public interface ITokenService
    {
        //JwtSecurityToken CreateToken (List<Claim> authClaims, IConfiguration configuration);

        JwtSecurityToken CreateToken(List<Claim> authClaims);

        public string GenerateRefreshToken();
        

        //string GenerateToken(string userId, string role);
    }
}
