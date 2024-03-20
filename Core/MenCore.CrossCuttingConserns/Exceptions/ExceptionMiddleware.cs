using System.Text.Json;
using MenCore.CrossCuttingConserns.Exceptions.Handlers;
using MenCore.CrossCuttingConserns.Logging;
using MenCore.CrossCuttingConserns.Serilog;
using Microsoft.AspNetCore.Http;

namespace MenCore.CrossCuttingConserns.Exceptions;

// HTTP istisnalarını işleyen bir middleware sınıfı
public class ExceptionMiddleware
{
    private readonly RequestDelegate _next; // Bir sonraki middleware bileşeni
    private readonly HttpExceptionHandler _httpExceptionHandler; // HTTP istisnalarını işleyen nesne
    private readonly IHttpContextAccessor _httpContextAccessor; // HTTP isteği bilgilerine erişim sağlayan nesne
    private readonly LoggerServiceBase _loggerServiceBase; // Loglama işlemlerini gerçekleştiren nesne

    // ExceptionMiddleware sınıfının kurucu metodu, RequestDelegate, IHttpContextAccessor ve LoggerServiceBase örnekleri alır
    public ExceptionMiddleware(RequestDelegate next, IHttpContextAccessor httpContextAccessor, LoggerServiceBase loggerServiceBase)
    {
        _next = next; // Bir sonraki middleware bileşeni atanır
        _httpExceptionHandler = new HttpExceptionHandler(); // HTTP istisnalarını işleyen nesne oluşturulur
        _httpContextAccessor = httpContextAccessor; // IHttpContextAccessor atanır
        _loggerServiceBase = loggerServiceBase; // LoggerServiceBase atanır
    }

    // Middleware bileşeninin Invoke metodu, HTTP isteği işler
    public async Task Invoke(HttpContext context)
    {
        try
        {
            await _next(context); // Bir sonraki middleware bileşenini çağırır
        }
        catch (Exception exception)
        {
            await HandleExceptionAsync(context.Response, exception); // Oluşan istisnayı işler
            await LogException(context, exception); // Oluşan istisnayı günlüğe kaydeder
        }
    }

    #region Oluşan istisnayı işleyen yardımcı metot
    private Task HandleExceptionAsync(HttpResponse response, Exception exception)
    {
        response.ContentType = "application/json"; // Yanıt içeriğinin JSON formatında olacağı belirtilir
        _httpExceptionHandler.Response = response; // HTTP yanıt nesnesi atanır
        return _httpExceptionHandler.HandleExceptionAsync(exception); // HttpExceptionHandler aracılığıyla istisna işlenir ve yanıt döndürülür
    }
    #endregion

    #region Oluşan istisnayı günlüğe kaydeden yardımcı metot
    private Task LogException(HttpContext context, Exception exception)
    {
        List<LogParameter> logParameters = new() // LogParameter listesi oluşturulur ve istisna türü ve değeri eklenir
        {
            new LogParameter { Type = context.GetType().Name, Value = exception.ToString() }
        };

        LogDetailWithException logDetail = new() // LogDetail nesnesi oluşturulur ve gerekli alanlar atanır
        {
            MethodName = _next.Method.Name, // Metod adı atanır
            Parameters = logParameters, // Parametreler atanır
            User = _httpContextAccessor.HttpContext.User?.Identity?.Name ?? "?", // Kullanıcı atanır
            ExceptionMessage = exception.Message
        };

        _loggerServiceBase.Error(JsonSerializer.Serialize(logDetail)); // LogDetail nesnesi JSON formatına dönüştürülür ve hata seviyesinde loglanır

        return Task.CompletedTask; // Görev tamamlandı olarak işaretlenir
    }
    #endregion
}
