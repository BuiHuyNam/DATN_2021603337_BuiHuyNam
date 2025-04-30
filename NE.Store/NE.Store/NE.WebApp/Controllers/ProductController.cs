using Microsoft.AspNetCore.Mvc;
using NE.Application.Dtos.BrandDto;
using NE.Application.Dtos.CategoryDto;
using NE.Application.Dtos.ProductColorDto;
using NE.Application.Dtos.ProductDto;
using NE.Application.Dtos.RoleDto;
using NE.Domain.Entitis;

namespace NE.WebApp.Controllers
{
    public class ProductController : Controller
    {
        private readonly HttpClient _httpClient;
        private const string ApiUrl = "https://localhost:7099/api/product";
        private const string ApiUrlProductColor = "https://localhost:7099/api/productColor";
        private const string ApiUrlUpload = "https://localhost:7099/api/ImageFile/Upload";
        private const string ApiUrlCategory = "https://localhost:7099/api/category";
        private const string ApiUrlBrand = "https://localhost:7099/api/brand";






        public ProductController(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        [HttpGet("admin/product")]
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet("admin/addProduct")]
        public IActionResult AddProduct()
        {
            return View();
        }

        [HttpPost("admin/addProduct")]
        public async Task<IActionResult> AddProduct( ProductCreateDto productCreateDto)
        {

            // Gửi yêu cầu tạo sản phẩm
            var response = await _httpClient.PostAsJsonAsync(ApiUrl, productCreateDto);

            if (!response.IsSuccessStatusCode)
            {
                TempData["Error"] = "Them san pham thai bai!";
                return RedirectToAction("Index", "Product");
            }


            // Đọc kết quả trả về ID sản phẩm
            var productResponse = await response.Content.ReadFromJsonAsync<Dictionary<string, int>>();
            int productId = productResponse["id"];

            var productColorCreateDto = new ProductColorCreateDto
            {
                ProductId = productId,
                Quantity = productCreateDto.Quantity,
                ColorId = productCreateDto.ColorId
            };

            // Gửi yêu cầu tạo ProductColor
            var responseProductColor = await _httpClient.PostAsJsonAsync(ApiUrlProductColor, productColorCreateDto);
            if (!responseProductColor.IsSuccessStatusCode)
            {
                TempData["Error"] = "Them mau san pham that bai!";
                return RedirectToAction("Index", "Product");
            }

            TempData["Success"] = "Them san pham thanh cong!";
            return RedirectToAction("Index", "Product");

        }

        [HttpGet("admin/AddProductByColor")]
        public IActionResult AddProductByColor()
        {
            return View();
        }

        [HttpPost("admin/AddProductByColor")]
        public async Task<IActionResult> AddProductByColor(ProductColorCreateDto productColorCreateDto)
        {
            var productColors = await _httpClient.GetFromJsonAsync<IEnumerable<ProductColorViewDto>>(ApiUrlProductColor);
            foreach(var pc in productColors)
            {
                if(productColorCreateDto.ProductId == pc.ProductId)
                {
                    TempData["Error"] = " San pham va mau da ton tai!";
                    return RedirectToAction("AddProductByColor", "Product");

                }
            }

            var response = await _httpClient.PostAsJsonAsync(ApiUrlProductColor, productColorCreateDto) ;
            if (!response.IsSuccessStatusCode)
            {
                TempData["Error"] = " Them that bai!";
            }
            else
            {
                TempData["Success"] = "Them thanh cong!";
            }
            return RedirectToAction("Index", "Product");
        }

        [HttpPost("admin/IsActiveProduct")]
        public async Task<IActionResult> IsActiveProduct(IsActiveProductDto isActiveProductDto)
        {
            var response = await _httpClient.PutAsJsonAsync(ApiUrl + "/IsActiveProduct", isActiveProductDto);
            return RedirectToAction("Index", "Product");
        }

        [HttpGet("ProductDetail/{id}")]
        public async Task<IActionResult> ProductDetail(int id)
        {
            var result = await _httpClient.GetFromJsonAsync<ProductViewDto>($"{ApiUrl}/{id}");
            return View(result);
        }

