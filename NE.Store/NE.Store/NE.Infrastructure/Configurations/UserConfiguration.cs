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
    public class UserConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.ToTable(nameof(User));
            builder.HasKey(x => x.Id);

            //Thiết lập id tự tăng
            builder.Property(x => x.Id).ValueGeneratedOnAdd();

            //Thiết lập UserName bắt buộc và tối đa 50 kí tự
            builder.Property(x => x.UserName).IsRequired().HasMaxLength(50);

            //Thiết lập Password bắt buộc và tối đa 50 kí tự
            builder.Property(x => x.Password).IsRequired();

            //Thiết lập Email bắt buộc và tối đa 30 kí tự
            builder.Property(x => x.Email).IsRequired().HasMaxLength(30);

            //Thiết lập quan hệ vs Role (N:1)
            builder.HasOne<Role>(x => x.Role).WithMany(x => x.Users).HasForeignKey(x => x.RoleId);

            //Thiết lập quan hệ với Ward(N:1)
            builder.HasOne<Ward>(x => x.Ward).WithMany(x => x.Users).HasForeignKey(x => x.WardId).IsRequired(false); // Cho phép null;

            builder.Property(e => e.CreatedDate)
               .HasColumnType("datetime2")
               .HasDefaultValueSql("GETDATE()");

            builder.Property(e => e.UpdatedDate)
                .HasColumnType("datetime2")
                .HasDefaultValueSql("GETUTCDATE()");

            // Thiết lập IsActive mặc định là true
            builder.Property(x => x.IsActive)
                .HasDefaultValue(true);
        }
    }
}
