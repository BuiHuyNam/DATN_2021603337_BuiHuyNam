﻿using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NE.Domain.Entitis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NE.Infrastructure.Configurations
{
    public class ColorConfiguration : IEntityTypeConfiguration<Color>
    {
        public void Configure(EntityTypeBuilder<Color> builder)
        {
            builder.ToTable(nameof(Color));
            builder.HasKey(x => x.Id);

            //Thiết lập id tự tăng
            builder.Property(x => x.Id).ValueGeneratedOnAdd();

            //Thiết lập CategoryName bắt buộc và tối đa 50 kí tự
            builder.Property(x => x.ColorName).IsRequired().HasMaxLength(50);

            builder.Property(e => e.CreatedDate)
                .HasColumnType("datetime2")
                .HasDefaultValueSql("GETDATE()");

            builder.Property(e => e.UpdatedDate)
                .HasColumnType("datetime2")
                .HasDefaultValueSql("GETUTCDATE()");
        }
    }
}
