using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NE.Application.Dtos.ProvinceDto;
using NE.Application.Dtos.WardDto;
using NE.Application.Services.Implementations;
using NE.Application.Services.Interfaces;
using NE.Domain.Entitis;

namespace NE.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WardController : ControllerBase
    {
        private readonly IWardService _wardService;
        private readonly IMapper _mapper;

        public WardController(IWardService wardService, IMapper mapper)
        {
            _wardService = wardService;
            _mapper = mapper;
        }

        [HttpPost()]
        public async Task<ActionResult> AddWard(WardCreateDto wardCreateDto)
        {
            var ward = _mapper.Map<Ward>(wardCreateDto);
            await _wardService.AddWardAsync(ward);
            return Ok();
        }


        [HttpGet()]
        public async Task<ActionResult> GetAllWard()
        {
            var wards = await _wardService.GetAllWardAsync();
            var wardDto = _mapper.Map<IEnumerable<WardViewDto>>(wards);
            return Ok(wardDto);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult> GetWardByIdAsync(int id)
        {
            var ward = await _wardService.GetWardByIdAsync(id);
            var wardDto = _mapper.Map<WardViewDto>(ward);
            return Ok(wardDto);
        }


        [HttpPut]
        public async Task<ActionResult> UpdateWard(WardUpdateDto wardUpdateDto)
        {
            var wardUpdate = _mapper.Map<Ward>(wardUpdateDto);
            await _wardService.UpdateWardAsync(wardUpdate);
            return Ok();

        }


        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteWard(int id)
        {
            await _wardService.DeleteWardAsync(id);
            return Ok();
        }
    }
}
