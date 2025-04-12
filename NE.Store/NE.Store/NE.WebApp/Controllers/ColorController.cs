using Microsoft.AspNetCore.Mvc;
using NE.Application.Dtos.ColorDto;

namespace NE.WebApp.Controllers
{
    [Route("admin")]
    public class ColorController : Controller
    {
        private readonly HttpClient _httpClient;
        private const string ApiUrl = "https://localhost:7099/api/color";

        public ColorController(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        [HttpGet("Color")]
        public IActionResult Index()
        {
            return View();
        }


        [HttpGet("addColor")]
        public IActionResult AddColor()
        {
            return View();
        }

        [HttpPost("addColor")]
        public async Task<IActionResult> AddColor(ColorCreateDto colorCreateDto)
        {
            var response = await _httpClient.PostAsJsonAsync(ApiUrl, colorCreateDto);
            if (!response.IsSuccessStatusCode)
            {
                TempData["Error"] = " Them that bai!";
            }
            else
            {
                TempData["Success"] = "Them thanh cong!";
            }
            return RedirectToAction("Index", "Color");
        }

        [HttpGet("editColor/{id}")]
        public async Task<IActionResult> EditColor(int id)
        {
            var response = await _httpClient.GetFromJsonAsync<ColorUpdateDto>($"{ApiUrl}/{id}");
            if (response == null)
            {
                return NotFound();
            }
            return View(response);
        }

        [HttpPost("editColor/{id}")]
        public async Task<IActionResult> EditColor(ColorUpdateDto colorUpdateDto)
        {
            var response = await _httpClient.PutAsJsonAsync(ApiUrl, colorUpdateDto);
            if (!response.IsSuccessStatusCode)
            {
                TempData["Error"] = "Sua that bai!";
            }
            else
            {
                TempData["Success"] = "Sua thanh cong!";
            }
            return RedirectToAction("Index", "Color");
        }

        [HttpPost("DeleteColor/{id}")]
        public async Task<IActionResult> DeleteColor(int id)
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
            return RedirectToAction("Index", "Color");
        }
    }
}
