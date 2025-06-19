using Microsoft.AspNetCore.Mvc;
using NE.Application.Dtos.CartDto;
using NE.Application.Dtos.Feedback;
using NE.Application.Dtos.WardDto;
using System.IdentityModel.Tokens.Jwt;

namespace NE.WebApp.Controllers
{
    public class FeedbackController : Controller
    {

        private readonly HttpClient _httpClient;
        private const string ApiUrl = "https://localhost:7099/api/feedback";

        public FeedbackController(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        [HttpPost("AddFeedback")]
        public async Task<IActionResult> AddFeedback(FeedbackCreateDto feedbackCreate)
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
                feedbackCreate.UserId = int.Parse(userIdClaim.Value);
                _httpClient.DefaultRequestHeaders.Authorization =
                                        new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

                // Gửi yêu cầu thêm mới
                var response = await _httpClient.PostAsJsonAsync(ApiUrl, feedbackCreate);
                if (!response.IsSuccessStatusCode)
                {
                    TempData["Error"] = "Gửi đánh giá thất bại!";
                }
                else
                {
                    TempData["Success"] = "Gửi đánh giá thành công!";
                }

                return RedirectToAction("UserProductDetail", "Product", new { id = feedbackCreate.ProductId });
            }
        }
    }
}
