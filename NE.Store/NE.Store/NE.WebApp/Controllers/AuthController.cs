using Microsoft.AspNetCore.Mvc;

namespace NE.WebApp.Controllers
{
    public class AuthController : Controller
    {
        [HttpGet("Login")]
        public IActionResult Login()
        {
            return View();
        }

        [HttpGet("Register")]
        public IActionResult Register()
        {
            return View();
        }

    }
}
