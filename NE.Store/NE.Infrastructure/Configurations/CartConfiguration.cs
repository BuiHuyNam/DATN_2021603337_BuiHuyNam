using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NE.Domain.Entitis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NE.Infrastructure.Configurations
{
    public class CartConfiguration : IEntityTypeConfiguration<Cart>
    {
        public void Configure(EntityTypeBuilder<Cart> builder)
        {
            builder.ToTable(nameof(Cart));
            builder.HasKey(x => x.Id);

            //Thiết lập id tự tăng
            builder.Property(x => x.Id).ValueGeneratedOnAdd();
            //Thiết lập Quantity bắt buộc 
            builder.Property(x => x.Quatity).IsRequired();

            builder.HasOne<User>(x => x.User).WithMany(x => x.Carts).HasForeignKey(x => x.UserId);
            builder.HasOne<Product>(x => x.Product).WithMany(x => x.Carts).HasForeignKey(x => x.ProductId);


         

            builder.Property(e => e.CreatedDate)
               .HasColumnType("datetime2")
               .HasDefaultValueSql("GETDATE()");

            builder.Property(e => e.UpdatedDate)
                .HasColumnType("datetime2")
                .HasDefaultValueSql("GETUTCDATE()");

        }
    }
}
