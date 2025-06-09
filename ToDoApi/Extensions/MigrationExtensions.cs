using Microsoft.EntityFrameworkCore;

namespace ToDoApi.Extensions;

public  static class MigrationExtensions
{
    public static void AddMigrations(this IApplicationBuilder app)
    {
        using IServiceScope scope = app.ApplicationServices.CreateScope();
        
        using ToDoDbContext dbContext = scope.ServiceProvider.GetRequiredService<ToDoDbContext>();

        dbContext.Database.Migrate();
        Console.WriteLine("Migrations applied");
    }
}