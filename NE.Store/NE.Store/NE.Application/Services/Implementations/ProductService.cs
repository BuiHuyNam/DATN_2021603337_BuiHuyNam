using NE.Application.Services.Interfaces;
using NE.Domain.Entitis;
using NE.Infrastructure.Repositories.Interfaces;
using NE.Infrastructure.UnitOfWork;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NE.Application.Services.Implementations
{
    public class ProductService : IProductService
    {
        private readonly IUnitOfWork _unitOfWork;

        public ProductService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task AddProductAsync(Product product)
        {
            var products = await _unitOfWork.Products.FindAsync(c => c.ProductName == product.ProductName);
            if (products.Any())
            {
                throw new Exception("Product already exists!");
            }
            await _unitOfWork.Products.AddAsync(product);
            await _unitOfWork.SaveChangesAsync();
        }

        public Task DeleteProductAsync(int id)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<Product>> GetAllProductAsync()
        {
            return await _unitOfWork.Products.GetAllAsync();

        }

        public async Task<Product> GetProductByIdAsync(int id)
        {
            var product = await _unitOfWork.Products.GetByIdAsync(id);
            if (product == null)
            {
                throw new Exception("San pham khong ton tai");
            }
            return product;
        }

        public async Task IsActiveProduct(Product product)
        {
            var productUpdate = await _unitOfWork.Products.GetByIdAsync(product.Id);

            if (productUpdate == null)
            {
                throw new Exception("Product does not exist!");
            }

            if (productUpdate.IsActive == true)
            {
                productUpdate.IsActive = false;
            }
            else
            {
                productUpdate.IsActive = true;
            }

            await _unitOfWork.Products.Update(productUpdate);
            await _unitOfWork.SaveChangesAsync();
        }

        public Task UpdateProductAsync(Product product)
        {
            throw new NotImplementedException();
        }
    }
}
