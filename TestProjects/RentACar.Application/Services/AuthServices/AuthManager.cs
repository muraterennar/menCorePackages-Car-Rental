using MenCore.Security.Entities;
using MenCore.Security.JWT;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using RentACar.Application.Services.Repositories;

namespace RentACar.Application.Services.AuthServices;

public class AuthManager : IAuthService
{
    private readonly IRefreshTokenRepository _refreshTokenRepository;
    private readonly ITokenHelper _tokenHelper;
    private readonly TokenOptions? _tokenOptions; // JWT token seçenekleri
    private readonly IUserOperationClaimRepository _userOperationClaimRepository;

    // Constructor, bağımlılıkları enjekte eder ve JWT token seçeneklerini alır.
    public AuthManager(IRefreshTokenRepository refreshTokenRepository,
        IUserOperationClaimRepository userOperationClaimRepository, ITokenHelper tokenHelper,
        IConfiguration configuration)
    {
        _refreshTokenRepository = refreshTokenRepository;
        _userOperationClaimRepository = userOperationClaimRepository;
        _tokenHelper = tokenHelper;
        _tokenOptions =
            configuration.GetSection("TokenOptions").Get<TokenOptions>(); // Token seçeneklerini yapılandırmadan alır
    }

    // Yeni bir yenileme tokenı ekler
    public async Task<RefreshToken> AddRefreshToken(RefreshToken refreshToken)
    {
        var addedRefreshToken = await _refreshTokenRepository.AddAsync(refreshToken);
        return addedRefreshToken;
    }

    // Kullanıcı için erişim tokenı oluşturur
    public async Task<AccessToken> CreateAccessToken(User user)
    {
        // Kullanıcıya ait yetki taleplerini alır ve JWT token oluşturur
        IList<OperationClaim> operationClaims = await _userOperationClaimRepository.Query().AsNoTracking()
            .Where(p => p.Id == user.Id)
            .Select(p => new OperationClaim { Id = p.OperationClaimId, Name = p.OperationClaim.Name }).ToListAsync();

        var accessToken = _tokenHelper.CreateToken(user, operationClaims);
        return accessToken;
    }

    // Eski yenileme tokenlarını siler
    public async Task DeleteOldRefreshTokens(int userId)
    {
        // Geçerli olmayan ve süresi dolan yenileme tokenlarını alır ve siler
        IList<RefreshToken> refreshTokens = await _refreshTokenRepository.Query().AsNoTracking().Where(
            r => r.UserId == userId && r.Revoked == null && r.Expires >= DateTime.UtcNow &&
                 r.CreatedDate.AddDays(_tokenOptions.RefreshTokenTTL) <= DateTime.UtcNow).ToListAsync();

        await _refreshTokenRepository.DeleteRangeAsync(refreshTokens);
    }

    // Tokena göre yenileme tokenı alır
    public async Task<RefreshToken?> GetRefreshTokenByToken(string token)
    {
        var refreshToken = await _refreshTokenRepository.GetAsync(r => r.Token == token);
        return refreshToken;
    }

    // Yeni bir yenileme tokenı oluşturur
    public Task<RefreshToken> CreateRefreshToken(User user, string ipAddress)
    {
        var refreshToken = _tokenHelper.CreateRefreshToken(user, ipAddress);
        return Task.FromResult(refreshToken);
    }

    // Yenileme tokenını iptal eder
    public async Task RevokeRefreshToken(RefreshToken refreshToken, string ipAddress, string? reason = null,
        string? replacedByToken = null)
    {
        // Yenileme tokenını iptal eder
        refreshToken.Revoked = DateTime.UtcNow;
        refreshToken.RevokedByIp = ipAddress;
        refreshToken.ReasonRevoked = reason;
        refreshToken.ReplacedByToken = replacedByToken;
        await _refreshTokenRepository.UpdateAsync(refreshToken);
    }

    // Eski yenileme tokenlarını iptal eder
    public async Task RevokeDescendantRefreshTokens(RefreshToken refreshToken, string ipAddress, string reason)
    {
        // Yenileme tokenını iptal eder ve yerine geçen tokenları da iptal eder
        var childToken = await _refreshTokenRepository.GetAsync(r => r.Token == refreshToken.ReplacedByToken);

        if (childToken != null && childToken.Revoked != null && childToken.Expires <= DateTime.UtcNow)
            await RevokeRefreshToken(childToken, ipAddress, reason);
        else
            await RevokeDescendantRefreshTokens(childToken, ipAddress, reason);
    }

    // Yenileme tokenını yeniler
    public async Task<RefreshToken> RotateRefreshToken(User user, RefreshToken refreshToken, string ipAddress)
    {
        // Yeni bir yenileme tokenı oluşturur ve eski tokenı iptal eder
        var newRefreshToken = _tokenHelper.CreateRefreshToken(user, ipAddress);
        await RevokeRefreshToken(refreshToken, ipAddress, "Replaced by new Token", newRefreshToken.Token);
        return newRefreshToken;
    }
}