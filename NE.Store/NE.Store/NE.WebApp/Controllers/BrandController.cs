using Microsoft.AspNetCore.Mvc;
using NE.Application.Dtos.BrandDto;
using NE.Application.Dtos.CategoryDto;

namespace NE.WebApp.Controllers
{
    [Route("admin")]
    public class BrandController : Controller
    {

        private readonly HttpClient _httpClient;
        private const string ApiUrl = "https://localhost:7099/api/brand";

        public BrandController(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        [HttpGet("Brand")]
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet("addBrand")]
        public IActionResult AddBrand()
        {
            return View();
        }

        [HttpPost("addBrand")]
        public async Task<IActionResult> AddBrand(BrandCreateDto brandCreateDto)
        {
            var response = await _httpClient.PostAsJsonAsync(ApiUrl, brandCreateDto);
            if (!response.IsSuccessStatusCode)
            {
                TempData["Error"] = " Them that bai!";
            }
            else
            {
                TempData["Success"] = "Them thanh cong!";
            }
            return RedirectToAction("Index", "Brand");
        }

        [HttpGet("editBrand/{id}")]
        public async Task<IActionResult> EditBrand(int id)
        {
            var response = await _httpClient.GetFromJsonAsync<BrandUpdateDto>($"{ApiUrl}/{id}");
            if (response == null)
            {
                return NotFound();
            }
            return View(response);
        }

        [HttpPost("editBrand/{id}")]
        public async Task<IActionResult> EditBrand(BrandUpdateDto brandUpdateDto)
        {
            var response = await _httpClient.PutAsJsonAsync(ApiUrl, brandUpdateDto);
            if (!response.IsSuccessStatusCode)
            {
                TempData["Error"] = "Sua that bai!";
            }
            else
            {
                TempData["Success"] = "Sua thanh cong!";
            }
            return RedirectToAction("Index", "Brand");
        }

        [HttpPost("DeleteBrand/{id}")]
        public async Task<IActionResult> DeleteBrand(int id)
        {
            var response = await _httpClient.DeleteAsync($"{ApiUrl}/{id}");
            if (!response.IsSuccessStatusCode)
            {
                TempData["Error"] = "Khong the xoa!";
            }
            else
            {
                TempData["Success"] = "Xoa thanh cong!";
            }
            return RedirectToAction("Index", "Brand");
        }

        [HttpPost("IsActiveBrand")]
        public async Task<IActionResult> IsActiveBrand(IsActiveBrandDto isActiveBrandDto)
        {
            var response = await _httpClient.PutAsJsonAsync(ApiUrl + "/IsActiveBrand", isActiveBrandDto);
            return RedirectToAction("Index", "Brand");


        }
    }
}
