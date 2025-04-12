using Microsoft.AspNetCore.Mvc;
using NE.Application.Dtos.CategoryDto;

namespace NE.WebApp.ViewComponents
{
    [ViewComponent(Name = "Category")]
    public class CategoryViewComponent : ViewComponent
    {
        private const string ApiUrl = "https://localhost:7099/api/category";
        private readonly HttpClient _httpClient;

        public CategoryViewComponent(IHttpClientFactory httpClientFactory)
        {
            _httpClient = httpClientFactory.CreateClient();
        }
        public async Task<IViewComponentResult> InvokeAsync()
        {
            var result = await _httpClient.GetFromJsonAsync<IEnumerable<CategoryViewDto>>(ApiUrl);
            return View(result);
        }
    }
}
