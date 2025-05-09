using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NE.Application.Dtos.UserDto;
using NE.Application.Dtos.RoleDto;
using NE.Application.Dtos.UserDto;
using NE.Application.Services.Implementations;
using NE.Application.Services.Interfaces;
using NE.Domain.Entitis;
using NE.Application.Dtos.BrandDto;
using System.Net.Http;
using NE.Application.Dtos.ProductDto;

namespace NE.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly IMapper _mapper;

        public UserController(IUserService userService, IMapper mapper)
        {
            _userService = userService;
            _mapper = mapper;
        }


        [HttpPost()]
        public async Task<ActionResult> AddUser(UserCreateDto userCreateDto)
        {
            var user = _mapper.Map<User>(userCreateDto);
            await _userService.AddUserAsync(user);
            return Ok();
        }

        [HttpGet()]
        public async Task<ActionResult> GetAllUser()
        {
            var users = await _userService.GetAllUserAsyns();
            var userDto = _mapper.Map<IEnumerable<UserViewDto>>(users);
            return Ok(userDto);
        }

        [HttpPut("IsActiveUser")]
        public async Task<IActionResult> IsActiveUser(IsActiveUserDto isActiveUserDto)
        {
            var userUpdate = _mapper.Map<User>(isActiveUserDto);
            await _userService.IsActiveUser(userUpdate);
            return Ok();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult> GetUserByIdAsync(int id)
        {
            var user = await _userService.GetUserByIdAsync(id);
            var userDto = _mapper.Map<UserViewDto>(user);
            return Ok(userDto);
        }
        [HttpPut]
        public async Task<IActionResult> EditUser(UpdateInforUserDto updateInforUserDto)
        {
            var userUpdate = _mapper.Map<User>(updateInforUserDto);
            await _userService.UpdateUserAsync(userUpdate);
            return Ok();
        }



    }
}
