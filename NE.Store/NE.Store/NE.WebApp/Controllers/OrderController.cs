using Microsoft.AspNetCore.Mvc;
using NE.Application.Dtos.BrandDto;
using NE.Application.Dtos.OrderDto;

namespace NE.WebApp.Controllers
{
    public class OrderController : Controller
    {

        private readonly HttpClient _httpClient;
        private const string ApiUrl = "https://localhost:7099/api/order";

        public OrderController(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        [HttpGet("admin/Order")]
        public IActionResult Index()
        {
            return View();
        }


        [HttpPost("UpdateOrderStatus")]
        public async Task<IActionResult> UpdateOrderStatus(UpdateOrderStatusDto updateOrderStatusDto)
        {
            var response = await _httpClient.PutAsJsonAsync(ApiUrl + "/UpdateOrderStatus", updateOrderStatusDto);
            if (!response.IsSuccessStatusCode)
            {
                TempData["Error"] = "Sua that bai!";
            }
            else
            {
                TempData["Success"] = "Sua thanh cong!";
            }
            return RedirectToAction("Index", "Order");
        }
    }
}
