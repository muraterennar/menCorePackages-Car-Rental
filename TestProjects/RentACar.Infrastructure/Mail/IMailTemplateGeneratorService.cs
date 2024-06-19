namespace RentACar.Infrastructure.Mail;

public interface IMailTemplateGeneratorService
{
    public string GenerateBody(string activationKey, string mailTemplateNames);
    public Task<string> GenerateBodyAsync(string activationKey, string mailTemplateNames);
}