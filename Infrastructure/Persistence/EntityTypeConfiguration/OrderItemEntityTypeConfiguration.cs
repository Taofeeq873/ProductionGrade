using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.EntityTypeConfiguration;

public class OrderItemEntityTypeConfiguration : IEntityTypeConfiguration<OrderItem>
{
    public void Configure(EntityTypeBuilder<OrderItem> builder)
    {
        builder.ToTable("OrderItems");
        builder.HasKey(o => o.Id);

        builder.HasOne(oi => oi.Order)
            .WithMany(oi => oi.Items)
            .HasForeignKey(oi => oi.OrderId);


        builder.HasOne(oi => oi.Product)
            .WithMany()
            .HasForeignKey(oi => oi.ProductId);


        builder.Property(oi => oi.CreatedAt)
            .IsRequired();

        builder.Property(oi => oi.ModifiedAt);

    }
}