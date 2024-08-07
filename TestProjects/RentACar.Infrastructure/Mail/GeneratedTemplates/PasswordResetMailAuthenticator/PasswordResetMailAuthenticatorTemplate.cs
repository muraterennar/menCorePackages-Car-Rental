using RentACar.Infrastructure.Mail.Constants;

namespace RentACar.Infrastructure.Mail.GeneratedTemplates.PasswordResetMailAuthenticator;

public class PasswordResetMailAuthenticatorTemplate : IPasswordResetMailAuthenticatorTemplate
{
    public async Task<string> GenerateBodyAsync(string activationKey, string mailTemplateNames)
    {
        var mailTemplate = await GetMailTemplate(MailTemplateUrl.ResetPasswordTemplate);
        string httpContent = EmailTemplateGenerate(activationKey, mailTemplate);
        return httpContent;
    }

    private static string EmailTemplateGenerate(string activationKey, string mailTemplate)
    {
        string htmlContent = mailTemplate;
        htmlContent = htmlContent.Replace("{{resetLink}}", activationKey);
        return htmlContent;
    }

    private async Task<string> GetMailTemplate(string mailTemplateUrl)
    {
        using HttpClient client = new HttpClient();
        HttpResponseMessage response = await client.GetAsync(mailTemplateUrl);
        if (response.IsSuccessStatusCode)
        {
            return await response.Content.ReadAsStringAsync();
        }
        else
        {
            return new ArgumentException("Mail template is not found!, Please check the url.").Message;
        }
    }
}