using NE.Domain.Entitis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NE.Application.Services.Interfaces
{
    public interface IProvinceService
    {
        Task<IEnumerable<Province>> GetAllProvinceAsync();
        Task<Province> GetProvinceByIdAsync(int id);
        Task AddProvinceAsync(Province province);
        Task DeleteProvinceAsync(int id);
        Task UpdateProvinceAsync(Province province);
    }
}
