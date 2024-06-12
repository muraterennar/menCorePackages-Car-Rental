using MenCore.Security.Entities;

namespace RentACar.Application.Services.AuthenticatorServices;

public interface IAuthenticatorService
{
    public Task<EmailAuthenticator> CreateEmailAuthenticator(User user);
    public Task<OtpAuthenticator> CreateOtpAuthenticator(User user);
    public Task<string> GenerateOtpQrCode(byte[] secretKey, string email, string issuer, string filePath);
    public Task<string> CreateOtpCode(byte[] secretKey);
    public Task<string> ConvertSecretKeyToString(byte[] secretKey);
    public Task SendAuthenticatorCode(User user);
    public Task VerifyAuthenticatorCode(User user, string authenticatorCode);
}