using Google.Apis.Auth;

namespace RentACar.Infrastructure.Auths.GoogleLogin;

public interface IGoogleAuthService
{
    public GoogleJsonWebSignature.ValidationSettings GoogleGenerateSetting(string clientId);

    public Task<GoogleJsonWebSignature.Payload> ValidateToken(string idToken,
        GoogleJsonWebSignature.ValidationSettings settings);
}