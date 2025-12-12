using System.Text.Json;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.EntityTypeConfigurations;

public class ProductEntityTypeConfiguration : IEntityTypeConfiguration<Product>
{
    public void Configure(EntityTypeBuilder<Product> builder)
    {
        builder.ToTable("Products");

        builder.HasKey(p => p.Id);

        builder.Property(p => p.Name)
            .IsRequired()
            .HasMaxLength(255);

        builder.Property(p => p.Description)
            .HasMaxLength(Int32.MaxValue);

        

        builder.Property(p => p.Status)
            .IsRequired()
            .HasConversion<string>();

       
        builder.Property(p => p.Price)
            .IsRequired()
            .HasColumnType("decimal(18,2)");
        
       
        builder.Property(p => p.Quantity)
            .IsRequired();

        builder.Property(p => p.AvailableQuantity)
            .HasDefaultValue(0)
            .IsRequired();

       
        builder.Property(p => p.CreatedAt)
            .IsRequired();


        builder.Property(p => p.ModifiedAt)
            .IsRequired(false);

        builder.Property(p => p.RowVersion)
            .IsRowVersion()
            .IsRequired(false);
    }
}