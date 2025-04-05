using NE.Domain.Entitis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NE.Application.Services.Interfaces
{
    public interface IColorService 
    {
        Task<IEnumerable<Color>> GetAllColorAsync();
        Task<Color> GetColorByIdAsync(int id);
        Task AddColorAsync(Color color);
        Task DeleteColorAsync(int id);
        Task UpdateColorAsync(Color color);

    }
}
