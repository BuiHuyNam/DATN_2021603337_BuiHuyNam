using Microsoft.AspNetCore.Mvc;
using NE.Application.Dtos.CategoryDto;
using NE.Application.Dtos.RoleDto;
using System.Threading.Tasks;

namespace NE.WebApp.Controllers
{
    
    public class CategoryController : Controller
    {
        private readonly HttpClient _httpClient;
        private const string ApiUrl = "https://localhost:7099/api/category";

        public CategoryController(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        [HttpGet("admin/category")]
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet("admin/addCategory")]
        public IActionResult AddCategory()
        {
            return View();
        }

        [HttpPost("admin/addCategory")]
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

        [HttpGet("admin/editCategory/{id}")]
        public async Task<IActionResult> EditCategory(int id)
        {
            var response = await _httpClient.GetFromJsonAsync<CategoryUpdateDto>($"{ApiUrl}/{id}");
            if (response == null)
            {
                return NotFound();
            }
            return View(response);
        }

        [HttpPost("admin/editCategory/{id}")]
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

        [HttpPost("admin/DeleteCategory/{id}")]
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

        [HttpGet("getCategory")]
        public async Task<IActionResult> GetAllCategory()
        {
            var categories = await _httpClient.GetFromJsonAsync<CategoryViewDto>(ApiUrl);
            ViewBag.Categories = categories; // Gửi qua ViewBag
            return View();
        }
    }
}
