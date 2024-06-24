using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RentACar.Domaim.Entities;

namespace RentACar.Persistence.EntityConfigurations;

public class UserLoginConfiguration : IEntityTypeConfiguration<UserLogin>
{
    public void Configure(EntityTypeBuilder<UserLogin> builder)
    {
        builder.ToTable("UserLogins").HasKey(u => u.Id);

        builder.Property(u => u.Id).HasColumnName("Id").IsRequired();
        builder.Property(u => u.LoginProvider).HasColumnName("LoginProvider").IsRequired();
        builder.Property(u => u.ProviderKey).HasColumnName("ProviderKey").IsRequired();
        builder.Property(u => u.ProviderDisplayName).HasColumnName("ProviderDisplayName");
        builder.Property(u => u.UserId).HasColumnName("UserId").IsRequired();

        builder.HasOne(m => m.User);
        
        builder.HasQueryFilter(u => !u.DeletedDate.HasValue);
    }
}