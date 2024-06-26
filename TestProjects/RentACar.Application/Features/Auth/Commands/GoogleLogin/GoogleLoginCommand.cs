using MediatR;
using MenCore.Application.Pipelines.Logging;
using MenCore.Application.Pipelines.Transaction;
using MenCore.Security.Entities;
using MenCore.Security.Enums;
using Microsoft.Extensions.Configuration;
using RentACar.Application.Features.Auth.Commands.Login;
using RentACar.Application.Features.Auth.Rules;
using RentACar.Application.Services.AuthenticatorServices;
using RentACar.Application.Services.AuthServices;
using RentACar.Application.Services.UserLoginServices;
using RentACar.Application.Services.UserServices;
using RentACar.Domaim.Entities;
using RentACar.Infrastructure.Auths.GoogleLogin;

namespace RentACar.Application.Features.Auth.Commands.GoogleLogin;

public class GoogleLoginCommand : IRequest<GoogleLoginResponse>, ITransactionalRequest, ILoggableRequest
{
    public string Id { get; set; }
    public string IdToken { get; set; }
    public string Email { get; set; }
    public string Name { get; set; }
    public string PhotoUrl { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Provider { get; set; }
    public string IPAddress { get; set; }


    public class GoogleLoginCommandHandler : IRequestHandler<GoogleLoginCommand, GoogleLoginResponse>
    {
        private readonly AuthBusinessRules _authBusinessRules;
        private readonly IUserLoginService _userLoginService;
        private readonly IUserService _userService;
        private readonly IAuthenticatorService _authenticatorService;
        private readonly IAuthService _authService;
        private readonly IGoogleAuthService _googleAuthService;
        private readonly GoogleAuthSettings _googleAuthSettings;


        public GoogleLoginCommandHandler(IConfiguration configuration, IUserLoginService userLoginService,
            IGoogleAuthService googleAuthService, AuthBusinessRules authBusinessRules, IUserService userService,
            IAuthenticatorService authenticatorService, IAuthService authService)
        {
            _googleAuthSettings = configuration.GetSection("GoogleAuthSettings").Get<GoogleAuthSettings>() ??
                                  throw new ArgumentException("GoogleAuthSettings Not Found in appsettings.json");
            _userLoginService = userLoginService;
            _googleAuthService = googleAuthService;
            _authBusinessRules = authBusinessRules;
            _userService = userService;
            _authenticatorService = authenticatorService;
            _authService = authService;
        }

        public async Task<GoogleLoginResponse> Handle(GoogleLoginCommand request, CancellationToken cancellationToken)
        {
            var settings = _googleAuthService.GoogleGenerateSetting(_googleAuthSettings.ClientId);
            var payload = await _googleAuthService.ValidateToken(request.IdToken, settings);

            var user = new User()
            {
                Email = request.Email,
                FirstName = request.FirstName,
                LastName = request.LastName,
                PhotoUrl = request.PhotoUrl,
                Username = request.Name,
                Status = true
            };

           // await _authBusinessRules.EnsureEmailDoesNotExist(user.Email);
            await _userService.CreateAsync(user);

            var userLogin = new UserLogin()
            {
                ProviderKey = request.Provider,
                LoginProvider = payload.Subject,
                ProviderDisplayName = request.Provider,
                UserId = user.Id
            };
            

            await _authBusinessRules.EnsureExternalUserDoesNotExistAsync(userLogin.LoginProvider,
                userLogin.ProviderKey);

            await _userLoginService.CreateAsync(userLogin);

            // Kullanıcı için erişim belirteci oluşturur
            var createdAccessToken = await _authService.CreateAccessToken(user);

            // Kullanıcı için yenileme belirteci oluşturur ve ekler
            var createdRefreshToken = await _authService.CreateRefreshToken(user, request.IPAddress);
            var addedRefreshToken = await _authService.AddRefreshToken(createdRefreshToken);

            // Kullanıcının eski yenileme belirtecini siler
            await _authService.DeleteOldRefreshTokens(user.Id);

            return new GoogleLoginResponse()
            {
                Token = createdAccessToken.Token,
                Expiration = createdAccessToken.Expiration,
                RefreshToken = addedRefreshToken,
            };
        }
    }
}