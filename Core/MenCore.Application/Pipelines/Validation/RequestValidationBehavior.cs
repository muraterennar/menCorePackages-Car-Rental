using FluentValidation;
using MediatR;
using MenCore.CrossCuttingConserns.Exceptions.Types;
using ValidationException = MenCore.CrossCuttingConserns.Exceptions.Types.ValidationException;

namespace MenCore.Application.Pipelines.Validation
{
    #region TRequest türünde olan ve TResponse türünde bir dönüş sağlayan IPipelineBehavior arayüzünü uygular
    public class RequestValidationBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    where TRequest : IRequest<TResponse> // TRequest, IRequest<TResponse> türünden olmalıdır
    {
        private readonly IEnumerable<IValidator<TRequest>> _validators; // TRequest türündeki doğrulayıcıların koleksiyonu

        // Doğrulama davranışının kurucu metodu, doğrulayıcıların koleksiyonunu alır
        public RequestValidationBehavior (IEnumerable<IValidator<TRequest>> validators)
        {
            _validators = validators; // Doğrulayıcıları saklar
        }

        #region İsteği işlerken doğrulama işlemlerini gerçekleştiren metot
        public async Task<TResponse> Handle (TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
        {
            ValidationContext<object> context = new(request); // İsteğin doğrulama bağlamı oluşturuluyor

            #region Doğrulayıcıları kullanarak doğrulama işlemleri gerçekleştiriliyor
            IEnumerable<ValidationExceptionModel> errors = _validators.Select(validator => validator.Validate(context))
            .SelectMany(result => result.Errors)
            .Where(failure => failure != null)
            .GroupBy(
                keySelector: p => p.PropertyName,
                resultSelector: (propertyName, errors) => new ValidationExceptionModel { Property = propertyName, Errors = errors.Select(e => e.ErrorMessage) }
            ).ToList();
            #endregion

            // Doğrulama sırasında hata oluştuysa istisna fırlatılıyor
            if (errors.Any())
                throw new ValidationException(errors);

            // Doğrulama başarılı ise bir sonraki işleme devam ediliyor
            TResponse response = await next(); // Bir sonraki işlem çağrılıyor
            return response; // Sonuç döndürülüyor
        }
        #endregion
    }
    #endregion
}
