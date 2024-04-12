using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace Core.Security.Extensions;

public static class ClaimExtensions
{
    // Bu yöntem, bir e-posta talebini temsil eden bir "Claim" öğesini bir koleksiyona ekler.
    public static void AddEmail(this ICollection<Claim> claims, string email) =>
        claims.Add(new Claim(JwtRegisteredClaimNames.Email, email));

    // Bu yöntem, bir isim talebini temsil eden bir "Claim" öğesini bir koleksiyona ekler.
    public static void AddName(this ICollection<Claim> claims, string name) =>
        claims.Add(new Claim(JwtRegisteredClaimNames.Name, name));

    // Bu yöntem, bir isim tanımlayıcı talebini temsil eden bir "Claim" öğesini bir koleksiyona ekler.
    public static void AddNameIdentifier(this ICollection<Claim> claims, string nameIdentifier) =>
        claims.Add(new Claim(ClaimTypes.NameIdentifier, nameIdentifier));

    // Bu yöntem, rollerin bir dizisini temsil eden bir "Claim" öğesini bir koleksiyona ekler.
    public static void AddRoles(this ICollection<Claim> claims, string[] roles) =>
        roles.ToList().ForEach(role => claims.Add(new Claim(ClaimTypes.Role, role)));
}
