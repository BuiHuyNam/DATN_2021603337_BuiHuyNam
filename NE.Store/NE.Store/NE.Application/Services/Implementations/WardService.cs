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
    public class WardService : IWardService
    {
        private readonly IUnitOfWork _unitOfWork;

        public WardService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task AddWardAsync(Ward ward)
        {
            await _unitOfWork.Wards.AddAsync(ward);
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task DeleteWardAsync(int id)
        {
            var ward = await _unitOfWork.Wards.GetByIdAsync(id);
            if (ward != null)
            {
                await _unitOfWork.Wards.Delete(ward);
                await _unitOfWork.SaveChangesAsync();
            }
            else
            {
                throw new Exception("Ward does not exist!");
            }
        }

        public async Task<IEnumerable<Ward>> GetAllWardAsync()
        {
            return await _unitOfWork.Wards.GetAllAsync();

        }

        public async Task<Ward> GetWardByIdAsync(int id)
        {
            var ward = await _unitOfWork.Wards.GetByIdAsync(id);
            if (ward == null)
            {
                throw new Exception("Ward does not exist!");
            }
            return ward;
        }

        public async Task UpdateWardAsync(Ward ward)
        {
            var wardUpdate = await _unitOfWork.Wards.GetByIdAsync(ward.Id);
            if (wardUpdate == null)
            {
                throw new Exception("Ward does not exist!");
            }
            await _unitOfWork.Wards.Update(ward);
            await _unitOfWork.SaveChangesAsync();
        }
    }
}
