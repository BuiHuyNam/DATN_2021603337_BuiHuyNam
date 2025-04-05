using NE.Domain.Entitis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NE.Application.Services.Interfaces
{
    public interface IRoleService
    {
        Task<IEnumerable<Role>> GetAllRoleAsync();
        Task<Role> GetRoleByIdAsync(int id);
        Task AddRoleAsync(Role role);
        Task DeleteRoleAsync(int id);
        Task UpdateRoleAsync(Role role);
    }
}
