using Microsoft.AspNetCore.Mvc;

namespace NE.WebApp.Controllers
{
    public class CartController : Controller
    {
        private readonly HttpClient _httpClient;
        private const string ApiUrl = "https://localhost:7099/api/cart";

        public CartController(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        [HttpGet("Cart")]
        public IActionResult Index()
        {
            return View();
        }
    }
}
