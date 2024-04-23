namespace MenCore.Mailing;

public interface IMailService
{
    // E-posta gönderme işlemini gerçekleştirir (senkron)
    void SendMail(Mail mail);

    // E-posta gönderme işlemini gerçekleştirir (asenkron)
    // Gönderme işlemi asenkron olduğu için bu metodun Task döndürmesi beklenir
    Task SendEmailAsync(Mail mail);
}