using Microsoft.AspNetCore.Mvc;
using NE.Application.Dtos.RoleDto;
using System.Net.Http;

namespace NE.WebApp.Controllers
{
    [Route("admin")]

    public class RoleController : Controller
    {
        private readonly HttpClient _httpClient;
        private const string ApiUrl = "https://localhost:7099/api/role";
        public RoleController(IHttpClientFactory httpClientFactory)
        {
            _httpClient = httpClientFactory.CreateClient();
        }

        [HttpGet("role")]
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet("addRole")]
        public IActionResult AddRole()
        {
            return View();
        }
        [HttpPost("addRole")]
        public async Task<IActionResult> AddRole(RoleCreateDto roleCreateDto)
        {
            var response = await _httpClient.PostAsJsonAsync(ApiUrl, roleCreateDto);
            if (!response.IsSuccessStatusCode)
            {
                TempData["Error"] = " Thêm thất bại!";
            }
            else
            {
                TempData["Success"] = "Thêm thành công!";
            }
            return RedirectToAction("Index", "Role");
        }

        [HttpGet("editRole/{id}")]
        public async Task<IActionResult> EditRole(int id)
        {
            var response = await _httpClient.GetFromJsonAsync<RoleUpdateDto>($"{ApiUrl}/{id}");

            if (response == null)
            {
                return NotFound();
            }

            return View(response);
        }

        [HttpPost("editRole/{id}")]
        public async Task<IActionResult> EditRole(RoleUpdateDto roleUpdateDto)
        {
            var response = await _httpClient.PutAsJsonAsync(ApiUrl, roleUpdateDto);
            if (!response.IsSuccessStatusCode)
            {
                TempData["Error"] = "Sửa thất bại!";
            }
            else
            {
                TempData["Success"] = "Sửa thành công!";
            }
            return RedirectToAction("Index", "Role");
        }

        [HttpPost("DeleteRole/{id}")]
        public async Task<IActionResult> DeleteRole(int id)
        {
            var response = await _httpClient.DeleteAsync($"{ApiUrl}/{id}");
            if (!response.IsSuccessStatusCode)
            {
                TempData["Error"] = "Không thể xóa!";
            }
            else
            {
                TempData["Success"] = "Xóa thành công!";
            }
            return RedirectToAction("Index", "Role");
        }
    }
}
