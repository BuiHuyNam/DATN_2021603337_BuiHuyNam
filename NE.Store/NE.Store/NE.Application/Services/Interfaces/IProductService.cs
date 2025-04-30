using NE.Application.Dtos.BaseDto;
using NE.Application.Dtos.ProductDto;
using NE.Domain.Entitis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NE.Application.Services.Interfaces
{
    public interface IProductService
    {
        Task<IEnumerable<Product>> GetAllProductAsync();
        Task<Product> GetProductByIdAsync(int id);
        Task AddProductAsync(Product product);
        Task DeleteProductAsync(int id);
        Task UpdateProductAsync(Product product);
        Task IsActiveProduct(Product product);
        Task<PageResultDto<ProductViewDto>> GetAllProductPage(int page, int pageSize);
        Task<List<Product>> Top5NewProduct();



    }
}
