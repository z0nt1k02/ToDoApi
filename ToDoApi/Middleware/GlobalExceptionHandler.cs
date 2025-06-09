using System.Text.Json;
namespace ToDoApi.Middleware;

public class GlobalExceptionHandler 
{
    private readonly RequestDelegate _next;
    private readonly ILogger<GlobalExceptionHandler> _logger;

    public GlobalExceptionHandler(RequestDelegate next,ILogger<GlobalExceptionHandler> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task Invoke(HttpContext context)
    {
        try
        {
            await _next(context); // Пропускаем запрос дальше по конвейеру
        }
        catch (Exception ex)
        {
            _logger.LogCritical($"Критическая ошибка: {ex.Message}. Запрос: {context.Request.Method}, {context.Request.Path}");
            context.Response.StatusCode = 500;
            context.Response.ContentType = "application/json";
            var errorResponse = new ServerException("Упс произошла ошибка");
            await context.Response.WriteAsync(JsonSerializer.Serialize(errorResponse));
        }
    }
}