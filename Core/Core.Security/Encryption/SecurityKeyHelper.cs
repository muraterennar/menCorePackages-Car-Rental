using System.Text;
using Microsoft.IdentityModel.Tokens;

namespace Core.Security.Encryption
{
    // SecurityKeyHelper sınıfı
    public static class SecurityKeyHelper
    {
        // Verilen güvenlik anahtarı ile bir SymmetricSecurityKey oluşturan metot
        public static SecurityKey CreateSecurityKey(string securityKey) => new SymmetricSecurityKey(Encoding.UTF8.GetBytes(securityKey));
    }
}
