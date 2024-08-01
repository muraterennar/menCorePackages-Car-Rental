using MenCore.Security.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace RentACar.Persistence.EntityConfigurations;

public class PasswordResetConfiguration: IEntityTypeConfiguration<PasswordReset>
{
    public void Configure(EntityTypeBuilder<PasswordReset> builder)
    {
        builder.ToTable("PasswordResets").HasKey(pr => pr.Id);
        builder.Property(p=>p.Id).HasColumnName("Id").IsRequired();
        builder.Property(p=>p.UserId).HasColumnName("UserId").IsRequired();
        builder.Property(p=>p.Token).HasColumnName("Token").IsRequired();
        builder.Property(p=>p.ExpiryDate).HasColumnName("ExpiryDate").IsRequired();
        builder.Property(p=>p.CreatedDate).HasColumnName("CreatedDate").IsRequired();
        builder.Property(p=>p.UpdatedDate).HasColumnName("UpdatedDate");
        builder.Property(p=>p.DeletedDate).HasColumnName("DeletedDate");
        
        builder.HasIndex(b => b.Id, "UK_PasswordResets_Id").IsUnique();
        
        builder.HasQueryFilter(u => !u.DeletedDate.HasValue);

        builder.HasOne(p => p.User);
    }
}