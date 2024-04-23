using System.Security.Cryptography;

namespace MenCore.Security.EmailAuthenticator;

public class EmailAuthenticatorHelper : IEmailAuthenticatorHelper
{
    // E-posta etkinleştirme anahtarı oluşturur
    public Task<string> CreateEmailActivationKey()
    {
        // Rastgele bir 64 bayt uzunluğunda anahtar oluşturur
        var key = Convert.ToBase64String(RandomNumberGenerator.GetBytes(64));
        return Task.FromResult(key);
    }

    // E-posta etkinleştirme kodu oluşturur
    public Task<string> CreateEmailActivationCode()
    {
        // Rastgele bir 6 haneli aktivasyon kodu oluşturur
        var code = RandomNumberGenerator
            .GetInt32(Convert.ToInt32(Math.Pow(10, 6)))
            .ToString()
            .PadLeft(6, '0');
        return Task.FromResult(code);
    }
}