using Microsoft.AspNetCore.Mvc;
using NE.Application.Dtos.BrandDto;
using NE.Application.Dtos.CartDto;
using NE.Application.Dtos.CategoryDto;
using NE.Application.Dtos.OrderDetailDto;
using NE.Application.Dtos.ProductColorDto;
using NE.Domain.Entitis;
using System.IdentityModel.Tokens.Jwt;

namespace NE.WebApp.Controllers
{
    public class CartController : Controller
    {
        private readonly HttpClient _httpClient;
        private const string ApiUrl = "https://localhost:7099/api/cart";
        private const string ApiUrlProductColor = "https://localhost:7099/api/ProductColor ";


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




                // Lấy toàn bộ ProductColor để so sánh số lượng
                var responseProductColor = await _httpClient.GetFromJsonAsync<List<ProductColorViewDto>>(ApiUrlProductColor);
                if (responseProductColor == null)
                {
                    TempData["Error"] = "Không thể lấy thông tin sản phẩm!";
                    return RedirectToAction("Cart", "User");
                }

                // Kiểm tra từng sản phẩm trong chi tiết đơn hàng

                var productColor = responseProductColor
                .FirstOrDefault(pc => pc.ProductId == cartCreateDto.ProductId && pc.ColorId == cartCreateDto.ColorId);

                if (productColor == null)
                {
                    //TempData["Error"] = $"Không tìm thấy sản phẩm có ID {orderDetailCreateDto.ProductId} với màu {orderDetailCreateDto.ColorId}.";
                    TempData["Error"] = "San pham da het hang! ";

                    return RedirectToAction("UserProductDetail", "Product", new { id = cartCreateDto.ProductId });
                }

              








                // Lấy danh sách cart hiện tại
                var responseCartItems = await _httpClient.GetFromJsonAsync<List<CartViewDto>>($"{ApiUrl}/GetCartByUserId/{cartCreateDto.UserId}");

                // Kiểm tra nếu đã có sản phẩm với ProductId và ColorId đó rồi
                var existedItem = responseCartItems.FirstOrDefault(c =>
                    c.ProductId == cartCreateDto.ProductId && c.ColorId == cartCreateDto.ColorId);

                if (existedItem != null)
                {
                    TempData["Error"] = "San pham nay da co trong gio hang!";
                }
                else
                {
                    // Gửi yêu cầu thêm mới
                    var response = await _httpClient.PostAsJsonAsync(ApiUrl, cartCreateDto);
                    if (!response.IsSuccessStatusCode)
                    {
                        TempData["Error"] = "Them that bai!";
                    }
                    else
                    {
                        TempData["Success"] = "Them thanh cong!";
                    }
                }
                return RedirectToAction("UserProductDetail", "Product", new { id = cartCreateDto.ProductId });
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

        [HttpPost("UpdateQuantity")]
        public async Task<IActionResult> UpdateQuantity(CartUpdateDto cartUpdateDto)
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
                int userId = int.Parse(userIdClaim.Value);
                _httpClient.DefaultRequestHeaders.Authorization =
                                        new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);



                var response = await _httpClient.PutAsJsonAsync($"{ApiUrl}", cartUpdateDto);


                return RedirectToAction("Index", "Cart", new { userId = userId });
            }

        }



    }
}
