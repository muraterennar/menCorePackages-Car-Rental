using Google.Apis.Auth;

namespace RentACar.Infrastructure.Auths.GoogleLogin;

public class GoogleAuthManager : IGoogleAuthService
{
    public GoogleJsonWebSignature.ValidationSettings GoogleGenerateSetting(string clientId)
    {
        var settings = new GoogleJsonWebSignature.ValidationSettings();
        settings.Audience = new List<string> { clientId };
        return settings;
    }

    public async Task<GoogleJsonWebSignature.Payload> ValidateToken(string idToken,
        GoogleJsonWebSignature.ValidationSettings settings)
    {
        return await GoogleJsonWebSignature.ValidateAsync(idToken, settings);
    }
}