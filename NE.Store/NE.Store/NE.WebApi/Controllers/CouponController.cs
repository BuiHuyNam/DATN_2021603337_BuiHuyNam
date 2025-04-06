using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NE.Application.Dtos.CouponDto;
using NE.Application.Services.Implementations;
using NE.Application.Services.Interfaces;
using NE.Domain.Entitis;

namespace NE.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CouponController : ControllerBase
    {
        private readonly ICouponService _couponService;
        private readonly IMapper _mapper;

        public CouponController(ICouponService couponService, IMapper mapper)
        {
            _couponService = couponService;
            _mapper = mapper;
        }

        [HttpPost()]
        public async Task<ActionResult> AddCoupon(CouponCreateDto couponCreateDto)
        {
            var coupon = _mapper.Map<Coupon>(couponCreateDto);
            await _couponService.AddCouponAsync(coupon);
            return Ok();
        }

        [HttpGet()]
        public async Task<ActionResult> GetAllCoupon()
        {
            var coupons = await _couponService.GetAllCouponAsync();
            var couponDto = _mapper.Map<IEnumerable<CouponViewDto>>(coupons);
            return Ok(couponDto);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult> GetCouponByIdAsync(int id)
        {
            var coupon = await _couponService.GetCouponByIdAsync(id);
            var couponDto = _mapper.Map<CouponViewDto>(coupon);
            return Ok(couponDto);
        }

        [HttpPut]
        public async Task<ActionResult> UpdateCoupon(CouponUpdateDto couponUpdateDto)
        {
            var couponUpdate = _mapper.Map<Coupon>(couponUpdateDto);
            await _couponService.UpdateCouponAsync(couponUpdate);
            return Ok();
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteCoupon(int id)
        {
            await _couponService.DeleteCouponAsync(id);
            return Ok();
        }
    }
}
