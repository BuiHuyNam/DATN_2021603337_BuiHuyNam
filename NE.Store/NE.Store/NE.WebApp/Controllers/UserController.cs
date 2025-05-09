using Microsoft.AspNetCore.Mvc;
using NE.Application.Dtos.ProductDto;
using NE.Application.Dtos.UserDto;

namespace NE.WebApp.Controllers
{
    public class UserController : Controller
    {

        private readonly HttpClient _httpClient;
        private const string ApiUrl = "https://localhost:7099/api/user";

        public UserController(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        [HttpGet("admin/User")]
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost("admin/IsActiveUser")]
        public async Task<IActionResult> IsActiveUser(IsActiveUserDto isActiveUserDto)
        {
            var response = await _httpClient.PutAsJsonAsync(ApiUrl + "/IsActiveUser", isActiveUserDto);
            return RedirectToAction("Index", "User");


        }

       
    }
}
