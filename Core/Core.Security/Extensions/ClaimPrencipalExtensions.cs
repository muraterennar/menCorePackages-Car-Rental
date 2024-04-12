using System.Security.Claims;

namespace MenCore.Security.Extensions;

public static class ClaimPrencipalExtensions
{
    // Bu yöntem, belirli bir talep türüne sahip tüm taleplerin değerlerini bir liste olarak döndürür.
    public static List<string>? Claims(this ClaimsPrincipal claimsPrincipal, string claimType)
    {
        var result = claimsPrincipal?.FindAll(claimType)?.Select(x => x.Value).ToList();
        return result;
    }

    // Bu yöntem, rol taleplerinin değerlerini bir liste olarak döndürür.
    public static List<string>? ClaimRoles(this ClaimsPrincipal claimsPrincipal) => claimsPrincipal?.Claims(ClaimTypes.Role);

    // Bu yöntem, kullanıcı tanımlayıcı talebinin değerini döndürür.
    public static int GetUserId(this ClaimsPrincipal claimsPrincipal) => Convert.ToInt32(claimsPrincipal?.Claims(ClaimTypes.NameIdentifier)?.FirstOrDefault());
}
