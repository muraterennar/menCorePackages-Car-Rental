using System.Security.Cryptography;
using System.Text;
using MailKit.Net.Smtp;
using Microsoft.Extensions.Configuration;
using MimeKit;
using MimeKit.Cryptography;
using Org.BouncyCastle.Crypto;
using Org.BouncyCastle.Crypto.Generators;
using Org.BouncyCastle.OpenSsl;
using Org.BouncyCastle.Security;

namespace MenCore.Mailing.MailKitImplementations;

public class MailKitMailService : IMailService
{
    private readonly MailSettings _mailSettings;
    private DkimSigner? _signer;
    private IConfiguration _configuration;

    public MailKitMailService(IConfiguration configuration)
    {
        _mailSettings = configuration.GetSection("MailSettings").Get<MailSettings>();
        _signer = null;
        _configuration = configuration;
    }

    public void SendMail(Mail mail)
    {
        // Gönderilecek e-posta adresleri yoksa işlemi sonlandırır
        if (mail.ToList == null || mail.ToList.Count < 1)
            return;

        // E-posta hazırlığı yapar
        EmailPrepare(mail, out var email, out var smtp);

        // E-postayı gönderir
        smtp.Send(email);

        // SMTP bağlantısını kapatır
        smtp.Disconnect(true);

        // E-posta ve SMTP nesnelerini temizler
        email.Dispose();
        smtp.Dispose();
    }

    public async Task SendEmailAsync(Mail mail)
    {
        // Gönderilecek e-posta adresleri yoksa işlemi sonlandırır
        if (mail.ToList == null || mail.ToList.Count < 1)
            return;

        // E-posta hazırlığı yapar
        EmailPrepare(mail, out var email, out var smtp);

        // E-postayı asenkron olarak gönderir
        await smtp.SendAsync(email);

        // SMTP bağlantısını kapatır
        smtp.Disconnect(true);

        // E-posta ve SMTP nesnelerini temizler
        email.Dispose();
        smtp.Dispose();
    }

    private void EmailPrepare(Mail mail, out MimeMessage email, out SmtpClient smtp)
    {
        // MimeMessage nesnesi oluşturur
        email = new MimeMessage();
        email.From.Add(new MailboxAddress(_mailSettings.SenderFullName, _mailSettings.SenderEmail));
        email.To.AddRange(mail.ToList);
        if (mail.CcList != null && mail.CcList.Any())
            email.Cc.AddRange(mail.CcList);
        if (mail.BccList != null && mail.BccList.Any())
            email.Bcc.AddRange(mail.BccList);

        email.Subject = mail.Subject;
        if (mail.UnscribeLink != null)
            email.Headers.Add("List-Unsubscribe", $"<{mail.UnscribeLink}>");
        BodyBuilder bodyBuilder = new() { TextBody = mail.TextBody, HtmlBody = mail.HtmlBody };

        // E-postaya ek dosyaları ekler
        if (mail.Attachments != null)
            foreach (var attachment in mail.Attachments)
                if (attachment != null)
                    bodyBuilder.Attachments.Add(attachment);

        email.Body = bodyBuilder.ToMessageBody();
        email.Prepare(EncodingConstraint.SevenBit);

        // DKIM imzalaması gerekiyorsa imzalar
        if (_mailSettings.DkimPrivateKey != null && _mailSettings.DkimSelector != null &&
            _mailSettings.DomainName != null)
        {
            _signer = new DkimSigner(ReadPrivateKeyFromPemEncodedString(), _mailSettings.DomainName,
                _mailSettings.DkimSelector)
            {
                HeaderCanonicalizationAlgorithm = DkimCanonicalizationAlgorithm.Simple,
                BodyCanonicalizationAlgorithm = DkimCanonicalizationAlgorithm.Simple,
                AgentOrUserIdentifier = $"@{_mailSettings.DomainName}",
                QueryMethod = "dns/txt"
            };
            var headers = new[] { HeaderId.From, HeaderId.Subject, HeaderId.To };
            _signer.Sign(email, headers);
        }

        // SmtpClient nesnesi oluşturur ve SMTP sunucusuna bağlanır
        smtp = new SmtpClient();
        smtp.Connect(_mailSettings.Server, _mailSettings.Port);
        if (_mailSettings.AuthenticationRequired)
            smtp.Authenticate(_mailSettings.UserName, _mailSettings.Password);
    }

    // PEM kodlanmış bir RSA özel anahtarını okur ve bu anahtarı döndürür
    private AsymmetricKeyParameter ReadPrivateKeyFromPemEncodedString()
    {
        AsymmetricKeyParameter result;
        AsymmetricCipherKeyPair keyCode = GenerateRSAKeyPair();
        string pemEncodedKey = ExportPrivateKeyToPEM(keyCode.Private);
        using (StringReader stringReader = new(pemEncodedKey))
        {
            PemReader pemReader = new(stringReader);
            object? pemObject = pemReader.ReadObject();
            if (pemObject is AsymmetricCipherKeyPair keyPair)
            {
                result = keyPair.Private;
            }
            else if (pemObject is AsymmetricKeyParameter keyParameter)
            {
                result = keyParameter;
            }
            else
            {
                throw new InvalidOperationException("Invalid PEM object.");
            }
        }

        return result;
    }
    
    public static AsymmetricCipherKeyPair GenerateRSAKeyPair(int keySize = 2048)
    {
        var keyGenerationParameters = new KeyGenerationParameters(new SecureRandom(), keySize);
        var keyPairGenerator = new RsaKeyPairGenerator();
        keyPairGenerator.Init(keyGenerationParameters);
        return keyPairGenerator.GenerateKeyPair();
    }
    
    public static string ExportPrivateKeyToPEM(AsymmetricKeyParameter privateKey)
    {
        using (var stringWriter = new StringWriter())
        {
            var pemWriter = new PemWriter(stringWriter);
            pemWriter.WriteObject(privateKey);
            pemWriter.Writer.Flush();
            return stringWriter.ToString();
        }
    }
}