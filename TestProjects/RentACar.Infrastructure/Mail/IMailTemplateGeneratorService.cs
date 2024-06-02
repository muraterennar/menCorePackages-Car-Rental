namespace RentACar.Infrastructure.Mail;

public interface IMailTemplateGeneratorService
{
    public string GenerateBody(string activationKey, string mailTemplateNames);
}