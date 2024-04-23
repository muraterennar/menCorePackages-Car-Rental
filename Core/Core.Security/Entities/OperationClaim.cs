using MenCore.Persistence.Repositories;

namespace MenCore.Security.Entities;

// OperationClaim sınıfı, Entity<int> sınıfından türetiliyor
public class OperationClaim : Entity<int>
{
    // Varsayılan yapılandırıcı
    public OperationClaim()
    {
    }

    // Sadece id parametresini alan yapılandırıcı
    public OperationClaim(int id) : base(id)
    {
    }

    // Tüm özelliklerin yanı sıra tarih bilgilerini de içeren yapılandırıcı
    public OperationClaim(int id, DateTime createdDate, DateTime? updatedDate, DateTime? deletedDate) : base(id,
        createdDate, updatedDate, deletedDate)
    {
    }

    // Operasyonun adı
    public string Name { get; set; }

    // Kullanıcı operasyon taleplerinin koleksiyonu
    public virtual ICollection<UserOperationClaim> UserOperationClaims { get; set; } = null!;
}