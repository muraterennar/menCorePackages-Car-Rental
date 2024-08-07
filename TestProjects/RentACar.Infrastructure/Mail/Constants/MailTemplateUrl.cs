namespace RentACar.Infrastructure.Mail.Constants;

public abstract class MailTemplateUrl
{
    public const string CodeEmailAuthenticatorTemplate = "https://firebasestorage.googleapis.com/v0/b/mencarrental.appspot.com/o/emailTemplate%2Fconfirm_your_mail_template.html?alt=media&token=212c3dcc-2b78-4149-960a-6a7d558ba14b";
    public const string VerifyEmailAuthenticatorTemplate = "https://firebasestorage.googleapis.com/v0/b/mencarrental.appspot.com/o/emailTemplate%2Fverify_to_email.html?alt=media&token=7fe069e3-51d4-4ab8-8fd2-48773c29b190";
    public const string ResetPasswordTemplate = "https://firebasestorage.googleapis.com/v0/b/mencarrental.appspot.com/o/emailTemplate%2FResetPasswordTemplate.html?alt=media&token=921362c6-27ba-408a-a64c-ac4b42068632";
}