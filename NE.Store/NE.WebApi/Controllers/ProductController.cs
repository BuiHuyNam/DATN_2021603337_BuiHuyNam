using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NE.Application.Dtos.BrandDto;
using NE.Application.Dtos.ProductDto;
using NE.Application.Dtos.ProvinceDto;
using NE.Application.Dtos.RoleDto;
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
        public async Task<ActionResult> AddProduct( ProductCreateDto productCreateDto)
        {
            var product = _mapper.Map<Product>(productCreateDto);
            await _productService.AddProductAsync(product);
            return Ok(new {id = product.Id});
        }

        [HttpGet()]
        public async Task<ActionResult> GetAllProduct()
        {
            var products = await _productService.GetAllProductAsync();
            var productDto =  _mapper.Map<IEnumerable<ProductViewDto>>(products);
            return Ok(productDto);
        }

        [HttpPut("IsActiveProduct")]
        public async Task<IActionResult> IsActiveProduct(IsActiveProductDto isActiveProductDto)
        {
            var productUpdate = _mapper.Map<Product>(isActiveProductDto);
            await _productService.IsActiveProduct(productUpdate);
            return Ok();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult> GetProductByIdAsync(int id)
        {
            var product = await _productService.GetProductByIdAsync(id);
            var productDto = _mapper.Map<ProductViewDto>(product);
            return Ok(productDto);
        }

        [HttpPut]
        public async Task<ActionResult> UpdateProduct(ProductUpdateDto productUpdateDto)
        {
            var productUpdate = _mapper.Map<Product>(productUpdateDto);
            await _productService.UpdateProductAsync(productUpdate);
            return Ok();

        }

        [HttpGet("ProductPages")]
        public async Task<ActionResult> GetAllBlogPage([FromQuery] int page = 1, [FromQuery] int pageSize = 8)
        {
            var products = await _productService.GetAllProductPage(page, pageSize);
            return Ok(products);
        }

        [HttpGet("Top5NewProduct")]
        public async Task<ActionResult> GetTop5NewProduct()
        {
            var products = await _productService.Top5NewProduct();
            var productsDto = _mapper.Map<List<ProductViewDto>>(products);
            return Ok(productsDto);
            
        }

    }
}
