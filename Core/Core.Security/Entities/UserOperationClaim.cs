using MenCore.Persistence.Repositories;

namespace Core.Security.Entities
{
    // UserOperationClaim sınıfı, Entity<int> sınıfından türetiliyor
    public class UserOperationClaim : Entity<int>
    {
        // Kullanıcı ID'si
        public int UserId { get; set; }

        // Operasyon talebi ID'si
        public int OperationClaimId { get; set; }

        // Kullanıcı referansı
        public virtual User User { get; set; }

        // Operasyon talebi referansı
        public virtual OperationClaim OperationClaims { get; set; }

        // Varsayılan yapılandırıcı
        public UserOperationClaim()
        {

        }

        // Sadece id parametresini alan yapılandırıcı
        public UserOperationClaim(int id) : base(id)
        {
        }

        // Tüm özelliklerin yanı sıra tarih bilgilerini de içeren yapılandırıcı
        public UserOperationClaim(int id, DateTime createdDate, DateTime? updatedDate, DateTime? deletedDate) : base(id, createdDate, updatedDate, deletedDate)
        {
        }
    }
}
