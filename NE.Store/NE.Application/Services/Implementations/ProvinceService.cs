using NE.Application.Services.Interfaces;
using NE.Domain.Entitis;
using NE.Infrastructure.UnitOfWork;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NE.Application.Services.Implementations
{
    public class ProvinceService : IProvinceService
    {
        private readonly IUnitOfWork _unitOfWork;

        public ProvinceService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task AddProvinceAsync(Province province)
        {
            await _unitOfWork.Provinces.AddAsync(province);
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task DeleteProvinceAsync(int id)
        {
            var province = await _unitOfWork.Provinces.GetByIdAsync(id);
            if (province != null)
            {
                await _unitOfWork.Provinces.Delete(province);
                await _unitOfWork.SaveChangesAsync();
            }
            else
            {
                throw new Exception("Province does not exist!");
            }
        }

        public async Task<IEnumerable<Province>> GetAllProvinceAsync()
        {
            return await _unitOfWork.Provinces.GetAllAsync();

        }

        public async Task<Province> GetProvinceByIdAsync(int id)
        {
            var province = await _unitOfWork.Provinces.GetByIdAsync(id);
            if (province == null)
            {
                throw new Exception("Province does not exist!");
            }
            return province;
        }

        public async Task UpdateProvinceAsync(Province province)
        {
            var provinceUpdate = await _unitOfWork.Provinces.GetByIdAsync(province.Id);
            if (provinceUpdate == null)
            {
                throw new Exception("Province does not exist!");
            }
            await _unitOfWork.Provinces.Update(province);
            await _unitOfWork.SaveChangesAsync();
        }
    }
}
