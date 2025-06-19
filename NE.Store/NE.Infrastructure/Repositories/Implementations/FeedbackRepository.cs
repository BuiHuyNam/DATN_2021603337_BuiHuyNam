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
    public class FeedbackRepository : GenericRepository<Feedback>, IFeedbackRepository
    {
        public FeedbackRepository(NEContext context) : base(context)
        {
        }

        public async Task<List<Feedback>> GetByIdProductAsync(int idProduct)
        {
            return await _context.Set<Feedback>()
                .Where(f => f.ProductId == idProduct)
                .OrderByDescending(f=>f.CreatedDate)
                .Include(f=>f.User)
                .ToListAsync();
        }
    }
}
