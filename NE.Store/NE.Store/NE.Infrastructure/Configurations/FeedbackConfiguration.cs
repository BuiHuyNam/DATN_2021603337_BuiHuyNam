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
    public class FeedbackConfiguration : IEntityTypeConfiguration<Feedback>
    {
        public void Configure(EntityTypeBuilder<Feedback> builder)
        {
            builder.ToTable(nameof(Feedback));
            builder.HasKey(x => x.Id);

            //Thiết lập id tự tăng
            builder.Property(x => x.Id).ValueGeneratedOnAdd();

            //Thiết lập Content bắt buộc 
            builder.Property(x => x.Content).IsRequired();

            //Thiết lập Star bắt buộc
            builder.Property(x => x.Star).IsRequired();

            builder.Property(e => e.CreatedDate)
                .HasColumnType("datetime2")
                .HasDefaultValueSql("GETDATE()");

            builder.Property(e => e.UpdatedDate)
                .HasColumnType("datetime2")
                .HasDefaultValueSql("GETUTCDATE()");

            //Thiết lập mối quan hệ với User (N:1)
            builder.HasOne<User>(x => x.User).WithMany(x => x.Feedbacks).HasForeignKey(x => x.UserId).OnDelete(DeleteBehavior.NoAction);
            //Thiết lập mối quan hệ với Product (N:1)
            builder.HasOne<Product>(x => x.Product).WithMany(x => x.Feedbacks).HasForeignKey(x => x.ProductId).OnDelete(DeleteBehavior.NoAction);


        }
    }
}
