using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Entities.Orders;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Persistence.Data.Configurations
{
    public class DeliveryMethodConfiguration : IEntityTypeConfiguration<DeliveryMethod>
    {
        public void Configure(EntityTypeBuilder<DeliveryMethod> builder)
        {
            builder.Property(p => p.ShortName).HasColumnType("varchar").HasMaxLength(128);
            builder.Property(p => p.Description).HasColumnType("varchar").HasMaxLength(256);
            builder.Property(p => p.DeliveryTime).HasColumnType("varchar").HasMaxLength(128);
            builder.Property(p => p.Price).HasColumnType("decimal(18,2)");

        }
    }
}
