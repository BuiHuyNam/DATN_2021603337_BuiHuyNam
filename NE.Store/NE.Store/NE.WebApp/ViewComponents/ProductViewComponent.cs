using Microsoft.AspNetCore.Mvc;
using NE.Application.Dtos.ProductDto;

namespace NE.WebApp.ViewComponents
{
    [ViewComponent(Name = "Product")]
    public class ProductViewComponent : ViewComponent
    {
        private const string ApiUrl = "https://localhost:7099/api/Product";
        private readonly HttpClient _httpClient;

        public ProductViewComponent(IHttpClientFactory httpClientFactory)
        {
            _httpClient = httpClientFactory.CreateClient();
        }
        public async Task<IViewComponentResult> InvokeAsync()
        {
            var result = await _httpClient.GetFromJsonAsync<IEnumerable<ProductViewDto>>(ApiUrl);
            return View(result);
        }
    }
}
