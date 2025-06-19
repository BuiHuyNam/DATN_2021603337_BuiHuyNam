using Microsoft.AspNetCore.Mvc;
using NE.Application.Dtos.BrandDto;
using NE.Application.Dtos.CategoryDto;

namespace NE.WebApp.ViewComponents
{
    [ViewComponent(Name ="Brand")]
    public class BrandViewComponent : ViewComponent
    {
        private const string ApiUrl = "https://localhost:7099/api/brand";
        private readonly HttpClient _httpClient;

        public BrandViewComponent(IHttpClientFactory httpClientFactory)
        {
            _httpClient = httpClientFactory.CreateClient();
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var result = await _httpClient.GetFromJsonAsync<IEnumerable<BrandViewDto>>(ApiUrl);
            return View(result);
        }
    }
}
