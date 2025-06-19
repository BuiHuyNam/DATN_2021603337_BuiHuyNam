using NE.Domain.Entitis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NE.Application.Services.Interfaces
{
    public interface IWardService
    {
        Task<IEnumerable<Ward>> GetAllWardAsync();
        Task<Ward> GetWardByIdAsync(int id);
        Task AddWardAsync(Ward ward);
        Task DeleteWardAsync(int id);
        Task UpdateWardAsync(Ward ward);
    }
}
