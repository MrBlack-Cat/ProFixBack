using DAL.SqlServer.Context;
using Domain.Entities.TokenSecurity;
using Microsoft.EntityFrameworkCore;
using Repository.Repositories.TokenSecurity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dal.SqlServer.Infrastructure.TokenSecurity;

public class SqlRefreshTokenRepository(AppDbContext context) : IRefreshTokenRepository
{
    public async Task<RefreshToken> GetStoredRefreshToken(string refreshToken)
    {
        return await context.RefreshTokens.FirstOrDefaultAsync(rt=>rt.Token == refreshToken);   
    }

    public async Task SaveRefreshToken(RefreshToken refreshToken)
    {
        await context.RefreshTokens.AddAsync(refreshToken);
    }
}
