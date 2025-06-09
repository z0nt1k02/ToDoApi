using ToDoApi.Models;

namespace ToDoApi.Contracts;

public interface ITokenProvider
{
    string GenerateToken(UserModel user);
    Task<RefreshTokenModel> GenerateRefreshTokenModel(UserModel user);

    string GenerateRefreshTokenString();
}