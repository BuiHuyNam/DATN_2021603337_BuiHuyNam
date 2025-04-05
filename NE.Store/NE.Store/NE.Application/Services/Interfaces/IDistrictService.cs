using NE.Domain.Entitis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NE.Application.Services.Interfaces
{
    public interface IDistrictService
    {
        Task<IEnumerable<District>> GetAllDistrictAsync();
        Task<District> GetDistrictByIdAsync(int id);
        Task AddDistrictAsync(District district);
        Task DeleteDistrictAsync(int id);
        Task UpdateDistrictAsync(District district);
    }
}
