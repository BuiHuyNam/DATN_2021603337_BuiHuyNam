using Microsoft.AspNetCore.Mvc;
using NE.Application.Dtos.BrandDto;
using NE.Application.Dtos.CartDto;
using NE.Application.Dtos.OrderDetailDto;
using NE.Application.Dtos.OrderDto;
using NE.Application.Dtos.ProductColorDto;
using NE.Application.Dtos.ProductDto;
using NE.Application.Dtos.UserDto;
using NE.Application.Dtos.VNPay;
using NE.Application.Dtos.WardDto;
using NE.Domain.Entitis;
using NE.WebApp.Service;
using System.IdentityModel.Tokens.Jwt;

namespace NE.WebApp.Controllers
{
    public class OrderController : Controller
    {

        private readonly HttpClient _httpClient;
        private const string ApiUrl = "https://localhost:7099/api/order";
        private const string ApiUrlOrderDetail = "https://localhost:7099/api/orderDetail";
        private const string ApiUrlUser = "https://localhost:7099/api/user";
        private const string ApiUrlProductColor = "https://localhost:7099/api/productColor";


        private readonly IVnPayService _vnPayService;


        public OrderController(HttpClient httpClient, IVnPayService vnPayService)
        {
            _httpClient = httpClient;
            _vnPayService = vnPayService;
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
                orderCreateDto.UserId = int.Parse(userIdClaim.Value);
                _httpClient.DefaultRequestHeaders.Authorization =
                                        new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);



               
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

                var orderDetailContent = await responseDetail.Content.ReadFromJsonAsync<OrderDetail>();
                int orderDetailId = orderDetailContent.Id;

                var responseOrderDetail = await _httpClient.GetFromJsonAsync<OrderDetailViewDto>($"{ApiUrlOrderDetail}/{orderDetailId}");


                return View(responseOrderDetail);
            }

        }

    

        [HttpPost("PlaceOrder")]
        public async Task<IActionResult> PlaceOrder(UpdateInforUserDto updateInforUserDto, PaymentInformationModel paymentInformationModel)
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
                updateInforUserDto.Id = int.Parse(userIdClaim.Value);
                _httpClient.DefaultRequestHeaders.Authorization =
                                        new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);




                //updateInforUserDto.Id = 4;
                var responseUser = await _httpClient.PutAsJsonAsync(ApiUrlUser, updateInforUserDto);

                if (!responseUser.IsSuccessStatusCode)
                {
                    TempData["Error"] = "Khong the cap nhat thong tin giao hang";
                }

                // 3. Tạo URL thanh toán và chuyển hướng



                string paymentUrl = _vnPayService.CreatePaymentUrl(paymentInformationModel, HttpContext);
                return Redirect(paymentUrl);
            }

        }

            [HttpGet]
        public async Task<IActionResult> PaymentCallbackVnpay()
        {  
            var response = _vnPayService.PaymentExecute(Request.Query);
            // Lấy OrderId từ VNPay callback

            int orderId = int.Parse(Request.Query["vnp_TxnRef"]);



            var updateOrderStatusDto = new UpdateOrderStatusDto
            {   
               Id = orderId,
                Status = 3 //Đã thanh toán
            };

            if (response.Success)
            {
                // Gọi API hoặc service lưu trạng thái đơn hàng
                var responseStatus = await _httpClient.PutAsJsonAsync(ApiUrl + "/UpdateOrderStatus", updateOrderStatusDto);

                if (!responseStatus.IsSuccessStatusCode)
                {
                    TempData["Error"] = "Không thể cập nhật trạng thái đơn hàng.";
                }

                var responseGetOrder = await _httpClient.GetFromJsonAsync<OrderViewDto>($"{ApiUrl}/{updateOrderStatusDto.Id}");
                if (responseGetOrder != null)
                {
                    var productIds = responseGetOrder.OrderDetails.Select(od => od.ProductId).ToList();
                    var colorIds = responseGetOrder.OrderDetails.Select(od => od.ColorId).ToList();
                    var quantities = responseGetOrder.OrderDetails.Select(od => od.Quantity).ToList();

                    var responseGetProductColor = await _httpClient.GetFromJsonAsync<List<ProductColorViewDto>>(ApiUrlProductColor);
                    foreach (var orderDetail in responseGetOrder.OrderDetails)
                    {
                        var productColor = responseGetProductColor
                                            .FirstOrDefault(pc => pc.ProductId == orderDetail.ProductId && pc.ColorId == orderDetail.ColorId);

                        if (productColor != null)
                        {
                            // Cập nhật số lượng sau khi bán
                            productColor.Quantity -= orderDetail.Quantity;

                            // Gửi yêu cầu API để cập nhật lại số lượng
                            var responseUpdateProductColor = await _httpClient.PutAsJsonAsync(ApiUrlProductColor, productColor);

                            if (!responseUpdateProductColor.IsSuccessStatusCode)
                            {
                                TempData["Error"] = "Không thể cập nhật số lượng sản phẩm.";
                            }
                        }
                    }



                }




            }

            return View(response);
        }
    }
}
