using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NE.Application.Dtos.CartDto;
using NE.Application.Dtos.WardDto;
using NE.Application.Services.Implementations;
using NE.Application.Services.Interfaces;
using NE.Domain.Entitis;

namespace NE.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CartController : ControllerBase
    {
        private readonly ICartService _cartService;
        private readonly IMapper _mapper;

        public CartController(ICartService cartService, IMapper mapper)
        {
            _cartService = cartService;
            _mapper = mapper;
        }

        [HttpPost()]
        [Authorize]
        public async Task<ActionResult> AddCart(CartCreateDto cartCreateDto)
        {
            var cart = _mapper.Map<Cart>(cartCreateDto);
            await _cartService.AddCartAsync(cart);
            return Ok();
        }

        [HttpGet()]
        public async Task<ActionResult> GetAllCart()
        {
            var carts = await _cartService.GetAllCartAsync();
            var cartDto = _mapper.Map<IEnumerable<CartViewDto>>(carts);
            return Ok(cartDto);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult> GetCartByIdAsync(int id)
        {
            var cart = await _cartService.GetCartByIdAsync(id);
            var cartDto = _mapper.Map<CartViewDto>(cart);
            return Ok(cartDto);
        }

        [HttpPut]
        public async Task<ActionResult> UpdateCart(CartUpdateDto cartUpdateDto)
        {
            var cartUpdate = _mapper.Map<Cart>(cartUpdateDto);
            await _cartService.UpdateCartAsync(cartUpdate);
            return Ok();
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteCart(int id)
        {
            await _cartService.DeleteCartAsync(id);
            return Ok();
        }


        [HttpGet("GetCartByUserId/{userId}")]
        [Authorize]
        public async Task<ActionResult> GetCartByUserId(int userId)
        {
            var cart = await _cartService.GetCartByUserIdAsync(userId);
            var cartDto = _mapper.Map<List<CartViewDto>>(cart);
            return Ok(cartDto);
        }
    }
}
