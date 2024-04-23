using MediatR;
using RentACar.Application.Features.Auth.Rules;
using RentACar.Application.Services.AuthServices;
using RentACar.Application.Services.UserServices;

namespace RentACar.Application.Features.Auth.Commands.RefleshToken;

public class RefreshTokenCommand : IRequest<RefreshedTokenResponse>
{
    public string? RefleshToken { get; set; }
    public string? IPAddress { get; set; }

    public class RefreshTokenCommandHandler : IRequestHandler<RefreshTokenCommand, RefreshedTokenResponse>
    {
        private readonly AuthBusinessRules _authBusinessRules;
        private readonly IAuthService _authService;
        private readonly IUserService _userService;

        public RefreshTokenCommandHandler(IAuthService authService, IUserService userService,
            AuthBusinessRules authBusinessRules)
        {
            _authService = authService;
            _userService = userService;
            _authBusinessRules = authBusinessRules;
        }

        public async Task<RefreshedTokenResponse> Handle(RefreshTokenCommand request,
            CancellationToken cancellationToken)
        {
            // Yenileme tokenını alır
            var refreshToken = await _authService.GetRefreshTokenByToken(request.RefleshToken);

            // Yenileme tokenının varlığını kontrol eder
            await _authBusinessRules.RefreshTokenShouldBeExists(refreshToken);

            // Yenileme tokenı iptal edilmişse, soyutlamış tokenları iptal eder
            if (refreshToken.Revoked != null)
                await _authService.RevokeDescendantRefreshTokens(
                    refreshToken,
                    request.IPAddress,
                    $"Attempted reuse of revoked ancestor token: {refreshToken.Token}"
                );

            // Yenileme tokenının geçerliliğini kontrol eder
            await _authBusinessRules.RefreshTokenShouldBeActive(refreshToken);

            // Yenileme tokenına bağlı kullanıcıyı alır
            var user = await _userService.GetByIdAsync(refreshToken.UserId);

            // Yeni bir yenileme tokenı oluşturur ve eski tokenı iptal eder
            var newRefreshToken = await _authService.RotateRefreshToken(user, refreshToken, request.IPAddress);

            // Yeni bir yenileme tokenını veritabanına ekler
            var addedRefreshToken = await _authService.AddRefreshToken(newRefreshToken);

            // Kullanıcının eski yenileme tokenlarını siler
            await _authService.DeleteOldRefreshTokens(refreshToken.UserId);

            // Kullanıcı için yeni bir erişim tokenı oluşturur
            var createdAccessToken = await _authService.CreateAccessToken(user);

            // Yenilenmiş token cevabını oluşturur
            RefreshedTokenResponse refreshedTokensResponse = new()
                { AccessToken = createdAccessToken, RefreshToken = addedRefreshToken };
            return refreshedTokensResponse;
        }
    }
}