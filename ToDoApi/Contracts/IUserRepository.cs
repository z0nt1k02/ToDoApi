using ToDoApi.Models;

namespace ToDoApi.Contracts;

public interface IUserRepository
{
    Task AddUser(UserModel user);
    Task<UserModel?> GetUser(string email);
}