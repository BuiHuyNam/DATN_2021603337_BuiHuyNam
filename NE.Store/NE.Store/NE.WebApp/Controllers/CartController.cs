using Microsoft.AspNetCore.Mvc;
using NE.Application.Dtos.BrandDto;
using NE.Application.Dtos.CartDto;
using NE.Application.Dtos.CategoryDto;
using NE.Domain.Entitis;
using System.IdentityModel.Tokens.Jwt;

namespace NE.WebApp.Controllers
{
    public class CartController : Controller
    {
        private readonly HttpClient _httpClient;
        private const string ApiUrl = "https://localhost:7099/api/cart";

        public CartController(IHttpClientFactory httpClientFactory)
        {
            _httpClient = httpClientFactory.CreateClient();
        }

        [HttpGet("Cart/{userId}")]
        public async Task<IActionResult> Index(int userId)
        {
            var token = HttpContext.Session.GetString("JwtToken");
            if (string.IsNullOrEmpty(token))
            {
                return RedirectToAction("Login", "Auth");
            }
            else
            {

                // Giải mã Token để lấy UserId
                var handler = new JwtSecurityTokenHandler();
                var jwtToken = handler.ReadJwtToken(token);
                var userIdClaim = jwtToken.Claims.FirstOrDefault(c => c.Type == "nameid");

                if (userIdClaim == null)
                {
                    return RedirectToAction("Login", "Auth");
                }

                // Gán UserId vào commentCreateDto
                userId = int.Parse(userIdClaim.Value);
                _httpClient.DefaultRequestHeaders.Authorization =
                                        new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

                var response = await _httpClient.GetFromJsonAsync<List<CartViewDto>>($"{ApiUrl}/GetCartByUserId/{userId}");
                if (response == null)
                {
                    return NotFound();
                }
                return View(response);
            }
        }

        [HttpPost("AddCart")]
        public async Task<IActionResult> AddCart(CartCreateDto cartCreateDto)
        {
            var token = HttpContext.Session.GetString("JwtToken");
            if (string.IsNullOrEmpty(token))
            {
                return RedirectToAction("Login", "Auth");
            }
            else
            {

                // Giải mã Token để lấy UserId
                var handler = new JwtSecurityTokenHandler();
                var jwtToken = handler.ReadJwtToken(token);
                var userIdClaim = jwtToken.Claims.FirstOrDefault(c => c.Type == "nameid");

                if (userIdClaim == null)
                {
                    return RedirectToAction("Login", "Auth");
                }

                // Gán UserId vào commentCreateDto
                cartCreateDto.UserId = int.Parse(userIdClaim.Value);
                _httpClient.DefaultRequestHeaders.Authorization =
                                        new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);



                var response = await _httpClient.PostAsJsonAsync(ApiUrl, cartCreateDto);
                if (!response.IsSuccessStatusCode)
                {
                    TempData["Error"] = " Them that bai!";
                }
                else
                {
                    TempData["Success"] = "Them thanh cong!";
                }
                return RedirectToAction("Index", "Cart");
            }
        }


        [HttpPost]
        public async Task<IActionResult> DeleteCartItem(int id)
        {

            var token = HttpContext.Session.GetString("JwtToken");
            if (string.IsNullOrEmpty(token))
            {
                // Nếu không có token, có thể trả về lỗi hoặc chuyển hướng
                return RedirectToAction("Login", "Auth");
            }

            // Thêm Authorization header
            _httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);


            var response = await _httpClient.DeleteAsync($"{ApiUrl}/{id}");
           
            return RedirectToAction("Index", "Cart");
        }
    }
}
