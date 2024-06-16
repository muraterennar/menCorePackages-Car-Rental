using OtpNet;
using Microsoft.Extensions.Configuration;
using QRCoder;
using System;
using System.IO;
using System.Threading.Tasks;
using MenCore.Security.JWT; // İlgili kütüphanenin tam ismi eklemesi yapıldı.

namespace MenCore.Security.OtpAuthenticator.OtpNet
{
    public class OtpNetOtpAuthenticatorHelper : IOtpAuthenticatorHelper
    {
        private int _secretKeyLength = 20;
        private int _otpCodeLength = 6;
        private int _step = 30; // RFC 6238'e göre genellikle 30 saniye adımı kullanılır.
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
            var otp = new Totp(secretKey, _step, OtpHashMode.Sha1, _otpCodeLength); // Google Authenticator genellikle SHA-1 kullanır.
            string otpCode = otp.ComputeTotp(DateTime.UtcNow);
            return Task.FromResult(otpCode);
        }

        public Task<bool> VerifyOtpCode(byte[] secretKey, string otpCode)
        {
            var totp = new Totp(secretKey, _step, OtpHashMode.Sha1, _otpCodeLength);
            bool isValid = totp.VerifyTotp(otpCode, out _, new VerificationWindow(2, 2));
            return Task.FromResult(isValid);
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
                $"otpauth://totp/{issuer}:{accountName}?secret={secretKeyString}&issuer={issuer}";

            QRCodeGenerator qrGenerator = new QRCodeGenerator();
            QRCodeData qrCodeData = qrGenerator.CreateQrCode(otpAuthUri, QRCodeGenerator.ECCLevel.Q);
            PngByteQRCode qrCode = new PngByteQRCode(qrCodeData);
            byte[] qrCodeImage = qrCode.GetGraphic(20); // QR code size set to 20 pixels
            await File.WriteAllBytesAsync(filePath, qrCodeImage);

            return filePath;
        }
    }
}
