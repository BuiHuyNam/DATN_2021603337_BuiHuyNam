using NE.Domain.Entitis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NE.Application.Services.Interfaces
{
    public interface IProductColorService
    {
        Task<IEnumerable<ProductColor>> GetAllProductColorAsync();
        Task<ProductColor> GetProductColorByIdAsync(int id);
        Task AddProductColorAsync(ProductColor productColor);
        Task DeleteProductColorAsync(int id);
        Task UpdateProductColorAsync(ProductColor productColor);
    }
}
