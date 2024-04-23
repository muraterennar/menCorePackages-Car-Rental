using MenCore.Persistence.Repositories;

namespace MenCore.Security.Entities;

// RefreshToken sınıfı, Entity<int> sınıfından türetiliyor
public class RefreshToken : Entity<int>
{
    // Varsayılan yapılandırıcı
    public RefreshToken()
    {
    }

    // Sadece id parametresini alan yapılandırıcı
    public RefreshToken(int id) : base(id)
    {
    }

    // Tüm özelliklerin yanı sıra tarih bilgilerini de içeren yapılandırıcı
    public RefreshToken(int id, DateTime createdDate, DateTime? updatedDate, DateTime? deletedDate) : base(id,
        createdDate, updatedDate, deletedDate)
    {
    }

    // Parametreleri kullanarak bir yenileme token'i oluşturan yapılandırıcı
    public RefreshToken(int userId, string token, DateTime expires, string createdByIp, DateTime? revoked,
        string? revokedByIp, string? replacedByToken, string? reasonRevoked, User user) : this(userId)
    {
        Token = token;
        Expires = expires;
        CreatedByIp = createdByIp;
        Revoked = revoked;
        RevokedByIp = revokedByIp;
        ReplacedByToken = replacedByToken;
        ReasonRevoked = reasonRevoked;
        User = user;
    }

    // Kullanıcı ID'si
    public int UserId { get; set; }

    // Token
    public string Token { get; set; }

    // Token'in geçerlilik süresi
    public DateTime Expires { get; set; }

    // Token'in oluşturulduğu IP adresi
    public string CreatedByIp { get; set; }

    // Token'in iptal edildiği tarih
    public DateTime? Revoked { get; set; }

    // İptal edilme işlemi tarafından belirtilen IP adresi
    public string? RevokedByIp { get; set; }

    // İptal edilen token tarafından değiştirilen token
    public string? ReplacedByToken { get; set; }

    // Token'in iptal edilme nedeni
    public string? ReasonRevoked { get; set; }

    // Kullanıcı referansı
    public virtual User User { get; set; }
}