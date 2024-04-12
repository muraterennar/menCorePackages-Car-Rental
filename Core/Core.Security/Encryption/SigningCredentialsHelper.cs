using Microsoft.IdentityModel.Tokens;

namespace MenCore.Security.Encryption
{
    // SigningCredentialsHelper sınıfı
    public static class SigningCredentialsHelper
    {
        // Verilen güvenlik anahtarı ile bir SigningCredentials oluşturan metot
        public static SigningCredentials CreateSigningCredentials(SecurityKey securityKey) => new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha512Signature);
    }
}
