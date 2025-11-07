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
    public class OrderConfiguration : IEntityTypeConfiguration<Order>
    {
        public void Configure(EntityTypeBuilder<Order> builder)
        {


            //configure relationships 

            builder.OwnsOne(p => p.ShippingAddress);
            builder.HasOne(p => p.deliveryMethod).WithMany().HasForeignKey(p => p.DeliveryMethodId);

            builder.HasMany(p => p.items).WithOne().OnDelete(DeleteBehavior.Cascade);


            builder.Property(p => p.SubTotal).HasColumnType("decimal(18,2)");



        }
    }
}
