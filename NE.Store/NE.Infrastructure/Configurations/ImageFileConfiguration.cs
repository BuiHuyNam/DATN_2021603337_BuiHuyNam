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
    public class ImageFileConfiguration : IEntityTypeConfiguration<ImageFile>
    {
        public void Configure(EntityTypeBuilder<ImageFile> builder)
        {
            builder.ToTable(nameof(ImageFile));
            builder.HasKey(x => x.Id);

            //Thiết lập id tự tăng
            builder.Property(x => x.Id).ValueGeneratedOnAdd();
            //Thiết lập  bắt buộc 
            builder.Property(x => x.FileName).IsRequired();
            builder.Property(x => x.FileData).IsRequired();
            builder.Property(x => x.ContentType).IsRequired();



            builder.HasOne(i => i.ProductColor).WithMany(pc => pc.ImageFiles).HasForeignKey(i => i.ProductColorId).OnDelete(DeleteBehavior.Cascade); 
            builder.HasOne(i => i.Product).WithMany(p => p.ImageFiles).HasForeignKey(i => i.ProductId).OnDelete(DeleteBehavior.NoAction);
            builder.HasOne(i => i.Color).WithMany(c => c.ImageFiles).HasForeignKey(i => i.ColorId).OnDelete(DeleteBehavior.NoAction);

            builder.Property(e => e.CreatedDate)
               .HasColumnType("datetime2")
               .HasDefaultValueSql("GETDATE()");

            builder.Property(e => e.UpdatedDate)
                .HasColumnType("datetime2")
                .HasDefaultValueSql("GETUTCDATE()");

        }
    }
}
