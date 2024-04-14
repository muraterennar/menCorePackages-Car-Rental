namespace MenCore.Security.EmailAuthenticator;

public interface IEmailAuthenticatorHelper
{
    // E-posta etkinleştirme anahtarı oluşturur
    // Döndürülen anahtar genellikle uzun ve rastgele bir dizedir
    Task<string> CreateEmailActivationKey();

    // E-posta etkinleştirme kodu oluşturur
    // Döndürülen kod genellikle belirli bir uzunlukta rastgele bir sayıdır
    Task<string> CreateEmailActivationCode();

}