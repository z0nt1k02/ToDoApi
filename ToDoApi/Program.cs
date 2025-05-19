using Microsoft.EntityFrameworkCore;
using ToDoApi;
using ToDoApi.Extensions;

var builder = WebApplication.CreateBuilder(args);

var services = builder.Services;
var configuration = builder.Configuration;
services.AddDbContext<ToDoDbContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
});
services.AddApiAutnentication(configuration);
services.AddCors(options =>
{
    options.AddPolicy("MyFront", policy =>
    {
        policy.WithOrigins("http://localhost:5173")
            .AllowAnyMethod()
            .AllowAnyHeader()
            .AllowCredentials();

    });
});

builder.Services.AddCustomServices();

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();
var app = builder.Build();

app.UseCors("MyFront");

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();