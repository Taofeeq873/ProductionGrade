using Domain.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Enum;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Infrastructure.Persistence.EntityTypeConfiguration
{
    public class OrderEntityTypeConfiguration : IEntityTypeConfiguration<Order>
    {
        public void Configure(EntityTypeBuilder<Order> builder)
        {
            builder.ToTable("Orders");
            builder.HasKey(o => o.Id);

            builder.Property(x => x.Status)
               .HasConversion<EnumToStringConverter<OrderStatus>>()
               .HasDefaultValue(OrderStatus.Pending)
               .IsRequired();

            builder.HasIndex(x => x.OrderReference).IsUnique();
            builder.Property(x => x.OrderReference)
                        .HasMaxLength(int.MaxValue)
                        .IsRequired();
            builder.Property(o => o.CustomerName).IsRequired().HasMaxLength(100);
            builder.Property(o => o.CustomerEmail).IsRequired().HasMaxLength(100);
            builder.Property(o => o.CustomerPhoneNumber).IsRequired().HasMaxLength(20);

            builder.Property(u => u.CreatedAt)
                .IsRequired();

            builder.Property(u => u.ModifiedAt);

        

        }
    }

}