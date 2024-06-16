using MediatR;
using RentACar.Application.Features.Auth.Rules;
using RentACar.Application.Services.Repositories;
using RentACar.Infrastructure.Mail;

namespace RentACar.Application.Features.Auth.Commands.VerifyEmailAuthenticator;

public class VerifyEmailAuthenticatorCommand : IRequest<VerifyEmailAuthenticatorResponse>
{
    public string ActivationKey { get; set; }

    // E-posta doğrulayıcıyı doğrulamak için komut işleyicisini uygular
    public class
        VerifyEmailAuthenticatorCommandHandler : IRequestHandler<VerifyEmailAuthenticatorCommand,
        VerifyEmailAuthenticatorResponse>
    {
        private readonly AuthBusinessRules _authBusinessRules;
        private readonly IEmailAuthenticatorRepository _emailAuthenticatorRepository;
        private readonly IMailTemplateGeneratorService _mailTemplateGeneratorService;

        // Bağımlılıkları enjekte ederek VerifyEmailAuthenticatorCommandHandler sınıfını oluşturur
        public VerifyEmailAuthenticatorCommandHandler(AuthBusinessRules authBusinessRules,
            IEmailAuthenticatorRepository emailAuthenticatorRepository,
            IMailTemplateGeneratorService mailTemplateGeneratorService)
        {
            _authBusinessRules = authBusinessRules;
            _emailAuthenticatorRepository = emailAuthenticatorRepository;
            _mailTemplateGeneratorService = mailTemplateGeneratorService;
        }

        // E-posta doğrulayıcıyı doğrular
        public async Task<VerifyEmailAuthenticatorResponse> Handle(VerifyEmailAuthenticatorCommand request,
            CancellationToken cancellationToken)
        {
            // Doğrulama anahtarına göre e-posta doğrulayıcıyı alır
            var emailAuthenticator =
                await _emailAuthenticatorRepository.GetAsync(e => e.ActivationKey == request.ActivationKey);

            // E-posta doğrulayıcıyı doğrulama anahtarı olmalıdır kuralını uygular
            await _authBusinessRules.EmailAuthenticatorActivationKeyShouldBeExists(emailAuthenticator);

            // E-posta doğrulayıcının var olması gerektiğini kontrol eder
            await _authBusinessRules.EmailAuthenticatorShouldBeExists(emailAuthenticator);

            // E-posta doğrulayıcının doğrulandığını işaretler
            emailAuthenticator.ActivationKey = null;
            emailAuthenticator.IsVerified = true;

            // E-posta doğrulayıcıyı günceller
            await _emailAuthenticatorRepository.UpdateAsync(emailAuthenticator);

            return new VerifyEmailAuthenticatorResponse()
            {
                Message = "The email verifier has been successfully verified."
            };
        }
    }
}