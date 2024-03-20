using System.Text.Json;
using MediatR;
using MenCore.CrossCuttingConserns.Logging;
using MenCore.CrossCuttingConserns.Serilog;
using Microsoft.AspNetCore.Http;

namespace MenCore.Application.Pipelines.Logging;

// MediatR ile birlikte loglama davranışını uygulayan sınıf
public class LoggingBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse> where TRequest : IRequest<TResponse>, ILoggableRequest
{
    private readonly IHttpContextAccessor _httpContextAccessor; // HTTP isteğine erişim sağlayan nesne
    private readonly LoggerServiceBase _loggerServiceBase; // LoggerServiceBase türünden bir nesne

    // IHttpContextAccessor ve LoggerServiceBase örneklerini alan kurucu metot
    public LoggingBehavior(IHttpContextAccessor httpContextAccessor, LoggerServiceBase loggerServiceBase)
    {
        _httpContextAccessor = httpContextAccessor; // IHttpContextAccessor atanır
        _loggerServiceBase = loggerServiceBase; // LoggerServiceBase atanır
    }

    // İsteği işleyen metot
    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        // LogParameter listesi oluşturulur ve istek türü ile isteğin kendisi eklenir
        List<LogParameter> logParameters = new()
        {
            new LogParameter { Type = request.GetType().Name, Value = request }
        };

        // LogDetail nesnesi oluşturulur ve gerekli alanlar atanır
        LogDetail logDetail = new()
        {
            MethodName = next.Method.Name, // Metod adı atanır
            User = _httpContextAccessor.HttpContext.User.Identity?.Name ?? "?", // Kullanıcı atanır
            Parameters = logParameters, // Parametreler atanır
            FullName = string.Format("{0}_{1}", arg0: _httpContextAccessor.HttpContext?.Connection?.RemoteIpAddress, arg1: _httpContextAccessor.HttpContext.User.Identity?.Name ?? "?") // Tam isim atanır
        };

        // LogDetail nesnesi JSON formatına dönüştürülür ve bilgi seviyesinde loglanır
        _loggerServiceBase.Info(JsonSerializer.Serialize(logDetail));

        return await next(); // Bir sonraki işleme devam edilir
    }
}