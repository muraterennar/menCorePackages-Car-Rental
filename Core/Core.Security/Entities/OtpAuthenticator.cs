using MenCore.Persistence.Repositories;

namespace MenCore.Security.Entities;

// OtpAuthenticator sınıfı, Entity<int> sınıfından türetiliyor
public class OtpAuthenticator : Entity<int>
{
    // Varsayılan yapılandırıcı
    public OtpAuthenticator()
    {
    }

    // Sadece id parametresini alan yapılandırıcı
    public OtpAuthenticator(int id) : base(id)
    {
    }

    // Tüm özelliklerin yanı sıra tarih bilgilerini de içeren yapılandırıcı
    public OtpAuthenticator(int id, DateTime createdDate, DateTime? updatedDate, DateTime? deletedDate) : base(id,
        createdDate, updatedDate, deletedDate)
    {
    }

    // Parametreleri kullanarak bir OTP doğrulayıcı oluşturan yapılandırıcı
    public OtpAuthenticator(int userId, byte[] secretKey, bool isVerified) : this(userId)
    {
        SecretKey = secretKey;
        IsVerified = isVerified;
    }

    // Kullanıcı ID'si
    public int UserId { get; set; }

    // Gizli anahtar (Secret Key)
    public byte[] SecretKey { get; set; }

    // Doğrulama durumu
    public bool IsVerified { get; set; }

    public virtual User User { get; set; }
}