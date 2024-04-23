using MenCore.CrossCuttingConserns.Exceptions.Extensions;
using MenCore.CrossCuttingConserns.Exceptions.HttpProblemDetails;
using MenCore.CrossCuttingConserns.Exceptions.Types;
using Microsoft.AspNetCore.Http;

namespace MenCore.CrossCuttingConserns.Exceptions.Handlers;

public class HttpExceptionHandler : ExceptionHandler
{
    private HttpResponse? _response; // HTTP yanıt nesnesi

    // HTTP yanıt nesnesinin özellikleri
    public HttpResponse Response
    {
        get => _response ?? throw new ArgumentNullException(nameof(_response)); // Null ise istisna fırlatılır
        set => _response = value; // HTTP yanıt nesnesi atanır
    }

    #region Genel Exception tipindeki istisnaları işleyen metot

    protected override Task HandleException(Exception exception)
    {
        Response.StatusCode = StatusCodes.Status500InternalServerError; // HTTP yanıt kodu atanır
        var details =
            new InternalServerErrorProblemDetails(exception.Message)
                .AsJson(); // İstisna detayları JSON formatına dönüştürülür
        return Response.WriteAsync(details); // İstisna detayları HTTP yanıta yazılır
    }

    #endregion

    #region Tiplere göre istisnaları işleyen metot

    protected override Task HandleException(BusinessException businessException)
    {
        Response.StatusCode = StatusCodes.Status400BadRequest; // HTTP yanıt kodu atanır
        var details =
            new BusinessProblemDetails(businessException.Message)
                .AsJson(); // İstisna detayları JSON formatına dönüştürülür
        return Response.WriteAsync(details); // İstisna detayları HTTP yanıta yazılır
    }

    protected override Task HandleException(ValidationException validationException)
    {
        Response.StatusCode = StatusCodes.Status400BadRequest;
        var details = new ValidationProblemDetails(validationException.Errors).AsJson();
        return Response.WriteAsync(details);
    }

    protected override Task HandleException(NotFoundException notFoundException)
    {
        Response.StatusCode = StatusCodes.Status404NotFound;
        var details = new NotFoundProblemDetails(notFoundException.Message).AsJson();
        return Response.WriteAsync(details);
    }

    protected override Task HandleException(AuthorizationException authorizationException)
    {
        Response.StatusCode = StatusCodes.Status401Unauthorized;
        var details = new AuthorizationProblemDetails(authorizationException.Message).AsJson();
        return Response.WriteAsync(details);
    }

    #endregion
}