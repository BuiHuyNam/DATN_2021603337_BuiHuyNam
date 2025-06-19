using Microsoft.AspNetCore.Mvc;

namespace NE.WebApp.Controllers
{
    public class ViewStaticController : Controller
    {
        [HttpGet("Contact")]
        public IActionResult Contact()
        {
            return View();
        }

        [HttpGet("Blog")]
        public IActionResult Blog()
        {
            return View();
        }

      
    }
}
