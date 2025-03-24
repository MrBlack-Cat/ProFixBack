using Domain.Entities.TokenSecurity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.Repositories.TokenSecurity;

public interface IRefreshTokenRepository
{
    Task<RefreshToken>GetStoredRefreshToken(string refreshToken);   
    Task SaveRefreshToken(RefreshToken refreshToken);   
}
