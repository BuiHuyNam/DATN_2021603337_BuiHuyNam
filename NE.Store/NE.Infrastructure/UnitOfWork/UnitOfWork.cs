using NE.Infrastructure.Context;
using NE.Infrastructure.Repositories.Implementations;
using NE.Infrastructure.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NE.Infrastructure.UnitOfWork
{
    public class UnitOfWork : IUnitOfWork
    {
        protected readonly NEContext _context;
        public IRoleRepository Roles { get; }
        public IProvinceRepository Provinces { get; }
        public IDistrictRepository Districts { get; }
        public IWardRepository Wards { get; }
        public ICategoryRepository Categories { get; }
        public IColorRepository Colors { get; }
        public IBrandRepository Brands { get; }
        public IProductRepository Products { get; }
        public IProductColorRepository ProductColors { get; }
        public IImageFileRepository ImageFiles { get; }
        public ICouponRepository Coupons { get; }
        public IUserRepository Users { get; }
        public ICartRepository Carts { get; }
        public IFeedbackRepository Feedbacks { get; }
        public IOrderRepository Orders { get; }
        public IOrderDetailRepository OrderDetails { get; }
       


        public UnitOfWork(NEContext context)
        {
            _context = context;
            Roles = new RoleRepository(_context);
            Provinces = new ProvinceRepository(_context);
            Districts = new DistrictRepository(_context);
            Wards = new WardRepository(_context);
            Categories = new CategoryRepository(_context);
            Colors = new ColorRepository(_context);
            Brands = new BrandRepository(_context);
            Products = new ProductRepository(_context);
            ProductColors = new ProductColorRepository(_context);
            ImageFiles = new ImageFileRepository(_context);
            Coupons = new CouponRepository(_context);
            Users = new UserRepository(_context);
            Carts = new CartRepository(_context);
            Feedbacks = new FeedbackRepository(_context);
            Orders = new OrderRepository(_context);
            OrderDetails = new OrderDetailRepository(_context);

        }

        public void Dispose()
        {
            _context.Dispose();
        }

        public async Task<int> SaveChangesAsync()
        {
            return await _context.SaveChangesAsync();

        }
    }
}
