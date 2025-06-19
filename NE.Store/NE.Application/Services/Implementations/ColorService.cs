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
    public class ColorService : IColorService
    {
        private readonly IUnitOfWork _unitOfWork;

        public ColorService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task AddColorAsync(Color color)
        {
            var colors = await _unitOfWork.Colors.FindAsync(c => c.ColorName == color.ColorName);
            if (colors.Any())
            {
                throw new Exception("Colors already exists!");
            }
            await _unitOfWork.Colors.AddAsync(color);
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task DeleteColorAsync(int id)
        {
            var color = await _unitOfWork.Colors.GetByIdAsync(id);
            if (color != null)
            {
                await _unitOfWork.Colors.Delete(color);
                await _unitOfWork.SaveChangesAsync();
            }
            else
            {
                throw new Exception("Color does not exist!");
            }
        }

        public async Task<IEnumerable<Color>> GetAllColorAsync()
        {
            return await _unitOfWork.Colors.GetAllAsync();

        }

        public async Task<Color> GetColorByIdAsync(int id)
        {
            var color = await _unitOfWork.Colors.GetByIdAsync(id);
            if (color == null)
            {
                throw new Exception("Color does not exist!");
            }
            return color;
        }

        public async Task UpdateColorAsync(Color color)
        {
            var colorUpdate = await _unitOfWork.Colors.FindAsync(c => c.Id == color.Id);

            if (!colorUpdate.Any())
            {
                throw new Exception("Color does not exist!");
            }

            await _unitOfWork.Colors.Update(color);
            await _unitOfWork.SaveChangesAsync();
        }
    }
}
