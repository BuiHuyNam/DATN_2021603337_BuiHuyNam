using Microsoft.AspNetCore.Mvc;
using NE.Application.Dtos.ProductDto;

namespace NE.WebApp.Controllers
{
    public class ProductController : Controller
    {
        private readonly HttpClient _httpClient;
        private const string ApiUrl = "https://localhost:7099/api/product";

        public ProductController(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        [HttpGet("admin/product")]         
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet("admin/addProduct")]
        public IActionResult AddProduct()
        {
            return View();
        }

        [HttpPost("admin/addProduct")]
        public async Task<IActionResult> AddProduct(ProductCreateDto productCreateDto)
        {
            var response = await _httpClient.PostAsJsonAsync(ApiUrl, productCreateDto);

            if (!response.IsSuccessStatusCode)
            {
                TempData["Error"] = " Them that bai!";
            }
            else
            {
                TempData["Success"] = "Them thanh cong!";
            }
            return RedirectToAction("Index", "Product");
        }



    }
}
