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
    public class WardRepository : GenericRepository<Ward>, IWardRepository
    {
        public WardRepository(NEContext context) : base(context)
        {
        }
        public override async Task<IEnumerable<Ward>> GetAllAsync()
        {
            return await _context.Set<Ward>()
                .Include(d=>d.District)
                    .ThenInclude(p=>p.Province)
                .ToListAsync();
        }
    }
}
