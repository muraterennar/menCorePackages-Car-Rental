using System.Web;
using MediatR;
using MenCore.Application.Pipelines.Logging;
using MenCore.Application.Pipelines.Transaction;
using MenCore.Mailing;
using MenCore.Security.Enums;
using MimeKit;
using RentACar.Application.Features.Auth.Rules;
using RentACar.Application.Services.AuthenticatorServices;
using RentACar.Application.Services.Repositories;
using RentACar.Application.Services.UserServices;
using RentACar.Infrastructure.Mail;
using RentACar.Infrastructure.Mail.Constants;
using RentACar.Infrastructure.Mail.GeneratedTemplates.EnableMailAuthenticator;

namespace RentACar.Application.Features.Auth.Commands.EnableEmailAuthenticator;

public class EnableEmailAuthenticatorCommand : IRequest, ITransactionalRequest, ILoggableRequest
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
        private readonly IEnableEmailAuthenticatorTemplate _mailTemplateGeneratorService;

        // Bağımlılıkları enjekte ederek EnableEmailAuthenticatorCommandHandler sınıfını oluşturur
        public EnableEmailAuthenticatorCommandHandler(IMailService mailService, IUserService userService,
            IEmailAuthenticatorRepository emailAuthenticatorRepository, AuthBusinessRules authBusinessRules,
            IAuthenticatorService authenticatorService,
            IEnableEmailAuthenticatorTemplate mailTemplateGeneratorService)
        {
            _mailService = mailService;
            _userService = userService;
            _emailAuthenticatorRepository = emailAuthenticatorRepository;
            _authBusinessRules = authBusinessRules;
            _authenticatorService = authenticatorService;
            _mailTemplateGeneratorService = mailTemplateGeneratorService;
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
            var updatedUser = await _userService.UpdateAsync(user);

            // Kullanıcı için e-posta doğrulayıcı oluşturur
            var emailAuthenticator = await _authenticatorService.CreateEmailAuthenticator(updatedUser);

            // Oluşturulan e-posta doğrulayıcıyı kaydeder
            var addedEmailAuthenticator = await _emailAuthenticatorRepository.AddAsync(emailAuthenticator);

            // E-posta göndermek için alıcı adresini oluşturur
            var toEmailList = new List<MailboxAddress>
                { new($"{updatedUser.FirstName} {updatedUser.LastName}", updatedUser.Email) };


            string activatedlink =
                $"{request.VerifyEmailUrlPrefix}?ActivationKey={HttpUtility.UrlEncode(addedEmailAuthenticator.ActivationKey)}";

            string mailTemplate =
                await _mailTemplateGeneratorService.GenerateBodyAsync(activatedlink, MailTemplateUrl.CodeEmailAuthenticatorTemplate);


            // E-posta ile kullanıcıya doğrulama bağlantısı gönderir
            await _mailService.SendEmailAsync(
                new Mail
                {
                    ToList = toEmailList,
                    Subject = "Verify Your Email - MenTech",
                    HtmlBody = mailTemplate
                }
            );
        }
    }
}