using ToDoApi.Models;

namespace ToDoApi.Contracts;

public interface ITokenProvider
{
    string GenerateToken(UserModel user);
}