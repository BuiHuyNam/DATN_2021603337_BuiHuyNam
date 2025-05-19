using Microsoft.AspNetCore.Mvc;
using NE.Application.Dtos.BrandDto;
using NE.Application.Dtos.CartDto;
using NE.Application.Dtos.CouponDto;
using System.IdentityModel.Tokens.Jwt;
using System.Net.Http;

namespace NE.WebApp.Controllers
{
    public class CouponController : Controller
    {
        private readonly HttpClient _httpClient;
        private const string ApiUrl = "https://localhost:7099/api/coupon";

        public CouponController(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        [HttpGet("admin/coupon")]
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet("admin/addCoupon")]
        public IActionResult AddCoupon()
        {
            return View();
        }

        [HttpPost("admin/addCoupon")]
        public async Task<IActionResult> AddCoupon(CouponCreateDto couponCreateDto)
        {
            // Kiểm tra ngày áp dụng và ngày hết hạn hợp lệ
            if (couponCreateDto.UseDate >= couponCreateDto.ExpiryDate)
            {
                TempData["Error"] = "Ngày áp dụng phải trước ngày hết hạn!";
                return RedirectToAction("AddCoupon", "Coupon"); // hoặc return View nếu bạn dùng View
            }
            var response = await _httpClient.PostAsJsonAsync(ApiUrl, couponCreateDto);
            if (!response.IsSuccessStatusCode)
            {
                TempData["Error"] = " Thêm thất bại!";
            }
            else
            {
                TempData["Success"] = "Thêm thành công!";
            }
            return RedirectToAction("Index", "Coupon");
        }


        [HttpPost("admin/DeleteCoupon/{id}")]
        public async Task<IActionResult> DeleteCoupon(int id)
        {
            var response = await _httpClient.DeleteAsync($"{ApiUrl}/{id}");
            if (!response.IsSuccessStatusCode)
            {
                TempData["Error"] = "Không thể xóa!";
            }
            else
            {
                TempData["Success"] = "Xóa thành công!";
            }
            return RedirectToAction("Index", "Coupon");
        }


        [HttpGet("admin/editCoupon/{id}")]
        public async Task<IActionResult> EditCoupon(int id)
        {
            var response = await _httpClient.GetFromJsonAsync<CouponUpdateDto>($"{ApiUrl}/{id}");
            if (response == null)
            {
                return NotFound();
            }
            return View(response);
        }

        [HttpPost("admin/editCoupon/{id}")]
        public async Task<IActionResult> EditCoupon(CouponUpdateDto couponUpdateDto)
        {
            // Kiểm tra ngày áp dụng và ngày hết hạn hợp lệ
            if (couponUpdateDto.UseDate >= couponUpdateDto.ExpiryDate)
            {
                TempData["Error"] = "Ngày áp dụng phải trước ngày hết hạn!";
                return RedirectToAction("Index", "Coupon"); // hoặc return View nếu bạn dùng View
            }

            var response = await _httpClient.PutAsJsonAsync(ApiUrl, couponUpdateDto);
            if (!response.IsSuccessStatusCode)
            {
                TempData["Error"] = "Sửa thất bại!";
            }
            else
            {
                TempData["Success"] = "Sửa thành công!";
            }
            return RedirectToAction("Index", "Coupon");
        }


        [HttpPost("ApplyCoupon")]
        public async Task<IActionResult> ApplyCoupon(string code)
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


                var response = await _httpClient.GetFromJsonAsync<List<CouponViewDto>>(ApiUrl);
                if (response == null)
                {
                    TempData["Error"] = "Mã giảm giá không tồn tại";
                    return RedirectToAction("Index", "Cart", new { userId = userId });
                }


                // Tìm mã hợp lệ
                var matchedCoupon = response.FirstOrDefault(c => c.Code.Equals(code, StringComparison.OrdinalIgnoreCase));

                if (matchedCoupon == null)
                {
                    TempData["Error"] = "Mã giảm giá không hợp lệ.";
                    return RedirectToAction("Index", "Cart", new { userId });
                }

                if (matchedCoupon.Quantity <=0)
                {
                    TempData["Error"] = "Mã giảm giá đã hết.";
                    return RedirectToAction("Index", "Cart", new { userId });
                }

                var now = DateTime.Now;

                if (matchedCoupon.UseDate > now)
                {
                    TempData["Error"] = "Mã giảm giá chưa kích hoạt.";
                    return RedirectToAction("Index", "Cart", new { userId });
                }

                if (matchedCoupon.ExpiryDate < now)
                {
                    TempData["Error"] = "Mã giảm giá đã hết hạn.";
                    return RedirectToAction("Index", "Cart", new { userId });
                }
                // Lưu mã hợp lệ vào TempData
                TempData["Discount"] = (int)matchedCoupon.Discount;
                TempData["CouponCode"] = matchedCoupon.Code;
                TempData["CouponId"] = matchedCoupon.Id;

                return RedirectToAction("Index", "Cart", new { userId });


            }



        }
    }
}
