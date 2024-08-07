﻿using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RentACar.Domaim.Entities;

namespace RentACar.Persistence.EntityConfigurations;

public class CarConfiguration : IEntityTypeConfiguration<Car>
{
    public void Configure(EntityTypeBuilder<Car> builder)
    {
        builder.ToTable("Cars").HasKey(b => b.Id);

        builder.Property(b => b.ModelId).HasColumnName("ModelId").IsRequired();
        builder.Property(b => b.Kilometer).HasColumnName("Kilometer");
        builder.Property(b => b.ModelYear).HasColumnName("ModelYear").IsRequired();
        builder.Property(b => b.Plate).HasColumnName("Plate").IsRequired();
        builder.Property(b => b.CarState).HasColumnName("CarState").IsRequired();

        builder.Property(b => b.CreatedDate).HasColumnName("CreatedDate").IsRequired();
        builder.Property(b => b.UpdatedDate).HasColumnName("UpdatedDate");
        builder.Property(b => b.DeletedDate).HasColumnName("DeletedDate");

        builder.HasIndex(b => b.Id, "UK_Cars_Id").IsUnique();

        builder.HasOne(b => b.Model);

        builder.HasQueryFilter(b => !b.DeletedDate.HasValue);
    }
}