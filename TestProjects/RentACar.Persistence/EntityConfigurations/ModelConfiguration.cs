using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RentACar.Domaim.Entities;

namespace RentACar.Persistence.EntityConfigurations;

public class ModelConfiguration : IEntityTypeConfiguration<Model>
{
    public void Configure(EntityTypeBuilder<Model> builder)
    {
        builder.ToTable("Models").HasKey(b => b.Id);
        builder.Property(b => b.Id).HasColumnName("Id").IsRequired();

        builder.Property(b => b.BrandId).HasColumnName("BrandId").IsRequired();
        builder.Property(b => b.TransmissionId).HasColumnName("TransmissionId").IsRequired();
        builder.Property(b => b.FuelId).HasColumnName("FuelId").IsRequired();
        builder.Property(b => b.ModelName).HasColumnName("ModelName").IsRequired();
        builder.Property(b => b.DailyPrice).HasColumnName("DailyPrice").HasPrecision(18, 4).IsRequired();
        builder.Property(b => b.ImageUrl).HasColumnName("ImageUrl");

        builder.Property(b => b.CreatedDate).HasColumnName("CreatedDate").IsRequired();
        builder.Property(b => b.UpdatedDate).HasColumnName("UpdatedDate");
        builder.Property(b => b.DeletedDate).HasColumnName("DeletedDate");

        builder.HasIndex(b => b.ModelName, "UK_Models_Name").IsUnique();

        builder.HasMany(f => f.Cars);
        builder.HasOne(m => m.Transmission);
        builder.HasOne(m => m.Fuel);
        builder.HasOne(m => m.Brand);

        builder.HasQueryFilter(b => !b.DeletedDate.HasValue);
    }
}