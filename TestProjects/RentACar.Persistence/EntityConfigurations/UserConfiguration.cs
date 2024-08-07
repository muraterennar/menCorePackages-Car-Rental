using MenCore.Security.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace RentACar.Persistence.EntityConfigurations;

public class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.ToTable("Users").HasKey(u => u.Id);

        builder.Property(u => u.Id).HasColumnName("Id").IsRequired();
        builder.Property(u => u.FirstName).HasColumnName("FirstName").IsRequired();
        builder.Property(u => u.LastName).HasColumnName("LastName").IsRequired();
        builder.Property(u => u.FullName).HasColumnName("FullName");
        builder.Property(u => u.Username).HasColumnName("Username");
        builder.Property(u => u.Email).HasColumnName("Email").IsRequired();
        builder.Property(u => u.IdentityNumber).HasColumnName("IdentityNumber");
        builder.Property(u => u.PasswordHash).HasColumnName("PasswordHash");
        builder.Property(u => u.PasswordSalt).HasColumnName("PasswordSalt");
        builder.Property(u => u.BirthYear).HasColumnName("BirthYear");
        builder.Property(u => u.Status).HasColumnName("Status").IsRequired();
        builder.Property(u => u.Provider).HasColumnName("Provider");
        builder.Property(u=>u.SecurityStamp).HasColumnName("SecurityStamp");
        builder.Property(u => u.AuthenticatorType).HasColumnName("AuthenticatorType");
        builder.Property(u => u.CreatedDate).HasColumnName("CreatedDate").IsRequired();
        builder.Property(u => u.UpdatedDate).HasColumnName("UpdatedDate");
        builder.Property(u => u.DeletedDate).HasColumnName("DeletedDate");

        builder.HasIndex(b => b.Id, "UK_Users_Id").IsUnique();
        builder.HasIndex(b => b.FirstName, "UK_Users_Name");
        builder.HasIndex(b => b.LastName, "UK_Users_LastName");
        builder.HasIndex(b => b.FullName, "UK_Users_FullName");
        builder.HasIndex(b => b.Username, "UK_Users_Username");
        builder.HasIndex(b => b.Email, "UK_Users_Email");
        
        builder.HasQueryFilter(u => !u.DeletedDate.HasValue);

        builder.HasMany(u => u.UserOperationClaims);
        builder.HasMany(u => u.RefreshTokens);
        builder.HasMany(u => u.EmailAuthenticators);
        builder.HasMany(u => u.OtpAuthenticators);
    }
}