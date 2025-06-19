using Microsoft.AspNetCore.Mvc;
using NE.Application.Dtos.ProductDto;

namespace NE.WebApp.ViewComponents;
[ViewComponent(Name = "Top5NewProduct")]
public class Top5NewProductComponent : ViewComponent
{
    private const string ApiUrl = "https://localhost:7099/api/Product/Top5NewProduct";
    private readonly HttpClient _httpClient;

    public Top5NewProductComponent(IHttpClientFactory httpClientFactory)
    {
        _httpClient = httpClientFactory.CreateClient();
    }
    public async Task<IViewComponentResult> InvokeAsync()
    {
        var result = await _httpClient.GetFromJsonAsync<IEnumerable<ProductViewDto>>(ApiUrl);
        return View(result);
    }
}