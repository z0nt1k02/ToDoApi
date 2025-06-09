using ToDoApi.Authentication;
using ToDoApi.Contracts;
using ToDoApi.Infrastructure;
using ToDoApi.Repositories;

namespace ToDoApi.Extensions;

public static class DIExtension
{
    public static void AddCustomServices(this IServiceCollection serviceCollection)
    {
        
        serviceCollection.AddScoped<IUserRepository, UserRepository>();
        serviceCollection.AddScoped<ITokenProvider, TokenProvider>();
        serviceCollection.AddScoped<IPasswordHasher, PasswordHasher>();
        
        serviceCollection.AddScoped<INoteRepository, NoteRepository>();
        serviceCollection.AddScoped<RefreshTokenRepository>();

        serviceCollection.AddScoped<LoginUserWithRefreshToken>();
        serviceCollection.AddScoped<RevokeRefreshTokens>();
    }
}