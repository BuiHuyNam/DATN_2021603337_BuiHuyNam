using Microsoft.AspNetCore.Mvc;
using NE.Application.Dtos.CategoryDto;
using NE.Application.Dtos.RoleDto;
using System.Threading.Tasks;

namespace NE.WebApp.Controllers
{
    [Route("admin")]
    public class CategoryController : Controller
    {
        private readonly HttpClient _httpClient;
        private const string ApiUrl = "https://localhost:7099/api/category";

        public CategoryController(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        [HttpGet("category")]
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet("addCategory")]
        public IActionResult AddCategory()
        {
            return View();
        }

        [HttpPost("addCategory")]
        public async Task<IActionResult> AddCategory(CategoryCreateDto categoryCreateDto)
        {
            var response = await _httpClient.PostAsJsonAsync(ApiUrl, categoryCreateDto);
            if (!response.IsSuccessStatusCode)
            {
                TempData["Error"] = " Them that bai!";
            }
            else
            {
                TempData["Success"] = "Them thanh cong!";
            }
            return RedirectToAction("Index", "Category");
        }

        [HttpGet("editCategory/{id}")]
        public async Task<IActionResult> EditCategory(int id)
        {
            var response = await _httpClient.GetFromJsonAsync<CategoryUpdateDto>($"{ApiUrl}/{id}");
            if (response == null)
            {
                return NotFound();
            }
            return View(response);
        }

        [HttpPost("editCategory/{id}")]
        public async Task<IActionResult> EditCategory(CategoryUpdateDto categoryUpdateDto)
        {
            var response = await _httpClient.PutAsJsonAsync(ApiUrl, categoryUpdateDto);
            if (!response.IsSuccessStatusCode)
            {
                TempData["Error"] = "Sua that bai!";
            }
            else
            {
                TempData["Success"] = "Sua thanh cong!";
            }
            return RedirectToAction("Index", "Category");
        }

        [HttpPost("DeleteCategory/{id}")]
        public async Task<IActionResult> DeleteCategory(int id)
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
            return RedirectToAction("Index", "Category");
        }
    }
}
