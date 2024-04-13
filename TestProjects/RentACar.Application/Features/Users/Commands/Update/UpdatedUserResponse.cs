using MenCore.Security.Enums;

namespace RentACar.Application.Features.Users.Commands.Update;

public class UpdatedUserResponse
{
    public int Id { get; set; }

    // Kullanıcının adı
    public string FirstName { get; set; }

    // Kullanıcının soyadı
    public string LastName { get; set; }

    // Kullanıcının tam adı (ad + soyad)
    public string? FullName { get; set; }

    // Kullanıcının kullanıcı adı (küçük harfe dönüştürülmüş ad ve soyadın birleşimi)
    public string Username { get; set; }

    // Kullanıcının kimlik numarası
    public string? IdentityNumber { get; set; }

    // Kullanıcının doğum yılı
    public short? BirthYear { get; set; }

    // Kullanıcının e-posta adresi
    public string Email { get; set; }

    // Kullanıcının durumu (aktif veya pasif)
    public bool Status { get; set; }

    // Kullanıcının Authentication tipi
    public AuthenticatorType AuthenticatorType { get; set; }
}