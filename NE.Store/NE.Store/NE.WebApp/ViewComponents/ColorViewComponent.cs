using Microsoft.AspNetCore.Mvc;
using NE.Application.Dtos.ColorDto;

namespace NE.WebApp.ViewComponents
{
    [ViewComponent(Name = "Color")]
    public class ColorViewComponent : ViewComponent
    {
        private const string ApiUrl = "https://localhost:7099/api/Color";
        private readonly HttpClient _httpClient;

        public ColorViewComponent(IHttpClientFactory httpClientFactory)
        {
            _httpClient = httpClientFactory.CreateClient();
        }
        public async Task<IViewComponentResult> InvokeAsync()
        {
            var result = await _httpClient.GetFromJsonAsync<IEnumerable<ColorViewDto>>(ApiUrl);
            return View(result);
        }
    }
}
