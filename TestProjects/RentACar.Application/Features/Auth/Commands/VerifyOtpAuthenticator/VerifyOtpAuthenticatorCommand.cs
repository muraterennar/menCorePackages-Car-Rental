using MediatR;
using MenCore.Application.Pipelines.Logging;
using MenCore.Application.Pipelines.Transaction;
using MenCore.Security.Enums;
using RentACar.Application.Features.Auth.Rules;
using RentACar.Application.Services.AuthenticatorServices;
using RentACar.Application.Services.Repositories;
using RentACar.Application.Services.UserServices;

namespace RentACar.Application.Features.Auth.Commands.VerifyOtpAuthenticator;

public class VerifyOtpAuthenticatorCommand : IRequest, ILoggableRequest, ITransactionalRequest
{
    public int UserId { get; set; }
    public string ActivationCode { get; set; }

    // OTP doğrulayıcıyı doğrulamak için komut işleyicisini uygular
    public class VerifyOtpAuthenticatorCommandHandler : IRequestHandler<VerifyOtpAuthenticatorCommand>
    {
        private readonly AuthBusinessRules _authBusinessRules;
        private readonly IAuthenticatorService _authenticatorService;
        private readonly IOtpAuthenticatorRepository _otpAuthenticatorRepository;
        private readonly IUserService _userService;

        // Bağımlılıkları enjekte ederek VerifyOtpAuthenticatorCommandHandler sınıfını oluşturur
        public VerifyOtpAuthenticatorCommandHandler(AuthBusinessRules authBusinessRules,
            IAuthenticatorService authenticatorService, IOtpAuthenticatorRepository otpAuthenticatorRepository,
            IUserService userService)
        {
            _authBusinessRules = authBusinessRules;
            _authenticatorService = authenticatorService;
            _otpAuthenticatorRepository = otpAuthenticatorRepository;
            _userService = userService;
        }

        // OTP doğrulayıcıyı doğrular
        public async Task Handle(VerifyOtpAuthenticatorCommand request, CancellationToken cancellationToken)
        {
            // OTP doğrulayıcıyı kullanıcı kimliğine göre alır
            var otpAuthenticator = await _otpAuthenticatorRepository.GetAsync(e => e.UserId == request.UserId);

            // OTP doğrulayıcının var olması gerektiğini kontrol eder
            await _authBusinessRules.OtpAuthenticatorShouldBeExists(otpAuthenticator);

            // Kullanıcıyı kullanıcı kimliğine göre alır
            var user = await _userService.GetByIdAsync(request.UserId);

            // OTP doğrulayıcının doğrulandığını işaretler
            otpAuthenticator.IsVerified = true;

            // Kullanıcının doğrulayıcı türünü OTP olarak ayarlar
            user.AuthenticatorType = AuthenticatorType.Otp;

            // OTP doğrulama kodunu doğrular
            await _authenticatorService.VerifyAuthenticatorCode(user, request.ActivationCode);

            // OTP doğrulayıcıyı günceller
            await _otpAuthenticatorRepository.UpdateAsync(otpAuthenticator);

            // Kullanıcıyı günceller
            await _userService.UpdateAsync(user);
        }
    }
}