using Microsoft.EntityFrameworkCore;
using NE.Domain.Entitis;
using NE.Infrastructure.Context;
using NE.Infrastructure.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NE.Infrastructure.Repositories.Implementations
{
    public class OrderRepository : GenericRepository<Order>, IOrderRepository
    {
        public OrderRepository(NEContext context) : base(context)
        {
        }

        public override async Task<IEnumerable<Order>> GetAllAsync()
        {
            return await _context.Set<Order>()
                .Include(od => od.OrderDetails)
                    .ThenInclude(p=>p.Product)
                .Include(u=>u.User)
                    .ThenInclude(w=>w.Ward)
                        .ThenInclude(d=>d.District)
                            .ThenInclude(p=>p.Province)
                .ToListAsync();
        }

        public async override Task<Order> GetByIdAsync(int id)
        {
            return await _context.Set<Order>()
                .Include(u=>u.User)
                .Include(od=>od.OrderDetails) 
                    .ThenInclude(p=>p.Product)
                        .ThenInclude(pc=>pc.ProductColors)
                            .ThenInclude(c=>c.Color)
                .Include(od => od.OrderDetails)
                    .ThenInclude(p => p.Product)
                        .ThenInclude(pc => pc.ProductColors)
                            .ThenInclude(i=>i.ImageFiles)
                .FirstOrDefaultAsync(bp => bp.Id == id);
        }

        public async Task<List<Order>> GetOrderByUserIdAsync(int userId)
        {
            return await _context.Set<Order>()
               .Include(u => u.User)
               .Include(od => od.OrderDetails)
                    .ThenInclude(p=>p.Product)
                        .ThenInclude(pc=>pc.ProductColors)
                            .ThenInclude(c=>c.Color)
                .Include(od => od.OrderDetails)
                    .ThenInclude(p => p.Product)
                        .ThenInclude(pc => pc.ProductColors)
                            .ThenInclude(i=>i.ImageFiles)


               .Where(u => u.UserId == userId)
               .OrderByDescending(x=>x.CreatedDate)
               .ToListAsync();
        }
    }
}
