namespace ToDoApi.Middleware;

public class GloabalLoggingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<GloabalLoggingMiddleware> _logger;

    public GloabalLoggingMiddleware(RequestDelegate next, ILogger<GloabalLoggingMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task Invoke(HttpContext httpContext)
    {
        await _next(httpContext);
        if (httpContext.Response.StatusCode > 199 && httpContext.Response.StatusCode < 300)
        {
            _logger.LogInformation($"Успешно Запрос: {httpContext.Request.Method}, {httpContext.Request.Path}.Ответ: Код:{httpContext.Response.StatusCode},тело{httpContext.Response.Body}");
        }
        else if (httpContext.Response.StatusCode >= 400 && httpContext.Response.StatusCode < 599)
        {
            _logger.LogError($"Ошибка: Запрос: {httpContext.Request.Method} {httpContext.Request.Path}.Ответ: Код:{httpContext.Response.StatusCode},тело{httpContext.Response.Body}");
        }
        
    }
}