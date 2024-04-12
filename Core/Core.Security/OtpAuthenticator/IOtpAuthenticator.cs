namespace MenCore.Security.OtpAuthenticator;

public interface IOtpAuthenticator
{
    public Task<byte[]> GenerateSecretKey();
    public Task<string> ConvertSecretKeyToString(byte[] secretKey);
    public Task<bool> VerifyCode(byte[] secretKey, string code);
}