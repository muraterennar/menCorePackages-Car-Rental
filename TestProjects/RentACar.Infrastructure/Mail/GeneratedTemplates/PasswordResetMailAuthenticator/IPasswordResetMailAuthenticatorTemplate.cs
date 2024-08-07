namespace RentACar.Infrastructure.Mail.GeneratedTemplates.PasswordResetMailAuthenticator;

public interface IPasswordResetMailAuthenticatorTemplate
{
    public Task<string> GenerateBodyAsync(string activationKey, string mailTemplateNames);
}