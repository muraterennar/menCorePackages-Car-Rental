using Microsoft.Extensions.Configuration;
using System.Security.Cryptography;

namespace MenCore.Mailing;

public class MailSettingsManager
{
    private readonly IConfiguration _config;

    public MailSettingsManager(IConfiguration config)
    {
        _config = config;
    }

    public void EnsureDkimPublicKey()
    {
        // DkimPrivateKey alanını kontrol et
        var mailSettings = _config.GetSection("MailSettings");
        string dkimPrivateKey = mailSettings["DkimPrivateKey"];

        // DkimPrivateKey alanı boşsa yeni public key ekleyelim
        if (string.IsNullOrEmpty(dkimPrivateKey))
        {
            // RSA anahtar çiftini oluştur
            using (RSA rsa = RSA.Create())
            {
                // RSA public key'i al
                var publicKey = rsa.ExportRSAPublicKey();

                // Public key'i base64 kodla
                string base64PublicKey = Convert.ToBase64String(publicKey);

                // DkimPrivateKey alanını güncelle
                mailSettings["DkimPrivateKey"] = base64PublicKey;

                // appsettings.json dosyasını kaydet
                var newConfig = new ConfigurationBuilder()
                    .AddConfiguration(_config)
                    .Build();

                var provider = new Microsoft.Extensions.Configuration.Json.JsonConfigurationProvider(new Microsoft.Extensions.Configuration.Json.JsonConfigurationSource());
                // DkimPrivateKey alanını güncelle
                mailSettings["DkimPrivateKey"] = base64PublicKey;

                // appsettings.json dosyasına geçici olarak yazmak
                provider.Set("MailSettings:DkimPrivateKey", base64PublicKey);

                throw new Exception("DkimPrivateKey alanı başarıyla eklendi.");
            }
        }
        else
        {
            throw new Exception("DkimPrivateKey alanı zaten dolu.");
        }
    }
}