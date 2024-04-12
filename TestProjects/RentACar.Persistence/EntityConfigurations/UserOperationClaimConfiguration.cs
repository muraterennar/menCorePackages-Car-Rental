using MenCore.Security.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace RentACar.Persistence.EntityConfigurations;

public class UserOperationClaimConfiguration : IEntityTypeConfiguration<UserOperationClaim>
{
    public void Configure(EntityTypeBuilder<UserOperationClaim> builder)
    {
        builder.ToTable("UserOperationCliams").HasKey(u => u.Id);

        builder.Property(u => u.Id).HasColumnName("Id").IsRequired();
        builder.Property(u => u.OperationClaimId).HasColumnName("OperationClaimId").IsRequired();
        builder.Property(u => u.UserId).HasColumnName("UserId").IsRequired();
        builder.Property(u => u.CreatedDate).HasColumnName("CreatedDate").IsRequired();
        builder.Property(u => u.UpdatedDate).HasColumnName("UpdatedDate");
        builder.Property(u => u.DeletedDate).HasColumnName("DeletedDate");

        builder.HasQueryFilter(u => !u.DeletedDate.HasValue);

        builder.HasOne(u => u.User);
        builder.HasOne(u => u.OperationClaim);
    }
}
