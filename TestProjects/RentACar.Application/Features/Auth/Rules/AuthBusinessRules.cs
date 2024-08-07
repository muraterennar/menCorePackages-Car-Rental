using MenCore.Application.Rules;
using MenCore.CrossCuttingConserns.Exceptions.Types;
using MenCore.Security.Entities;
using MenCore.Security.Enums;
using MenCore.Security.Hashing;
using RentACar.Application.Features.Auth.Constants;
using RentACar.Application.Services.Repositories;

namespace RentACar.Application.Features.Auth.Rules;

public class AuthBusinessRules : BaseBusinessRules
{
    private readonly IEmailAuthenticatorRepository _emailAuthenticatorRepository;
    private readonly IUserRepository _userRepository;
    private readonly IUserLoginRepository _userLoginRepository;

    public AuthBusinessRules(IUserRepository userRepository, IEmailAuthenticatorRepository emailAuthenticatorRepository,
        IUserLoginRepository userLoginRepository)
    {
        _userRepository = userRepository;
        _emailAuthenticatorRepository = emailAuthenticatorRepository;
        _userLoginRepository = userLoginRepository;
    }

    // E-posta kimlik doğrulayıcısının var olup olmadığını kontrol eder
    public Task EmailAuthenticatorShouldBeExists(EmailAuthenticator? emailAuthenticator)
    {
        if (emailAuthenticator is null)
            throw new BusinessException(AuthMessages.EmailAuthenticatorDontExists);
        return Task.CompletedTask;
    }

    // OTP (One-Time Password) kimlik doğrulayıcısının var olup olmadığını kontrol eder
    public Task OtpAuthenticatorShouldBeExists(OtpAuthenticator? otpAuthenticator)
    {
        if (otpAuthenticator is null)
            throw new BusinessException(AuthMessages.OtpAuthenticatorDontExists);
        return Task.CompletedTask;
    }

    // Onaylanmış OTP kimlik doğrulayıcısının var olmamasını kontrol eder
    public Task OtpAuthenticatorThatVerifiedShouldNotBeExists(OtpAuthenticator? otpAuthenticator)
    {
        if (otpAuthenticator is not null && otpAuthenticator.IsVerified)
            throw new BusinessException(AuthMessages.AlreadyVerifiedOtpAuthenticatorIsExists);
        return Task.CompletedTask;
    }

    // E-posta kimlik doğrulayıcısının etkinleştirme anahtarının var olup olmadığını kontrol eder
    public Task EmailAuthenticatorActivationKeyShouldBeExists(EmailAuthenticator emailAuthenticator)
    {
        if (emailAuthenticator.ActivationKey is null)
            throw new BusinessException(AuthMessages.EmailActivationKeyDontExists);
        return Task.CompletedTask;
    }

    // Kullanıcının var olup olmadığını kontrol eder
    public Task UserShouldBeExists(User? user)
    {
        if (user == null)
            throw new BusinessException(AuthMessages.UserDontExists);
        return Task.CompletedTask;
    }

    // Kullanıcının bir kimlik doğrulayıcısının olup olmadığını kontrol eder
    public Task UserShouldNotBeHaveAuthenticator(User user)
    {
        if (user.AuthenticatorType != AuthenticatorType.None)
            throw new BusinessException(AuthMessages.UserHaveAlreadyAAuthenticator);
        return Task.CompletedTask;
    }

    // Yenileme tokenının var olup olmadığını kontrol eder
    public Task RefreshTokenShouldBeExists(RefreshToken? refreshToken)
    {
        if (refreshToken == null)
            throw new BusinessException(AuthMessages.RefreshDontExists);
        return Task.CompletedTask;
    }

    // Yenileme tokenının geçerli olup olmadığını kontrol eder
    public Task RefreshTokenShouldBeActive(RefreshToken refreshToken)
    {
        if (refreshToken.Revoked != null && DateTime.UtcNow >= refreshToken.Expires)
            throw new BusinessException(AuthMessages.InvalidRefreshToken);
        return Task.CompletedTask;
    }

    // Verilen e-posta adresine sahip bir kullanıcının olup olmadığını kontrol eder
    public async Task UserEmailShouldBeNotExists(string email)
    {
        var user = await _userRepository.GetAsync(u => u.Email == email, enableTracking: false);
        if (user != null)
            throw new BusinessException(AuthMessages.UserMailAlreadyExists);
    }
    
    public async Task UserEmailShouldBeExists(string email)
    {
        var user = await _userRepository.GetAsync(u => u.Email == email, enableTracking: false);
        if (user == null)
            throw new BusinessException(AuthMessages.UserDontExists);
    }

    // Kullanıcının verilen parolasının eşleşip eşleşmediğini kontrol eder
    public async Task UserPasswordShouldBeMatch(int id, string password)
    {
        var user = await _userRepository.GetAsync(u => u.Id == id, enableTracking: false);
        if (!HashingHelper.VerifyPasswordHash(password, user.PasswordHash, user.PasswordSalt))
            throw new BusinessException(AuthMessages.PasswordDontMatch);
    }

    // User Login İşlemi Sırasında Aynı Kullanıcıcının Olup Olmadığını Kontrol Eder
    public async Task EnsureExternalUserDoesNotExistAsync(string loginProvider, string providerKey)
    {
        var user = await _userLoginRepository.GetAsync(
            u => u.LoginProvider == loginProvider && u.ProviderKey == providerKey, enableTracking: false);
        if (user != null)
            throw new BusinessException(AuthMessages.UserAlreadyExists);
    }
    
    public async Task EnsureUserLoginExists(string loginProvider, string providerKey)
    {
        var userLogin =
            await _userLoginRepository.GetAsync(u => u.LoginProvider == loginProvider && u.ProviderKey == providerKey,
                enableTracking: false);
        if (userLogin == null)
            throw new BusinessException(AuthMessages.UserDontExists);
    }

    public async Task EnsureUserDoesNotExist(int userId)
    {
        var user = await _userRepository.GetAsync(u => u.Id == userId, enableTracking: false);
        if (user != null)
            throw new BusinessException(AuthMessages.UserAlreadyExists);
    }

    public async Task EnsureEmailDoesNotExist(string email)
    {
        var userWithEmail = await _userRepository.GetAsync(u => u.Email == email, enableTracking: false);
        if (userWithEmail != null)
            throw new BusinessException(AuthMessages.UserMailAlreadyExists);
    }
    
    public async Task IsTokenValid(string token, string email)
    {
        var user = await _userRepository.GetAsync(u=>u.Email == email, enableTracking: false);
        if (user == null)
            throw new BusinessException(AuthMessages.UserDontExists);
        if (user.SecurityStamp != token)
            throw new BusinessException(AuthMessages.TokenIsInvalid);
    }
    
    public async Task IsSystemUser(int userId)
    {
        var user = await _userRepository.GetAsync(u => u.Id == userId, enableTracking: false);
        if (user?.Provider != AuthProvider.System && user?.PasswordHash == null && user?.PasswordSalt == null)
            throw new BusinessException(AuthMessages.UserIsNotSystemUser);
    }
}