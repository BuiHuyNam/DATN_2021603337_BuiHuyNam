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
    public class BrandConfiguration : IEntityTypeConfiguration<Brand>
    {
        public void Configure(EntityTypeBuilder<Brand> builder)
        {
            builder.ToTable(nameof(Brand));
            builder.HasKey(x => x.Id);

            //Thiết lập id tự tăng
            builder.Property(x => x.Id).ValueGeneratedOnAdd();

            //Thiết lập BrandName bắt buộc và tối đa 50 kí tự
            builder.Property(x => x.BrandName).IsRequired().HasMaxLength(50);

            //Thiết lập isActive mặc định true
            builder.Property(x => x.IsActive).HasDefaultValue(true);

            builder.Property(e => e.CreatedDate)
                .HasColumnType("datetime2")
                .HasDefaultValueSql("GETDATE()");

            builder.Property(e => e.UpdatedDate)
                .HasColumnType("datetime2")
                .HasDefaultValueSql("GETUTCDATE()");
        }
    }
}
