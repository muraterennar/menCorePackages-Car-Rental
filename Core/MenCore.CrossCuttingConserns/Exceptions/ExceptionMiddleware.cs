using MenCore.CrossCuttingConserns.Exceptions.Handlers;
using Microsoft.AspNetCore.Http;

namespace MenCore.CrossCuttingConserns.Exceptions;

public class ExceptionMiddleware
{
    private readonly RequestDelegate _next; // Bir sonraki middleware bileşeni
    private readonly HttpExceptionHandler _httpExceptionHandler; // HTTP istisnalarını işleyen nesne

    // ExceptionMiddleware sınıfının kurucu metodu, RequestDelegate örneği alır
    public ExceptionMiddleware (RequestDelegate next)
    {
        _next = next; // Bir sonraki middleware bileşeni atanır
        _httpExceptionHandler = new HttpExceptionHandler(); // HttpExceptionHandler örneği oluşturulur
    }

    #region Middleware bileşeninin Invoke metodu, HTTP isteği işler
    public async Task Invoke (HttpContext context)
    {
        try
        {
            await _next(context); // Bir sonraki middleware bileşenini çağırır
        }
        catch (Exception exception)
        {
            await HandleExceptionAsync(context.Response, exception); // Oluşan istisnayı işler
        }
    }
    #endregion

    #region Oluşan istisnayı işleyen yardımcı metot
    private Task HandleExceptionAsync (HttpResponse response, Exception exception)
    {
        response.ContentType = "application/json"; // Yanıt içeriğinin JSON formatında olacağı belirtilir
        _httpExceptionHandler.Response = response; // HTTP yanıt nesnesi atanır
        return _httpExceptionHandler.HandleExceptionAsync(exception); // HttpExceptionHandler aracılığıyla istisna işlenir ve yanıt döndürülür
    }
    #endregion
}
