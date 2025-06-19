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
    public class CartRepository : GenericRepository<Cart>, ICartRepository
    {
        public CartRepository(NEContext context) : base(context)
        {
        }

        public async Task<List<Cart>> GetCartByUserIdAsync(int userId)
        {
            return await _context.Set<Cart>()
                .Where(c => c.UserId == userId)
                .Include(c => c.Product)
                    .ThenInclude(p => p.ProductColors) // Bao gồm ProductColors
                        .ThenInclude(pc => pc.Color) // Bao gồm Color
                        .Include(c => c.Product)
                    .ThenInclude(p => p.ProductColors) // Bao gồm ProductColors
                        .ThenInclude(pc => pc.ImageFiles)

                .ToListAsync();
        }
    }
}
