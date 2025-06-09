using System.Security.Claims;
using Microsoft.EntityFrameworkCore;

namespace ToDoApi.Infrastructure;

public sealed class RevokeRefreshTokens(ToDoDbContext context,IHttpContextAccessor httpContextAccessor)
{
    public async Task<bool> Handle(Guid userId)
    {
        if (userId != GetCurrentUserId())
        {
            throw new ApplicationException("You cant't do this");
        }
        await context.RefreshTokens
            .Where(r => r.UserId == userId)
            .ExecuteDeleteAsync();
        return true;
    }

    
    private Guid? GetCurrentUserId()
    {
        return Guid.TryParse(
            httpContextAccessor.HttpContext?.User.FindFirstValue(ClaimTypes.NameIdentifier), 
            out Guid parsed) 
            ? parsed :  null;
    }
}