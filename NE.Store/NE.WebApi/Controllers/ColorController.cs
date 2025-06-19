using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NE.Application.Dtos.CategoryDto;
using NE.Application.Dtos.ColorDto;
using NE.Application.Services.Implementations;
using NE.Application.Services.Interfaces;
using NE.Domain.Entitis;

namespace NE.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ColorController : ControllerBase
    {
        private readonly IColorService _colorService;
        private readonly IMapper _mapper;

        public ColorController(IColorService colorService, IMapper mapper)
        {
            _colorService = colorService;
            _mapper = mapper;
        }

        [HttpPost()]
        public async Task<ActionResult> AddColor(ColorCreateDto colorCreateDto)
        {
            var color = _mapper.Map<Color>(colorCreateDto);
            await _colorService.AddColorAsync(color);
            return Ok();
        }

        [HttpGet()]
        public async Task<ActionResult> GetAllColor()
        {
            var colors = await _colorService.GetAllColorAsync();
            var colorDto = _mapper.Map<IEnumerable<ColorViewDto>>(colors);
            return Ok(colorDto);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult> GetColorByIdAsync(int id)
        {
            var color = await _colorService.GetColorByIdAsync(id);
            var colorDto = _mapper.Map<ColorViewDto>(color);
            return Ok(colorDto);
        }

        [HttpPut]
        public async Task<ActionResult> UpdateColor(ColorUpdateDto colorUpdateDto)
        {
            var colorUpdate = _mapper.Map<Color>(colorUpdateDto);
            await _colorService.UpdateColorAsync(colorUpdate);
            return Ok();

        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteColor(int id)
        {
            await _colorService.DeleteColorAsync(id);
            return Ok();
        }
    }
}
