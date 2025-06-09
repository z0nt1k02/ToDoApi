using Microsoft.EntityFrameworkCore;
using ToDoApi.Models;

namespace ToDoApi.Repositories;

public class RefreshTokenRepository(ToDoDbContext context)
{
    public async Task AddToken(RefreshTokenModel refreshToken)
    {
        await context.RefreshTokens.AddAsync(refreshToken);
        await context.SaveChangesAsync();
    }

    public async Task<RefreshTokenModel?> GetRefreshToken(string Token)
    {
        return await context.RefreshTokens
            .Include(u=>u.User)
            .FirstOrDefaultAsync(u=>u.Token == Token);
    }

    public async Task UpdateToken(RefreshTokenModel refreshToken)
    {
        context.RefreshTokens.Update(refreshToken);
        await context.SaveChangesAsync();
    }
}