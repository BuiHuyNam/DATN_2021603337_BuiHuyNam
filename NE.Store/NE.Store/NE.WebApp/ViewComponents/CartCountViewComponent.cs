using Microsoft.AspNetCore.Mvc;
using NE.Application.Dtos.BrandDto;
using NE.Application.Dtos.CartDto;
using NE.Application.Dtos.CategoryDto;
using NE.Application.Dtos.OrderDto;
using NE.Application.Dtos.ProductDto;
using System.IdentityModel.Tokens.Jwt;

namespace NE.WebApp.ViewComponents
{
    [ViewComponent(Name = "CartCount")]
    public class CartCountViewComponent : ViewComponent
    {
        private const string ApiUrl = "https://localhost:7099/api/cart";
        private const string ApiUrlProduct = "https://localhost:7099/api/product";

        private readonly HttpClient _httpClient;

        public CartCountViewComponent(IHttpClientFactory httpClientFactory)
        {
            _httpClient = httpClientFactory.CreateClient();
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {

            var token = HttpContext.Session.GetString("JwtToken");
         

            var handler = new JwtSecurityTokenHandler();
            var jwtToken = handler.ReadJwtToken(token);
            var userIdClaim = jwtToken.Claims.FirstOrDefault(c => c.Type == "nameid");


            int UserId = int.Parse(userIdClaim.Value);
            _httpClient.DefaultRequestHeaders.Authorization =
                new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);


            var cartItems = await _httpClient.GetFromJsonAsync<List<CartViewDto>>($"{ApiUrl}/GetCartByUserId/{UserId}");

            var products = await _httpClient.GetFromJsonAsync<List<ProductViewDto>>($"{ApiUrlProduct}");

            double totalMoney = 0;

            foreach (var item in cartItems)
            {
                var product = products.FirstOrDefault(p => p.Id == item.ProductId);
                if (product != null)
                {
                    var discountedPrice = product.Price * (1 - product.Discount / 100.0);
                    totalMoney += discountedPrice * item.Quatity;
                }
            }

            ViewBag.TotalMoney = totalMoney;

            return View(cartItems);
        }
    }
}
