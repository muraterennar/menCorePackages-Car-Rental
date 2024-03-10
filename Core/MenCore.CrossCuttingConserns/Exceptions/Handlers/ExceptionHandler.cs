using MenCore.CrossCuttingConserns.Exceptions.Types;

namespace MenCore.CrossCuttingConserns.Exceptions.Handlers;

#region Genel bir istisna işleyicisinin soyut temsilini sağlar.
public abstract class ExceptionHandler
{
    // Asenkron olarak bir istisnayı işler.
    public Task HandleExceptionAsync (Exception exception) =>
        // Gelen istisna türüne göre uygun işleme metodu çağrılır.
        exception switch
        {
            // Eğer gelen istisna bir BusinessException ise,
            // HandleException metodu ile işlenir.
            BusinessException businessException => HandleException(businessException),

            // Eğer gelen istisna bir ValidationException ise,
            // HandleException metodu ile işlenir.
            ValidationException validationException => HandleException(validationException),

            // Diğer durumlarda gelen istisna, genel Exception
            // türünde olduğu kabul edilir ve HandleException
            // metodu ile işlenir.
            _ => HandleException(exception)
        };

    // Türetilmiş sınıflar tarafından uygulanmalıdır. Bir ValidationException
    // istisnasını işler.
    protected abstract Task HandleException (BusinessException businessException);

    // Türetilmiş sınıflar tarafından uygulanmalıdır. Bir BusinessException
    // istisnasını işler.
    protected abstract Task HandleException (ValidationException validationException);

    // Türetilmiş sınıflar tarafından uygulanmalıdır. Bir genel Exception
    // istisnasını işler.
    protected abstract Task HandleException (Exception exception);
}
#endregion