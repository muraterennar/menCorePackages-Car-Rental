using MediatR;
using MenCore.Application.Dtos;
using MenCore.Security.Enums;
using RentACar.Application.Features.Auth.Rules;
using RentACar.Application.Services.AuthenticatorServices;
using RentACar.Application.Services.AuthServices;
using RentACar.Application.Services.UserServices;

namespace RentACar.Application.Features.Auth.Commands.Login;

public class LoginCommand : IRequest<LoginResponse>
{
    public UserForLoginDto UserForLoginDto { get; set; }
    public string IPAddress { get; set; }

    public class LoginCommandHandler : IRequestHandler<LoginCommand, LoginResponse>
    {
        private readonly AuthBusinessRules _authBusinessRules;
        private readonly IAuthenticatorService _authenticatorService;
        private readonly IAuthService _authService;
        private readonly IUserService _userService;

        public LoginCommandHandler(IUserService userService, IAuthService authService,
            AuthBusinessRules authBusinessRules, IAuthenticatorService authenticatorService)
        {
            _userService = userService;
            _authService = authService;
            _authBusinessRules = authBusinessRules;
            _authenticatorService = authenticatorService;
        }

        // Kullanıcı giriş işlemini yönetir
        public async Task<LoginResponse> Handle(LoginCommand request, CancellationToken cancellationToken)
        {
            // Kullanıcıyı e-posta veya kullanıcı adına göre alır
            var user = await _userService.GetByEmailAsync(request.UserForLoginDto.EmailOrUsername);

            // Eğer kullanıcı e-posta ile bulunamazsa, kullanıcı adına göre tekrar arar
            if (user == null) user = await _userService.GetByUsernameAsync(request.UserForLoginDto.EmailOrUsername);

            // Kullanıcının varlığını kontrol eder
            await _authBusinessRules.UserShouldBeExists(user);

            // Kullanıcının parolasının eşleştiğini doğrular
            await _authBusinessRules.UserPasswordShouldBeMatch(user.Id, request.UserForLoginDto.Password);

            // Giriş yanıtını oluşturur
            LoginResponse loginResponse = new();

            // Kullanıcının kimlik doğrulayıcısı varsa
            if (user.AuthenticatorType is not AuthenticatorType.None)
            {
                // Eğer kullanıcı doğrulayıcı kodu sağlamadıysa
                if (request.UserForLoginDto.AuthenticatorCode is null)
                {
                    // Doğrulayıcı kodu kullanıcıya gönderir ve yanıtı döndürür
                    await _authenticatorService.SendAuthenticatorCode(user);
                    loginResponse.RequiredAuthenticatorType = user.AuthenticatorType;
                    return loginResponse;
                }

                // Kullanıcının doğrulayıcı kodunu doğrular
                await _authenticatorService.VerifyAuthenticatorCode(user, request.UserForLoginDto.AuthenticatorCode);
            }

            // Kullanıcı için erişim belirteci oluşturur
            var createdAccessToken = await _authService.CreateAccessToken(user);

            // Kullanıcı için yenileme belirteci oluşturur ve ekler
            var createdRefreshToken = await _authService.CreateRefreshToken(user, request.IPAddress);
            var addedRefreshToken = await _authService.AddRefreshToken(createdRefreshToken);

            // Kullanıcının eski yenileme belirtecini siler
            await _authService.DeleteOldRefreshTokens(user.Id);

            // Giriş yanıtını ayarlar ve döndürür
            loginResponse.AccessToken = createdAccessToken;
            loginResponse.RefreshToken = addedRefreshToken;
            return loginResponse;
        }
    }
}