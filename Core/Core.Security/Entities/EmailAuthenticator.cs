using MenCore.Persistence.Repositories;

namespace MenCore.Security.Entities
{
    // EmailAuthenticator sınıfı, Entity<int> sınıfından türetiliyor
    public class EmailAuthenticator : Entity<int>
    {
        // Kullanıcı ID'si
        public int UserId { get; set; }

        // Aktivasyon anahtarı (Activation Key)
        public string? ActivationKey { get; set; }

        // Doğrulama durumu
        public bool IsVerified { get; set; }

        // Kullanıcı referansı
        public virtual User User { get; set; } = null!;

        // Varsayılan yapılandırıcı
        public EmailAuthenticator()
        {
        }

        // Sadece id parametresini alan yapılandırıcı
        public EmailAuthenticator(int id) : base(id)
        {
        }

        // Tüm özelliklerin yanı sıra tarih bilgilerini de içeren yapılandırıcı
        public EmailAuthenticator(int id, DateTime createdDate, DateTime? updatedDate, DateTime? deletedDate) : base(id, createdDate, updatedDate, deletedDate)
        {
        }

        // Parametreleri kullanarak bir e-posta doğrulayıcı oluşturan yapılandırıcı
        public EmailAuthenticator(int userId, string? activationKey, bool isVerified) : this(userId)
        {
            ActivationKey = activationKey;
            IsVerified = isVerified;
        }
    }
}
