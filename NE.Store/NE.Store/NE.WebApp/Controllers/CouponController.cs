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
                TempData["Error"] = "Ngay ap dung phai truoc ngay het han!";
                return RedirectToAction("AddCoupon", "Coupon"); // hoặc return View nếu bạn dùng View
            }
            var response = await _httpClient.PostAsJsonAsync(ApiUrl, couponCreateDto);
            if (!response.IsSuccessStatusCode)
            {
                TempData["Error"] = " Them that bai!";
            }
            else
            {
                TempData["Success"] = "Them thanh cong!";
            }
            return RedirectToAction("Index", "Coupon");
        }


        [HttpPost("admin/DeleteCoupon/{id}")]
        public async Task<IActionResult> DeleteCoupon(int id)
        {
            var response = await _httpClient.DeleteAsync($"{ApiUrl}/{id}");
            if (!response.IsSuccessStatusCode)
            {
                TempData["Error"] = "Khong the xoa!";
            }
            else
            {
                TempData["Success"] = "Xoa thanh cong!";
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
                TempData["Error"] = "Ngay ap dung phai truoc ngay het han!";
                return RedirectToAction("Index", "Coupon"); // hoặc return View nếu bạn dùng View
            }

            var response = await _httpClient.PutAsJsonAsync(ApiUrl, couponUpdateDto);
            if (!response.IsSuccessStatusCode)
            {
                TempData["Error"] = "Sua that bai!";
            }
            else
            {
                TempData["Success"] = "Sua thanh cong!";
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
                    TempData["Error"] = "Ma giam gia khong ton tai";
                    return RedirectToAction("Index", "Cart", new { userId = userId });
                }


                // Tìm mã hợp lệ
                var matchedCoupon = response.FirstOrDefault(c => c.Code.Equals(code, StringComparison.OrdinalIgnoreCase));

                if (matchedCoupon == null)
                {
                    TempData["Error"] = "Ma giam gia khong hop le.";
                    return RedirectToAction("Index", "Cart", new { userId });
                }

                if (matchedCoupon.Quantity <=0)
                {
                    TempData["Error"] = "Ma giam gia da het.";
                    return RedirectToAction("Index", "Cart", new { userId });
                }

                var now = DateTime.Now;

                if (matchedCoupon.UseDate > now)
                {
                    TempData["Error"] = "Ma giam gia chua kich hoat.";
                    return RedirectToAction("Index", "Cart", new { userId });
                }

                if (matchedCoupon.ExpiryDate < now)
                {
                    TempData["Error"] = "Ma giam gia da het han.";
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