        [HttpPost("admin/UpdateQuantity")]
        public async Task<IActionResult> UpdateQuantity(ProductColorUpdateDto productColorUpdateDto)
        {
            var response = await _httpClient.PutAsJsonAsync(ApiUrlProductColor, productColorUpdateDto);
            if (!response.IsSuccessStatusCode)
            {
                TempData["Error"] = "Sua that bai!";
            }
            else
            {
                TempData["Success"] = "Sua thanh cong!";
            }
            int productId = Convert.ToInt32(TempData["ProductId"]);
            return RedirectToAction("ProductDetail", new { id = productId });

           
        }

        [HttpPost("admin/DeleteProductColor{id}")]
        public async Task<IActionResult> DeleteProductColor(int id)
        {
            var response = await _httpClient.DeleteAsync($"{ApiUrlProductColor}/{id}");
            if (!response.IsSuccessStatusCode)
            {
                TempData["Error"] = "Khong the xoa!";
            }
            else
            {
                TempData["Success"] = "Xoa thanh cong!";
            }
          
            int productId = Convert.ToInt32(TempData["ProductId"]);
            return RedirectToAction("ProductDetail", new { id = productId });
        }

        [HttpGet("admin/EditProduct/{id}")]
        public async Task<IActionResult> EditProduct(int id)
        {
            var response = await _httpClient.GetFromJsonAsync<ProductUpdateDto>($"{ApiUrl}/{id}");

            if (response == null)
            {
                return NotFound();
            }

            // Lấy danh sách danh mục
            var categoryViewDto = await _httpClient.GetFromJsonAsync<IEnumerable<CategoryViewDto>>(ApiUrlCategory);
            if (categoryViewDto == null)
            {
                return View(response); // Trả về view với dữ liệu sản phẩm nếu không lấy được danh mục
            }

            // Truyền danh sách thương hiệu vào ViewBag
            ViewBag.Category = categoryViewDto.Select(x => new { x.Id, x.CategoryName }).ToList();

            // Lấy danh sách danh mục
            var brandViewDto = await _httpClient.GetFromJsonAsync<IEnumerable<BrandViewDto>>(ApiUrlBrand);
            if (brandViewDto == null)
            {
                return View(response); // Trả về view với dữ liệu sản phẩm nếu không lấy được thương hiệu
            }

            // Truyền danh sách thương hiệu vào ViewBag
            ViewBag.Brand = brandViewDto.Select(x => new { x.Id, x.BrandName }).ToList();

            return View(response);
        }

        [HttpPost("admin/EditProduct/{id}")]
        public async Task<IActionResult> EditProduct(ProductUpdateDto productUpdateDto)
        {
            var response = await _httpClient.PutAsJsonAsync(ApiUrl, productUpdateDto);
            if (!response.IsSuccessStatusCode)
            {
                TempData["Error"] = "Sua that bai!";
            }
            else
            {
                TempData["Success"] = "Sua thanh cong!";
            }
            return RedirectToAction("Index", "Product");
        }

        [HttpGet("admin/ImageProduct")]
        public IActionResult ImageProduct()
        {
            return View();
        }

        [HttpGet("admin/AddImageProduct")]
        public IActionResult AddImageProduct(int productColorId, int productId, int colorId)
        {
            ViewBag.ProductColorId = productColorId;
            ViewBag.ProductId = productId;
            ViewBag.ColorId = colorId;
            return View();
        }

        [HttpGet("ImageProductDetail/{id}")]
        public async Task<IActionResult> ImageProductDetail(int id)
        {
            var result = await _httpClient.GetFromJsonAsync<ProductViewDto>($"{ApiUrl}/{id}");
            return View(result);
        }

        [HttpGet("GetAllProduct")]
        public async Task<IActionResult> GetAllProduct()
        {
            var product = await _httpClient.GetFromJsonAsync<List<Product>>(ApiUrl);
            ViewBag.Products = product;
            return View();
        }


        






    }
}
                                                        