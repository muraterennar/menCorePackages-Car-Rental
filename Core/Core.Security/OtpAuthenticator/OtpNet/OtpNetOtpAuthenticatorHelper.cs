using OtpNet;
using MenCore.Security.JWT;
using Microsoft.Extensions.Configuration;
using QRCoder;

namespace MenCore.Security.OtpAuthenticator.OtpNet;

public class OtpNetOtpAuthenticatorHelper : IOtpAuthenticatorHelper
{
    private int _secretKeyLength = 20;
    private int _otpCodeLength = 6;
    private int _step = 60;
    private readonly TokenOptions _tokenOptions;
    private readonly IConfiguration Configuration;

    public OtpNetOtpAuthenticatorHelper(IConfiguration configuration)
    {
        Configuration = configuration;
        const string configurationSection = "TokenOptions";
        _tokenOptions = Configuration.GetSection(configurationSection).Get<TokenOptions>()
                        ?? throw new NullReferenceException(
                            $"\"{configurationSection}\" section cannot found in configuration.");
    }

    public Task<byte[]> GenerateSecretKey()
    {
        byte[] secretKey = KeyGeneration.GenerateRandomKey(_secretKeyLength);
        return Task.FromResult(secretKey);
    }

    public Task<string> GenerateOtpCode(byte[] secretKey)
    {
        var otp = new Totp(secretKey, _step, OtpHashMode.Sha512, _otpCodeLength);
        string otpCode = otp.ComputeTotp(DateTime.UtcNow);
        return Task.FromResult(otpCode);
    }

    public Task<bool> VerifyOtpCode(byte[] secretKey, string otpCode)
    {
        var totp = new Totp(secretKey, _step, OtpHashMode.Sha512, _otpCodeLength);
        bool isTrue = totp.VerifyTotp(otpCode, out _, new VerificationWindow(2, 2));
        return Task.FromResult(isTrue);
    }

    public Task<string> ConvertSecretKeyToString(byte[] secretKey)
    {
        string secretKeyString = Base32Encoding.ToString(secretKey);
        return Task.FromResult(secretKeyString);
    }

    public async Task<string> GenerateQrCode(byte[] secretKey, string issuer, string accountName, string filePath)
    {
        string secretKeyString = await ConvertSecretKeyToString(secretKey);
        string otpAuthUri =
            $"otpauth://totp/{_tokenOptions.Issuer}:{_tokenOptions.Audience}?secret={secretKeyString}&issuer={issuer}";

        QRCodeGenerator qrGenerator = new QRCodeGenerator();
        QRCodeData qrCodeData = qrGenerator.CreateQrCode(otpAuthUri, QRCodeGenerator.ECCLevel.Q);
        PngByteQRCode qrCode = new PngByteQRCode(qrCodeData);
        byte[] qrCodeImage = qrCode.GetGraphic(20);
        await File.WriteAllBytesAsync(filePath, qrCodeImage);

        return filePath;
    }
}