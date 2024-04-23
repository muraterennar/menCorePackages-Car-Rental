using System.Web;
using MediatR;
using MenCore.Mailing;
using MenCore.Security.Enums;
using MimeKit;
using RentACar.Application.Features.Auth.Rules;
using RentACar.Application.Services.AuthenticatorServices;
using RentACar.Application.Services.Repositories;
using RentACar.Application.Services.UserServices;

namespace RentACar.Application.Features.Auth.Commands.EnableEmailAuthenticator;

public class EnableEmailAuthenticatorCommand : IRequest
{
    public int UserId { get; set; }
    public string VerifyEmailUrlPrefix { get; set; }

    // E-posta doğrulayıcıyı etkinleştirmek için komut işleyicisini uygular
    public class EnableEmailAuthenticatorCommandHandler : IRequestHandler<EnableEmailAuthenticatorCommand>
    {
        private readonly AuthBusinessRules _authBusinessRules;
        private readonly IAuthenticatorService _authenticatorService;
        private readonly IEmailAuthenticatorRepository _emailAuthenticatorRepository;
        private readonly IMailService _mailService;
        private readonly IUserService _userService;

        // Bağımlılıkları enjekte ederek EnableEmailAuthenticatorCommandHandler sınıfını oluşturur
        public EnableEmailAuthenticatorCommandHandler(IMailService mailService, IUserService userService,
            IEmailAuthenticatorRepository emailAuthenticatorRepository, AuthBusinessRules authBusinessRules,
            IAuthenticatorService authenticatorService)
        {
            _mailService = mailService;
            _userService = userService;
            _emailAuthenticatorRepository = emailAuthenticatorRepository;
            _authBusinessRules = authBusinessRules;
            _authenticatorService = authenticatorService;
        }

        // E-posta doğrulayıcıyı etkinleştirir
        public async Task Handle(EnableEmailAuthenticatorCommand request, CancellationToken cancellationToken)
        {
            // Kullanıcıyı kimlik numarasına göre alır
            var user = await _userService.GetByIdAsync(request.UserId);

            // Kullanıcının varlığını kontrol eder
            await _authBusinessRules.UserShouldBeExists(user);

            // Kullanıcının daha önce doğrulayıcıya sahip olmadığını kontrol eder
            await _authBusinessRules.UserShouldNotBeHaveAuthenticator(user);

            // Kullanıcının doğrulayıcı türünü e-posta olarak ayarlar ve günceller
            user.AuthenticatorType = AuthenticatorType.Email;
            await _userService.UpdateAsync(user);

            // Kullanıcı için e-posta doğrulayıcı oluşturur
            var emailAuthenticator = await _authenticatorService.CreateEmailAuthenticator(user);

            // Oluşturulan e-posta doğrulayıcıyı kaydeder
            var addedEmailAuthenticator = await _emailAuthenticatorRepository.AddAsync(emailAuthenticator);

            // E-posta göndermek için alıcı adresini oluşturur
            var toEmailList = new List<MailboxAddress> { new($"{user.FirstName} {user.LastName}", user.Email) };

            // E-posta ile kullanıcıya doğrulama bağlantısı gönderir
            await _mailService.SendEmailAsync(
                new Mail
                {
                    ToList = toEmailList,
                    Subject = "Verify Your Email - MenTech",
                    TextBody =
                        $"Click on the link to verify your email: {request.VerifyEmailUrlPrefix}?ActivationKey={HttpUtility.UrlEncode(addedEmailAuthenticator.ActivationKey)}"
                }
            );
        }
    }
}