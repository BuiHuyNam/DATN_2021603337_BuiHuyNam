using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NE.Application.Dtos.DistrictDto;
using NE.Application.Dtos.ProvinceDto;
using NE.Application.Services.Implementations;
using NE.Application.Services.Interfaces;
using NE.Domain.Entitis;

namespace NE.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DistrictController : ControllerBase
    {
        private readonly IDistrictService _districtService;
        private readonly IMapper _mapper;

        public DistrictController(IDistrictService districtService, IMapper mapper)
        {
            _districtService = districtService;
            _mapper = mapper;
        }

        [HttpPost()]
        public async Task<ActionResult> AddDistrict(DistrictCreateDto districtCreateDto)
        {
            var district = _mapper.Map<District>(districtCreateDto);
            await _districtService.AddDistrictAsync(district);
            return Ok();
        }

        [HttpGet()]
        public async Task<ActionResult> GetAllDistrict()
        {
            var districts = await _districtService.GetAllDistrictAsync();
            var districtDto = _mapper.Map<IEnumerable<DistrictViewDto>>(districts);
            return Ok(districtDto);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult> GetDistrictByIdAsync(int id)
        {
            var district = await _districtService.GetDistrictByIdAsync(id);
            var districtDto = _mapper.Map<DistrictViewDto>(district);
            return Ok(districtDto);
        }

        [HttpPut]
        public async Task<ActionResult> UpdateDistrict(DistrictUpdateDto districtUpdateDto)
        {
            var districtUpdate = _mapper.Map<District>(districtUpdateDto);
            await _districtService.UpdateDistrictAsync(districtUpdate);
            return Ok();

        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteDistrict(int id)
        {
            await _districtService.DeleteDistrictAsync(id);
            return Ok();
        }

    }
}
