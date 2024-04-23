using System.Text;
using Microsoft.IdentityModel.Tokens;

namespace MenCore.Security.Encryption;

// SecurityKeyHelper sınıfı
public static class SecurityKeyHelper
{
    // Verilen güvenlik anahtarı ile bir SymmetricSecurityKey oluşturan metot
    public static SecurityKey CreateSecurityKey(string securityKey)
    {
        return new SymmetricSecurityKey(Encoding.UTF8.GetBytes(securityKey));
    }
}