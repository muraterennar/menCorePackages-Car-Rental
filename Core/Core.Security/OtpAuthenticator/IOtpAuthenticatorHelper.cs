namespace MenCore.Security.OtpAuthenticator;

public interface IOtpAuthenticatorHelper
{
    Task<byte[]> GenerateSecretKey();
    Task<string> GenerateOtpCode(byte[] secretKey);
    Task<bool> VerifyOtpCode(byte[] secretKey, string otpCode);
    Task<string> ConvertSecretKeyToString(byte[] secretKey);
    Task<string> GenerateQrCode(byte[] secretKey, string issuer, string accountName, string filePath);
}