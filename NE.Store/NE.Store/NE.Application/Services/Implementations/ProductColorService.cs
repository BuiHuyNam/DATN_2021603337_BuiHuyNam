using NE.Application.Services.Interfaces;
using NE.Domain.Entitis;
using NE.Infrastructure.UnitOfWork;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NE.Application.Services.Implementations
{
    public class ProductColorService : IProductColorService
    {
        private readonly IUnitOfWork _unitOfWork;

        public ProductColorService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task AddProductColorAsync(ProductColor productColor)
        {
            await _unitOfWork.ProductColors.AddAsync(productColor);
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task DeleteProductColorAsync(int id)
        {
            var productColor = await _unitOfWork.ProductColors.GetByIdAsync(id);
            if (productColor != null)
            {
                await _unitOfWork.ProductColors.Delete(productColor);
                await _unitOfWork.SaveChangesAsync();
            }
            else
            {
                throw new Exception("ProductColor does not exist!");
            }
        }

        public async Task<IEnumerable<ProductColor>> GetAllProductColorAsync()
        {
            return await _unitOfWork.ProductColors.GetAllAsync();

        }

        public async Task<ProductColor> GetProductColorByIdAsync(int id)
        {
            var productColor = await _unitOfWork.ProductColors.GetByIdAsync(id);
            if (productColor == null)
            {
                throw new Exception("ProductColor does not exist!");
            }
            return productColor; 
        }

        public async Task UpdateProductColorAsync(ProductColor productColor)
        {
            var productColorUpdate = await _unitOfWork.ProductColors.GetByIdAsync(productColor.Id);

            if (productColorUpdate==null)
            {
                throw new Exception("ProductColor does not exist!");
            }
            productColor.ProductId = productColorUpdate.ProductId;
            productColor.ColorId = productColorUpdate.ColorId;

            await _unitOfWork.ProductColors.Update(productColor);
            await _unitOfWork.SaveChangesAsync();
        }
    }
}
