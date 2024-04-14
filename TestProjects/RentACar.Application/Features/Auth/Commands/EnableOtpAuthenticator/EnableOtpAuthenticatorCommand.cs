using MediatR;
using MenCore.Security.Entities;
using RentACar.Application.Features.Auth.Rules;
using RentACar.Application.Services.AuthenticatorServices;
using RentACar.Application.Services.Repositories;
using RentACar.Application.Services.UserServices;

namespace RentACar.Application.Features.Auth.Commands.EnableOtpAuthenticator;

public class EnableOtpAuthenticatorCommand : IRequest<EnabledOtpAuthenticatorResponse>
{
    public int UserId { get; set; }

    // Otp doğrulayıcıyı etkinleştirmek için komut işleyicisini uygular
    public class EnableOtpAuthenticatorCommandHandler : IRequestHandler<EnableOtpAuthenticatorCommand, EnabledOtpAuthenticatorResponse>
    {
        private readonly AuthBusinessRules _authBusinessRules;
        private readonly IAuthenticatorService _authenticatorService;
        private readonly IOtpAuthenticatorRepository _otpAuthenticatorRepository;
        private readonly IUserService _userService;

        // Bağımlılıkları enjekte ederek EnableOtpAuthenticatorCommandHandler sınıfını oluşturur
        public EnableOtpAuthenticatorCommandHandler(AuthBusinessRules authBusinessRules, IAuthenticatorService authenticatorService, IOtpAuthenticatorRepository otpAuthenticatorRepository, IUserService userService)
        {
            _authBusinessRules = authBusinessRules;
            _authenticatorService = authenticatorService;
            _otpAuthenticatorRepository = otpAuthenticatorRepository;
            _userService = userService;
        }

        // Otp doğrulayıcıyı etkinleştirir
        public async Task<EnabledOtpAuthenticatorResponse> Handle(EnableOtpAuthenticatorCommand request, CancellationToken cancellationToken)
        {
            // Kullanıcıyı kimlik numarasına göre alır
            User? user = await _userService.GetById(request.UserId);

            // Kullanıcının varlığını kontrol eder
            await _authBusinessRules.UserShouldBeExists(user);

            // Kullanıcının daha önce doğrulayıcıya sahip olmadığını kontrol eder
            await _authBusinessRules.UserShouldNotBeHaveAuthenticator(user);

            // Kullanıcının zaten doğrulanmış bir Otp doğrulayıcısının olmadığını kontrol eder
            OtpAuthenticator? isExistsOtpAuthenticator = await _otpAuthenticatorRepository.GetAsync(o => o.UserId == request.UserId);
            await _authBusinessRules.OtpAuthenticatorThatVerifiedShouldNotBeExists(isExistsOtpAuthenticator);

            // Eğer mevcut bir Otp doğrulayıcı varsa, silinir
            if (isExistsOtpAuthenticator is not null)
                await _otpAuthenticatorRepository.DeleteAsync(isExistsOtpAuthenticator);

            // Yeni bir Otp doğrulayıcı oluşturur
            OtpAuthenticator? newOtpAuthenticator = await _authenticatorService.CreateOtpAuthenticator(user);
            OtpAuthenticator? addedAuthenticator = await _otpAuthenticatorRepository.AddAsync(newOtpAuthenticator);

            // Otp doğrulayıcının gizli anahtarını dizeye dönüştürür
            string secretKey = await _authenticatorService.ConvertSecretKeyToString(addedAuthenticator.SecretKey);

            // Etkinleştirilmiş Otp doğrulayıcı yanıtını oluşturur ve döndürür
            EnabledOtpAuthenticatorResponse enabledOtpAuthenticatorDto = new()
            {
                SecretKey = secretKey
            };

            return enabledOtpAuthenticatorDto;
        }
    }

}