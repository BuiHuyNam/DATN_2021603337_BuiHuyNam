using Microsoft.AspNetCore.Mvc;
using NE.Application.Dtos.BrandDto;
using NE.Application.Dtos.CouponDto;
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
    }
}
