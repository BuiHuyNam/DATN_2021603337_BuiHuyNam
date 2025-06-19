using Azure;
using Microsoft.AspNetCore.Mvc;
using NE.Application.Dtos.BrandDto;
using NE.Application.Dtos.CartDto;
using NE.Application.Dtos.CouponDto;
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
using System.Net.Http.Json;

namespace NE.WebApp.Controllers
{
    public class OrderController : Controller
    {

        private readonly HttpClient _httpClient;
        private const string ApiUrl = "https://localhost:7099/api/order";
        private const string ApiUrlOrderDetail = "https://localhost:7099/api/orderDetail";
        private const string ApiUrlUser = "https://localhost:7099/api/user";
        private const string ApiUrlProductColor = "https://localhost:7099/api/productColor";
        private const string ApiUrlCart = "https://localhost:7099/api/cart";
        private const string ApiUrlCoupon = "https://localhost:7099/api/coupon";




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
            var result = await _httpClient.GetFromJsonAsync<OrderViewDto>($"{ApiUrl}/{updateOrderStatusDto.Id}");
            if(updateOrderStatusDto.Status <= result.Status)
            {
                TempData["Error"] = "Không thể cập nhật trạng thái này!";
            }
            else
            {

                if(updateOrderStatusDto.Status == 8)
                {
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
                                productColor.Quantity += orderDetail.Quantity;

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

                var response = await _httpClient.PutAsJsonAsync(ApiUrl + "/UpdateOrderStatus", updateOrderStatusDto);
                if (!response.IsSuccessStatusCode)
                {
                    TempData["Error"] = "Sửa thất bại!";
                }
                else
                {
                    TempData["Success"] = "Sửa thành công!";
                }
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




                // Lấy toàn bộ ProductColor để so sánh số lượng
                var responseProductColor = await _httpClient.GetFromJsonAsync<List<ProductColorViewDto>>(ApiUrlProductColor);
                if (responseProductColor == null)
                {
                    TempData["Error"] = "Không thể lấy thông tin sản phẩm!";
                    return RedirectToAction("Cart", "User");
                }

                // Kiểm tra từng sản phẩm trong chi tiết đơn hàng
              
                    var productColor = responseProductColor
                        .FirstOrDefault(pc => pc.ProductId == orderDetailCreateDto.ProductId && pc.ColorId == orderDetailCreateDto.ColorId);

                    if (productColor == null)
                    {
                    //TempData["Error"] = $"Không tìm thấy sản phẩm có ID {orderDetailCreateDto.ProductId} với màu {orderDetailCreateDto.ColorId}.";
                    TempData["Error"] = "Sản phẩm đã hết hàng! ";

                    return RedirectToAction("UserProductDetail", "Product", new { id = orderDetailCreateDto.ProductId });

                }

                if (orderDetailCreateDto.Quantity > productColor.Quantity)
                    {
                    //TempData["Error"] = $"Sản phẩm \"{productColor.ProductName}\" màu \"{productColor.ColorName}\" chỉ còn {productColor.Quantity} sản phẩm!";
                    TempData["Error"] = "Sản phẩm đã hết hàng! ";

                    return RedirectToAction("UserProductDetail", "Product", new { id = orderDetailCreateDto.ProductId });


                }






                // B1: Gửi Order trước
                var responseOrder = await _httpClient.PostAsJsonAsync(ApiUrl, orderCreateDto);
                if (!responseOrder.IsSuccessStatusCode)
                {
                    TempData["Error"] = "Không thể tạo đơn hàng!";
                }
                // B2: Lấy OrderId từ response
                var orderContent = await responseOrder.Content.ReadFromJsonAsync<Order>();
                int orderId = orderContent.Id;

                // B3: Gửi OrderDetail (gán OrderId)
                orderDetailCreateDto.OrderId = orderId;
                var responseDetail = await _httpClient.PostAsJsonAsync(ApiUrlOrderDetail, orderDetailCreateDto);

                if (!responseDetail.IsSuccessStatusCode)
                {
                    TempData["Error"] = "Tạo đơn hàng thành công, lỗi tạo chi tiết đơn hàng!";
                }

                var orderDetailContent = await responseDetail.Content.ReadFromJsonAsync<OrderDetail>();
                int orderDetailId = orderDetailContent.Id;

                var responseOrderDetail = await _httpClient.GetFromJsonAsync<OrderDetailViewDto>($"{ApiUrlOrderDetail}/{orderDetailId}");


                return View(responseOrderDetail);
            }

        }

    
        [HttpPost("PlaceOrder")]
        public async Task<IActionResult> PlaceOrder(OrderUpdateDto orderUpdateDto, PaymentInformationModel paymentInformationModel)
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
                //updateInforUserDto.Id = int.Parse(userIdClaim.Value);
                _httpClient.DefaultRequestHeaders.Authorization =
                                        new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);


