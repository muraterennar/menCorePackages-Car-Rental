using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using Core.Security.Encryption;
using Core.Security.Entities;
using Core.Security.Extensions;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace Core.Security.JWT;

public class JwtHelper : ITokenHelper
{
    private readonly IConfiguration Configuration;
    private readonly TokenOptions _tokenOptions;
    private DateTime _accessTokenExpiration;

    public JwtHelper(IConfiguration configuration)
    {
        Configuration = configuration;
        const string configurationSection = "TokenOptions";
        // Konfigürasyon dosyasından token seçeneklerini alır veya hata fırlatır.
        _tokenOptions = Configuration.GetSection(configurationSection).Get<TokenOptions>()
            ?? throw new NullReferenceException($"\"{configurationSection}\" section cannot found in configuration.");
    }

    // Kullanıcıya dayalı olarak ve bir IP adresi ile birlikte bir yenileme Token oluşturur.
    public RefreshToken CreateRefreshToken(User user, string ipAddress)
    {
        RefreshToken refreshToken = new()
        {
            UserId = user.Id,
            Token = RandomRefreshToken(),
            Expires = DateTime.UtcNow.AddDays(7),
            CreatedByIp = ipAddress
        };

        return refreshToken;
    }

    // Kullanıcıya ve yetkilendirme taleplerine dayanarak bir erişim Token oluşturur.
    public AccessToken CreateToken(User user, IList<OperationClaim> operationClaims)
    {
        // Erişim belirtecinin son kullanma tarihini hesaplar.
        _accessTokenExpiration = DateTime.Now.AddMinutes(_tokenOptions.AccessTokenExpiration);

        // Güvenlik anahtarını oluşturur.
        SecurityKey security = SecurityKeyHelper.CreateSecurityKey(_tokenOptions.SecurityKey);

        // İmzalama kimlik bilgilerini oluşturur.
        SigningCredentials signingCredentials = SigningCredentialsHelper.CreateSigningCredentials(security);

        // JWT'yi oluşturur.
        JwtSecurityToken jwt = CreateJwtSecurityToken(_tokenOptions, user, signingCredentials, operationClaims);

        // JWT'yi işleyen nesneyi oluşturur.
        JwtSecurityTokenHandler jwtSecurityTokenHandler = new();

        // JWT'yi bir dize olarak yazarak erişim belirteci oluşturur ve döndürür.
        string? token = jwtSecurityTokenHandler.WriteToken(jwt);

        // Oluşturulan erişim belirtecini ve son kullanma tarihini içeren bir AccessToken nesnesi döndürür.
        return new AccessToken { Token = token, Expiration = _accessTokenExpiration };
    }


    // JWT oluşturur.
    public JwtSecurityToken CreateJwtSecurityToken(TokenOptions tokenOptions, User user, SigningCredentials signingCredentials, IList<OperationClaim> operationClaims)
    {
        JwtSecurityToken jwt = new(tokenOptions.Issuer, tokenOptions.Audience, expires: _accessTokenExpiration, notBefore: DateTime.Now, claims: SetClaims(user, operationClaims), signingCredentials: signingCredentials);

        return jwt;
    }

    // JWT için talepleri ayarlar.
    private IEnumerable<Claim> SetClaims(User user, IList<OperationClaim> operationClaims)
    {
        List<Claim> claims = new();
        claims.AddNameIdentifier(user.Id.ToString());
        claims.AddEmail(user.Email);
        claims.AddName(user.FullName);
        claims.AddRoles(operationClaims.Select(c => c.Name).ToArray());
        return claims;
    }

    // Rastgele bir yenileme Token oluşturur.
    private string RandomRefreshToken()
    {
        byte[] numberByte = new byte[32];
        using var random = RandomNumberGenerator.Create();
        random.GetBytes(numberByte);
        return Convert.ToBase64String(numberByte);
    }
}
