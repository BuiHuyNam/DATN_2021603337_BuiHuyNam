using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NE.Application.Dtos.ProvinceDto;
using NE.Application.Dtos.RoleDto;
using NE.Application.Services.Implementations;
using NE.Application.Services.Interfaces;
using NE.Domain.Entitis;

namespace NE.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProvinceController : ControllerBase
    {
        private readonly  IProvinceService _provinceService;
        private readonly IMapper _mapper;

        public ProvinceController(IProvinceService provinceService, IMapper mapper)
        {
            _provinceService = provinceService;
            _mapper = mapper;
        }


        [HttpPost()]
        public async Task<ActionResult> AddProvince(ProvinceCreateDto provinceCreateDto)
        {
            var province = _mapper.Map<Province>(provinceCreateDto);
            await _provinceService.AddProvinceAsync(province);
            return Ok();
        }

        [HttpGet()]
        public async Task<ActionResult> GetAllProvince()
        {
            var provinces = await _provinceService.GetAllProvinceAsync();
            var provinceDto = _mapper.Map<IEnumerable<ProvinceViewDto>>(provinces);
            return Ok(provinceDto);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult> GetProvinceByIdAsync(int id)
        {
            var province = await _provinceService.GetProvinceByIdAsync(id);
            var provinceDto = _mapper.Map<ProvinceViewDto>(province);
            return Ok(provinceDto);
        }

        [HttpPut]
        public async Task<ActionResult> UpdateProvince(ProvinceUpdateDto provinceUpdateDto)
        {
            var provinceUpdate = _mapper.Map<Province>(provinceUpdateDto);
            await _provinceService.UpdateProvinceAsync(provinceUpdate);
            return Ok();

        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteProvince(int id)
        {
            await _provinceService.DeleteProvinceAsync(id);
            return Ok();
        }



    }
}
