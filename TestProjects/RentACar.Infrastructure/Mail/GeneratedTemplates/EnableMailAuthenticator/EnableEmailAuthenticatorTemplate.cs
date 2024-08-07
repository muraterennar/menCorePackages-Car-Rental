using Microsoft.Extensions.Configuration;
using RentACar.Infrastructure.Mail.Constants;

namespace RentACar.Infrastructure.Mail.GeneratedTemplates.EnableMailAuthenticator;

public class EnableEmailAuthenticatorTemplate : IEnableEmailAuthenticatorTemplate
{
    private readonly MailTemplateSettings _mailTemplateSettings;

    public EnableEmailAuthenticatorTemplate(IConfiguration configuration)
    {
        _mailTemplateSettings = configuration.GetSection("MailTemplateSettings").Get<MailTemplateSettings>()
                                ?? throw new ArgumentNullException(nameof(MailTemplateSettings));
    }

    public string GenerateBody(string activationKey, string mailTemplateNames)
    {
        string mailTemplate = GetMailTemplateRoot(mailTemplateNames);

        string htmlContent = EmailHtmlBodyGenerate(activationKey, mailTemplate, _mailTemplateSettings);

        return htmlContent;
    }

    public async Task<string> GenerateBodyAsync(string activationKey, string mailTemplateNames)
    {
        using HttpClient client = new HttpClient();
        HttpResponseMessage response = await client.GetAsync(mailTemplateNames);
        if (response.IsSuccessStatusCode)
        {
            string mailTemplate = await response.Content.ReadAsStringAsync();
            string htmlContent = EmailHtmlBodyGenerate(activationKey, mailTemplate, _mailTemplateSettings);
            return htmlContent;
        }
        else
        {
            return new ArgumentException("Mail template is not found!, Please check the url.").Message;
        }
    }

    private static string EmailHtmlBodyGenerate(string activationKey, string mailTemplate,
        MailTemplateSettings mailTemplateSettings)
    {
        string htmlContent = mailTemplate;

        htmlContent = htmlContent.Replace("{{mail_title}}", mailTemplateSettings.Title);
        htmlContent = htmlContent.Replace("{{mail_logo_link}}", mailTemplateSettings.MailLogo);

        htmlContent = htmlContent.Replace("{{mail_category1}}", mailTemplateSettings.MailCategoryLink1);
        htmlContent = htmlContent.Replace("{{mail_category_link1}}", mailTemplateSettings.MailCategoryLink1);
        htmlContent = htmlContent.Replace("{{mail_category2}}", mailTemplateSettings.MailCategory2);
        htmlContent = htmlContent.Replace("{{mail_category_link2}}", mailTemplateSettings.MailCategoryLink2);
        htmlContent = htmlContent.Replace("{{mail_category3}}", mailTemplateSettings.MailCategory3);
        htmlContent = htmlContent.Replace("{{mail_category_link23}}", mailTemplateSettings.MailCategoryLink3);
        htmlContent = htmlContent.Replace("{{mail_category4}}", mailTemplateSettings.MailCategory4);
        htmlContent = htmlContent.Replace("{{mail_category_link4}}", mailTemplateSettings.MailCategoryLink4);

        htmlContent = htmlContent.Replace("{{mail_activated_link}}", activationKey);

        htmlContent = htmlContent.Replace("{{mail_facebook_link}}", mailTemplateSettings.FacebookLink);
        htmlContent = htmlContent.Replace("{{mail_x_link}}", mailTemplateSettings.XLink);
        htmlContent = htmlContent.Replace("{{mail_instagram_link}}", mailTemplateSettings.InstagramLink);
        htmlContent = htmlContent.Replace("{{mail_youtube_link}}", mailTemplateSettings.YoutubeLink);

        htmlContent = htmlContent.Replace("{{mail_company_name}}", mailTemplateSettings.CompanyName);
        htmlContent = htmlContent.Replace("{{mail_company_year}}", mailTemplateSettings.CompanyYear);
        htmlContent = htmlContent.Replace("{{mail_company_address}}", mailTemplateSettings.CompanyAddress);

        htmlContent = htmlContent.Replace("{{mail_visit_link}}", mailTemplateSettings.MailVisitUs);
        htmlContent = htmlContent.Replace("{{mail_privacy_link}}", mailTemplateSettings.MailPrivacyPolicy);
        htmlContent = htmlContent.Replace("{{mail_terms_link}}", mailTemplateSettings.MailTermsOfUse);

        return htmlContent;
    }

    private string GetMailTemplateRoot(string mailTemplateName)
    {
        // var path = Path.Combine(Directory.GetCurrentDirectory(), "~/TestProjects/RentACar.WebAPI/wwwroot/Templates")
        var path = Path.Combine(Directory.GetCurrentDirectory(),
            "/Users/muraterennar/Projects/menCorePackages/TestProjects/RentACar.WebAPI/wwwroot/Templates",
            mailTemplateName);
        return mailTemplateName;
    }

    public async Task<string> GetMailTemplate(string mailTemplateUrl)
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