using Microsoft.AspNetCore.Mvc;

namespace NE.WebApp.Controllers
{
    [Route("admin")]
    public class AdminController : Controller
    {
        [HttpGet("home")]
        public IActionResult Index()
        {
            return View();
        }
    }
}
