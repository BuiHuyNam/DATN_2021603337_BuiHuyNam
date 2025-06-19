using NE.Domain.Entitis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NE.Application.Services.Interfaces
{
    public interface IBrandService
    {
        Task<IEnumerable<Brand>> GetAllBrandAsync();
        Task<Brand> GetBrandByIdAsync(int id);
        Task AddBrandAsync(Brand brand);
        Task DeleteBrandAsync(int id);                   
        Task UpdateBrandAsync(Brand brand);
        Task IsActiveBrand(Brand brand);
    }
}
