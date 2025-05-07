using Microsoft.AspNetCore.Mvc;
using NE.Application.Dtos.BrandDto;
using NE.Application.Dtos.CartDto;
using NE.Application.Dtos.OrderDetailDto;
using NE.Application.Dtos.OrderDto;
using NE.Domain.Entitis;

namespace NE.WebApp.Controllers
{
    public class OrderController : Controller
    {

        private readonly HttpClient _httpClient;
        private const string ApiUrl = "https://localhost:7099/api/order";
        private const string ApiUrlOrderDetail = "https://localhost:7099/api/orderDetail";


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

        [HttpPost("CreateOrderSingleProduct")]
        public async Task<IActionResult> CreateOrderSingleProduct(OrderCreateDto orderCreateDto, OrderDetailCreateDto orderDetailCreateDto) {

            orderCreateDto.UserId = 4;
            // B1: Gửi Order trước
            var responseOrder = await _httpClient.PostAsJsonAsync(ApiUrl, orderCreateDto);
            if (!responseOrder.IsSuccessStatusCode)
            {
                TempData["Error"] = "Khong the tao don hang!";
            }
            // B2: Lấy OrderId từ response
            var orderContent = await responseOrder.Content.ReadFromJsonAsync<Order>();
            int orderId = orderContent.Id;

            // B3: Gửi OrderDetail (gán OrderId)
            orderDetailCreateDto.OrderId = orderId;
            var responseDetail = await _httpClient.PostAsJsonAsync(ApiUrlOrderDetail, orderDetailCreateDto);

            if (!responseDetail.IsSuccessStatusCode)
            {
                TempData["Error"] = "Tao don hang thanh cong, loi tao chi tiet don hang!";
            }

            return View(responseDetail);

        }
    }
}
