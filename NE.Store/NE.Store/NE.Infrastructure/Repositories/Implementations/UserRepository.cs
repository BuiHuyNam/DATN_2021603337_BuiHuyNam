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
    public class UserRepository : GenericRepository<User>, IUserRepository
    {
        public UserRepository(NEContext context) : base(context)
        {
        }

        public override async Task<IEnumerable<User>> GetAllAsync()
        {
            return await _context.Set<User>()
                .Include(r=>r.Role)
                .Include(w=>w.Ward)
                .ToListAsync();
        }

        public override async Task<User> GetByIdAsync(int id)
        {
            return await _context.Set<User>()
                .Include(r => r.Role)
                .Include(w => w.Ward)
                .Where(u => u.Id == id)
                .FirstOrDefaultAsync();
        }
    }
}
