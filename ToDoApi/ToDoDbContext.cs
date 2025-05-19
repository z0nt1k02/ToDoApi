using Microsoft.EntityFrameworkCore;
using ToDoApi.Contracts;
using ToDoApi.Models;

namespace ToDoApi;

public class ToDoDbContext(DbContextOptions<ToDoDbContext> options) : DbContext(options)
{
    public DbSet<NoteModel> Notes { get; set; }
    public DbSet<UserModel> Users { get; set; }
    
    public DbSet<RefreshTokenModel> RefreshTokens { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfiguration(new RefreshTokenConfiguration());
    }
}