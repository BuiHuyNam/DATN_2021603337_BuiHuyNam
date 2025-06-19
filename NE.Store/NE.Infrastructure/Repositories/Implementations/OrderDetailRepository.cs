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
    public class OrderDetailRepository : GenericRepository<OrderDetail>, IOrderDetailRepository
    {
        public OrderDetailRepository(NEContext context) : base(context)
        {
            
        }
        public async override Task<OrderDetail> GetByIdAsync(int id)
        {
            return await _context.Set<OrderDetail>()
                .Include(p => p.Product)
                .Include(o=>o.Order)
                .FirstOrDefaultAsync(bp => bp.Id == id);
        }
    }
}