                orderUpdateDto.Id = paymentInformationModel.OrderId;
                orderUpdateDto.Status = 2;
                orderUpdateDto.TotalMoney = paymentInformationModel.Amount;


                //updateInforUserDto.Id = 4;
                var responseUser = await _httpClient.PutAsJsonAsync(ApiUrl, orderUpdateDto);

                if (!responseUser.IsSuccessStatusCode)
                {
                    TempData["Error"] = "Không thể cập nhật thông tin đơn hàng";
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


            // Kiểm tra mã phản hồi từ VNPAY
            if (response.VnPayResponseCode == "00")
            {

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

                        if (responseGetOrder.CouponId != null)
                        {
                            var responseGetCounpon = await _httpClient.GetFromJsonAsync<CouponViewDto>($"{ApiUrlCoupon}/{responseGetOrder.CouponId}");

                            responseGetCounpon.Quantity -= 1;

                            var updateCoupon = await _httpClient.PutAsJsonAsync(ApiUrlCoupon, responseGetCounpon);
                        }





                    }




                }
            }
            else
            {
                // Giao dịch thất bại => có thể lưu trạng thái "hủy"
                var updateOrderStatusDto = new UpdateOrderStatusDto
                {
                    Id = orderId,
                    Status = 6 // Ví dụ: 6 = Đã hủy
                };

                await _httpClient.PutAsJsonAsync(ApiUrl + "/UpdateOrderStatus", updateOrderStatusDto);
                //TempData["Error"] = $"Giao dịch thất bại. Mã lỗi: {response.VnPayResponseCode}";
            }

            return View(response);
            
            //return Json(response);
        }


        [HttpPost("CheckOut")]
        public async Task<IActionResult> CheckOut(OrderCreateDto orderCreateDto, List<OrderDetailCreateDto> orderDetails, List<int> CartId)
        {
            var token = HttpContext.Session.GetString("JwtToken");
            if (string.IsNullOrEmpty(token))
            {
                return RedirectToAction("Login", "Auth");
            }

            var handler = new JwtSecurityTokenHandler();
            var jwtToken = handler.ReadJwtToken(token);
            var userIdClaim = jwtToken.Claims.FirstOrDefault(c => c.Type == "nameid");

            if (userIdClaim == null)
            {
                return RedirectToAction("Login", "Auth");
            }

            orderCreateDto.UserId = int.Parse(userIdClaim.Value);
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
            foreach (var detail in orderDetails)
            {
                var productColor = responseProductColor
                    .FirstOrDefault(pc => pc.ProductId == detail.ProductId && pc.ColorId == detail.ColorId);

                if (productColor == null)
                {
                    TempData["Error"] = $"Không tìm thấy sản phẩm có ID {detail.ProductId} với màu {detail.ColorId}.";
                    return RedirectToAction("Index", "Cart", new { userId = orderCreateDto.UserId });
                }

                if (detail.Quantity > productColor.Quantity)
                {
                    //TempData["Error"] = $"Sản phẩm \"{productColor.Pr}\" màu \"{productColor.Color.ColorName}\" chỉ còn {productColor.Quantity} sản phẩm!";
                    TempData["Error"] = "Số lượng trong kho cửa hàng không đủ! ";
                    return RedirectToAction("Index", "Cart", new { userId = orderCreateDto.UserId });

                }
            }







            // B1: Tạo đơn hàng
            var responseOrder = await _httpClient.PostAsJsonAsync(ApiUrl, orderCreateDto);
            if (!responseOrder.IsSuccessStatusCode)
            {
                TempData["Error"] = "Không thể tạo đơn hàng!";
                return RedirectToAction("Cart", "User");
            }

            var orderContent = await responseOrder.Content.ReadFromJsonAsync<Order>();
            int orderId = orderContent.Id;

            // B2: Tạo chi tiết đơn hàng cho từng sản phẩm
            var orderDetailResults = new List<OrderDetailViewDto>();
            foreach (var detailDto in orderDetails)
            {
                detailDto.OrderId = orderId;

                var responseDetail = await _httpClient.PostAsJsonAsync(ApiUrlOrderDetail, detailDto);
                if (!responseDetail.IsSuccessStatusCode)
                {
                    TempData["Error"] = "Một số sản phẩm không thể thêm vào đơn hàng!";
                    continue;
                }

                var createdDetail = await responseDetail.Content.ReadFromJsonAsync<OrderDetail>();
                var detailView = await _httpClient.GetFromJsonAsync<OrderDetailViewDto>($"{ApiUrlOrderDetail}/{createdDetail.Id}");

                if (detailView != null)
                {
                    orderDetailResults.Add(detailView);
                }
            }
            // B3: Xóa các mục giỏ hàng đã đặt
            foreach (var id in CartId)
            {
                var deleteResponse = await _httpClient.DeleteAsync($"{ApiUrlCart}/{id}");
                if (!deleteResponse.IsSuccessStatusCode)
                {
                    // Ghi log lỗi hoặc lưu thông báo nếu cần
                    TempData["Error"] = "Không thể xóa giỏ hàng!";
                }
            }

            var getOrder = await _httpClient.GetFromJsonAsync<OrderViewDto>($"{ApiUrl}/{orderId}");


            // B3: Trả về view hiển thị danh sách chi tiết đơn hàng vừa tạo
            //return View("CheckOut", orderDetailResults);
            return View("CheckOut", getOrder);

        }


        [HttpGet("Admin/OrderDetail/{id}")]
        public async Task<IActionResult> OrderDetail(int id)
        {
            var result = await _httpClient.GetFromJsonAsync<OrderViewDto>($"{ApiUrl}/{id}");
            return View(result);
        }

        [HttpGet("Admin/EditOrder/{id}")]
        public async Task<IActionResult> EditOrder(int id)
        {
            var result = await _httpClient.GetFromJsonAsync<OrderViewDto>($"{ApiUrl}/{id}");
            return View(result);
        }

        [HttpPost("Admin/EditOrder/{id}")]
        public async Task<IActionResult> EditOrder(OrderUpdateDto orderUpdateDto)
        {
            var response = await _httpClient.PutAsJsonAsync(ApiUrl, orderUpdateDto);
            if (!response.IsSuccessStatusCode)
            {
                TempData["Error"] = "Sửa thất bại!";
            }
            else
            {
                TempData["Success"] = "Sửa thành công!";
            }
            return RedirectToAction("Index", "Order");

        }

        [HttpPost("Admin/DeleteOrder/{id}")]
        public async Task<IActionResult> DeleteOrder(int id)
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
            return RedirectToAction("Index", "Order");
        }

        [HttpPost("CancelOrder")]
        public async Task<IActionResult> CancelOrder(UpdateOrderStatusDto updateOrderStatusDto)
        {
            var result = await _httpClient.GetFromJsonAsync<OrderViewDto>($"{ApiUrl}/{updateOrderStatusDto.Id}");
            var response = await _httpClient.PutAsJsonAsync(ApiUrl + "/UpdateOrderStatus", updateOrderStatusDto);
            if (!response.IsSuccessStatusCode)
            {
                TempData["Error"] = "Sửa thất bại!";
            }
            else
            {


                if (updateOrderStatusDto.Status == 6 && (result.Status ==3 || result.Status ==4))
                {
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
                                productColor.Quantity += orderDetail.Quantity;

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



                    TempData["Success"] = "Sửa thành công!";
            }



            return RedirectToAction("HistoryOrder", "User");
        }

    }
}
