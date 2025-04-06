using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NE.Application.Dtos.BrandDto;
using NE.Application.Services.Interfaces;
using NE.Domain.Entitis;

namespace NE.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BrandController : ControllerBase
    {
        private readonly IBrandService _brandService;
        private readonly IMapper _mapper;

        public BrandController(IBrandService brandService, IMapper mapper)
        {
            _brandService = brandService;
            _mapper = mapper;
        }

        [HttpPost()]
        public async Task<ActionResult> AddBrand(BrandCreateDto brandCreateDto)
        {
            var brand = _mapper.Map<Brand>(brandCreateDto);
            await _brandService.AddBrandAsync(brand);
            return Ok();
        }

        [HttpGet()]
        public async Task<ActionResult> GetAllBrand()
        {
            var brands = await _brandService.GetAllBrandAsync();
            var brandDto = _mapper.Map<IEnumerable<BrandViewDto>>(brands);
            return Ok(brandDto);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult> GetBrandByIdAsync(int id)
        {
            var brand = await _brandService.GetBrandByIdAsync(id);
            var brandDto = _mapper.Map<BrandViewDto>(brand);
            return Ok(brandDto);
        }

        [HttpPut]
        public async Task<ActionResult> UpdateBrand(BrandUpdateDto brandUpdateDto)
        {
            var brandUpdate = _mapper.Map<Brand>(brandUpdateDto);
            await _brandService.UpdateBrandAsync(brandUpdate);
            return Ok();

        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteBrand(int id)
        {
            await _brandService.DeleteBrandAsync(id);
            return Ok();
        }
    }
}
