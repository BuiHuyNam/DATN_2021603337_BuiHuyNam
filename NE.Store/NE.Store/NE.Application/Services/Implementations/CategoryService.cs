using Microsoft.Extensions.Logging;
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
    public class CategoryService: ICategoryService
    {
        private readonly IUnitOfWork _unitOfWork;

        public CategoryService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task AddCategoryAsync(Category category)
        {
            var categories = await _unitOfWork.Categories.FindAsync(c => c.CategoryName == category.CategoryName);
            if (categories.Any())
            {
                throw new Exception("Category already exists!");
            }
            await _unitOfWork.Categories.AddAsync(category);
            await _unitOfWork.SaveChangesAsync();
        

        }

        public async Task DeleteCategoryAsync(int id)
        {
            var category = await _unitOfWork.Categories.GetByIdAsync(id);
            if (category != null)
            {
                await _unitOfWork.Categories.Delete(category);
                await _unitOfWork.SaveChangesAsync();
            }
            else
            {
                throw new Exception("Category does not exist!");
            }
        }

        public async Task<IEnumerable<Category>> GetAllCategoryAsync()
        {
            return await _unitOfWork.Categories.GetAllAsync();
        }

        public async Task<Category> GetCategoryByIdAsync(int id)
        {
            var category = await _unitOfWork.Categories.GetByIdAsync(id);
            if (category == null)
            {
                throw new Exception("Category does not exist!");
            }
            return category;
        }

        public async Task UpdateCategoryAsync(Category category)
        {
            var categoryUpdate = await _unitOfWork.Categories.FindAsync(c => c.Id == category.Id);

            if (!categoryUpdate.Any())
            {
                throw new Exception("Category does not exist!");
            }

            await _unitOfWork.Categories.Update(category);
            await _unitOfWork.SaveChangesAsync();
        }
    }
}
