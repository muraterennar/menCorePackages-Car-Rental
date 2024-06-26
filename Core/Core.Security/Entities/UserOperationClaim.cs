﻿using MenCore.Persistence.Repositories;

namespace MenCore.Security.Entities;

// UserOperationClaim sınıfı, Entity<int> sınıfından türetiliyor
public class UserOperationClaim : Entity<int>
{
    // Varsayılan yapılandırıcı
    public UserOperationClaim()
    {
    }

    // Sadece id parametresini alan yapılandırıcı
    public UserOperationClaim(int id) : base(id)
    {
    }

    // Tüm özelliklerin yanı sıra tarih bilgilerini de içeren yapılandırıcı
    public UserOperationClaim(int id, DateTime createdDate, DateTime? updatedDate, DateTime? deletedDate) : base(id,
        createdDate, updatedDate, deletedDate)
    {
    }

    // Kullanıcı ID'si
    public int UserId { get; set; }

    // Operasyon talebi ID'si
    public int OperationClaimId { get; set; }

    // Kullanıcı referansı
    public virtual User User { get; set; }

    // Operasyon talebi referansı
    public virtual OperationClaim OperationClaim { get; set; }
}