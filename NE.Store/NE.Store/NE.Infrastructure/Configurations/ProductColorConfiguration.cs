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
    public class ProductColorConfiguration : IEntityTypeConfiguration<ProductColor>
    {
        public void Configure(EntityTypeBuilder<ProductColor> builder)
        {
            builder.ToTable(nameof(ProductColor));
            builder.HasKey(x => x.Id);


            //Thiết lập id tự tăng
            builder.Property(x => x.Id).ValueGeneratedOnAdd();
            //Thiết lập quantity bắt buộc 
            builder.Property(x => x.Quantity).IsRequired();


            //Thiết lập quan hệ vs Color (N:1)
            builder.HasOne<Color>(x => x.Color).WithMany(x => x.ProductColors).HasForeignKey(x => x.ColorId).OnDelete(DeleteBehavior.Cascade);
            //Thiết lập quan hệ vs Product (N:1)

            builder.HasOne<Product>(x => x.Product).WithMany(x => x.ProductColors).HasForeignKey(x => x.ProductId).OnDelete(DeleteBehavior.Cascade);



            builder.Property(e => e.CreatedDate)
               .HasColumnType("datetime2")
               .HasDefaultValueSql("GETDATE()");

            builder.Property(e => e.UpdatedDate)
                .HasColumnType("datetime2")
                .HasDefaultValueSql("GETUTCDATE()");



        }
    }
}
