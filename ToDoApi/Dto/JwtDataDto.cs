namespace ToDoApi.Dto;

public record class JwtDataDto(string userId,string token, string lifeTime,string email,string refreshToken);