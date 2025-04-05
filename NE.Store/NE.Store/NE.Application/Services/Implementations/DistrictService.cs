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
    public class DistrictService : IDistrictService
    {
        private readonly IUnitOfWork _unitOfWork;

        public DistrictService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task AddDistrictAsync(District district)
        {
            await _unitOfWork.Districts.AddAsync(district);
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task DeleteDistrictAsync(int id)
        {
            var district = await _unitOfWork.Districts.GetByIdAsync(id);
            if (district != null)
            {
                await _unitOfWork.Districts.Delete(district);
                await _unitOfWork.SaveChangesAsync();
            }
            else
            {
                throw new Exception("District does not exist!");
            }
        }

        public async Task<IEnumerable<District>> GetAllDistrictAsync()
        {
            return await _unitOfWork.Districts.GetAllAsync();
        }

        public async Task<District> GetDistrictByIdAsync(int id)
        {
            var district = await _unitOfWork.Districts.GetByIdAsync(id);
            if(district == null)
            {
                throw new Exception("District does not exist!");
            }
            return district;
        }

        public async Task UpdateDistrictAsync(District district)
        {
            var districtUpdate = await _unitOfWork.Districts.GetByIdAsync(district.Id);
            if (districtUpdate == null)
            {
                throw new Exception("District does not exist!");
            }
            await _unitOfWork.Districts.Update(district);
            await _unitOfWork.SaveChangesAsync();
        }
    }
}
