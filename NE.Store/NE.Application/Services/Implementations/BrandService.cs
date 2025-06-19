using NE.Application.Services.Interfaces;
using NE.Domain.Entitis;
using NE.Infrastructure.UnitOfWork;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NE.Application.Services.Implementations
{
    public class BrandService : IBrandService
    {
        private readonly IUnitOfWork _unitOfWork;

        public BrandService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task AddBrandAsync(Brand brand)
        {
            var brands = await _unitOfWork.Brands.FindAsync(c => c.BrandName == brand.BrandName);
            if (brands.Any())
            {
                throw new Exception("Brand already exists!");
            }
            await _unitOfWork.Brands.AddAsync(brand);
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task DeleteBrandAsync(int id)
        {
            var brand = await _unitOfWork.Brands.GetByIdAsync(id);
            if (brand != null)
            {
                await _unitOfWork.Brands.Delete(brand);
                await _unitOfWork.SaveChangesAsync();
            }
            else
            {
                throw new Exception("Brand does not exist!");
            }
        }

        public async Task<IEnumerable<Brand>> GetAllBrandAsync()
        {
            return await _unitOfWork.Brands.GetAllAsync();
        }

        public async Task<Brand> GetBrandByIdAsync(int id)
        {
            var brand = await _unitOfWork.Brands.GetByIdAsync(id);
            if (brand == null)
            {
                throw new Exception("Brand does not exist!");
            }
            return brand;
        }

        public async Task IsActiveBrand(Brand brand)
        {
            var brandUpdate = await _unitOfWork.Brands.GetByIdAsync(brand.Id);

            if (brandUpdate == null)
            {
                throw new Exception("Brand does not exist!");
            }

            if(brandUpdate.IsActive == true)
            {
                brandUpdate.IsActive = false;
            }
            else
            {
                brandUpdate.IsActive = true;
            }
            
            await _unitOfWork.Brands.Update(brandUpdate);
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task UpdateBrandAsync(Brand brand)
        {
            //var brandUpdate = await _unitOfWork.Brands.FindAsync(c => c.Id == brand.Id);
            var brandUpdate = await _unitOfWork.Brands.GetByIdAsync(brand.Id);



            if (brandUpdate == null)
            {
                throw new Exception("Brand does not exist!");
            }

            brand.IsActive = brandUpdate.IsActive;

            await _unitOfWork.Brands.Update(brand);
            await _unitOfWork.SaveChangesAsync();
        }
    }
}
