using Microsoft.EntityFrameworkCore;
using NE.Domain.Entitis;
using NE.Infrastructure.Configurations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NE.Infrastructure.Context
{
    public class NEContext : DbContext
    {
        public NEContext(DbContextOptions options) : base(options)
        {
            ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;

        }

        public DbSet<Province> Provinces { get; set; }
        public DbSet<District> Districts { get; set; }
        public DbSet<Ward> Wards { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Brand> Brands { get; set; }
        public DbSet<Color> Colors { get; set; }
        public DbSet<ProductColor> ProductColors { get; set; }
        public DbSet<ImageFile> ImageFiles { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<Feedback> Feedbacks { get; set; }
        public DbSet<Cart> Carts { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<Coupon> Coupons { get; set; }
        public DbSet<OrderDetail> OrderDetails { get; set; }



        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.ApplyConfiguration(new ProvinceConfiguration());
            modelBuilder.ApplyConfiguration(new DistrictConfiguration());
            modelBuilder.ApplyConfiguration(new WardConfiguration());
            modelBuilder.ApplyConfiguration(new UserConfiguration());
            modelBuilder.ApplyConfiguration(new RoleConfiguration());
            modelBuilder.ApplyConfiguration(new CategoryConfiguration());
            modelBuilder.ApplyConfiguration(new BrandConfiguration());
            modelBuilder.ApplyConfiguration(new ColorConfiguration());
            modelBuilder.ApplyConfiguration(new ProductColorConfiguration());
            modelBuilder.ApplyConfiguration(new ImageFileConfiguration());
            modelBuilder.ApplyConfiguration(new ProductConfiguration());
            modelBuilder.ApplyConfiguration(new FeedbackConfiguration());
            modelBuilder.ApplyConfiguration(new CartConfiguration());
            modelBuilder.ApplyConfiguration(new OrderConfiguration());
            modelBuilder.ApplyConfiguration(new CouponConfiguration());
            modelBuilder.ApplyConfiguration(new OrderDetailConfiguration());


            //seed data
            modelBuilder.Entity<Role>().HasData(
                new Role { Id = 1, RoleName = "User", CreatedDate = DateTime.Now, UpdatedDate = DateTime.Now },
                new Role { Id = 2, RoleName = "Admin", CreatedDate = DateTime.Now, UpdatedDate = DateTime.Now }
            );


            modelBuilder.Entity<User>().HasData(
                new User
                {
                    Id = 1,
                    Email = "user1@gmail.com",
                    UserName = "user1",
                    Password = "AQAAAAIAAYagAAAAEBNGex1hl4E+gtdA5aLSEdtgfmzxAVdesbVZX7urB568FV8LRAUZYLf9PKBuy1qRKA==",
                    FullName = "Nguyễn Văn A",
                    PhoneNumber = "0987643298",
                    DateOfBirth = new DateTime(1990, 1, 1),
                    RoleId = 1,
                    CreatedDate = DateTime.Now,
                    UpdatedDate = DateTime.Now
                },
                new User
                {
                    Id = 2,
                    Email = "user2@gmail.com",
                    UserName = "user2",
                    Password = "AQAAAAIAAYagAAAAEBNGex1hl4E+gtdA5aLSEdtgfmzxAVdesbVZX7urB568FV8LRAUZYLf9PKBuy1qRKA==",
                    FullName = "Nguyễn Văn B",
                    PhoneNumber = "0987643299",
                    DateOfBirth = new DateTime(1990, 1, 1),
                    RoleId = 1,
                    CreatedDate = DateTime.Now,
                    UpdatedDate = DateTime.Now
                },
                new User
                {
                    Id = 3,
                    Email = "user3@gmail.com",
                    UserName = "user3",
                    Password = "AQAAAAIAAYagAAAAEBNGex1hl4E+gtdA5aLSEdtgfmzxAVdesbVZX7urB568FV8LRAUZYLf9PKBuy1qRKA==",
                    FullName = "Nguyễn Văn C",
                    PhoneNumber = "0987643297",
                    DateOfBirth = new DateTime(1990, 1, 1),
                    RoleId = 1,
                    CreatedDate = DateTime.Now,
                    UpdatedDate = DateTime.Now
                },
                    new User
                    {
                        Id = 4,
                        Email = "admin1@gmail.com",
                        UserName = "admin1",
                        Password = "AQAAAAIAAYagAAAAEBNGex1hl4E+gtdA5aLSEdtgfmzxAVdesbVZX7urB568FV8LRAUZYLf9PKBuy1qRKA==",
                        FullName = "Bùi Huy Nam",
                        PhoneNumber = "0987643298",
                        DateOfBirth = new DateTime(1990, 1, 1),
                        RoleId = 2,
                        CreatedDate = DateTime.Now,
                        UpdatedDate = DateTime.Now
                    },
                        new User
                        {
                            Id = 5,
                            Email = "admin2@gmail.com",
                            UserName = "admin2",
                            Password = "AQAAAAIAAYagAAAAEBNGex1hl4E+gtdA5aLSEdtgfmzxAVdesbVZX7urB568FV8LRAUZYLf9PKBuy1qRKA==",
                            FullName = "Bùi Huy Nam",
                            PhoneNumber = "0987643298",
                            DateOfBirth = new DateTime(1990, 1, 1),
                            RoleId = 2,
                            CreatedDate = DateTime.Now,
                            UpdatedDate = DateTime.Now
                        }
                );


            modelBuilder.Entity<Category>().HasData(
                new Category { Id = 1, CategoryName = "Điện thoại", CreatedDate = DateTime.Now, UpdatedDate = DateTime.Now },
                new Category { Id = 2, CategoryName = "Máy tính bảng", CreatedDate = DateTime.Now, UpdatedDate = DateTime.Now },
                new Category { Id = 3, CategoryName = "Laptop", CreatedDate = DateTime.Now, UpdatedDate = DateTime.Now },
                new Category { Id = 4, CategoryName = "Đồng hồ", CreatedDate = DateTime.Now, UpdatedDate = DateTime.Now },
                new Category { Id = 5, CategoryName = "Tai nghe", CreatedDate = DateTime.Now, UpdatedDate = DateTime.Now },
                new Category { Id = 6, CategoryName = "Phụ kiện", CreatedDate = DateTime.Now, UpdatedDate = DateTime.Now },
                new Category { Id = 7, CategoryName = "PC", CreatedDate = DateTime.Now, UpdatedDate = DateTime.Now },
                new Category { Id = 8, CategoryName = "Tivi", CreatedDate = DateTime.Now, UpdatedDate = DateTime.Now },
                new Category { Id = 9, CategoryName = "Tủ lạnh", CreatedDate = DateTime.Now, UpdatedDate = DateTime.Now },
                new Category { Id = 10, CategoryName = "Điều hòa", CreatedDate = DateTime.Now, UpdatedDate = DateTime.Now },
                new Category { Id = 11, CategoryName = "Lò vi sóng", CreatedDate = DateTime.Now, UpdatedDate = DateTime.Now }
            );

            modelBuilder.Entity<Brand>().HasData(
                new Brand { Id = 1, BrandName = "Apple", IsActive = true, CreatedDate = DateTime.Now, UpdatedDate = DateTime.Now },
                new Brand { Id = 2, BrandName = "Samsung", IsActive = true, CreatedDate = DateTime.Now, UpdatedDate = DateTime.Now },
                new Brand { Id = 3, BrandName = "Xiaomi", IsActive = true, CreatedDate = DateTime.Now, UpdatedDate = DateTime.Now },
                new Brand { Id = 4, BrandName = "Oppo", IsActive = true, CreatedDate = DateTime.Now, UpdatedDate = DateTime.Now },
                new Brand { Id = 5, BrandName = "Asus", IsActive = true, CreatedDate = DateTime.Now, UpdatedDate = DateTime.Now },
                new Brand { Id = 6, BrandName = "Dell", IsActive = true, CreatedDate = DateTime.Now, UpdatedDate = DateTime.Now },
                new Brand { Id = 7, BrandName = "HP", IsActive = true, CreatedDate = DateTime.Now, UpdatedDate = DateTime.Now },
                new Brand { Id = 8, BrandName = "LG", IsActive = true, CreatedDate = DateTime.Now, UpdatedDate = DateTime.Now },
                new Brand { Id = 9, BrandName = "Ascer", IsActive = true, CreatedDate = DateTime.Now, UpdatedDate = DateTime.Now },
                new Brand { Id = 10, BrandName = "Panasonic", IsActive = true, CreatedDate = DateTime.Now, UpdatedDate = DateTime.Now }
            );



            modelBuilder.Entity<Color>().HasData(
                new Color { Id = 1, ColorName = "Đen", CreatedDate = DateTime.Now, UpdatedDate = DateTime.Now },
                new Color { Id = 2, ColorName = "Trắng", CreatedDate = DateTime.Now, UpdatedDate = DateTime.Now },
                new Color { Id = 3, ColorName = "Xám", CreatedDate = DateTime.Now, UpdatedDate = DateTime.Now },
                new Color { Id = 4, ColorName = "Đỏ", CreatedDate = DateTime.Now, UpdatedDate = DateTime.Now },
                new Color { Id = 5, ColorName = "Xanh", CreatedDate = DateTime.Now, UpdatedDate = DateTime.Now },
                new Color { Id = 6, ColorName = "Cam", CreatedDate = DateTime.Now, UpdatedDate = DateTime.Now },
                new Color { Id = 7, ColorName = "Vàng", CreatedDate = DateTime.Now, UpdatedDate = DateTime.Now },
                new Color { Id = 8, ColorName = "Hồng", CreatedDate = DateTime.Now, UpdatedDate = DateTime.Now },
                new Color { Id = 9, ColorName = "Tím", CreatedDate = DateTime.Now, UpdatedDate = DateTime.Now },
                new Color { Id = 10, ColorName = "Bạc", CreatedDate = DateTime.Now, UpdatedDate = DateTime.Now }
            );





        }
    }
}
