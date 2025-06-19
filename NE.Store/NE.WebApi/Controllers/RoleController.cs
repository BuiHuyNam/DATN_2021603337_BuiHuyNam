using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NE.Application.Dtos.RoleDto;
using NE.Application.Services.Interfaces;
using NE.Domain.Entitis;

namespace NE.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RoleController : ControllerBase
    {
        private readonly IRoleService _roleService;
        private readonly IMapper _mapper;

        public RoleController(IRoleService roleService, IMapper mapper)
        {
            _roleService = roleService;
            _mapper = mapper;
        }

        [HttpPost()]
        public async Task<ActionResult> AddRole(RoleCreateDto roleCreateDto)
        {
            var role = _mapper.Map<Role>(roleCreateDto);
            await _roleService.AddRoleAsync(role);
            return Ok();
        }

        [HttpGet()]
        public async Task<ActionResult> GetAllRole()
        {
            var roles = await _roleService.GetAllRoleAsync();
            var roleDto = _mapper.Map<IEnumerable<RoleViewDto>>(roles);
            return Ok(roleDto);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult> GetRoleByIdAsync(int id)
        {
            var role = await _roleService.GetRoleByIdAsync(id);
            var roleDto = _mapper.Map<RoleViewDto>(role);
            return Ok(roleDto);
        }

        [HttpPut]
        public async Task<ActionResult> UpdateRole(RoleUpdateDto roleUpdateDto)
        {
            var roleUpdate = _mapper.Map<Role>(roleUpdateDto);
            await _roleService.UpdateRoleAsync(roleUpdate);
            return Ok();

        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteRole(int id)
        {
            await _roleService.DeleteRoleAsync(id);
            return Ok();
        }
    }
}
