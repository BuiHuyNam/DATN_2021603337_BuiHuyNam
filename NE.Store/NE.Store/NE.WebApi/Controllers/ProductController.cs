using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NE.Application.Dtos.BrandDto;
using NE.Application.Dtos.ProductDto;
using NE.Application.Services.Implementations;
using NE.Application.Services.Interfaces;
using NE.Domain.Entitis;

namespace NE.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly IProductService _productService;
        private readonly IMapper _mapper;

        public ProductController(IProductService productService, IMapper mapper)
        {
            _productService = productService;
            _mapper = mapper;
        }

        [HttpPost()]
        public async Task<ActionResult> AddProduct(ProductCreateDto productCreateDto)
        {
            var product = _mapper.Map<Product>(productCreateDto);
            await _productService.AddProductAsync(product);
            return Ok();
        }

        [HttpGet()]
        public async Task<ActionResult> GetAllProduct()
        {
            var products = await _productService.GetAllProductAsync();
            var productDto =  _mapper.Map<IEnumerable<ProductViewDto>>(products);
            return Ok(productDto);
        }
    }
}
