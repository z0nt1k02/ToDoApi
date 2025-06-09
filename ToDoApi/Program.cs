using System.Reflection;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using Serilog;
using ToDoApi;
using ToDoApi.Extensions;
using ToDoApi.Middleware;

var builder = WebApplication.CreateBuilder(args);

var services = builder.Services;
var configuration = builder.Configuration;

builder.Host.UseSerilog((context, loggerConfig) =>
{
    loggerConfig.ReadFrom.Configuration(context.Configuration);
});
services.AddDbContext<ToDoDbContext>(options =>
{
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection"));
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
builder.Services.AddHttpContextAccessor();

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();
services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1",new OpenApiInfo{Title = "CoursesPlatform",Version = "v1"});
    var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
    c.IncludeXmlComments(xmlPath);
});
var app = builder.Build();

app.UseCors("MyFront");

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "CoursesPlatform v1");
});
app.UseSerilogRequestLogging();
app.UseMiddleware<GlobalExceptionHandler>();
app.UseMiddleware<GloabalLoggingMiddleware>();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();