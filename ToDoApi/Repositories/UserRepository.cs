using Microsoft.EntityFrameworkCore;
using ToDoApi.Contracts;
using ToDoApi.Models;

namespace ToDoApi.Repositories;

public class UserRepository : IUserRepository
{
    private readonly ToDoDbContext _context;

    public UserRepository(ToDoDbContext context)
    {
        _context = context;
    }
    public async Task AddUser(UserModel user)
    {
        await _context.Users.AddAsync(user);
        await _context.SaveChangesAsync();
    }

    public async Task<UserModel?> GetUser(string email)
    {
       var user = await _context.Users.FirstOrDefaultAsync(u=>u.Email == email);
       return user;
    }
}