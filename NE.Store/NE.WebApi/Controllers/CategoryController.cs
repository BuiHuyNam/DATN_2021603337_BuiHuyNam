using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NE.Application.Dtos.CategoryDto;
using NE.Application.Services.Interfaces;
using NE.Domain.Entitis;

namespace NE.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoryController : ControllerBase
    {
        private readonly ICategoryService _categoryService;
        private readonly IMapper _mapper;

        public CategoryController(ICategoryService categoryService, IMapper mapper)
        {
            _categoryService = categoryService;
            _mapper = mapper;
        }

        [HttpPost()]
        public async Task<ActionResult> AddCategory(CategoryCreateDto categoryCreateDto)
        {
            var category = _mapper.Map<Category>(categoryCreateDto);
            await _categoryService.AddCategoryAsync(category);
            return Ok();
        }

        [HttpGet()]
        public async Task<ActionResult> GetAllCategory()
        {
            var categories = await _categoryService.GetAllCategoryAsync();
            var categoryDto = _mapper.Map<IEnumerable<CategoryViewDto>>(categories);
            return Ok(categoryDto);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult> GetCategoryByIdAsync(int id)
        {
            var category = await _categoryService.GetCategoryByIdAsync(id);
            var categoryDto = _mapper.Map<CategoryViewDto>(category);
            return Ok(categoryDto);
        }

        [HttpPut]
        public async Task<ActionResult> UpdateCategory(CategoryUpdateDto categoryUpdateDto)
        {
            var categoryUpdate = _mapper.Map<Category>(categoryUpdateDto);
            await _categoryService.UpdateCategoryAsync(categoryUpdate);
            return Ok();

        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteCategory(int id)
        {
            await _categoryService.DeleteCategoryAsync(id);
            return Ok();
        }
    }
}
