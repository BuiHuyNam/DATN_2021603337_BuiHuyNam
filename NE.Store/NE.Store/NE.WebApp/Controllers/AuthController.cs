using Microsoft.AspNetCore.Mvc;
using NE.Application.Dtos.AuthDto;
using NE.Application.Dtos.UserDto;
using System.Net.Http;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;


namespace NE.WebApp.Controllers
{
    public class AuthController : Controller
    {



        private readonly HttpClient _httpClient;
        private const string ApiUrl = "https://localhost:7099/api/user";
        private const string ApiUrlAuth = "https://localhost:7099/api/Auth";


        public AuthController(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        [HttpGet("Login")]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost("Login")]
        public async Task<IActionResult> Login(LoginDto loginDto)
        {
            if (!ModelState.IsValid)
            {
                return View(loginDto);
            }
           




            var response = await _httpClient.PostAsJsonAsync($"{ApiUrlAuth}/login", loginDto);

            if (!response.IsSuccessStatusCode)
            {
                ModelState.AddModelError("", "❌Sai tài khoản hoặc mật khẩu.");
                return View(loginDto);
            }
            
             

            var token = await response.Content.ReadAsStringAsync();
            HttpContext.Session.SetString("JwtToken", token);

            // ✅ Parse token để lấy role
            var handler = new JwtSecurityTokenHandler();
            var jwtToken = handler.ReadJwtToken(token);

            var isActiveClaim = jwtToken.Claims.FirstOrDefault(c => c.Type == "IsActive");

            if (isActiveClaim != null && isActiveClaim.Value == "false")
            {
                ModelState.AddModelError("", "❌Tài khoản đã bị vô hiệu hóa.");
                return View(loginDto);
            }



            // Tùy theo API, claim có thể là "role", "roles", hoặc ClaimTypes.Role
            var role = jwtToken.Claims.FirstOrDefault(c =>
                c.Type == ClaimTypes.Role || c.Type == "role" || c.Type == "roles")?.Value;

            HttpContext.Session.SetString("UserRole", role ?? "Guest");

            // 👉 Phân trang theo role
            if (role == "User")
            {
                return RedirectToAction("GetAllProductPages", "Product");
            }
                
            else
            {
                return RedirectToAction("Index", "Admin");
            }
              
        }

        [HttpGet("Register")]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost("Register")]
        public async Task<IActionResult> Register(UserCreateDto userCreateDto)
        {
            var response = await _httpClient.PostAsJsonAsync(ApiUrl, userCreateDto);

            if (!response.IsSuccessStatusCode)
            {
                TempData["Error"] = "Email , ten nguoi dung ton tai!";
                return View(userCreateDto);
            }
            TempData["Success"] = " Dang ky thanh cong! Vui long dang nhap.";
            return RedirectToAction("Login", "Auth");


        }

        [HttpGet("Logout")]
        public IActionResult LogOut()
        {

            HttpContext.Session.Remove("JwtToken");


            return RedirectToAction("Login", "Auth");
        }

    }
}
