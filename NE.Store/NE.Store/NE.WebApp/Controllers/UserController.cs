using Microsoft.AspNetCore.Mvc;

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
    }
}
