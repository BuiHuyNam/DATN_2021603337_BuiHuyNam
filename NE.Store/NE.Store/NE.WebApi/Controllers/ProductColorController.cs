using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NE.Application.Dtos.ColorDto;
using NE.Application.Dtos.ProductColorDto;
using NE.Application.Dtos.ProductDto;
using NE.Application.Services.Implementations;
using NE.Application.Services.Interfaces;
using NE.Domain.Entitis;

namespace NE.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductColorController : ControllerBase
    {
        private readonly IProductColorService _productColorService;
        private readonly IMapper _mapper;

        public ProductColorController(IProductColorService productColorService, IMapper mapper)
        {
            _productColorService = productColorService;
            _mapper = mapper;
        }

        [HttpPost()]
        public async Task<ActionResult> AddProductColor(ProductColorCreateDto productColorCreateDto)
        {
            var productColor = _mapper.Map<ProductColor>(productColorCreateDto);
            await _productColorService.AddProductColorAsync(productColor);
            return Ok(new {id = productColor.Id} );
        }
        [HttpGet()]
        public async Task<ActionResult> GetAllProductColor()
        {
            var productColors = await _productColorService.GetAllProductColorAsync();
            var productColorDto = _mapper.Map<IEnumerable<ProductColorViewDto>>(productColors);
            return Ok(productColorDto);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult> GetProductColorByIdAsync(int id)
        {
            var productColor = await _productColorService.GetProductColorByIdAsync(id);
            var productColorDto = _mapper.Map<ProductColorViewDto>(productColor);
            return Ok(productColorDto);
        }

        [HttpPut]
        public async Task<ActionResult> UpdateProductColor(ProductColorUpdateDto productColorUpdateDto)
        {
            var productColorUpdate = _mapper.Map<ProductColor>(productColorUpdateDto);
            await _productColorService.UpdateProductColorAsync(productColorUpdate);
            return Ok();

        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteProductColor(int id)
        {
            await _productColorService.DeleteProductColorAsync(id);
            return Ok();
        }

    }
}
