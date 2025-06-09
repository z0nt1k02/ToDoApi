
using ToDoApi.Contracts;
using ToDoApi.Dto;
using ToDoApi.Repositories;
using RefreshTokenModel = ToDoApi.Models.RefreshTokenModel;

namespace ToDoApi.Infrastructure;

public class LoginUserWithRefreshToken(ITokenProvider tokenProvider,RefreshTokenRepository tokenRepository)
{
    
    public sealed record Response(string AccessToken, string RefreshToken);

    public async Task<Response> Handle(RefreshTokenAuthDto dto)
    {
        RefreshTokenModel? refreshToken = await tokenRepository.GetRefreshToken(dto.RefreshToken);

        if (refreshToken is null || refreshToken.ExpiresOnUtc < DateTime.UtcNow)
            throw new ApplicationException("The refresh token has expired");

        string accessToken = tokenProvider.GenerateToken(refreshToken.User!);
        
        refreshToken.Token = tokenProvider.GenerateRefreshTokenString();
        refreshToken.ExpiresOnUtc = DateTime.UtcNow.AddDays(3);
        await tokenRepository.UpdateToken(refreshToken);
        return new Response(accessToken, refreshToken.Token);
    }
}