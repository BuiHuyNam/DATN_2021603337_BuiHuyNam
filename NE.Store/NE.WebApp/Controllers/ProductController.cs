﻿using Microsoft.AspNetCore.Mvc;
using NE.Application.Dtos.BaseDto;
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
                TempData["Error"] = "Thêm sản phẩm thất bại!";
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
                TempData["Error"] = "Thêm màu sản phẩm thất bại!";
                return RedirectToAction("Index", "Product");
            }

            TempData["Success"] = "Thêm sản phẩm thành công!";
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
                if(productColorCreateDto.ProductId == pc.ProductId && productColorCreateDto.ColorId == pc.ColorId)
                {
                    TempData["Error"] = " San pham va mau da ton tai!";
                    return RedirectToAction("AddProductByColor", "Product");

                }
            }

            var response = await _httpClient.PostAsJsonAsync(ApiUrlProductColor, productColorCreateDto) ;
            if (!response.IsSuccessStatusCode)
            {
                TempData["Error"] = " Thêm thất bại!";
            }
            else
            {
                TempData["Success"] = "Thêm thành công!";
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
                TempData["Error"] = "Sửa thất bại!";
            }
            else
            {
                TempData["Success"] = "Sửa thành công!";
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
                TempData["Error"] = "Không thể xóa!";
            }
            else
            {
                TempData["Success"] = "Xóa thành công!";
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
                TempData["Error"] = "Sửa thất bại!";
            }
            else
            {
                TempData["Success"] = "Sửa thành công!";
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

        [HttpGet()]
        public async Task<IActionResult> GetAllProductPages(int page = 1, int pageSize = 8, string searchItem = "", int? categoryId = null)
        {
            var response = await _httpClient.GetFromJsonAsync<PageResultDto<ProductViewDto>>($"{ApiUrl}/Productpages?page=1&pageSize=20");
            if (response == null || response.Items == null)
            {
                return View(new List<ProductViewDto>());
            }



            // Lọc dữ liệu trên frontend
            var filteredProducts = response.Items;
            if (!string.IsNullOrEmpty(searchItem))
            {
                filteredProducts = filteredProducts
                    .Where(b => b.ProductName.Contains(searchItem, StringComparison.OrdinalIgnoreCase) ||
                                b.Description.Contains(searchItem, StringComparison.OrdinalIgnoreCase))
                    .ToList();
            }

            // Lọc theo danh mục nếu có categoryId
            if (categoryId.HasValue)
            {
                filteredProducts = filteredProducts
                    .Where(p => p.CategoryId == categoryId.Value)
                    .ToList();
            }

            // Phân trang sau khi lọc
            int totalItems = filteredProducts.Count;
            ViewBag.TotalPages = (int)Math.Ceiling((double)totalItems / pageSize);
            ViewBag.CurrentPage = page;
            ViewBag.SearchItem = searchItem;

            var paginatedProducts = filteredProducts
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToList();

            return View(paginatedProducts);

        }


        [HttpGet("Product/Detail/{id}")]
        public async Task<IActionResult> UserProductDetail(int id)
        {
            var result = await _httpClient.GetFromJsonAsync<ProductViewDto>($"{ApiUrl}/{id}");
            return View(result);
        }

        [HttpGet("shop")]
        public async Task<IActionResult> Shop(int page = 1, int pageSize = 6, string searchItem = "", int? categoryId = null, int? brandId = null, int? minPrice = null,
                                                int? maxPrice = null)
        {
            var response = await _httpClient.GetFromJsonAsync<PageResultDto<ProductViewDto>>($"{ApiUrl}/Productpages?page=1&pageSize=20");
            if (response == null || response.Items == null)
            {
                return View(new List<ProductViewDto>());
            }



            // Lọc dữ liệu trên frontend
            var filteredProducts = response.Items;
            if (!string.IsNullOrEmpty(searchItem))
            {
                filteredProducts = filteredProducts
                    .Where(b => b.ProductName.Contains(searchItem, StringComparison.OrdinalIgnoreCase) ||
                                b.Description.Contains(searchItem, StringComparison.OrdinalIgnoreCase))
                    .ToList();
            }

            // Lọc theo danh mục nếu có categoryId
            if (categoryId.HasValue)
            {
                filteredProducts = filteredProducts
                    .Where(p => p.CategoryId == categoryId.Value)
                    .ToList();
            }

            // Lọc theo thương hiệu nếu có brandId
            if (brandId.HasValue)
            {
                filteredProducts = filteredProducts
                    .Where(p => p.BrandId == brandId.Value)
                    .ToList();
            }

            ViewBag.MinPrice = minPrice ?? 0;
            ViewBag.MaxPrice = maxPrice ?? 50000000;

            // 👇 Lọc theo khoảng giá
            if (minPrice.HasValue)
            {
                filteredProducts = filteredProducts
                    .Where(p => p.Price >= minPrice.Value)
                    .ToList();
            }

            if (maxPrice.HasValue)
            {
                filteredProducts = filteredProducts
                    .Where(p => p.Price <= maxPrice.Value)
                    .ToList();
            }

            // Phân trang sau khi lọc
            int totalItems = filteredProducts.Count;
            ViewBag.TotalPages = (int)Math.Ceiling((double)totalItems / pageSize);
            ViewBag.CurrentPage = page;
            ViewBag.SearchItem = searchItem;

            var paginatedProducts = filteredProducts
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToList();

            return View(paginatedProducts);

        }










    }
}
                                                        