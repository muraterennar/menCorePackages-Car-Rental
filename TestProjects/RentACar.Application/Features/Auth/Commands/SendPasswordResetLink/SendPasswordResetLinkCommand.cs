using MediatR;
using MenCore.Application.Extensions;
using MenCore.Application.Pipelines.Logging;
using MenCore.Application.Pipelines.Transaction;
using MenCore.Mailing;
using MenCore.Security.JWT;
using Microsoft.Extensions.Configuration;
using MimeKit;
using RentACar.Application.Features.Auth.Rules;
using RentACar.Application.Services.ResetPasswordServices;
using RentACar.Application.Services.UserServices;
using RentACar.Infrastructure.Mail;
using RentACar.Infrastructure.Mail.Constants;
using RentACar.Infrastructure.Mail.GeneratedTemplates.PasswordResetMailAuthenticator;

namespace RentACar.Application.Features.Auth.Commands.SendPasswordResetLink;

public class SendPasswordResetLinkCommand : IRequest<SendPasswordResetLinkResponse>, ILoggableRequest,
    ITransactionalRequest
{
    public string Email { get; set; }


    public class
        SendPasswordResetLinkCommandHandler : IRequestHandler<SendPasswordResetLinkCommand,
        SendPasswordResetLinkResponse>
    {
        private readonly IUserService _userService;
        private readonly ITokenHelper _tokenHelper;
        private readonly IResetPasswordService _resetPasswordService;
        private readonly AuthBusinessRules _authBusinessRules;
        private readonly IConfiguration _configuration;
        private readonly IPasswordResetMailAuthenticatorTemplate _mailTemplateGeneratorService;
        private readonly IMailService _mailService;

        public SendPasswordResetLinkCommandHandler(IUserService userService, ITokenHelper tokenHelper,
            IResetPasswordService resetPasswordService, AuthBusinessRules authBusinessRules,
            IConfiguration configuration, IPasswordResetMailAuthenticatorTemplate mailTemplateGeneratorService,
            IMailService mailService)
        {
            _userService = userService;
            _tokenHelper = tokenHelper;
            _resetPasswordService = resetPasswordService;
            _authBusinessRules = authBusinessRules;
            _configuration = configuration;
            _mailTemplateGeneratorService = mailTemplateGeneratorService;
            _mailService = mailService;
        }

        public async Task<SendPasswordResetLinkResponse> Handle(SendPasswordResetLinkCommand request,
            CancellationToken cancellationToken)
        {
            await _authBusinessRules.UserEmailShouldBeExists(request.Email);
            
            var user = await _userService.GetByEmailAsync(request.Email);

            await _authBusinessRules.IsSystemUser(user.Id);

            var token = await _resetPasswordService.GenerateResetPasswordTokenAsync();

            var clientOrigin = _configuration["WebAPIConfiguration:PasswordResetUrl"];

            var passwordResetUrl = $"{clientOrigin}/{user?.Id}/{token}";

            var mailTemplate =
                await _mailTemplateGeneratorService.GenerateBodyAsync(passwordResetUrl,
                    MailTemplateNames.ResetPassword);

            var emailTo = new MailboxAddress($"{user?.FirstName} {user?.LastName}", request.Email);

            await _mailService.SendEmailAsync(new Mail
            {
                ToList = [emailTo],
                Subject = "Reset Password",
                HtmlBody = mailTemplate
            });

            user.SecurityStamp = token;

            await _userService.UpdateAsync(user);

            return new SendPasswordResetLinkResponse()
            {
                PasswordResetLink = passwordResetUrl
            };
        }
    }
}