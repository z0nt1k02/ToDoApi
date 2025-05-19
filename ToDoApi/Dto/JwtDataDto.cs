namespace ToDoApi.Dto;

public record class JwtDataDto(string token, string lifeTime,string email);