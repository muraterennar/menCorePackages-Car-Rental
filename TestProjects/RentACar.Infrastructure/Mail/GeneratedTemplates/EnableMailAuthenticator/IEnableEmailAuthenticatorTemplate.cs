namespace RentACar.Infrastructure.Mail.GeneratedTemplates.EnableMailAuthenticator;

public interface IEnableEmailAuthenticatorTemplate
{
    public Task<string> GenerateBodyAsync(string activationKey, string mailTemplateNames);
}